using System;
using System.Collections.Generic;

namespace lib.notify
{
    //这段代码定义了一个通知管理器类，允许注册、移除和触发通知回调，通知回调通过实现 ICallback 接口的 onMessage 方法来处理通知。它使用字典来存储不同通知类型对应的回调链表。同时，它定义了一些常量来表示不同的通知类型。
    // 这是一个通知管理器的类，用于注册、移除和触发通知回调。

    // 声明一个类，名为 Notifier
    public class Notifier
    {
        // 常量：表示通知类型为错误
        public const int TYPE_ERROR = 1;
        // 常量：表示通知类型为游戏开始
        public const int TYPE_GAME_START = 100;

        // 用于存储不同通知类型对应的回调链表
        private static Dictionary<int, LinkedList<ICallback>> NotifyMap = new Dictionary<int, LinkedList<ICallback>>();

        // 注册一个通知类型和对应的回调
        public static void Register(int notificationType, ICallback callback)
        {
            // 初始化一个链表来存储回调
            LinkedList<ICallback> linkedList;

            // 如果已经存在该通知类型，则获取已有的链表
            if (Notifier.NotifyMap.ContainsKey(notificationType))
            {
                linkedList = Notifier.NotifyMap[notificationType];
            }
            else
            {
                // 如果不存在该通知类型，创建一个新的链表
                linkedList = new LinkedList<ICallback>();
                Notifier.NotifyMap.Add(notificationType, linkedList);
            }

            // 如果回调不在链表中，将其添加到链表
            if (linkedList.Find(callback) == null)
            {
                linkedList.AddLast(callback);
            }
        }

        // 移除指定通知类型的回调
        public static void Remove(int notificationType, ICallback callback)
        {
            if (Notifier.NotifyMap.ContainsKey(notificationType))
            {
                LinkedList<ICallback> linkedList = Notifier.NotifyMap[notificationType];
                linkedList.Remove(callback);
            }
        }

        // 触发指定通知类型的回调，并传递参数
        public static void Notify(int notificationType, params object[] param)
        {
            if (Notifier.NotifyMap.ContainsKey(notificationType))
            {
                LinkedList<ICallback> linkedList = Notifier.NotifyMap[notificationType];

                // 遍历链表并调用每个回调的onMessage方法，传递通知类型和参数
                LinkedListNode<ICallback> next;
                for (LinkedListNode<ICallback> linkedListNode = linkedList.First; linkedListNode != null; linkedListNode = next)
                {
                    next = linkedListNode.Next;
                    linkedListNode.Value.onMessage(notificationType, param);
                }
            }
        }
    }
}
