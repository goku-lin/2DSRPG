using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    public class GameLogic
    {
        public enum SCENE_TYPE
        {
            Title,
            Home,
            Battle
        }

        //游戏的版本描述和版本号
        public const string VER_DESC = "";
        public const string VERSION = "0.1.0";
        //当前游戏关卡
        public static GLevel CurLevel = null;
        //当前游戏关卡是否已结束或中止
        public static bool IsLevelEnd = false;
        public static bool IsLevelAborted = false;

        public static GameLogic.SCENE_TYPE CurScene;
        public static int currentBGM;
        public static bool currentBGMLoopState;
        public static int previousBGM;
        public static bool previousBGMLoopState;
        public static List<int> currentLoopSFX = new List<int>();

        public static bool IsMouseMoved = true;
        private static Vector3 LastMousePos = Vector3.zero;

        public static void WriteRecord(BinaryWriter writer)
        {

        }

        public static void ReadRecord(BinaryReader reader)
        {

        }

        public static void CheckMousePos()
        {
            if (Math.Abs(GameLogic.LastMousePos.x - Input.mousePosition.x) > 0.2f || Math.Abs(GameLogic.LastMousePos.y - Input.mousePosition.y) > 0.2f || Math.Abs(GameLogic.LastMousePos.z - Input.mousePosition.z) > 0.2f)
            {
                GameLogic.IsMouseMoved = true;
            }
            GameLogic.LastMousePos = Input.mousePosition;
        }

    }
}