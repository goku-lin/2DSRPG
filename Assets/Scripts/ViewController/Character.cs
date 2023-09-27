using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameDefine;

public class Character : MonoBehaviour
{
    public int uid; //代表重复角色的编号
    public int pid;

    [Header("人物属性")]
    public string unitName;
    public int movePower;
    public int min_AttackRange = 1;
    public int max_AttackRange = 1;

    public AIType tempjob;
    public string weaponName;

    [Header("队伍属性")]
    public Sect sect;
    public int tileIndex;
    public int startIndex;
    public PlayerSate state;

    public Character target;
    public BehaviorAI behaviorAI;
    internal Image hpImage;
    internal Transform hpImageTrs;

    public Role beforeRole;
    public Role role;

    public float hp_percentage
    {
        get
        {
            return (float)this.getRole().hp / this.getRole().maxHp;
        }
    }

    private void Start()
    {
        EventDispatcher.instance.DispatchEvent<Character>(GameEventType.playerInitSkill, this);
    }

    public void Init()
    {
        if (sect != BattleManager.Instance.mySect)
        {
            ////随机敌人位置
            startIndex = tileIndex;
            transform.position = BattleManager.Instance.mapTiles[tileIndex].transform.position;
            BattleManager.Instance.mapTiles[tileIndex].character = this;
        }
        else
        {
            //玩家
            startIndex = tileIndex;
            transform.position = BattleManager.Instance.mapTiles[tileIndex].transform.position;
            BattleManager.Instance.mapTiles[tileIndex].character = this;
        }
        //Role role = getRole();
        this.role = getRole();
        this.unitName = role.unitName;
        InitBecauseEquip();
    }

    public void InitBecauseEquip()
    {
        this.movePower = role.movePower;
        if (role.equip != null)
        {
            this.weaponName = role.equip.info.Name;
            min_AttackRange = role.equip.info.RangeI;
            max_AttackRange = role.equip.info.RangeO;
        }
        else
        {
            min_AttackRange = 1;
            max_AttackRange = 1;
        }
    }

    public int GetMaxAttackAndSkillRange()
    {
        int maxRange = max_AttackRange;
        foreach (Skill skill in getRole().equipedSkills)
        {
            if (skill != null && skill.activeSkillAction != null && skill.CD == 0)
            {
                if (skill.RangeO > maxRange)
                    maxRange = (int)skill.RangeO;
            }
        }
        return maxRange;
    }

    public void BeforeBattle()
    {
        beforeRole = role.clone();
        target.beforeRole = target.role.clone();
        EventDispatcher.instance.DispatchEvent<Character, Character>(GameEventType.battle_Start, this, target);
    }

    /// <summary>
    /// 主动进攻判断时能不能反击
    /// </summary>
    /// <returns></returns>
    public IEnumerator Attack()
    {
        EventDispatcher.instance.DispatchEvent<Role>(GameEventType.playAttackVoice, this.getRole());

        AttackAnimation(target.transform.position, target);
        yield return new WaitForSeconds(0.5f);
        if (target.getRole().hp > 0)
        {
            var manhattanPower = AStar.ManhattanPower(tileIndex, target.tileIndex, BattleManager.Instance.map);
            if (manhattanPower <= target.max_AttackRange && manhattanPower >= target.min_AttackRange)
            {
                target.AttackAnimation(transform.position, this);
                yield return new WaitForSeconds(0.5f);
            }
            if (this.getRole().quick - target.getRole().quick >= 5)
            {
                AttackAnimation(target.transform.position, target);
                yield return new WaitForSeconds(0.5f);
            }
        }
        EventDispatcher.instance.DispatchEvent<Character, Character>(GameEventType.battle_End, this, target);
        //恢复的逻辑放到battleend事件去了
        //TODO:在这加经验
        target = null;
        BattleManager.Instance.Wait();
    }

    /// <summary>
    /// 攻击执行
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="defender"></param>
    public void AttackAnimation(Vector3 targetPos, Character defender)
    {
        Vector3 originPos = transform.position;
        transform.DOMove(targetPos, 0.25f).OnComplete(
            () =>
            {
                //TODO:暴击在这加
                defender.BeAttack(calcAtt() - defender.calcDef(this.getRole()));
                transform.DOMove(originPos, 0.25f);
            }
            );
    }

    public void BeAttack(int damage)
    {
        damage = Math.Max(damage, 1);   //别给对面加血了
        this.getRole().hp -= damage;
        EventDispatcher.instance.DispatchEvent<int, Vector3>(GameEventType.showHudDamage, damage, this.transform.position + Vector3.up * 0.5f);
        EventDispatcher.instance.DispatchEvent(GameEventType.playHitBodySound);
        UIManager.Instance.UpdateHp(this);
        if (this.getRole().hp <= 0)
        {
            BattleManager.Instance.CharacterDie(this);
        }
    }

