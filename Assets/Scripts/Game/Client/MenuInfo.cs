using System;

namespace Game.Client
{
    // 表示菜单信息的结构
    public struct MenuInfo
    {
        /// <summary>
        /// 构造函数，用于初始化菜单信息
        /// </summary>
        /// <param name="uiname">菜单的唯一名称</param>
        /// <param name="mgr">关联的UI管理器</param>
        public MenuInfo(string uiname, NUIManager mgr)
        {
            this.uiname = uiname; // 初始化菜单名称
            this.mgr = mgr; // 初始化关联的UI管理器
        }

        // 菜单的唯一名称
        public string uiname;

        // 关联的UI管理器
        public NUIManager mgr;
    }
}
