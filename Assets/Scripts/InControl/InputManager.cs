using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UnityEngine;

namespace InControl
{
    public class InputManager
    {
        public static readonly VersionInfo Version = VersionInfo.InControlVersion();
        private static List<InputDeviceManager> deviceManagers = new List<InputDeviceManager>();
        private static Dictionary<Type, InputDeviceManager> deviceManagerTable = new Dictionary<Type, InputDeviceManager>();
        private static InputDevice activeDevice = InputDevice.Null;
        private static List<InputDevice> devices = new List<InputDevice>();
        private static List<PlayerActionSet> playerActionSets = new List<PlayerActionSet>();
        public static ReadOnlyCollection<InputDevice> Devices;
        private static bool applicationIsFocused;
        private static float initialTime;
        private static float currentTime;
        private static float lastUpdateTime;
        private static ulong currentTick;
        private static VersionInfo? unityVersion;
        private static bool enabled;

        public static event Action OnSetup;
        public static event Action<ulong, float> OnUpdate;
        public static event Action OnReset;
        public static event Action<InputDevice> OnDeviceAttached;
        public static event Action<InputDevice> OnDeviceDetached;
        public static event Action<InputDevice> OnActiveDeviceChanged;
        internal static event Action<ulong, float> OnUpdateDevices;
        internal static event Action<ulong, float> OnCommitDevices;

        public static bool CommandWasPressed { get; private set; }

        public static bool InvertYAxis { get; set; }

        public static bool IsSetup { get; private set; }

        internal static string Platform { get; private set; }

        internal static bool SetupInternal()
        {
            if (InputManager.IsSetup)
            {
                return false;
            }
            InputManager.Platform = Utility.GetWindowsVersion().ToUpper();
            InputManager.enabled = true;
            InputManager.initialTime = 0f;
            InputManager.currentTime = 0f;
            InputManager.lastUpdateTime = 0f;
            InputManager.currentTick = 0UL;
            InputManager.applicationIsFocused = true;
            InputManager.deviceManagers.Clear();
            InputManager.deviceManagerTable.Clear();
            InputManager.devices.Clear();
            InputManager.Devices = new ReadOnlyCollection<InputDevice>(InputManager.devices);
            InputManager.activeDevice = InputDevice.Null;
            InputManager.playerActionSets.Clear();
            InputManager.IsSetup = true;
            bool flag = true;
            bool flag2 = InputManager.EnableNativeInput && NativeInputDeviceManager.Enable();
            if (flag2)
            {
                flag = false;
            }
            if (InputManager.EnableXInput && flag)
            {
                XInputDeviceManager.Enable();
            }
            if (InputManager.OnSetup != null)
            {
                InputManager.OnSetup();
                InputManager.OnSetup = null;
            }
            if (flag)
            {
                InputManager.AddDeviceManager<UnityInputDeviceManager>();
            }
            return true;
        }

        internal static void ResetInternal()
        {
            if (InputManager.OnReset != null)
            {
                InputManager.OnReset();
            }
            InputManager.OnSetup = null;
            InputManager.OnUpdate = null;
            InputManager.OnReset = null;
            InputManager.OnActiveDeviceChanged = null;
            InputManager.OnDeviceAttached = null;
            InputManager.OnDeviceDetached = null;
            InputManager.OnUpdateDevices = null;
            InputManager.OnCommitDevices = null;
            InputManager.DestroyDeviceManagers();
            InputManager.DestroyDevices();
            InputManager.playerActionSets.Clear();
            InputManager.IsSetup = false;
        }

        internal static void UpdateInternal()
        {
            InputManager.AssertIsSetup();
            if (InputManager.OnSetup != null)
            {
                InputManager.OnSetup();
                InputManager.OnSetup = null;
            }
            if (!InputManager.enabled)
            {
                return;
            }
            if (InputManager.SuspendInBackground && !InputManager.applicationIsFocused)
            {
                return;
            }
            InputManager.currentTick += 1UL;
            InputManager.UpdateCurrentTime();
            float num = InputManager.currentTime - InputManager.lastUpdateTime;
            InputManager.UpdateDeviceManagers(num);
            InputManager.CommandWasPressed = false;
            InputManager.UpdateDevices(num);
            InputManager.CommitDevices(num);
            InputManager.UpdateActiveDevice();
            InputManager.UpdatePlayerActionSets(num);
            if (InputManager.OnUpdate != null)
            {
                InputManager.OnUpdate(InputManager.currentTick, num);
            }
            InputManager.lastUpdateTime = InputManager.currentTime;
        }

        public static void Reload()
        {
            InputManager.ResetInternal();
            InputManager.SetupInternal();
        }

        private static void AssertIsSetup()
        {
            if (!InputManager.IsSetup)
            {
                throw new Exception("InputManager is not initialized. Call InputManager.Setup() first.");
            }
        }

