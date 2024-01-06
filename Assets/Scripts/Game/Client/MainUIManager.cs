using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client
{
    public class MainUIManager : Singleton<MainUIManager>
    {
        protected UILogic _logic;

        public MainUIManager()
        {
            this._logic = new UILogic();
        }

        // 阻止或解除阻止键盘事件
        public void BlockKeyEvent(bool isBlock)
        {
            this._logic.BlockKeyEvent(isBlock);
        }

        // 检查是否阻止了键盘事件
        public bool IsBlockKeyEvent()
        {
            return this._logic.IsBlockKeyEvent();
        }

        // 根据类名创建弹出式用户界面
        public PopManager CreatePopUpWindowsByClassName(string uiname, object arg, bool isInQueue = false, bool ignoreSame = false, bool isHidePrev = false, bool isExcludePop = false)
        {
            return this._logic.CreatePopUpWindowsByClassName(uiname, arg, isInQueue, ignoreSame, isHidePrev, isExcludePop);
        }

        // 关闭指定弹出式用户界面
        public void ClosePopUpWindows(PopManager mgr, object arg, string uiname, bool isNeedAfterDeal = true)
        {
            this._logic.ClosePopUpWindows(mgr, arg, uiname, isNeedAfterDeal);
        }

        // 关闭指定名称的弹出式用户界面
        public void ClosePopUpWindowsByName(string uiname, object arg, bool isNeedAfterDeal = true)
        {
            this._logic.ClosePopUpWindowsByName(uiname, arg, isNeedAfterDeal);
        }

        // 弹出指定用户界面
        public void PopWindows(string uiname, object arg, bool isReopen = true, bool isNeedAfterDeal = true)
        {
            this._logic.PopWindows(uiname, arg, isReopen, isNeedAfterDeal);
        }

        // 根据类名推送用户界面到显示栈
        public NUIManager PushWindowsByClassName(string uiname, object arg)
        {
            return this._logic.PushWindowsByClassName(uiname, arg);
        }
    }
}