using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Video;

namespace Game.Client
{
    public class AssetBundleLoader : MonoBehaviour
    {
        private void Init()
        {
            this.nameToPath = new Dictionary<string, string>();
            this.loadedBundles = new Dictionary<string, AssetBundle>();
            this.onDones = new Dictionary<string, List<AssetBundleLoadDone>>();
            this.dontDestroyBundle = new HashSet<string>();
            this.scanAssetBundle();
        }

        public void scanAssetBundle()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
            FileInfo[] files = directoryInfo.GetFiles();
            this.nameToPath.Clear();
            foreach (FileInfo fileInfo in files)
            {
                if (fileInfo.Name.EndsWith(".assetbundle"))
                {
                    string key = fileInfo.Name.Substring(0, fileInfo.Name.Length - ".assetbundle".Length);
                    string fullName = fileInfo.FullName;
                    Debug.Log(fullName);
                    if (!this.nameToPath.ContainsKey(key))
                    {
                        this.nameToPath.Add(key, fullName);
                    }
                    else
                    {
                        this.nameToPath[key] = fullName;
                    }
                }
                else
                {
                    string name = fileInfo.Name;
                    string fullName2 = fileInfo.FullName;
                    Debug.Log(fullName2);
                    if (!this.nameToPath.ContainsKey(name))
                    {
                        this.nameToPath.Add(name, fullName2);
                    }
                }
            }
        }

        public static AssetBundleLoader getInstance()
        {
            if (bundleLoader == null || LOADER == null)
            {
                bundleLoader = new GameObject();
                bundleLoader.AddComponent<AssetBundleLoader>();
                LOADER = bundleLoader.GetComponent<AssetBundleLoader>();
                LOADER.Init();
                DontDestroyOnLoad(bundleLoader);
            }
            return LOADER;
        }

        public void setDontDestroy(string bundleName)
        {
            if (this.dontDestroyBundle != null)
            {
                this.dontDestroyBundle.Add(bundleName);
            }
        }

        public void removeDontDestroy(string bundleName)
        {
            if (this.dontDestroyBundle != null)
            {
                this.dontDestroyBundle.Remove(bundleName);
            }
        }

        public void SetDontDestroyOnSceneSwitch(AssetBundle assetbundle)
        {
            if (this.dontDestroyOnSceneSwitch != null)
            {
                this.dontDestroyOnSceneSwitch.Add(assetbundle);
            }
        }

        public void AddToUnloadBatch(AssetBundle assetbundle)
        {
            this.unloadBatch.Add(assetbundle);
        }

        public void ClearUnloadBatch()
        {
            foreach (AssetBundle assetBundle in this.unloadBatch)
            {
                if (assetBundle != null)
                {
                    this.unloadAssetBundle(assetBundle.name, false);
                }
            }
        }

        public void RemoveDontDestroyOnSceneSwitch(AssetBundle assetbundle)
        {
            if (this.dontDestroyOnSceneSwitch != null)
            {
                this.dontDestroyOnSceneSwitch.Remove(assetbundle);
            }
        }

        public void unloadAssetBundle(string name, bool unloadLoaded = true)
        {
            name = this.formatAssetBundleFilename(name);
            if (this.loadedBundles.ContainsKey(name) && !this.onDones.ContainsKey(name) && !this.dontDestroyBundle.Contains(name))
            {
                this.loadedBundles[name].Unload(unloadLoaded);
                this.loadedBundles.Remove(name);
            }
        }

        public void loadAssetBundleAsyn(string name, AssetBundleLoadDone onDone, bool useLocalCache = false, string suffix = ".assetbundle")
        {
            this.loadAssetBundle(name, onDone, useLocalCache, true, suffix);
        }

        public void loadAssetBundle(string name, AssetBundleLoadDone onDone, bool useLocalCache = false, string suffix = ".assetbundle")
        {
            this.loadAssetBundle(name, onDone, useLocalCache, false, suffix);
        }

        private string formatAssetBundleFilename(string name)
        {
            string fileName = Path.GetFileName(name);
            string directoryName = Path.GetDirectoryName(name);
            if (string.IsNullOrEmpty(directoryName))
            {
                return fileName.ToLower();
            }
            return directoryName + Path.DirectorySeparatorChar + fileName.ToLower();
        }

        public void loadAssetBundle(string name, AssetBundleLoadDone onDone, bool useLocalCache, bool isAsyn, string suffix = ".assetbundle")
        {
            name = this.formatAssetBundleFilename(name);
            if (this.loadedBundles.ContainsKey(name) && this.loadedBundles[name] != null)
            {
                if (onDone != null)
                {
                    onDone(this.loadedBundles[name]);
                }
            }
            else
            {
                if (!this.onDones.ContainsKey(name))
                {
                    this.onDones.Add(name, new List<AssetBundleLoadDone>());
                }
                if (onDone != null)
                {
                    if (this.onDones[name].Contains(onDone))
                    {
                        return;
                    }
                    this.onDones[name].Add(onDone);
                    if (this.onDones[name].Count > 1)
                    {
                        return;
                    }
                }
                if (isAsyn)
                {
                    string loadPath = this.getLoadPath(name, true, suffix);
                    WWW url;
                    if (useLocalCache)
                    {
                        url = WWW.LoadFromCacheOrDownload(loadPath, 0);
                    }
                    else
                    {
                        url = new WWW(loadPath);
                    }
                    AssetBundleLoader.LoadTask loadTask = new AssetBundleLoader.LoadTask();
                    loadTask.url = url;
                    loadTask.name = name;
                    base.StartCoroutine(this.waitUtilDone(url, name));
                }
                else
                {
                    string loadPath = this.getLoadPath(name, false, suffix);
                    try
                    {
                        AssetBundle bundle = AssetBundle.LoadFromFile(loadPath);
                        this.DealLoadedAssetbundle(bundle, name);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("load bundle error: " + loadPath);
                        Debug.LogError("load bundle error stack: " + ex.Message);
                    }
                }
            }
        }

