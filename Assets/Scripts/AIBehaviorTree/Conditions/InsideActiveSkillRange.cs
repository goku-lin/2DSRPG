using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InActiveSkillRange : Condition_Behavior
{
    //当前AI
    public Character playerC;
    //AI能攻击到的敌方
    public List<Character> insidePlayers;
    public Skill resultSkill;
    public List<int> skillRangePath = new List<int>();

    public override IEnumerator Start()
    {
        insidePlayers = null;
        resultSkill = null;
        skillRangePath = new List<int>();
        return base.Start();
    }

    public override IEnumerator Execute()
    {
        foreach (Skill skill in playerC.getRole().equipedSkills)
        {
            if (skill != null && skill.activeSkillAction != null && skill.cd == 0)
            {
                Dictionary<int, AStarNode> dic = new Dictionary<int, AStarNode>();
                AStar.AttackableArea(playerC, playerC.tileIndex, skill.Info.RangeO, BattleManager.Instance.map, dic, skillRangePath);
                var enemys = BattleManager.Instance.GetEnemy(playerC.sect);
                //查找技能射程范围内的玩家
                foreach (var i in dic.Keys)
                {
                    skillRangePath.Add(i);
                }

                insidePlayers = enemys.FindAll(enemy => skillRangePath.Contains(enemy.tileIndex));

                if (insidePlayers.Count > 0)
                {
                    state = State.Succeed;
                    resultSkill = skill;
                    //resultPath = path;
                }
                else
                {
                    state = State.Fail;
                }
            }
        }
        //放在最后加，不然前面遍历的时候，重复添加多次玩家所在点
        skillRangePath.Add(playerC.tileIndex);
        //没有技能符合条件
        if (resultSkill == null)
        {
            state = State.Fail;
            yield break;
        }
    }
}