        private static void SetZeroTickOnAllControls()
        {
            int count = InputManager.devices.Count;
            for (int i = 0; i < count; i++)
            {
                ReadOnlyCollection<InputControl> controls = InputManager.devices[i].Controls;
                int count2 = controls.Count;
                for (int j = 0; j < count2; j++)
                {
                    InputControl inputControl = controls[j];
                    if (inputControl != null)
                    {
                        inputControl.SetZeroTick();
                    }
                }
            }
        }

        public static void ClearInputState()
        {
            int count = InputManager.devices.Count;
            for (int i = 0; i < count; i++)
            {
                InputManager.devices[i].ClearInputState();
            }
            int count2 = InputManager.playerActionSets.Count;
            for (int j = 0; j < count2; j++)
            {
                InputManager.playerActionSets[j].ClearInputState();
            }
            InputManager.activeDevice = InputDevice.Null;
        }

        internal static void OnApplicationFocus(bool focusState)
        {
            if (!focusState)
            {
                if (InputManager.SuspendInBackground)
                {
                    InputManager.ClearInputState();
                }
                InputManager.SetZeroTickOnAllControls();
            }
            InputManager.applicationIsFocused = focusState;
        }

        internal static void OnApplicationPause(bool pauseState)
        {
        }

        internal static void OnApplicationQuit()
        {
            InputManager.ResetInternal();
        }

        internal static void OnLevelWasLoaded()
        {
            InputManager.SetZeroTickOnAllControls();
            InputManager.UpdateInternal();
        }

        public static void AddDeviceManager(InputDeviceManager deviceManager)
        {
            InputManager.AssertIsSetup();
            Type type = deviceManager.GetType();
            if (InputManager.deviceManagerTable.ContainsKey(type))
            {
                //Logger.LogError("A device manager of type '" + type.Name + "' already exists; cannot add another.");
                UnityEngine.Debug.LogError(type.Name);
                return;
            }
            InputManager.deviceManagers.Add(deviceManager);
            InputManager.deviceManagerTable.Add(type, deviceManager);
            deviceManager.Update(InputManager.currentTick, InputManager.currentTime - InputManager.lastUpdateTime);
        }

        public static void AddDeviceManager<T>() where T : InputDeviceManager, new()
        {
            InputManager.AddDeviceManager(Activator.CreateInstance<T>());
        }

        public static T GetDeviceManager<T>() where T : InputDeviceManager
        {
            InputDeviceManager inputDeviceManager;
            if (InputManager.deviceManagerTable.TryGetValue(typeof(T), out inputDeviceManager))
            {
                return inputDeviceManager as T;
            }
            return (T)((object)null);
        }

        public static bool HasDeviceManager<T>() where T : InputDeviceManager
        {
            return InputManager.deviceManagerTable.ContainsKey(typeof(T));
        }

        private static void UpdateCurrentTime()
        {
            if (InputManager.initialTime < 1.401298E-45f)
            {
                InputManager.initialTime = Time.realtimeSinceStartup;
            }
            InputManager.currentTime = Mathf.Max(0f, Time.realtimeSinceStartup - InputManager.initialTime);
        }

        private static void UpdateDeviceManagers(float deltaTime)
        {
            int count = InputManager.deviceManagers.Count;
            for (int i = 0; i < count; i++)
            {
                InputManager.deviceManagers[i].Update(InputManager.currentTick, deltaTime);
            }
        }

        private static void DestroyDeviceManagers()
        {
            int count = InputManager.deviceManagers.Count;
            for (int i = 0; i < count; i++)
            {
                InputManager.deviceManagers[i].Destroy();
            }
            InputManager.deviceManagers.Clear();
            InputManager.deviceManagerTable.Clear();
        }

        private static void DestroyDevices()
        {
            int count = InputManager.devices.Count;
            for (int i = 0; i < count; i++)
            {
                InputDevice inputDevice = InputManager.devices[i];
                inputDevice.OnDetached();
            }
            InputManager.devices.Clear();
            InputManager.activeDevice = InputDevice.Null;
        }

        private static void UpdateDevices(float deltaTime)
        {
            int count = InputManager.devices.Count;
            for (int i = 0; i < count; i++)
            {
                InputDevice inputDevice = InputManager.devices[i];
                inputDevice.Update(InputManager.currentTick, deltaTime);
            }
            if (InputManager.OnUpdateDevices != null)
            {
                InputManager.OnUpdateDevices(InputManager.currentTick, deltaTime);
            }
        }

        private static void CommitDevices(float deltaTime)
        {
            int count = InputManager.devices.Count;
            for (int i = 0; i < count; i++)
            {
                InputDevice inputDevice = InputManager.devices[i];
                inputDevice.Commit(InputManager.currentTick, deltaTime);
                if (inputDevice.CommandWasPressed)
                {
                    InputManager.CommandWasPressed = true;
                }
            }
            if (InputManager.OnCommitDevices != null)
            {
                InputManager.OnCommitDevices(InputManager.currentTick, deltaTime);
            }
        }

