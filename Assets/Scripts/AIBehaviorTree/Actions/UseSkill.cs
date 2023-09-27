using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkill : ActionBehavior
{
    public InActiveSkillRange inActiveSkillRange;
    public override IEnumerator Execute()
    {
        //为了看清楚 节点的 运行过程
        //下面那几行加上去就出bug了
        //BattleManager.Instance.ShowRange(inActiveSkillRange.skillRangePath);

        //yield return new WaitForSeconds(0.5f);
        //BattleManager.Instance.ClearTile(inActiveSkillRange.skillRangePath);

        var data = inActiveSkillRange;

        //data.playerC.actionRangePath = null;

        var from = data.playerC;
        var to = data.insidePlayers[0];
        //AI的技能释放
        BattleManager.Instance.SkillSelectionTarget(from, to, data.resultSkill);

        state = State.Succeed;
        //等待路径计算完成
        //while (data.playerC.actionRangePath == null) yield return null;

        //为了看清楚 节点的 运行过程
        yield return new WaitForSeconds(0.5f);

        BattleManager.Instance.confirmUseSkill_AI(from, to);
    }
}
