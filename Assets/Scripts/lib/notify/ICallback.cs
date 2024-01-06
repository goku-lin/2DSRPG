using System;

namespace lib.notify
{
    public interface ICallback
    {
        /// <summary>
        /// 消息注册
        /// </summary>
        /// <param name="notificationType"></param>
        /// <param name="param"></param>
        void onMessage(int notificationType, params object[] param);

        /// <summary>
        /// 消息注销
        /// </summary>
        /// <param name="notificationType"></param>
        void onRemoveNotify(int notificationType);
    }
}
