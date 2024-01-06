using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 只要子节点有一个返回true，则停止执行其它子节点，并且Selector返回true。
/// 如果所有子节点都返回false
/// </summary>
public class Selector : Composite
{
    public override IEnumerator Execute()
    {
        foreach (var node in nodes)
        {
            yield return BehaviorCtrl.instance.StartCoroutine(node.Start());
            if (node.state == State.Succeed) yield break;
        }

        this.state = AIBehaviorTree.State.Fail;

        //return base.Execute();
    }
}