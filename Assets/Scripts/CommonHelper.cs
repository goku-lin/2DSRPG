using System;
using System.Collections.Generic;
using Game.Client;

// 通用辅助方法的静态类,用于创建标题菜单和检查是否按下了任何键。
public static class CommonHelper
{
    // 创建标题菜单的方法
    public static NUIManager CreateTitleMenu()
    {
        // 创建一个包含参数的列表
        List<object> arg = new List<object>();
        // 通过单例模式获取主UI管理器，并调用PushWindowsByClassName方法来创建标题菜单
        return Singleton<MainUIManager>.Instance.PushWindowsByClassName("Game.Client.UITitleMenuManager", arg);
    }

    // 检查是否按下了任何键的方法
    public static bool CheckPressAnyKey(int keyCode, int keyState)
    {
        // 检查keyCode是否为一组指定的按键，并检查keyState是否为指定的状态
        return (keyCode == 1 || keyCode == 2 || keyCode == 7 || keyCode == 3 || keyCode == 4 || keyCode == 5 || keyCode == 6 || keyCode == 2001 || keyCode == 2002 || keyCode == 8 || keyCode == 9 || keyCode == 12 || keyCode == 13 || keyCode == 10 || keyCode == 11 || keyCode == 14 || keyCode == 15 || keyCode == 16 || keyCode == 17 || keyCode == 18 || keyCode == 19 || keyCode == 20 || keyCode == 21) && (keyState == 2 || keyState == 1 || keyState == 3);
    }
}
