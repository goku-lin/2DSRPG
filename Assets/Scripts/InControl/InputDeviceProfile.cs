using System;
using System.Collections.Generic;
using UnityEngine;

namespace InControl
{
    public abstract class InputDeviceProfile
    {
        public InputDeviceProfile()
        {
            this.Name = string.Empty;
            this.Meta = string.Empty;
            this.AnalogMappings = new InputControlMapping[0];
            this.ButtonMappings = new InputControlMapping[0];
            this.IncludePlatforms = new string[0];
            this.ExcludePlatforms = new string[0];
            this.MinSystemBuildNumber = 0;
            this.MaxSystemBuildNumber = 0;
            this.DeviceClass = InputDeviceClass.Unknown;
            this.DeviceStyle = InputDeviceStyle.Unknown;
        }

        [SerializeField]
        public string Name { get; protected set; }

        [SerializeField]
        public string Meta { get; protected set; }

        [SerializeField]
        public InputControlMapping[] AnalogMappings { get; protected set; }

        [SerializeField]
        public InputControlMapping[] ButtonMappings { get; protected set; }

        [SerializeField]
        public string[] IncludePlatforms { get; protected set; }

        [SerializeField]
        public string[] ExcludePlatforms { get; protected set; }

        [SerializeField]
        public int MaxSystemBuildNumber { get; protected set; }

        [SerializeField]
        public int MinSystemBuildNumber { get; protected set; }

        [SerializeField]
        public InputDeviceClass DeviceClass { get; protected set; }

        [SerializeField]
        public InputDeviceStyle DeviceStyle { get; protected set; }

        [SerializeField]
        public float Sensitivity
        {
            get
            {
                return this.sensitivity;
            }
            protected set
            {
                this.sensitivity = Mathf.Clamp01(value);
            }
        }

        [SerializeField]
        public float LowerDeadZone
        {
            get
            {
                return this.lowerDeadZone;
            }
            protected set
            {
                this.lowerDeadZone = Mathf.Clamp01(value);
            }
        }

        [SerializeField]
        public float UpperDeadZone
        {
            get
            {
                return this.upperDeadZone;
            }
            protected set
            {
                this.upperDeadZone = Mathf.Clamp01(value);
            }
        }

        [Obsolete("This property has been renamed to IncludePlatforms.", false)]
        public string[] SupportedPlatforms
        {
            get
            {
                return this.IncludePlatforms;
            }
            protected set
            {
                this.IncludePlatforms = value;
            }
        }

        public virtual bool IsSupportedOnThisPlatform
        {
            get
            {
                int systemBuildNumber = Utility.GetSystemBuildNumber();
                if (this.MaxSystemBuildNumber > 0 && systemBuildNumber > this.MaxSystemBuildNumber)
                {
                    return false;
                }
                if (this.MinSystemBuildNumber > 0 && systemBuildNumber < this.MinSystemBuildNumber)
                {
                    return false;
                }
                if (this.ExcludePlatforms != null)
                {
                    int num = this.ExcludePlatforms.Length;
                    for (int i = 0; i < num; i++)
                    {
                        if (InputManager.Platform.Contains(this.ExcludePlatforms[i].ToUpper()))
                        {
                            return false;
                        }
                    }
                }
                if (this.IncludePlatforms == null || this.IncludePlatforms.Length == 0)
                {
                    return true;
                }
                if (this.IncludePlatforms != null)
                {
                    int num2 = this.IncludePlatforms.Length;
                    for (int j = 0; j < num2; j++)
                    {
                        if (InputManager.Platform.Contains(this.IncludePlatforms[j].ToUpper()))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        internal static void Hide(Type type)
        {
            InputDeviceProfile.hideList.Add(type);
        }

        internal bool IsHidden
        {
            get
            {
                return InputDeviceProfile.hideList.Contains(base.GetType());
            }
        }

        public int AnalogCount
        {
            get
            {
                return this.AnalogMappings.Length;
            }
        }

        public int ButtonCount
        {
            get
            {
                return this.ButtonMappings.Length;
            }
        }

        private static HashSet<Type> hideList = new HashSet<Type>();

        private float sensitivity = 1f;

        private float lowerDeadZone;

        private float upperDeadZone = 1f;
    }
}
