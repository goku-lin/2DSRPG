using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MoveToTarget : ActionBehavior
{
    public Character playerC;
    private List<int> currentMovePath;

    public override IEnumerator Start()
    {
        currentMovePath = null;
        return base.Start();
    }

    public override IEnumerator Execute()
    {
        //查找任意敌人，并移动到他身边
        var enemy = BattleManager.Instance.GetEnemy(playerC.sect);
        List<int> enemyLists = new List<int>();
        Dictionary<int, AStarNode> dic = new Dictionary<int, AStarNode>();
        List<int> moveRangePath = new List<int>();
        foreach (var item in enemy)
        {
            enemyLists.Add(item.tileIndex);
        }
        //这里用enemy[0]来寻路，实际上应该选最短或者血最少的
        AStar.MoveableArea(playerC, playerC.tileIndex, playerC.getRole().movePower, BattleManager.Instance.map, dic, enemyLists);
        moveRangePath.Add(playerC.tileIndex);
        foreach (var i in dic.Keys)
        {
            moveRangePath.Add(i);
        }
        if (enemy.Count == 0)
        {
            state = State.Fail;
            yield break;

        }
        currentMovePath = AStar.FindPath(playerC, playerC.tileIndex, playerC.tileIndex, enemy[0].tileIndex,
                        true, playerC.getRole().movePower, playerC.getRole().movePower, BattleManager.Instance.map, 0, 0,
                        true, true, null, null, null, moveRangePath, enemyLists);

        BattleManager.Instance.SetDestination(currentMovePath, playerC);

        yield return new WaitForSeconds(0.5f);

        while (BattleManager.Instance.isMoving) yield return new WaitForSeconds(0.5f);


        this.state = AIBehaviorTree.State.Succeed;
    }

    private void OnMovePathOk(List<int> path)
    {
        currentMovePath = path;
    }
}
