using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ActionBehavior
{
    public Character playerC;
    public override IEnumerator Execute()
    {
        BattleManager.Instance.Wait_AI(playerC);
        return base.Execute();
    }
}