    public void DamgeBySkill(int dps)
    {
        EventDispatcher.instance.DispatchEvent<int, Vector3>(GameEventType.showHudDamage, -dps, this.transform.position + Vector3.up * 0.5f);
        // 封装防止数值溢出
        this.getRole().hp += -dps;
        UIManager.Instance.UpdateHp(this);

        if (this.getRole().hp <= 0)
        {
            BattleManager.Instance.CharacterDie(this);
        }
    }

    public void RestoreHealth(int addHp)
    {
        this.getRole().hp += addHp;
        if (this.getRole().hp >= this.getRole().maxHp) this.getRole().hp = this.getRole().maxHp;
        UIManager.Instance.UpdateHp(this);

        UIManager.Instance.ShowRestoreHealth(addHp, this);
    }

    public Skill ShowActiveSkill_ReleaseRange(int skillId)
    {
        var skill = this.getRole().equipedSkills[skillId];

        return skill;
    }

    public void Releaseskill(Skill usingSkill, Character skillTarget)
    {
        this.state = PlayerSate.skill;
        //if (skillTarget != this) this.lookatTarget(skillTarget);

        var players = BattleManager.Instance.characters;

        var filterPlayers = new List<Character>();

        filterPlayers.AddRange(players);
        //筛选 符合技能执行条件的对象
        filterPlayers = filterPlayers.FindAll(player => BattleManager.Instance.canSelect(this, player, usingSkill));
        //筛选在技能作用范围内的对象
        filterPlayers = filterPlayers.FindAll(player => BattleManager.Instance.skillRangePath.Contains(player.tileIndex));
        //代入语言环境描述 气愈之术 筛选出 对在技能有效范围内 且 是友方单位的 玩家集合

        //显示起手特效
        usingSkill.activeSkillAction.BeforReleaseskill(this, filterPlayers);
        //人物动作 武器插入地面时加血
        //人物动作 播放完成时行动结束
        System.Action method = () => {
            //usingSkill.activeSkillConfig.activeSkillAction.Releaseskill(this, filterPlayers);
            SkillSystem.Instance.Releaseskill(usingSkill, this, filterPlayers);
        };
        method.Invoke();
        //StartCoroutine(c_PlayAnimation("skill03", 0.6f, method));
        System.Action methodEnd = () => { BattleManager.Instance.ActionEnd(); };
        methodEnd.Invoke();
        //StartCoroutine(c_PlayAnimationEnd("skill03", methodEnd));
    }

    internal void Cd_Add(int value)
    {
        foreach (var item in this.getRole().equipedSkills)
        {
            if (item != null)
            {
                //主动技能
                item.CD += value;
                if (item.CD <= 0) item.CD = 0;
            }
        }
    }

    public void UseItem(Skill usingSkill)
    {
        this.state = PlayerSate.skill;
        var filterPlayers = new List<Character>
        {
            this
        };
        //显示起手特效
        usingSkill.activeSkillAction.BeforReleaseskill(this, filterPlayers);
        //人物动作 武器插入地面时加血
        //人物动作 播放完成时行动结束
        System.Action method = () => {
            //usingSkill.activeSkillConfig.activeSkillAction.Releaseskill(this, filterPlayers);
            SkillSystem.Instance.Releaseskill(usingSkill, this, filterPlayers);
        };
        method.Invoke();
        //StartCoroutine(c_PlayAnimation("skill03", 0.6f, method));
        System.Action methodEnd = () => { BattleManager.Instance.ActionEnd(); };
        methodEnd.Invoke();
        //StartCoroutine(c_PlayAnimationEnd("skill03", methodEnd));
    }

    public Role getRole()
    {
        return PlayerData.GetRole(this.getCharacterId(), this.getId());
    }

    public int getCharacterId()
    {
        return this.pid;
    }

    public int getId()
    {
        return this.uid;
    }

    public int calcAtt()
    {
        Role attackRole = this.getRole();
        int attackerAtk = attackRole.calcAtt();
        return attackerAtk;
    }

    public int calcDef(Role attacker)
    {
        Role role = this.getRole();
        int defend = role.calcDef(attacker);
        return defend;
    }

}

/// <summary>
/// 专门用在地图编辑保存单位数据
/// </summary>
public class UnitInfo
{
    public string unitName;
    public int tileIndex;
    public bool isPlayer;

    public UnitInfo(string unitName, int tileIndex, bool isPlayer)
    {
        this.unitName = unitName;
        this.tileIndex = tileIndex;
        this.isPlayer = isPlayer;
    }

}