        public string getLoadPath(string filename, bool isAsyn, string suffix)
        {
            string text;
            if (isAsyn)
            {
                if (this.nameToPath.ContainsKey(filename))
                {
                    text = AssetBundleLoader.LocalPath_prefix + this.nameToPath[filename];
                    Debug.Log("path : " + text);
                }
                else
                {
                    text = AssetBundleLoader.StreamingAssetsPath_prefix + AssetBundleLoader.StreamingAssetsAsynPathURL + filename + suffix;
                }
            }
            else if (this.nameToPath.ContainsKey(filename))
            {
                text = this.nameToPath[filename];
                Debug.Log("path : " + text);
            }
            else
            {
                text = AssetBundleLoader.StreamingAssetsPathURL + filename + suffix;
            }
            return text;
        }

        public void loadAssetText(string name, Action<object> onDone)
        {
            string url = AssetBundleLoader.StreamingAssetsPath_prefix + AssetBundleLoader.StreamingAssetsAsynPathURL + name;
            base.StartCoroutine(this.waitLoadAssetTextDone(url, onDone));
        }

        private IEnumerator waitLoadAssetTextDone(string url, Action<object> onDone)
        {
            WWW www = new WWW(url);
            yield return www;
            onDone(www.text);
            yield break;
        }

        public void loadAssetMovie(string name, Action<VideoPlayer> onDone)
        {
            string url = AssetBundleLoader.StreamingAssetsPathURL + name;
            base.StartCoroutine(this.WaitLoadAssetVideoDone(url, onDone));
        }

        private IEnumerator WaitLoadAssetVideoDone(string url, Action<VideoPlayer> onDone)
        {
            using (var www = new WWW(url))
            {
                yield return www;
                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError("Error: " + www.error);
                    yield break;
                }

                // 创建一个新的VideoPlayer组件
                var videoPlayer = gameObject.AddComponent<VideoPlayer>();

                // 指定视频源
                videoPlayer.url = url;

                // 等待视频准备好
                while (!videoPlayer.isPrepared)
                {
                    yield return null;
                }

                // 触发完成回调
                onDone(videoPlayer);
            }
        }

        private IEnumerator waitUtilDone(WWW url, string name)
        {
            while (url != null && !url.isDone)
            {
                yield return new WaitForSeconds(0.1f);
            }
            if (!this.onDones.ContainsKey(name))
            {
                yield break;
            }
            AssetBundle bundle = null;
            if (!string.IsNullOrEmpty(url.error))
            {
                Debug.Log("load " + name + " error! error code : " + url.error);
            }
            else
            {
                bundle = url.assetBundle;
            }
            Debug.Log(name + " loaded over: " + DateTime.Now.Ticks);
            this.DealLoadedAssetbundle(bundle, name);
            yield break;
        }

        private void DealLoadedAssetbundle(AssetBundle bundle, string name)
        {
            if (bundle != null)
            {
                if (!this.loadedBundles.ContainsKey(name))
                {
                    this.loadedBundles.Add(name, bundle);
                }
                else
                {
                    this.loadedBundles[name] = bundle;
                }
            }
            if (this.loadedBundles.ContainsKey(name))
            {
                bundle = this.loadedBundles[name];
            }
            if (this.onDones.ContainsKey(name))
            {
                foreach (AssetBundleLoadDone assetBundleLoadDone in this.onDones[name])
                {
                    assetBundleLoadDone(bundle);
                }
            }
            this.onDones.Remove(name);
        }

        public void UnloadAll(bool ignoreAll = false)
        {
            this.dontDestroyBundle.Clear();
            foreach (AssetBundle assetBundle in this.loadedBundles.Values)
            {
                if ((!this.dontDestroyOnSceneSwitch.Contains(assetBundle) || ignoreAll) && assetBundle != null)
                {
                    assetBundle.Unload(true);
                }
            }
            this.loadedBundles.Clear();
        }
        private static GameObject bundleLoader;
        private static AssetBundleLoader LOADER;
        public const string SUFFIX_BUNDLE = ".assetbundle";
        private Dictionary<string, string> nameToPath;
        private Dictionary<string, AssetBundle> loadedBundles;
        private Dictionary<string, List<AssetBundleLoadDone>> onDones;
        private HashSet<string> dontDestroyBundle;
        private HashSet<AssetBundle> dontDestroyOnSceneSwitch = new HashSet<AssetBundle>();
        private List<LoadTask> tasks = new List<LoadTask>();
        private List<AssetBundle> unloadBatch = new List<AssetBundle>();
        public static string StreamingAssetsPath_prefix = "file://";
        public static readonly string LocalPath_prefix = "file:///";
        public static string StreamingAssetsPathURL = Application.streamingAssetsPath + Path.DirectorySeparatorChar;
        public static string StreamingAssetsAsynPathURL = Application.streamingAssetsPath + Path.DirectorySeparatorChar;

        private class LoadTask
        {
            public string name;
            public WWW url;
        }
    }
}
