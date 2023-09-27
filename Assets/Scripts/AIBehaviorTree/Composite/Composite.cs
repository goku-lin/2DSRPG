using AIBehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : BehaviorNode
{
    public List<BehaviorNode> nodes = new List<BehaviorNode>();
}

