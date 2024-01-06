using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lib.keyEvent;
using UnityEngine.EventSystems;
using System;
using lib.notify;
using UnityEngine.XR;

namespace Game.Client
{
    public class UILogic : IGameKeyEventListener
    {
        protected bool _blockKeyEvent; // 用于表示是否阻止按键事件的布尔值
        protected GameObject parent; // 用于存储父级GameObject的引用
        protected List<PopManager> popList = new List<PopManager>(); // 用于存储弹出窗口的列表
        protected Stack<MenuInfo> menuList = new Stack<MenuInfo>(); // 用于存储菜单信息的栈
        protected Queue<MenuPopProp> popMenuCache = new Queue<MenuPopProp>(); // 用于存储菜单弹出属性的队列
        protected NUIManager _destoryedUI; // 用于存储已销毁的UI管理器
        protected PopManager _destoryedPop; // 用于存储已销毁的弹出窗口管理器

        public UILogic()
        {
            KeyEvent.Register(this);
        }

        public int getPriority()
        {
            return int.MaxValue;
        }

        public bool onGameAxisStickValueChanged(int axisStick, float angle, float radii, bool hasBeenReceived)
        {
            throw new System.NotImplementedException();
        }

        // 当游戏按键状态改变时的事件处理
        public bool onGameKeyStateChanged(int gameKey, GAMEKEY_STATE keyState, bool hasBeenReceived)
        {
            if (hasBeenReceived)
            {
                return hasBeenReceived;
            }
            if (gameKey == 3 || gameKey == 4 || gameKey == 5 || gameKey == 6 || gameKey == 18 || gameKey == 19 || gameKey == 20 || gameKey == 21)
            {
                this._blockKeyEvent = false;
            }
            bool isMouseKey = gameKey == 2001 || gameKey == 2002;
            if (this.popList.Count > 0)
            {
                int i = this.popList.Count - 1;
                while (i >= 0)
                {
                    PopManager popManager = this.popList[i];
                    if (!popManager.BeforeDealKeyEvent())
                    {
                        if (this._blockKeyEvent)
                        {
                            this._blockKeyEvent = false;
                            return popManager.IsBlockKeyEvent();
                        }
                        bool flag = popManager.DealKeyEvent(gameKey, (int)keyState);
                        popManager.AfterDealKeyEvent();
                        this._destoryedPop = null;
                        this._destoryedUI = null;
                        this._blockKeyEvent = false;
                        return flag | this.CheckOnUI(isMouseKey);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            if (this.menuList.Count > 0)
            {
                MenuInfo menuInfo = this.menuList.Peek();
                if (!menuInfo.mgr.BeforeDealKeyEvent())
                {
                    if (this._blockKeyEvent)
                    {
                        this._blockKeyEvent = false;
                        return menuInfo.mgr.IsBlockKeyEvent();
                    }
                    bool flag2 = menuInfo.mgr.DealKeyEvent(gameKey, (int)keyState);
                    menuInfo.mgr.AfterDealKeyEvent();
                    this._destoryedPop = null;
                    this._destoryedUI = null;
                    this._blockKeyEvent = false;
                    return flag2 | this.CheckOnUI(isMouseKey);
                }
            }
            this._blockKeyEvent = false;
            this._destoryedPop = null;
            this._destoryedUI = null;
            return this.CheckOnUI(isMouseKey);
        }

        // 检查鼠标键是否在用户界面上
        protected bool CheckOnUI(bool isMouseKey)
        {
            return isMouseKey && InteractWithUI();
        }

        bool InteractWithUI()
        {
            //EventSystem.current.currentSelectedGameObject 只作用于按钮 IsPointerOverGameObject 作用于全部
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            return false;
        }

        // 是否阻止按键事件
        public void BlockKeyEvent(bool isBlock)
        {
            this._blockKeyEvent = isBlock;
        }

        // 检查按键事件是否被阻止
        public bool IsBlockKeyEvent()
        {
            return this._blockKeyEvent;
        }

        // 检查指定名称的窗口是否已经打开
        protected bool CheckWindowsAlreadyOpen(string uiname)
        {
            foreach (MenuInfo menuInfo in this.menuList)
            {
                if (menuInfo.uiname.Equals(uiname))
                {
                    return true;
                }
            }
            return false;
        }

        // 关闭指定名称的窗口并重新打开顶部窗口
        public void PopWindows(string uiname, object arg, bool isReopen = true, bool isNeedAfterDeal = true)
        {
            if (!this.CheckWindowsAlreadyOpen(uiname))
            {
                return;
            }
            while (this.menuList.Count > 0)
            {
                MenuInfo menuInfo = this.menuList.Pop();
                if (menuInfo.uiname.Equals(uiname))
                {
                    menuInfo.mgr.DestroyUI(arg);
                    break;
                }
                menuInfo.mgr.DestroyUI(arg);
            }
            if (isReopen && this.menuList.Count > 0)
            {
                this.menuList.Peek().mgr.ReopenUI(arg);
            }
        }

        // 通过类名打开窗口
        public NUIManager PushWindowsByClassName(string uiname, object arg)
        {
            return this.PushWindowsBase(uiname, arg, true);
        }

        // 打开指定名称的窗口
        protected NUIManager PushWindowsBase(string uiname, object arg, bool useClaseName)
        {
            if (this.parent == null)
            {
                this.parent = GameObject.Find("Anchor center");
            }
            if (this.parent == null)
            {
                return null;
            }
            if (this.CheckWindowsAlreadyOpen(uiname))
            {
                this.PopToCloseWindows(uiname, arg);
            }
            else if (this.menuList.Count > 0)
            {
                this.menuList.Peek().mgr.HideUI(null);
            }
            NUIManager nuimanager;
            if (useClaseName)
            {
                GameObject gameObject = new GameObject();
                Type type = Type.GetType(uiname);
                nuimanager = (NUIManager)gameObject.AddComponent(type);
            }
            else
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(uiname)) as GameObject;
                nuimanager = gameObject.GetComponent<NUIManager>();
            }
            nuimanager.isCreateByClassName = useClaseName;
            nuimanager.anchor = this.parent.transform;
            nuimanager.uiname = uiname;
            nuimanager.OpenUI(arg);
            MenuInfo t = new MenuInfo(uiname, nuimanager);
            this.menuList.Push(t);
            return nuimanager;
        }


        // 通过类名创建弹出窗口
        public PopManager CreatePopUpWindowsByClassName(string uiname, object arg, bool isInQueue = false, bool ignoreSame = false, bool isHidePrev = false, bool isExcludePop = false)
        {
            return this.CreatePopUpWindowsBase(uiname, arg, isInQueue, ignoreSame, isHidePrev, true, isExcludePop);
        }

        public PopManager CreatePopUpWindowsBase(string uiname, object arg, bool isInQueue = false, bool ignoreSame = false, bool isHidePrev = false, bool useClassName = false, bool isExcludePop = false)
        {
            if (this.parent == null)
            {
                this.parent = GameObject.Find("PopWindow");
            }
            if (this.parent == null)
            {
                return null;
            }
            if (isInQueue)
            {
                MenuPopProp item = new MenuPopProp(uiname, arg, false);
                item.isCreateByClassName = useClassName;
                if (this.popMenuCache.Count > 0)
                {
                    this.popMenuCache.Enqueue(item);
                    return null;
                }
                this.popMenuCache.Enqueue(item);
            }
            if (!ignoreSame)
            {
                PopManager popManager = this.CheckAlreadyPopOpen(uiname);
                if (popManager != null)
                {
                    popManager.ReopenUI(arg);
                    return popManager;
                }
            }
            PopManager popManager2;
            if (useClassName)
            {
                GameObject gameObject = new GameObject();
                Type type = Type.GetType(uiname);
                popManager2 = (PopManager)gameObject.AddComponent(type);
            }
            else
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(uiname)) as GameObject;
                popManager2 = gameObject.GetComponent<PopManager>();
            }
            popManager2.isCreateByClassName = useClassName;
            popManager2.anchor = this.parent.transform;
            popManager2.uiname = uiname;
            popManager2.isHidePrev = isHidePrev;
            popManager2.OpenUI(arg);
            popManager2.needExclude = isExcludePop;
            if (isHidePrev && this.popList.Count > 0)
            {
                for (int i = this.popList.Count - 1; i >= 0; i--)
                {
                    if (!this.popList[i].uiname.Equals("Game.Client.PopBlockKeyManager"))
                    {
                        this.popList[i].HideUI(null);
                        popManager2.HidePrevName = this.popList[i].uiname;
                        break;
                    }
                }
            }
            this.popList.Add(popManager2);
            return popManager2;
        }

