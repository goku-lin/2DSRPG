using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace InControl
{
    /// <summary>
    /// InControlManager 是一个用于管理输入控制的单例 MonoBehavior。
    /// </summary>
    public class InControlManager : SingletonMonoBehavior<InControlManager, MonoBehaviour>
    {
        /// <summary>
        /// 当启用对象时调用。
        /// </summary>
        private void OnEnable()
        {
            if (!base.EnforceSingleton())
            {
                return;
            }

            // 设置 InControlManager 的属性
            InputManager.InvertYAxis = this.invertYAxis;
            InputManager.SuspendInBackground = this.suspendInBackground;
            InputManager.EnableICade = this.enableICade;
            InputManager.EnableXInput = this.enableXInput;
            InputManager.XInputUpdateRate = (uint)Mathf.Max(this.xInputUpdateRate, 0);
            InputManager.XInputBufferSize = (uint)Mathf.Max(this.xInputBufferSize, 0);
            InputManager.EnableNativeInput = this.enableNativeInput;
            InputManager.NativeInputEnableXInput = this.nativeInputEnableXInput;
            InputManager.NativeInputUpdateRate = (uint)Mathf.Max(this.nativeInputUpdateRate, 0);
            InputManager.NativeInputPreventSleep = this.nativeInputPreventSleep;

            // 设置日志信息
            if (InputManager.SetupInternal())
            {
                //if (this.logDebugInfo)
                //{
                //    Debug.Log("InControl (version " + InputManager.Version + ")");
                //    if (InControlManager.fmg0 == null)
                //    {
                //        InControlManager.fmg0 = new Action<LogMessage>(InControlManager.LogMessage);
                //    }
                //    Logger.OnLogMessage -= InControlManager.fmg0;
                //    if (InControlManager.fmg1 == null)
                //    {
                //        InControlManager.fmg1 = new Action<LogMessage>(InControlManager.LogMessage);
                //    }
                //    Logger.OnLogMessage += InControlManager.fmg1;
                //}

                // 遍历自定义配置文件并添加到输入管理器
                foreach (string text in this.customProfiles)
                {
                    Type type = Type.GetType(text);
                    if (type == null)
                    {
                        Debug.LogError("Cannot find class for custom profile: " + text);
                    }
                    else
                    {
                        UnityInputDeviceProfileBase unityInputDeviceProfileBase = Activator.CreateInstance(type) as UnityInputDeviceProfileBase;
                        if (unityInputDeviceProfileBase != null)
                        {
                            InputManager.AttachDevice(new UnityInputDevice(unityInputDeviceProfileBase));
                        }
                    }
                }
            }
            SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
            SceneManager.sceneLoaded += this.OnSceneWasLoaded;
            if (this.dontDestroyOnLoad)
            {
                UnityEngine.Object.DontDestroyOnLoad(this);
            }
        }

        /// <summary>
        /// 当禁用对象时调用。
        /// </summary>
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= this.OnSceneWasLoaded;
            if (SingletonMonoBehavior<InControlManager, MonoBehaviour>.Instance == this)
            {
                InputManager.ResetInternal();
            }
        }

        /// <summary>
        /// 更新方法。
        /// </summary>
        private void Update()
        {
            if (!this.useFixedUpdate || Utility.IsZero(Time.timeScale))
            {
                InputManager.UpdateInternal();
            }
        }

        /// <summary>
        /// 固定更新方法。
        /// </summary>
        private void FixedUpdate()
        {
            if (this.useFixedUpdate)
            {
                InputManager.UpdateInternal();
            }
        }

        /// <summary>
        /// 当应用程序焦点发生变化时调用。
        /// </summary>
        private void OnApplicationFocus(bool focusState)
        {
            InputManager.OnApplicationFocus(focusState);
        }

        /// <summary>
        /// 当应用程序暂停时调用。
        /// </summary>
        private void OnApplicationPause(bool pauseState)
        {
            InputManager.OnApplicationPause(pauseState);
        }

        /// <summary>
        /// 当应用程序退出时调用。
        /// </summary>
        private void OnApplicationQuit()
        {
            InputManager.OnApplicationQuit();
        }

        /// <summary>
        /// 当场景被加载时调用。
        /// </summary>
        private void OnSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            InputManager.OnLevelWasLoaded();
        }

        /// <summary>
        /// 日志消息的处理方法。
        /// </summary>
        //private static void LogMessage(LogMessage logMessage)
        //{
        //    LogMessageType type = logMessage.type;
        //    if (type != LogMessageType.Info)
        //    {
        //        if (type != LogMessageType.Warning)
        //        {
        //            if (type == LogMessageType.Error)
        //            {
        //                Debug.LogError(logMessage.text);
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogWarning(logMessage.text);
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log(logMessage.text);
        //    }
        //}

        public bool logDebugInfo;
        public bool invertYAxis;
        public bool useFixedUpdate;
        public bool dontDestroyOnLoad;
        public bool suspendInBackground;
        public bool enableICade;
        public bool enableXInput;
        public bool xInputOverrideUpdateRate;
        public int xInputUpdateRate;
        public bool xInputOverrideBufferSize;
        public int xInputBufferSize;
        public bool enableNativeInput;
        public bool nativeInputEnableXInput = true;
        public bool nativeInputPreventSleep;
        public bool nativeInputOverrideUpdateRate;
        public int nativeInputUpdateRate;
        public List<string> customProfiles = new List<string>();

        //[CompilerGenerated]
        //private static Action<LogMessage> fmg0;

        //[CompilerGenerated]
        //private static Action<LogMessage> fmg1;
    }
}
