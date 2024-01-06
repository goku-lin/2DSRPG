using System;

namespace Game.Client
{
    // 表示UI管理器的接口,允许关闭、打开、重新打开、销毁和隐藏UI，并且可以接受可选的参数用于传递额外的信息。
    public interface IUIManager
    {
        // 关闭UI，接受一个可选参数arg，用于传递额外的信息
        void CloseUI(object arg);

        // 打开UI，接受一个可选参数arg，用于传递额外的信息
        void OpenUI(object arg);

        // 重新打开UI，接受一个可选参数arg，用于传递额外的信息
        void ReopenUI(object arg);

        // 销毁UI，接受一个可选参数arg，用于传递额外的信息
        void DestroyUI(object arg);

        // 隐藏UI，接受一个可选参数arg，用于传递额外的信息
        void HideUI(object arg);
    }
}
