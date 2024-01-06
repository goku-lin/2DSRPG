using System;
using UnityEngine;

namespace InControl
{
    public class UnityInputDevice : InputDevice
    {
        public UnityInputDevice(UnityInputDeviceProfileBase deviceProfile) : this(deviceProfile, 0, string.Empty)
        {
        }

        public UnityInputDevice(int joystickId, string joystickName) : this(null, joystickId, joystickName)
        {
        }

        public UnityInputDevice(UnityInputDeviceProfileBase deviceProfile, int joystickId, string joystickName)
        {
            this.profile = deviceProfile;
            this.JoystickId = joystickId;
            if (joystickId != 0)
            {
                base.SortOrder = 100 + joystickId;
            }
            UnityInputDevice.SetupAnalogQueries();
            UnityInputDevice.SetupButtonQueries();
            base.AnalogSnapshot = null;
            if (this.IsKnown)
            {
                base.Name = this.profile.Name;
                base.Meta = this.profile.Meta;
                base.DeviceClass = this.profile.DeviceClass;
                base.DeviceStyle = this.profile.DeviceStyle;
                int analogCount = this.profile.AnalogCount;
                for (int i = 0; i < analogCount; i++)
                {
                    InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
                    //if (Utility.TargetIsAlias(inputControlMapping.Target))
                    //{
                    //    Debug.LogError(string.Concat(new object[]
                    //    {
                    //        "Cannot map control \"",
                    //        inputControlMapping.Handle,
                    //        "\" as InputControlType.",
                    //        inputControlMapping.Target,
                    //        " in profile \"",
                    //        deviceProfile.Name,
                    //        "\" because this target is reserved as an alias. The mapping will be ignored."
                    //    }));
                    //}
                    //else
                    {
                        InputControl inputControl = base.AddControl(inputControlMapping.Target, inputControlMapping.Handle);
                        inputControl.Sensitivity = Mathf.Min(this.profile.Sensitivity, inputControlMapping.Sensitivity);
                        inputControl.LowerDeadZone = Mathf.Max(this.profile.LowerDeadZone, inputControlMapping.LowerDeadZone);
                        inputControl.UpperDeadZone = Mathf.Min(this.profile.UpperDeadZone, inputControlMapping.UpperDeadZone);
                        inputControl.Raw = inputControlMapping.Raw;
                        inputControl.Passive = inputControlMapping.Passive;
                    }
                }
                int buttonCount = this.profile.ButtonCount;
                for (int j = 0; j < buttonCount; j++)
                {
                    InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
                    //if (Utility.TargetIsAlias(inputControlMapping2.Target))
                    //{
                    //    Debug.LogError(string.Concat(new object[]
                    //    {
                    //        "Cannot map control \"",
                    //        inputControlMapping2.Handle,
                    //        "\" as InputControlType.",
                    //        inputControlMapping2.Target,
                    //        " in profile \"",
                    //        deviceProfile.Name,
                    //        "\" because this target is reserved as an alias. The mapping will be ignored."
                    //    }));
                    //}
                    //else
                    {
                        InputControl inputControl2 = base.AddControl(inputControlMapping2.Target, inputControlMapping2.Handle);
                        inputControl2.Passive = inputControlMapping2.Passive;
                    }
                }
            }
            else
            {
                base.Name = "Unknown Device";
                base.Meta = "\"" + joystickName + "\"";
                for (int k = 0; k < this.NumUnknownButtons; k++)
                {
                    base.AddControl(InputControlType.Button0 + k, "Button " + k);
                }
                for (int l = 0; l < this.NumUnknownAnalogs; l++)
                {
                    base.AddControl(InputControlType.Analog0 + l, "Analog " + l, 0.2f, 0.9f);
                }
            }
        }

        internal int JoystickId { get; private set; }

        public override void Update(ulong updateTick, float deltaTime)
        {
            if (this.IsKnown)
            {
                int analogCount = this.profile.AnalogCount;
                for (int i = 0; i < analogCount; i++)
                {
                    InputControlMapping inputControlMapping = this.profile.AnalogMappings[i];
                    float value = inputControlMapping.Source.GetValue(this);
                    InputControl control = base.GetControl(inputControlMapping.Target);
                    if (!inputControlMapping.IgnoreInitialZeroValue || !control.IsOnZeroTick || !Utility.IsZero(value))
                    {
                        float value2 = inputControlMapping.MapValue(value);
                        control.UpdateWithValue(value2, updateTick, deltaTime);
                    }
                }
                int buttonCount = this.profile.ButtonCount;
                for (int j = 0; j < buttonCount; j++)
                {
                    InputControlMapping inputControlMapping2 = this.profile.ButtonMappings[j];
                    bool state = inputControlMapping2.Source.GetState(this);
                    base.UpdateWithState(inputControlMapping2.Target, state, updateTick, deltaTime);
                }
            }
            else
            {
                for (int k = 0; k < this.NumUnknownButtons; k++)
                {
                    base.UpdateWithState(InputControlType.Button0 + k, this.ReadRawButtonState(k), updateTick, deltaTime);
                }
                for (int l = 0; l < this.NumUnknownAnalogs; l++)
                {
                    base.UpdateWithValue(InputControlType.Analog0 + l, this.ReadRawAnalogValue(l), updateTick, deltaTime);
                }
            }
        }

        private static void SetupAnalogQueries()
        {
            if (UnityInputDevice.analogQueries == null)
            {
                UnityInputDevice.analogQueries = new string[10, 20];
                for (int i = 1; i <= 10; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        UnityInputDevice.analogQueries[i - 1, j] = string.Concat(new object[]
                        {
                            "joystick ",
                            i,
                            " analog ",
                            j
                        });
                    }
                }
            }
        }

        private static void SetupButtonQueries()
        {
            if (UnityInputDevice.buttonQueries == null)
            {
                UnityInputDevice.buttonQueries = new string[10, 20];
                for (int i = 1; i <= 10; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        UnityInputDevice.buttonQueries[i - 1, j] = string.Concat(new object[]
                        {
                            "joystick ",
                            i,
                            " button ",
                            j
                        });
                    }
                }
            }
        }

        private static string GetAnalogKey(int joystickId, int analogId)
        {
            return UnityInputDevice.analogQueries[joystickId - 1, analogId];
        }

        private static string GetButtonKey(int joystickId, int buttonId)
        {
            return UnityInputDevice.buttonQueries[joystickId - 1, buttonId];
        }

        internal override bool ReadRawButtonState(int index)
        {
            if (index < 20)
            {
                string name = UnityInputDevice.buttonQueries[this.JoystickId - 1, index];
                return Input.GetKey(name);
            }
            return false;
        }

        internal override float ReadRawAnalogValue(int index)
        {
            if (index < 20)
            {
                string axisName = UnityInputDevice.analogQueries[this.JoystickId - 1, index];
                return Input.GetAxisRaw(axisName);
            }
            return 0f;
        }

        public override bool IsSupportedOnThisPlatform
        {
            get
            {
                return this.profile == null || this.profile.IsSupportedOnThisPlatform;
            }
        }

        public override bool IsKnown
        {
            get
            {
                return this.profile != null;
            }
        }

        internal override int NumUnknownButtons
        {
            get
            {
                return 20;
            }
        }

        internal override int NumUnknownAnalogs
        {
            get
            {
                return 20;
            }
        }

        private static string[,] analogQueries;

        private static string[,] buttonQueries;

        public const int MaxDevices = 10;

        public const int MaxButtons = 20;

        public const int MaxAnalogs = 20;

        private UnityInputDeviceProfileBase profile;
    }
}
