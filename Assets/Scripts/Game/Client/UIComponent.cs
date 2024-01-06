using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Game.Client
{
    public abstract class UIComponent : MonoBehaviour
    {
        // UI组件的名称
        private string _uiName;
        // 是否需要缓存UI
        private bool _needCache;
        // 当前UI是否已经打开的标志
        private bool isOpened;
        // 是否存在已完成的回调标志
        private bool hasCompleteCallback;
        // 存储UI关闭时的事件回调列表
        //public List<EventDelegate> onClosed;
        // 存储UI打开时的事件回调列表
        //public List<EventDelegate> onOpened;
        // 存储额外的组件（外部引用）
        public List<GameObject> exComponents;
        // 存储打开完成时的回调委托列表
        protected List<OnOpenComplete> openCompleteCallbacks = new List<OnOpenComplete>();
        // 存储关闭完成时的回调委托列表
        protected List<Action> closeCompleteCallbacks = new List<Action>();

        // 无参数构造函数
        public UIComponent()
        {
        }

        // UI组件的名称属性
        public string uiName
        {
            get
            {
                return this._uiName;
            }
            set
            {
                this._uiName = value;
            }
        }

        // 是否需要缓存UI属性
        public bool needCache
        {
            get
            {
                return this._needCache;
            }
            set
            {
                this._needCache = value;
            }
        }

        // 初始化UI组件
        public virtual void Init()
        {
            //if (this.onOpened != null)
            //{
            //    foreach (EventDelegate eventDelegate in this.onOpened)
            //    {
            //        eventDelegate.Execute();
            //    }
            //}
        }

        // 添加UI打开完成时的回调委托
        public void AddOnOpenComplete(OnOpenComplete callWhenOpenComplete)
        {
            if (callWhenOpenComplete != null && !this.openCompleteCallbacks.Contains(callWhenOpenComplete))
            {
                this.openCompleteCallbacks.Add(callWhenOpenComplete);
            }
        }

        // UI关闭完成时的回调方法
        public virtual void OnCloseComplete()
        {
            if (!this.needCache)
            {
                //UIManager.GetInstance().ReleaseUIBundle(this.uiName);
                UnityEngine.Object.Destroy(base.transform.gameObject);
            }
            else
            {
                //UIManager.GetInstance().ReleaseUIMaterials(base.transform);
            }
            foreach (Action action in this.closeCompleteCallbacks)
            {
                if (action != null)
                {
                    action();
                }
            }
            this.closeCompleteCallbacks.Clear();
            Singleton<MainUIManager>.Instance.ClosePopUpWindowsByName("Game.Client.PopBlockKeyManager", null, true);
            this.hasCompleteCallback = false;
        }

        // 打开UI组件
        public virtual void Open()
        {
            this.isOpened = false;
            this.hasCompleteCallback = false;
            Debug.Log(base.transform.name + " call open");
            if (!this.hasCompleteCallback)
            {
                Debug.Log("force call on open complete");
                this.OnOpenComplete();
            }
        }

        public virtual void OnOpenComplete()
        {
            //foreach (OnOpenComplete onOpenComplete in this.openCompleteCallbacks)
            //{
            //    if (onOpenComplete != null)
            //    {
            //        onOpenComplete(base.gameObject);
            //    }
            //}
            //this.openCompleteCallbacks.Clear();
            this.isOpened = true;
        }
    }
}
