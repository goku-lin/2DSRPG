using System;
using System.Numerics;
using UnityEngine;

public class tile
{
    public int index;
    public int moveCost;

    public tile(int _index, int _moveCost)
    {
        index = _index;
        moveCost = _moveCost;
    }
}

public class GMapTile : MonoBehaviour
{
    public int index;
    public int moveCost = 1;
    private int height;
    public Character character;

    public GMapTile(int index)
    {
        this.index = index;
        height = 1;
    }

    public void SetCost(int cost)
    {
        this.moveCost = cost;
    }
    public int GetHeight()
    {
        return this.height;
    }

    public bool hasActor()
    {
        return this.character != null;
    }

}
