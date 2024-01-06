using France.Game.model.level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GFightUnit : GActor
{
    public string pid;
    public int teamId = 0;

    public GameObject hpGroup;
    private GameObject hpObject;

    public GFightUnit(int id) : base(id, 1)
    {
    }

    public override void createObj()
    {
        base.createObj();
        //角色不存在，直接返回
        if (this.getCharacterId() == null)
        {
            //base.disableActorFlag(DataType.ACTOR_FLAG.ACTIVE);
            return;
        }
        //初始化角色的血条对象，并将其设置为角色的子对象
        GameObject gameObject = this.actorObject.FindObject("Unit_HpBar");
        GameObject hpFriend = this.actorObject.FindObject("hpFriend");
        GameObject hpEnemy = this.actorObject.FindObject("hpEnemy");

        // 根据队伍ID设置血条对象
        if (teamId == 0)
        {
            hpEnemy.SetActive(false);
            hpFriend.SetActive(true);
            this.hpObject = hpFriend;
        }
        else
        {
            hpEnemy.SetActive(true);
            hpFriend.SetActive(false);
            this.hpObject = hpEnemy;
        }
        //// 查找并设置 MP 对象数组
        //for (int j = 0; j < this.mpObjects.Length; j++)
        //{
        //    this.mpObjects[j] = this.actorObject.FindObject("mp" + j);
        //}
        //// 获取角色的 classFlag
        //string classFlag = this.getRole().getClassInfo().classFlag;
        //// 如果 classFlag 不为空，查找并显示相应的对象
        //if (classFlag != null && classFlag.Length > 0)
        //{
        //    GameObject gameObject4 = this.actorObject.FindObject(classFlag);
        //    if (gameObject4 != null)
        //    {
        //        gameObject4.SafeActive(true);
        //    }
        //}
        // 更新 HP
        this.updateHP();
    }

    /// <summary>
    /// 更新血量，之后把所有这个放到血量变更里面
    /// </summary>
    public void updateHP()
    {
        if (this.hpObject != null)
        {
            float hpMax = this.getRole().maxHp;
            //TODO:role感觉不应该由控制当前hp的职能
            float x = (hpMax <= 0f) ? 0f : ((int)getRole().hp * 1f / (int)hpMax);
            this.hpObject.transform.localScale = new Vector3(x, 1f, 1f);
        }
    }

    public Role getRole()
    {
        return PlayerData.GetRole(this.getCharacterId(), this.getId());
    }

    public string getCharacterId()
    {
        return this.pid;
    }

    public void DamgeBySkill(int dps)
    {
        // 封装防止数值溢出
        this.getRole().hp += -dps;

    }

    public void RestoreHealth(int addHp)
    {
        this.getRole().hp += addHp;
        if (this.getRole().hp >= this.getRole().maxHp) this.getRole().hp = this.getRole().maxHp;
    }

    internal void Cd_Add(int value)
    {
        foreach (var item in this.getRole().equipedSkills)
        {
            if (item != null)
            {
                //主动技能
                item.cd += value;
                if (item.cd <= 0) item.cd = 0;
            }
        }
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
