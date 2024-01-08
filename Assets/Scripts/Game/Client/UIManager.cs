using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Game.Client
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager INSTANCE;
        private bool _isOpening;
        private Dictionary<string, AssetInfo> assetInfo;
        private Dictionary<string, UIManager.BundleCounter> counts;
        private Dictionary<string, GameObject> cachedWindows;

        public bool isLoaded { get; private set; }

        private UIManager()
        {
        }

        public static UIManager GetInstance()
        {
            if (UIManager.INSTANCE == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "_UIManager";
                gameObject.AddComponent<UIManager>();
                UIManager.INSTANCE = gameObject.GetComponent<UIManager>();
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
            return UIManager.INSTANCE;
        }

        public void Initialize()
        {
            if (!this.isLoaded)
            {
                this.counts = new Dictionary<string, UIManager.BundleCounter>();
                this.assetInfo = new Dictionary<string, UIManager.AssetInfo>();
                //this.onOpens = new List<OnOpenUI>();
                //this.onOpenCompletes = new List<OnOpenComplete>();
                //this.onCloses = new List<OnCloseUI>();
                //this.onCloseCompletes = new List<OnCloseComplete>();
                //this.isLoaded = false;
                //AssetBundleLoader.getInstance().loadAssetBundle("uiconfig", new AssetBundleLoadDone(this.onConfigLoaded), false, ".assetbundle");
                this._isOpening = false;
                this.cachedWindows = new Dictionary<string, GameObject>();

                onConfigLoaded();
            }
        }

        private void onConfigLoaded()
        {
            this.isLoaded = true;
            this.assetInfo = new Dictionary<string, UIManager.AssetInfo>();

            TextAsset textAsset = Resources.Load<TextAsset>("GameData/UIConfig");
            XmlDocument uiXML = new XmlDocument();
            uiXML.LoadXml(textAsset.text);
            XmlElement documentElement = uiXML.DocumentElement;
            XmlNodeList xmlNodeList = documentElement.SelectNodes("UI");

            foreach (XmlNode node in xmlNodeList)
            {
                UIManager.AssetInfo assetInfo = new UIManager.AssetInfo();
                string uiName = node.Attributes["uiname"].Value;
                assetInfo.assetBundleName = node.Attributes["assetName"].Value;
                assetInfo.gameObjectName = node.Attributes["goname"].Value;
                assetInfo.loadType = (UIManager.LoadType)int.Parse(node.Attributes["type"].Value);

                if (!this.assetInfo.ContainsKey(uiName))
                {
                    this.assetInfo.Add(uiName, assetInfo);
                }
                else
                {
                    Debug.LogError("Duplicated UI name: " + uiName);
                }
            }
        }

        public void OpenUI(string[] uiName, OnOpenComplete complete, OnInstantiateOver[] onInstOvers = null, Transform parent = null, bool cache = true)
        {
            if (this.isLoaded)
            {
                if (onInstOvers == null)
                {
                    onInstOvers = new OnInstantiateOver[uiName.Length];
                }
                else if (onInstOvers.Length != uiName.Length)
                {
                    Debug.LogError("uiname's length not compatible with onInstOvers");
                    return;
                }
                for (int i = 0; i < onInstOvers.Length; i++)
                {
                    OnInstantiateOver onInsOver = onInstOvers[i];
                    onInstOvers[i] = delegate (GameObject window)
                    {
                        try
                        {
                            onInsOver(window);
                        }
                        catch (Exception message)
                        {
                            Debug.Log(message);
                        }
                    };
                }
                bool flag = false;
                foreach (string key in uiName)
                {
                    if (!this.assetInfo.ContainsKey(key))
                    {
                        flag = true;
                    }
                }
                if (!flag)
                {
                    this.DisableCameraTouch();
                    this._isOpening = true;
                    UIManager.LoadCounter loadCounter = new UIManager.LoadCounter();
                    loadCounter.wrappers.Add(new UIManager.OnceCalledWrapper(complete));
                    loadCounter.wrappers.Add(new UIManager.AlwaysCalledWrapper(new OnOpenComplete(this.uiOpenOver)));
                    loadCounter.SetCount(uiName.Length);
                    int num = 0;
                    foreach (string text in uiName)
                    {
                        Debug.Log(text);
                        //if (this.cachedWindows.ContainsKey(text) && this.cachedWindows[text] != null)
                        //{
                        //    base.StartCoroutine(this.waitForOther(this.cachedWindows[text], loadCounter, parent, onInstOvers[num]));
                        //}
                        //else
                        {
                            UIManager.AssetInfo assetInfo = this.assetInfo[text];
                            if (assetInfo.loadType == UIManager.LoadType.ASSETBUNDLE)
                            {
                                Debug.Log("还没弄");
                                //LoadedOverHandler @object = new LoadedOverHandler(assetInfo.gameObjectName, this, assetInfo.assetBundleName, onInstOvers[num], text, cache, parent, loadCounter);
                                //AssetBundleLoader.getInstance().loadAssetBundle(assetInfo.assetBundleName, new AssetBundleLoadDone(@object.onLoadedOver), false, ".assetbundle");
                            }
                            else if (assetInfo.loadType == UIManager.LoadType.RESOURCES)
                            {
                                LoadedOverHandler loadedOverHandler = new LoadedOverHandler(assetInfo.gameObjectName, this, assetInfo.assetBundleName, onInstOvers[num], text, cache, parent, loadCounter);
                                loadedOverHandler.onLoadedOver(null);
                            }
                        }
                        num++;
                    }
                }
                else
                {
                    Debug.LogError("none exist GUI");
                }
            }
        }

        private void uiOpenOver(GameObject obj)
        {
            //OnOpenComplete[] array = this.onOpenCompletes.ToArray();
            //this.RestoreCameraTouch();
            //foreach (OnOpenComplete onOpenComplete in array)
            //{
            //    if (onOpenComplete != null)
            //    {
            //        onOpenComplete(obj);
            //    }
            //}
            //this._isOpening = false;
        }

        private void DisableCameraTouch()
        {
            //UICamera camera = this.GetCamera();
            //if (camera != null)
            //{
            //    CameraManager.GetInstance().PushTouchDisable();
            //    this._cameraDisableTimes++;
            //    this._cameraDisableTime = Time.time;
            //}
        }

        private class BundleCounter
        {
            public AssetBundle bundle;
            public int count;
        }

        private class LoadedOverHandler
        {
            private string gameObjectName;
            private string windowName;
            private UIManager owner;
            private string bundlePath;
            private bool needCache;
            public Transform parent;
            private LoadCounter counter;
            private OnInstantiateOver over;

            public LoadedOverHandler(string gameObjName, UIManager owner, string bundlePath, OnInstantiateOver onOver, string windowName, bool needCache, Transform parent = null, UIManager.LoadCounter counter = null)
            {
                this.needCache = needCache;
                this.gameObjectName = gameObjName;
                this.owner = owner;
                this.windowName = windowName;
                this.bundlePath = bundlePath;
                if (counter == null)
                {
                    this.counter = new UIManager.LoadCounter();
                    counter.SetCount(1);
                }
                else
                {
                    this.counter = counter;
                }
                this.parent = parent;
                this.over = onOver;
            }

            public void onLoadedOver(AssetBundle assetbundle)
            {
                if (bundlePath != null && owner.counts.ContainsKey(bundlePath))
                {
                    owner.counts[bundlePath].count++;
                }
                else if (bundlePath != null)
                {
                    UIManager.BundleCounter bundleCounter = new UIManager.BundleCounter();
                    bundleCounter.bundle = assetbundle;
                    bundleCounter.count = 1;
                    owner.counts.Add(bundlePath, bundleCounter);
                }
                GameObject gameObject;
                if (assetbundle != null)
                {
                    gameObject = Instantiate(assetbundle.LoadAsset(gameObjectName)) as GameObject;
                }
                else
                {
                    Debug.Log(gameObjectName);
                    gameObject = Instantiate(Resources.Load(gameObjectName)) as GameObject;
                }
                owner.AssignRebindScript(gameObject);
                gameObject.transform.SetParent(parent, false);
                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                UIComponent component = gameObject.GetComponent<UIComponent>();
                component.Init();
                component.uiName = bundlePath;
                if (over != null)
                {
                    over(gameObject);
                }
                component.AddOnOpenComplete(new OnOpenComplete(counter.CountDown));
                component.Open();
                component.needCache = needCache;
                if (needCache)
                {
                    if (owner.cachedWindows.ContainsKey(windowName))
                    {
                        owner.cachedWindows[windowName] = component.gameObject;
                    }
                    else
                    {
                        owner.cachedWindows.Add(windowName, component.gameObject);
                    }
                }
            }
        }

        private class OnceCalledWrapper : UIManager.CallWrapper
        {
            private int callCount;
            private OnOpenComplete onOpenComplete;

            public OnceCalledWrapper(OnOpenComplete onOpenCmplt)
            {
                this.onOpenComplete = onOpenCmplt;
                this.callCount = 1;
            }

            public void Call(GameObject go)
            {
                callCount--;
                if (callCount == 0 && onOpenComplete != null)
                {
                    onOpenComplete(go);
                }
            }
        }

        private class AlwaysCalledWrapper : UIManager.CallWrapper
        {
            private OnOpenComplete onOpenComplete;

            public AlwaysCalledWrapper(OnOpenComplete onOpenCmplt)
            {
                this.onOpenComplete = onOpenCmplt;
            }

            public void Call(GameObject go)
            {
                try
                {
                    if (onOpenComplete != null)
                    {
                        onOpenComplete(go);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex + "error");
                }
            }
        }

        private void AssignRebindScript(GameObject windowRoot)
        {
        }

        private class LoadCounter
        {
            private int LoadCount;
            private int CallCountdown;
            public List<UIManager.CallWrapper> wrappers = new List<UIManager.CallWrapper>();
            public List<GameObject> calledObjects = new List<GameObject>();

            public void SetCount(int count)
            {
                this.LoadCount = count;
                this.CallCountdown = count;
            }

            public int Count
            {
                get
                {
                    return this.LoadCount;
                }
            }

            public void LoadCountDown()
            {
                this.LoadCount--;
            }

            public void CountDown(GameObject go)
            {
                if (!this.calledObjects.Contains(go))
                {
                    this.calledObjects.Add(go);
                }
                this.CallCountdown--;
                if (this.CallCountdown == 0)
                {
                    foreach (GameObject go2 in this.calledObjects)
                    {
                        foreach (UIManager.CallWrapper callWrapper in this.wrappers)
                        {
                            callWrapper.Call(go2);
                        }
                    }
                }
            }
        }

        private interface CallWrapper
        {
            void Call(GameObject go);
        }

        private enum LoadType
        {
            ASSETBUNDLE,
            RESOURCES
        }

        private class AssetInfo
        {
            public string assetBundleName;
            public string gameObjectName;
            public LoadType loadType;
        }
    }
}