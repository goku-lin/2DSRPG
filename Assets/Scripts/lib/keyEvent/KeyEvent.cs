using System;
using System.Collections.Generic;

namespace lib.keyEvent
{
    // KeyEvent 类用于管理游戏中的按键事件监听器
    public class KeyEvent
    {
        // 游戏键盘事件类型常量
        public const int OK = 1;
        public const int CANCEL = 2;
        public const int UP = 3;
        public const int DOWN = 4;
        public const int LEFT = 5;
        public const int RIGHT = 6;
        public const int HOME = 7;
        public const int FUNCTION1 = 8;
        public const int FUNCTION2 = 9;
        public const int MENU1 = 10;
        public const int MENU2 = 11;
        public const int FUNCTION3 = 12;
        public const int FUNCTION4 = 13;
        public const int SCROLL_UP = 14;
        public const int SCROLL_DOWN = 15;
        public const int SCROLL_LEFT = 16;
        public const int SCROLL_RIGHT = 17;
        public const int LEFT_SCROLL_UP = 18;
        public const int LEFT_SCROLL_DOWN = 19;
        public const int LEFT_SCROLL_LEFT = 20;
        public const int LEFT_SCROLL_RIGHT = 21;
        public const int LEFT_AXIS_STICK = 1001;
        public const int RIGHT_AXIS_STICK = 1002;
        public const int MOUSE_LEFT = 2001;
        public const int MOUSE_RIGHT = 2002;

        // 存储游戏键盘事件监听器的列表
        protected static List<IGameKeyEventListener> eventListenerList = new List<IGameKeyEventListener>();

        // 注册一个游戏键盘事件监听器
        public static void Register(IGameKeyEventListener listener)
        {
            int num = KeyEvent.eventListenerList.IndexOf(listener);
            if (num >= 0)
            {
                // 如果监听器已存在，将其从列表中移除
                KeyEvent.eventListenerList.RemoveAt(num);
            }
            for (int i = 0; i < KeyEvent.eventListenerList.Count; i++)
            {
                if (KeyEvent.eventListenerList[i].getPriority() <= listener.getPriority())
                {
                    // 根据监听器的优先级，将监听器插入到列表中的适当位置
                    KeyEvent.eventListenerList.Insert(i, listener);
                    return;
                }
            }
            // 如果没有更高优先级的监听器，将监听器添加到列表末尾
            KeyEvent.eventListenerList.Add(listener);
        }

        // 从监听器列表中移除一个游戏键盘事件监听器
        public static void Remove(IGameKeyEventListener listener)
        {
            KeyEvent.eventListenerList.Remove(listener);
        }

        // 清空所有的游戏键盘事件监听器
        public static void Clear()
        {
            KeyEvent.eventListenerList.Clear();
        }
    }
}
