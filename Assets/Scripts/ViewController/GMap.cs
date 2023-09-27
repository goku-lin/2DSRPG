using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMap
{
    public int x;
    public int y;
    private int num;
    public GMapTile[] tiles;
    public List<tile> tilesList;

    public GMap(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.num = x * y;
        tiles = new GMapTile[num];
        tilesList = new List<tile>();
    }

    public int[] neighborhood(int startIndex)
    {
        return new int[]
        {
                this.leftTile(startIndex),
                this.rightTile(startIndex),
                this.upTile(startIndex),
                this.downTile(startIndex)
        };
    }

    public int upTile(int index)
    {
        if (index < 0 || index >= this.tiles.Length)
        {
            return -1;
        }
        int num = this.getRow(index) + 1;
        if (num >= this.y)
        {
            return index;
        }
        int col = this.getCol(index);
        return this.getIndex(col, num);
    }

    public int downTile(int index)
    {
        if (index < 0 || index >= this.tiles.Length)
        {
            return -1;
        }
        int num = this.getRow(index) - 1;
        if (num < 0)
        {
            return index;
        }
        int col = this.getCol(index);
        return this.getIndex(col, num);
    }

    public int leftTile(int index)
    {
        if (index < 0 || index >= this.tiles.Length)
        {
            return -1;
        }
        int num = this.getCol(index) - 1;
        if (num < 0)
        {
            return index;
        }
        int row = this.getRow(index);
        return this.getIndex(num, row);
    }

    public int rightTile(int index)
    {
        if (index < 0 || index >= this.tiles.Length)
        {
            return -1;
        }
        int num = this.getCol(index) + 1;
        if (num >= this.x)
        {
            return index;
        }
        int row = this.getRow(index);
        return this.getIndex(num, row);
    }
    public int getRow(int index)
    {
        if (index < 0 || index >= this.tiles.Length)
        {
            return -1;
        }
        return Mathf.FloorToInt((float)(index / this.x));
    }

    public int getCol(int index)
    {
        if (index < 0 || index >= this.tiles.Length)
        {
            return -1;
        }
        return Mathf.FloorToInt((float)(index % this.x));
    }
    public int getIndex(int col, int row)
    {
        if (col < 0 || row < 0)
        {
            return -1;
        }
        return col + row * this.x;
    }
    public GMapTile GetTileAt(int index)
    {
        if (index < 0 || index >= this.tiles.Length)
        {
            return null;
        }
        return this.tiles[index];
    }

    public int Distance(int startIndex, int endIndex)
    {
        int row = this.getRow(startIndex);
        int col = this.getCol(startIndex);
        int row2 = this.getRow(endIndex);
        int col2 = this.getCol(endIndex);
        return Mathf.Abs(row2 - row) + Mathf.Abs(col2 - col);
    }
}
