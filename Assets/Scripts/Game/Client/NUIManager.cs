using System;
using System.Collections.Generic;
//using FranceGame.Plugins;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Game.Client
{
    // 用于管理UI的类，继承自MonoBehaviour和IUIManager接口
    public class NUIManager : MonoBehaviour, IUIManager
    {
        // 存储子UI的字典，键为UI的唯一名称，值为PopManager类型的子UI对象
        private Dictionary<string, PopManager> children = new Dictionary<string, PopManager>();

        // UI的根锚点
        public Transform anchor;

        // UI的唯一名称
        public string uiname;

        // 标识是否存在UI背景
        protected bool isHasBG;

        // 标识是否需要隐藏UI
        public bool _needHide;

        // 标识UI是否由类名创建
        public bool isCreateByClassName;

        // 向子UI字典中添加子UI
        public void AddUIChild(string key, PopManager child)
        {
            if (this.children.ContainsKey(key))
            {
                //Debugger.Log(LogLevel.VERBOSE, "key is already exsit!", new object[0]);
                return;
            }
            this.children.Add(key, child);
        }

        // 从子UI字典中移除指定的子UI
        public void RemoveUIChild(string key)
        {
            if (!this.children.ContainsKey(key))
            {
                return;
            }
            if (this.children[key] != null)
            {
                this.children[key].CloseUI(null);
            }
            this.children.Remove(key);
        }

        // 获取子UI
        public PopManager GetUIChild(string key)
        {
            if (!this.children.ContainsKey(key))
            {
                return null;
            }
            return this.children[key];
        }

        // 关闭UI
        public virtual void CloseUI(object arg)
        {
            this._needHide = false;
            if (this.uiname != null)
            {
                Singleton<MainUIManager>.Instance.PopWindows(this.uiname, arg, true, true);
                Singleton<MainUIManager>.Instance.BlockKeyEvent(true);
                foreach (string key in this.children.Keys)
                {
                    if (!(this.children[key] == null))
                    {
                        this.children[key].CloseUI(arg);
                    }
                }
                this.children.Clear();
            }
        }

        // 仅关闭UI，不隐藏
        public virtual void CloseUIOnly(object arg)
        {
            if (this.uiname != null)
            {
                Singleton<MainUIManager>.Instance.PopWindows(this.uiname, arg, false, true);
                Singleton<MainUIManager>.Instance.BlockKeyEvent(true);
                foreach (string key in this.children.Keys)
                {
                    if (!(this.children[key] == null))
                    {
                        this.children[key].CloseUI(arg);
                    }
                }
                this.children.Clear();
            }
        }

        // 自动关闭UI
        public virtual void CloseUIAuto(object arg)
        {
            if (this.uiname != null)
            {
                Singleton<MainUIManager>.Instance.PopWindows(this.uiname, arg, false, false);
                foreach (string key in this.children.Keys)
                {
                    if (!(this.children[key] == null))
                    {
                        this.children[key].CloseUI(arg);
                    }
                }
                this.children.Clear();
            }
        }

        // 打开UI，暂未实现
        public virtual void OpenUI(object arg)
        {
            Singleton<MainUIManager>.Instance.BlockKeyEvent(true);
            //Debugger.Log(LogLevel.VERBOSE, "open do nothing", new object[0]);
        }

        // 重新打开UI，暂未实现
        public virtual void ReopenUI(object arg)
        {
            Singleton<MainUIManager>.Instance.BlockKeyEvent(true);
            this._needHide = false;
            //Debugger.Log(LogLevel.VERBOSE, "reopen do nothing", new object[0]);
            foreach (string key in this.children.Keys)
            {
                if (!(this.children[key] == null))
                {
                    this.children[key].ReopenUI(arg);
                }
            }
        }

        // 销毁UI，暂未实现
        public virtual void DestroyUI(object arg)
        {
            //Debugger.Log(LogLevel.VERBOSE, "Destroy do nothing", new object[0]);
        }

        // 隐藏UI，暂未实现
        public virtual void HideUI(object arg)
        {
            this._needHide = true;
            //Debugger.Log(LogLevel.VERBOSE, "hide do nothing", new object[0]);
            foreach (string key in this.children.Keys)
            {
                if (!(this.children[key] == null))
                {
                    this.children[key].HideUI(arg);
                }
            }
        }

        // 是否阻止按键事件，始终返回true
        public virtual bool IsBlockKeyEvent()
        {
            return true;
        }

        // 处理摇杆事件，始终返回false
        public virtual bool DealAxisStick()
        {
            return false;
        }

        // 处理按键事件，检查是否按下了任何按键
        public virtual bool DealKeyEvent(int keyCode, int keyState)
        {
            return CommonHelper.CheckPressAnyKey(keyCode, keyState);
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
