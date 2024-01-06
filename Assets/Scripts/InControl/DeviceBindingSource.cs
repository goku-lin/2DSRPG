using System;
using System.IO;
using UnityEngine;

namespace InControl
{
    public class DeviceBindingSource : BindingSource
    {
        internal DeviceBindingSource()
        {
            this.Control = InputControlType.None;
        }

        public DeviceBindingSource(InputControlType control)
        {
            this.Control = control;
        }

        public InputControlType Control { get; protected set; }

        public override float GetValue(InputDevice inputDevice)
        {
            if (inputDevice == null)
            {
                return 0f;
            }
            return inputDevice.GetControl(this.Control).Value;
        }

        public override bool GetState(InputDevice inputDevice)
        {
            return inputDevice != null && inputDevice.GetControl(this.Control).State;
        }

        public override string Name
        {
            get
            {
                if (base.BoundTo == null)
                {
                    return string.Empty;
                }
                InputDevice device = base.BoundTo.Device;
                InputControl control = device.GetControl(this.Control);
                if (control == InputControl.Null)
                {
                    return this.Control.ToString();
                }
                return device.GetControl(this.Control).Handle;
            }
        }

        public override string DeviceName
        {
            get
            {
                if (base.BoundTo == null)
                {
                    return string.Empty;
                }
                InputDevice device = base.BoundTo.Device;
                if (device == InputDevice.Null)
                {
                    return "Controller";
                }
                return device.Name;
            }
        }

        public override InputDeviceClass DeviceClass
        {
            get
            {
                return (base.BoundTo != null) ? base.BoundTo.Device.DeviceClass : InputDeviceClass.Unknown;
            }
        }

        public override InputDeviceStyle DeviceStyle
        {
            get
            {
                return (base.BoundTo != null) ? base.BoundTo.Device.DeviceStyle : InputDeviceStyle.Unknown;
            }
        }

        public override bool Equals(BindingSource other)
        {
            if (other == null)
            {
                return false;
            }
            DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
            return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            DeviceBindingSource deviceBindingSource = other as DeviceBindingSource;
            return deviceBindingSource != null && this.Control == deviceBindingSource.Control;
        }

        public override int GetHashCode()
        {
            return this.Control.GetHashCode();
        }

        public override BindingSourceType BindingSourceType
        {
            get
            {
                return BindingSourceType.DeviceBindingSource;
            }
        }

        internal override void Save(BinaryWriter writer)
        {
            writer.Write((int)this.Control);
        }

        internal override void Load(BinaryReader reader, ushort dataFormatVersion)
        {
            this.Control = (InputControlType)reader.ReadInt32();
        }

        internal override bool IsValid
        {
            get
            {
                if (base.BoundTo == null)
                {
                    Debug.LogError("Cannot query property 'IsValid' for unbound BindingSource.");
                    return false;
                }
                return base.BoundTo.Device.HasControl(this.Control) || Utility.TargetIsStandard(this.Control);
            }
        }
    }
}
