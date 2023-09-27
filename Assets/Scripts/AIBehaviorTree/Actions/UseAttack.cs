using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UseAttack : ActionBehavior
{
    public Character playerC;

    public override IEnumerator Execute()
    {
        var enemys = BattleManager.Instance.GetEnemy(playerC.sect);

        List<int> moveRangePath = new List<int>
        {
            playerC.tileIndex
        };
        Dictionary<int, AStarNode> attackDic = new Dictionary<int, AStarNode>();
        AStar.AttackableArea(playerC, playerC.tileIndex, playerC.max_AttackRange, BattleManager.Instance.map, attackDic, moveRangePath);
        enemys = enemys.FindAll(e => attackDic.ContainsKey(e.tileIndex));

        if (enemys.Count <= 0)
        {
            this.state = State.Fail;
            yield break;
        }
        //优先攻击 范围之内血量最少的敌人
        enemys.Sort(OrderBy_Hp);
        EventDispatcher.instance.DispatchEvent<Character, Character>(GameEventType.battle_Start, playerC, enemys[0]);
        BattleManager.Instance.AttackSelect_AI(playerC, enemys[0], moveRangePath);
        while (BattleManager.Instance.isMoving) yield return new WaitForSeconds(0.5f);
        BattleManager.Instance.Attack(playerC);

        this.state = State.Succeed;

    }
    /// <summary>
    /// 升序排序，血量小在前
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int OrderBy_Hp(Character x, Character y)
    {
        var x1 = x.getRole().hp;

        var x2 = y.getRole().hp;

        if (x1 > x2) return 1;
        if (x1 < x2) return -1;
        return 0;

    }
}