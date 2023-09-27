using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 只要有一个子节点返回false，则停止执行其它子节点，并且Sequence返回false。
//如果所有子节点都返回true，则Sequence返回true。
/// </summary>

public class Sequence : Composite
{
    public override IEnumerator Execute()
    {
        foreach (var node in nodes)
        {
            yield return BehaviorCtrl.instance.StartCoroutine(node.Start());
            //只要有一个子节点返回false，则停止执行其它子节点，
            if (node.state == State.Fail)
            {
                this.state = AIBehaviorTree.State.Fail;
                yield break;
            }
        }

        //如果所有子节点都返回true，则Sequence返回true。
        this.state = AIBehaviorTree.State.Succeed;
    }
}