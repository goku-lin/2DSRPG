using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefine
{
    public enum Sect : uint
    {
        Blue, 
        Red,
        Grey
    }

    public enum PlayerSate : uint
    {
        idle,
        wait,
        skill
    }

    public enum AIType : byte
    {
        Attack,
        Heal
    }

    public enum SceneType : int
    {
        GameTilte = 0, GameWorld = 1, GameLevel = 2,
    }

    public enum AttackType
    {
        Str,
        Magic,
    }

    public enum MoveType
    {
        Foot,
        Rider,
        Fly
    }
}
