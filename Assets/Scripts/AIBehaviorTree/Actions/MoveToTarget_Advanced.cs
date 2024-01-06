using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoveResult
{
    public List<int> currentMovePath;
    public Character moveToPlayer;
    public float fullDistance;
}


public class MoveToTarget_Advanced : ActionBehavior
{
    public Character playerC;
    private List<int> moveRangePath;
    public BehaviorType behaviorType;
    public List<MoveResult> moveResult = new List<MoveResult>();
    public override IEnumerator Start()
    {
        moveRangePath = new List<int>();
        moveResult.Clear();
        return base.Start();
    }

    public override IEnumerator Execute()
    {
        //查找任意玩家，并移动到他身边
        //移动范围
        //List<int> enemyLists = new List<int>();
        Dictionary<int, AStarNode> dic = new Dictionary<int, AStarNode>();
        AStar.MoveableArea(playerC, playerC.tileIndex, playerC.getRole().movePower, BattleManager.Instance.map, dic, BattleManager.Instance.GetEnemyList(playerC.sect));
        moveRangePath.Add(playerC.tileIndex);
        foreach (var i in dic.Keys)
        {
            moveRangePath.Add(i);
        }

        var players = new List<Character>();
        players = BehaviorCtrl.instance.GetPlayers(this.behaviorType, playerC);
        if (players.Count == 0)
        {
            state = State.Fail;
            yield break;
        }
        int maxRange = playerC.GetMaxAttackAndSkillRange();
        foreach (Character p_player in players)
        {
            //TODO:这里不应该这样用，应该要用什么技能就用什么技能的射程会好一点，不过先这样吧
            List<int> movePath = AStar.FindPath(playerC, playerC.tileIndex, playerC.tileIndex, p_player.tileIndex,
                        true, playerC.getRole().movePower, playerC.getRole().movePower, BattleManager.Instance.map, 0, maxRange,
                        true, true, null, null, null, moveRangePath, BattleManager.Instance.GetEnemyList(playerC.sect));
            float fullDistance = AStar.ManhattanPower(playerC.tileIndex, p_player.tileIndex, BattleManager.Instance.map);
            var m = new MoveResult() { currentMovePath = movePath, moveToPlayer = p_player, fullDistance = fullDistance };

            moveResult.Add(m);
        }

        moveResult.Sort(SortResult);

        var currentMovePath = moveResult[0].currentMovePath;
        BattleManager.Instance.SetDestination(currentMovePath, playerC);

        //playerC.moving 异步赋值原因需要等待
        yield return new WaitForSeconds(0.5f);

        while (BattleManager.Instance.isMoving) yield return new WaitForSeconds(0.5f);
        Debug.Log(this.GetType().ToString() + "开始移动");
        this.state = AIBehaviorTree.State.Succeed;
    }

    private int SortResult(MoveResult x, MoveResult y)
    {
        var x1 = GetWeight(x);
        var x2 = GetWeight(y);


        if (x1 > x2) return -1;
        if (x1 < x2) return 1;
        return 0;
    }

    /// <summary>
    /// 思考维度 目标的 距离,血量,辅助技能
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    int GetWeight(MoveResult t)
    {
        //血量越少,权重越高
        var p = 1f - ((float)t.moveToPlayer.getRole().hp / t.moveToPlayer.getRole().maxHp);
        var hp_weight = Mathf.FloorToInt(p * 10);
        int auxiliaryWeight = 0;
        int attackWeight = 0;
        //距离越远权重越低
        var distanceWeight = -((3 * t.fullDistance));

        //辅助决策权重
        if (behaviorType == BehaviorType.Auxiliary)
        {
            if (t.moveToPlayer.sect == playerC.sect)
            {
                auxiliaryWeight = 15;
                //友军血量高于95%辅助权重变低
                if (t.moveToPlayer.hp_percentage > 0.95f)
                    auxiliaryWeight = -1000;

            }
        }
        else if (behaviorType == BehaviorType.Attck && t.moveToPlayer.sect != playerC.sect)
        { ////如果是进攻类型则优先移动到敌人身边
            attackWeight = 10;
        }



        return hp_weight + (int)distanceWeight + attackWeight + auxiliaryWeight;
    }
}
