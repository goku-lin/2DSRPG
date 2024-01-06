using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Client
{
    public class PopManager : MonoBehaviour, IUIManager
    {
        // 标识是否需要隐藏弹出窗口
        public bool _needHide;
        // 标识是否隐藏前一个窗口
        public bool isHidePrev;
        // 前一个窗口的名称
        public string HidePrevName = string.Empty;
        public string uiname;
        // 弹出窗口的根锚点
        public Transform anchor;
        // 标识UI是否由类名创建
        public bool isCreateByClassName;
        // 标识是否需要排除其他窗口
        public bool needExclude;

        public Action UpdateAction;

        public void CloseUI(object arg)
        {
            _needHide = false;
            if (uiname != null)
            {
                Singleton<MainUIManager>.Instance.ClosePopUpWindows(this, arg, this.uiname, true);
                Singleton<MainUIManager>.Instance.BlockKeyEvent(true);
            }
        }

        // 自动关闭弹出窗口，接受一个可选参数arg，用于传递额外的信息
        public virtual void CloseUIAuto(object arg)
        {
            _needHide = false;
            if (uiname != null)
            {
                //Singleton<MainUIManager>.Instance.ClosePopUpWindows(this, arg, this.uiname, false);
            }
        }

        public virtual void DestroyUI(object arg)
        {
        }

        public virtual void HideUI(object arg)
        {
            _needHide = true;
        }

        public virtual void OpenUI(object arg)
        {
            Singleton<MainUIManager>.Instance.BlockKeyEvent(true);
        }

        public virtual void ReopenUI(object arg)
        {
            Singleton<MainUIManager>.Instance.BlockKeyEvent(true);
            _needHide = false;
        }

        // 处理摇杆事件，始终返回true
        public virtual bool DealAxisStick()
        {
            return true;
        }

        /// <summary>
        /// 处理按键事件，检查是否按下了特定按键
        /// </summary>
        /// <param name="keyCode">KeyEvent，1开始是ok，2是cencel</param>
        /// <param name="keyState">GAMEKEY_STATE,不变，按下，释放，持续</param>
        /// <returns></returns>
        public virtual bool DealKeyEvent(int keyCode, int keyState)
        {
            if ((keyCode == 2 || keyCode == 2002) && keyState == 2)
            {
                //UIButtonTools.onButtonCancel();
                this.CloseUI(null);
                return true;
            }
            //return CommonHelper.CheckPressAnyKey(keyCode, keyState);
            return false;
        }

        // 是否阻止按键事件，始终返回true
        public virtual bool IsBlockKeyEvent()
        {
            return true;
        }

        // 处理按键事件之前，始终返回false
        public virtual bool BeforeDealKeyEvent()
        {
            return false;
        }

        // 处理按键事件之后，重置_needHide并返回false
        public virtual bool AfterDealKeyEvent()
        {
            this._needHide = false;
            return false;
        }
    }

}