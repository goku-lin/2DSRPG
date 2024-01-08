using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public int mapIndex;
    public int prevNodeIndex;
    public float movePowerRemain;
    public float totalCost;
    public float totalIntentPower;

    public AStarNode(int mapIndex, int prevIndex, float movePower, float totalCost, float totalIntentPower)
    {
        this.mapIndex = mapIndex;
        this.prevNodeIndex = prevIndex;
        this.movePowerRemain = movePower;
        this.totalCost = totalCost;
        this.totalIntentPower = totalIntentPower;
    }
}
