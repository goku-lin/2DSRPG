using InControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameAxisKeyValue
    {
        // 游戏轴键值的输入控制类型
        public InputControlType key;
        // 游戏轴键值的值
        public float value;

        // 构造函数，初始化游戏轴键值
        public GameAxisKeyValue(InputControlType key, float value)
        {
            this.key = key;     // 设置输入控制类型
            this.value = value; // 设置值
        }
    }
}