        //检查并处理已打开的弹出窗口，以确保每个窗口的唯一性，并提供了一种关闭已打开窗口的方式
        private PopManager CheckAlreadyPopOpen(string name)
        {
            int num = 0; // 创建一个整数变量num，用于跟踪迭代次数
            foreach (PopManager popManager in this.popList)
            {
                if (popManager.uiname.Equals(name)) // 如果popManager对象的uiname属性与传入的name参数相等
                {
                    if (num == this.popList.Count - 1) // 如果当前迭代是popList的最后一个元素
                    {
                        return popManager; // 返回匹配的PopManager对象
                    }
                    this.popList.Remove(popManager); // 从popList中移除匹配的PopManager对象
                    popManager.DestroyUI(null); // 销毁UI对象
                    return null; // 返回null表示未找到匹配的PopManager对象
                }
                else
                {
                    num++; // 增加num以跟踪迭代次数
                }
            }
            return null; // 如果未找到匹配的PopManager对象，返回null
        }

        // 通过名称关闭弹出窗口
        public void ClosePopUpWindowsByName(string uiname, object arg, bool isNeedAfterDeal = true)
        {
            foreach (PopManager popManager in this.popList)
            {
                if (popManager.uiname.Equals(uiname))
                {
                    this.ClosePopUpWindows(popManager, arg, uiname, isNeedAfterDeal);
                    break;
                }
            }
        }