        private static void UpdateActiveDevice()
        {
            InputDevice inputDevice = InputManager.ActiveDevice;
            int count = InputManager.devices.Count;
            for (int i = 0; i < count; i++)
            {
                InputDevice inputDevice2 = InputManager.devices[i];
                if (inputDevice2.LastChangedAfter(InputManager.ActiveDevice) && !inputDevice2.Passive)
                {
                    InputManager.ActiveDevice = inputDevice2;
                }
            }
            if (inputDevice != InputManager.ActiveDevice && InputManager.OnActiveDeviceChanged != null)
            {
                InputManager.OnActiveDeviceChanged(InputManager.ActiveDevice);
            }
        }

        public static void AttachDevice(InputDevice inputDevice)
        {
            InputManager.AssertIsSetup();
            if (!inputDevice.IsSupportedOnThisPlatform)
            {
                return;
            }
            if (inputDevice.IsAttached)
            {
                return;
            }
            if (!InputManager.devices.Contains(inputDevice))
            {
                InputManager.devices.Add(inputDevice);
                InputManager.devices.Sort((InputDevice d1, InputDevice d2) => d1.SortOrder.CompareTo(d2.SortOrder));
            }
            inputDevice.OnAttached();
            if (InputManager.OnDeviceAttached != null)
            {
                InputManager.OnDeviceAttached(inputDevice);
            }
        }

        public static void DetachDevice(InputDevice inputDevice)
        {
            if (!InputManager.IsSetup)
            {
                return;
            }
            if (!inputDevice.IsAttached)
            {
                return;
            }
            InputManager.devices.Remove(inputDevice);
            if (InputManager.ActiveDevice == inputDevice)
            {
                InputManager.ActiveDevice = InputDevice.Null;
            }
            inputDevice.OnDetached();
            if (InputManager.OnDeviceDetached != null)
            {
                InputManager.OnDeviceDetached(inputDevice);
            }
        }

        public static void HideDevicesWithProfile(Type type)
        {
            if (type.IsSubclassOf(typeof(UnityInputDeviceProfile)))
            {
                InputDeviceProfile.Hide(type);
            }
        }

        internal static void AttachPlayerActionSet(PlayerActionSet playerActionSet)
        {
            if (!InputManager.playerActionSets.Contains(playerActionSet))
            {
                InputManager.playerActionSets.Add(playerActionSet);
            }
        }

        internal static void DetachPlayerActionSet(PlayerActionSet playerActionSet)
        {
            InputManager.playerActionSets.Remove(playerActionSet);
        }

        internal static void UpdatePlayerActionSets(float deltaTime)
        {
            int count = InputManager.playerActionSets.Count;
            for (int i = 0; i < count; i++)
            {
                InputManager.playerActionSets[i].Update(InputManager.currentTick, deltaTime);
            }
        }

        public static bool AnyKeyIsPressed
        {
            get
            {
                return KeyCombo.Detect(true).IncludeCount > 0;
            }
        }

        public static InputDevice ActiveDevice
        {
            get
            {
                return (InputManager.activeDevice != null) ? InputManager.activeDevice : InputDevice.Null;
            }
            private set
            {
                InputManager.activeDevice = ((value != null) ? value : InputDevice.Null);
            }
        }

        public static bool Enabled
        {
            get
            {
                return InputManager.enabled;
            }
            set
            {
                if (InputManager.enabled != value)
                {
                    if (value)
                    {
                        InputManager.SetZeroTickOnAllControls();
                        InputManager.UpdateInternal();
                    }
                    else
                    {
                        InputManager.ClearInputState();
                        InputManager.SetZeroTickOnAllControls();
                    }
                    InputManager.enabled = value;
                }
            }
        }

        public static bool SuspendInBackground { get; internal set; }

        public static bool EnableNativeInput { get; internal set; }

        public static bool EnableXInput { get; internal set; }

        public static uint XInputUpdateRate { get; internal set; }

        public static uint XInputBufferSize { get; internal set; }

        public static bool NativeInputEnableXInput { get; internal set; }

        public static bool NativeInputPreventSleep { get; internal set; }

        public static uint NativeInputUpdateRate { get; internal set; }

        public static bool EnableICade { get; internal set; }

        internal static VersionInfo UnityVersion
        {
            get
            {
                if (InputManager.unityVersion == null)
                {
                    InputManager.unityVersion = new VersionInfo?(VersionInfo.UnityVersion());
                }
                return InputManager.unityVersion.Value;
            }
        }

        internal static ulong CurrentTick
        {
            get
            {
                return InputManager.currentTick;
            }
        }
    }
}
