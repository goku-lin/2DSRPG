using System;

namespace Game.Client
{
    // 结构体 MenuPopProp 用于传递菜单和弹出窗口的属性信息,包括名称、附加参数以及是否根据类名创建实例。通常用于传递这些信息以在应用程序中创建和管理菜单和弹出窗口。
    public struct MenuPopProp
    {
        // 菜单或弹出窗口的名称
        public string uiname;
        // 附加参数，用于初始化菜单或弹出窗口
        public object arg;
        // 指示是否根据类名创建实例
        public bool isCreateByClassName;

        // 构造函数，用于初始化 MenuPopProp 的实例
        // 参数 uiname：菜单或弹出窗口的名称
        // 参数 arg：附加参数，用于初始化菜单或弹出窗口
        // 参数 isCreateByClassName：是否根据类名创建实例
        public MenuPopProp(string uiname, object arg, bool isCreateByClassName)
        {
            this.uiname = uiname;
            this.arg = arg;
            this.isCreateByClassName = isCreateByClassName;
        }
    }
}
