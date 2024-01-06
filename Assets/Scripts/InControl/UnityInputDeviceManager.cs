using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
    public class UnityInputDeviceManager : InputDeviceManager
    {
        public UnityInputDeviceManager()
        {
            this.AddSystemDeviceProfiles();
            this.QueryJoystickInfo();
            this.AttachDevices();
        }

        public override void Update(ulong updateTick, float deltaTime)
        {
            this.deviceRefreshTimer += deltaTime;
            if (this.deviceRefreshTimer >= 1f)
            {
                this.deviceRefreshTimer = 0f;
                this.QueryJoystickInfo();
                if (this.JoystickInfoHasChanged)
                {
                    //Logger.LogInfo("Change in attached Unity joysticks detected; refreshing device list.");
                    this.DetachDevices();
                    this.AttachDevices();
                }
            }
        }

        private void QueryJoystickInfo()
        {
            this.joystickNames = Input.GetJoystickNames();
            this.joystickCount = this.joystickNames.Length;
            this.joystickHash = 527 + this.joystickCount;
            for (int i = 0; i < this.joystickCount; i++)
            {
                this.joystickHash = this.joystickHash * 31 + this.joystickNames[i].GetHashCode();
            }
        }

        private bool JoystickInfoHasChanged
        {
            get
            {
                return this.joystickHash != this.lastJoystickHash || this.joystickCount != this.lastJoystickCount;
            }
        }

        private void AttachDevices()
        {
            this.AttachKeyboardDevices();
            this.AttachJoystickDevices();
            this.lastJoystickCount = this.joystickCount;
            this.lastJoystickHash = this.joystickHash;
        }

        private void DetachDevices()
        {
            int count = this.devices.Count;
            for (int i = 0; i < count; i++)
            {
                InputManager.DetachDevice(this.devices[i]);
            }
            this.devices.Clear();
        }

        public void ReloadDevices()
        {
            this.QueryJoystickInfo();
            this.DetachDevices();
            this.AttachDevices();
        }

        private void AttachDevice(UnityInputDevice device)
        {
            this.devices.Add(device);
            InputManager.AttachDevice(device);
        }

        private void AttachKeyboardDevices()
        {
            int count = this.systemDeviceProfiles.Count;
            for (int i = 0; i < count; i++)
            {
                UnityInputDeviceProfileBase unityInputDeviceProfileBase = this.systemDeviceProfiles[i];
                if (unityInputDeviceProfileBase.IsNotJoystick && unityInputDeviceProfileBase.IsSupportedOnThisPlatform)
                {
                    this.AttachDevice(new UnityInputDevice(unityInputDeviceProfileBase));
                }
            }
        }

        private void AttachJoystickDevices()
        {
            try
            {
                for (int i = 0; i < this.joystickCount; i++)
                {
                    this.DetectJoystickDevice(i + 1, this.joystickNames[i]);
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex.Message);
                //Logger.LogError(ex.StackTrace);
                Debug.LogError(ex.Message);
                Debug.LogError(ex.StackTrace);
            }
        }

        private bool HasAttachedDeviceWithJoystickId(int unityJoystickId)
        {
            int count = this.devices.Count;
            for (int i = 0; i < count; i++)
            {
                UnityInputDevice unityInputDevice = this.devices[i] as UnityInputDevice;
                if (unityInputDevice != null && unityInputDevice.JoystickId == unityJoystickId)
                {
                    return true;
                }
            }
            return false;
        }

        private void DetectJoystickDevice(int unityJoystickId, string unityJoystickName)
        {
            if (this.HasAttachedDeviceWithJoystickId(unityJoystickId))
            {
                return;
            }
            if (unityJoystickName.IndexOf("webcam", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return;
            }
            if (InputManager.UnityVersion < new VersionInfo(4, 5, 0, 0) && (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) && unityJoystickName == "Unknown Wireless Controller")
            {
                return;
            }
            if (InputManager.UnityVersion >= new VersionInfo(4, 6, 3, 0) && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer) && string.IsNullOrEmpty(unityJoystickName))
            {
                return;
            }
            UnityInputDeviceProfileBase unityInputDeviceProfileBase = null;
            if (unityInputDeviceProfileBase == null)
            {
                unityInputDeviceProfileBase = this.customDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasJoystickName(unityJoystickName));
            }
            if (unityInputDeviceProfileBase == null)
            {
                unityInputDeviceProfileBase = this.systemDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasJoystickName(unityJoystickName));
            }
            if (unityInputDeviceProfileBase == null)
            {
                unityInputDeviceProfileBase = this.customDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasLastResortRegex(unityJoystickName));
            }
            if (unityInputDeviceProfileBase == null)
            {
                unityInputDeviceProfileBase = this.systemDeviceProfiles.Find((UnityInputDeviceProfileBase config) => config.HasLastResortRegex(unityJoystickName));
            }
            if (unityInputDeviceProfileBase == null)
            {
                UnityInputDevice device = new UnityInputDevice(unityJoystickId, unityJoystickName);
                this.AttachDevice(device);
                Debug.Log(string.Concat(new object[]
                {
                    "[InControl] Joystick ",
                    unityJoystickId,
                    ": \"",
                    unityJoystickName,
                    "\""
                }));
                //Logger.LogWarning(string.Concat(new object[]
                //{
                //    "Device ",
                //    unityJoystickId,
                //    " with name \"",
                //    unityJoystickName,
                //    "\" does not match any supported profiles and will be considered an unknown controller."
                //}));
                return;
            }
            if (!unityInputDeviceProfileBase.IsHidden)
            {
                UnityInputDevice device2 = new UnityInputDevice(unityInputDeviceProfileBase, unityJoystickId, unityJoystickName);
                this.AttachDevice(device2);
                //Logger.LogInfo(string.Concat(new object[]
                //{
                //    "Device ",
                //    unityJoystickId,
                //    " matched profile ",
                //    unityInputDeviceProfileBase.GetType().Name,
                //    " (",
                //    unityInputDeviceProfileBase.Name,
                //    ")"
                //}));
            }
            else
            {
                //Logger.LogInfo(string.Concat(new object[]
                //{
                //    "Device ",
                //    unityJoystickId,
                //    " matching profile ",
                //    unityInputDeviceProfileBase.GetType().Name,
                //    " (",
                //    unityInputDeviceProfileBase.Name,
                //    ") is hidden and will not be attached."
                //}));
            }
        }

        private void AddSystemDeviceProfile(UnityInputDeviceProfile deviceProfile)
        {
            if (deviceProfile.IsSupportedOnThisPlatform)
            {
                this.systemDeviceProfiles.Add(deviceProfile);
            }
        }

        private void AddSystemDeviceProfiles()
        {
            foreach (string typeName in UnityInputDeviceProfileList.Profiles)
            {
                UnityInputDeviceProfile deviceProfile = (UnityInputDeviceProfile)Activator.CreateInstance(Type.GetType(typeName));
                this.AddSystemDeviceProfile(deviceProfile);
            }
        }

        private const float deviceRefreshInterval = 1f;

        private float deviceRefreshTimer;

        private List<UnityInputDeviceProfileBase> systemDeviceProfiles = new List<UnityInputDeviceProfileBase>(UnityInputDeviceProfileList.Profiles.Length);

        private List<UnityInputDeviceProfileBase> customDeviceProfiles = new List<UnityInputDeviceProfileBase>();

        private string[] joystickNames;

        private int lastJoystickCount;

        private int lastJoystickHash;

        private int joystickCount;

        private int joystickHash;
    }
}
