using System;

namespace lib.keyEvent
{
    // 游戏按键状态的枚举类型
    public enum GAMEKEY_STATE
    {
        /// <summary>
        /// 保持不变，表示按键状态没有改变
        /// </summary>
        UNCHANGED,
        /// <summary>
        /// 按下，表示按键当前被按下
        /// </summary>
        PRESSED,
        /// <summary>
        /// 释放，表示按键当前被释放
        /// </summary>
        RELEASED,
        /// <summary>
        /// 重复，表示按键持续被按下（通常用于连续按键事件）
        /// </summary>
        REPEATED
    }
}
