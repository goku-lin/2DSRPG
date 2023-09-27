using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIBehaviorTree
{
    public enum State
    {
        Running = 0,
        Succeed,
        Fail
    }

    public class BehaviorNode
    {
        public State state = State.Running;

        public virtual IEnumerator Execute()
        {
            yield return 1;
        }

        public virtual IEnumerator Start()
        {
            //Debug.Log(this.GetType().ToString());
            state = State.Running;

            yield return BehaviorCtrl.instance.StartCoroutine(Execute());
        }
    }

    /// <summary>
    /// AI偏好
    /// </summary>
    public enum BehaviorType
    {
        None,
        Attck,
        Auxiliary
    }
}
