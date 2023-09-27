using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GLevel
{
    public string levelName;
    public string background;
    public GMap map;
    public Vector3 pos;
    public Vector3 size;
    
    public GLevel() { }

    public GLevel(string levelName, GMap map, string background, Vector3 pos, Vector3 size)
    {
        this.levelName = levelName;
        this.map = map;
        this.background = background;
        this.pos = pos;
        this.size = size;
    }
}