        // 关闭直到指定名称的窗口
        protected void PopToCloseWindows(string uiname, object arg)
        {
            while (this.menuList.Count > 0)
            {
                MenuInfo menuInfo = this.menuList.Peek();
                if (menuInfo.uiname.Equals(uiname))
                {
                    menuInfo.mgr.DestroyUI(arg);
                    this.menuList.Pop();
                    break;
                }
                menuInfo.mgr.DestroyUI(arg);
                this.menuList.Pop();
            }
        }

        // 关闭指定弹出窗口
        public void ClosePopUpWindows(PopManager mgr, object arg, string uiname, bool isNeedAfterDeal = true)
        {
            foreach (PopManager popManager in this.popList)
            {
                if (popManager == mgr)
                {
                    this.popList.Remove(popManager);
                    popManager.DestroyUI(arg);
                    Notifier.Notify(10102, new object[]
                    {
                        popManager
                    });
                    break;
                }
            }
            if (mgr.isHidePrev && this.popList.Count > 0)
            {
                for (int i = this.popList.Count - 1; i >= 0; i--)
                {
                    if (this.popList[i].uiname.Equals(mgr.HidePrevName))
                    {
                        this.popList[i].ReopenUI(null);
                        break;
                    }
                }
            }
            if (this.popMenuCache.Count > 0 && this.popMenuCache.Peek().uiname.Equals(uiname))
            {
                this.popMenuCache.Dequeue();
                if (this.popMenuCache.Count > 0)
                {
                    MenuPopProp menuPopProp = this.popMenuCache.Peek();
                    if (menuPopProp.isCreateByClassName)
                    {
                        this.CreatePopUpWindowsByClassName(menuPopProp.uiname, menuPopProp.arg, false, false, false, false);
                    }
                    else
                    {
                        this.CreatePopUpWindows(menuPopProp.uiname, menuPopProp.arg, false, false, false, false);
                    }
                }
            }
        }

        // 创建弹出窗口
        public PopManager CreatePopUpWindows(string uiname, object arg, bool isInQueue = false, bool ignoreSame = false, bool isHidePrev = false, bool isExcludePop = false)
        {
            return this.CreatePopUpWindowsBase(uiname, arg, isInQueue, ignoreSame, isHidePrev, false, isExcludePop);
        }

    }
}