using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据模型
/// </summary>
public class InActiveSkillRangeResult
{
    public List<Character> insidePlayers;
    public Skill resultSkill;
    public List<int> skillRangePath;
}

public class InActiveSkillRange_Advanced : Condition_Behavior
{
    public Character playerC;
    public List<InActiveSkillRangeResult> data;


    protected virtual List<Character> GetTargetPlayers()
    {
        return null;
    }


    public override IEnumerator Start()
    {
        data = new List<InActiveSkillRangeResult>();
        return base.Start();
    }

    public override IEnumerator Execute()
    {
        foreach (Skill skill in playerC.getRole().equipedSkills)
        {
            if (skill != null && skill.Info.SkillType != 0 && skill.Info.CD == 0)
            {
                Dictionary<int, AStarNode> dic = new Dictionary<int, AStarNode>();
                List<int> skillRangePath = new List<int>
                {
                    playerC.tileIndex
                };
                AStar.AttackableArea(playerC, playerC.tileIndex, skill.Info.RangeO, BattleManager.Instance.map, dic, skillRangePath);
                //查找技能射程范围内的玩家
                foreach (var i in dic.Keys)
                {
                    skillRangePath.Add(i);
                    if (i == playerC.tileIndex) Debug.Log(dic.Count);
                }
                //根据技能的配置获取作用对象
                var players = GetTargetPlayers();
                //派生伤害技能和辅助的技能的原因是为了减少if esle 提高代码可描述性

                //查找技能射程范围内的玩家
                var insidePlayers = players.FindAll(f_plyaer => skillRangePath.Contains(f_plyaer.tileIndex));

                if (insidePlayers.Count > 0)
                {
                    data.Add(new InActiveSkillRangeResult()
                    {
                        insidePlayers = insidePlayers,
                        skillRangePath = skillRangePath,
                        resultSkill = skill
                    });
                }
            }
        }
        //没有技能符合条件
        if (data.Count > 0)
        {
            state = State.Succeed;
            Debug.Log(this.GetType().ToString() + "有可释放技能的目标");
        }
        else
        {
            state = State.Fail;
            Debug.Log(this.GetType().ToString() + "没有技能的目标");
            yield break;
        }

        //return base.Execute();
    }

    protected virtual bool CanSelectSkill(SkillInfo skill)
    {
        return false;
    }
}

public class InDamageSkillRange : InActiveSkillRange_Advanced
{
    protected override bool CanSelectSkill(SkillInfo skill)
    {
        return skill.RangeTarget == (int)SkillTarget.Enemy;
    }

    protected override List<Character> GetTargetPlayers()
    {
        return BattleManager.Instance.GetEnemy(playerC.sect);
    }
}

public class InAuxiliarySkillRange : InActiveSkillRange_Advanced
{
    protected override bool CanSelectSkill(SkillInfo skill)
    {
        return skill.RangeTarget == (int)SkillTarget.Friendly;
    }
    protected override List<Character> GetTargetPlayers()
    {
        return BattleManager.Instance.GetPlayers(playerC.sect);
    }
}