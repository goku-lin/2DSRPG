using System;
using System.IO;
using UnityEngine;

namespace InControl
{
    public class UnknownDeviceBindingSource : BindingSource
    {
        internal UnknownDeviceBindingSource()
        {
            this.Control = UnknownDeviceControl.None;
        }

        public UnknownDeviceBindingSource(UnknownDeviceControl control)
        {
            this.Control = control;
        }

        public UnknownDeviceControl Control { get; protected set; }

        public override float GetValue(InputDevice device)
        {
            return this.Control.GetValue(device);
        }

        public override bool GetState(InputDevice device)
        {
            return device != null && Utility.IsNotZero(this.GetValue(device));
        }

        public override string Name
        {
            get
            {
                if (base.BoundTo == null)
                {
                    return string.Empty;
                }
                string str = string.Empty;
                if (this.Control.SourceRange == InputRangeType.ZeroToMinusOne)
                {
                    str = "Negative ";
                }
                else if (this.Control.SourceRange == InputRangeType.ZeroToOne)
                {
                    str = "Positive ";
                }
                InputDevice device = base.BoundTo.Device;
                if (device == InputDevice.Null)
                {
                    return str + this.Control.Control.ToString();
                }
                InputControl control = device.GetControl(this.Control.Control);
                if (control == InputControl.Null)
                {
                    return str + this.Control.Control.ToString();
                }
                return str + control.Handle;
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
                    return "Unknown Controller";
                }
                return device.Name;
            }
        }

        public override InputDeviceClass DeviceClass
        {
            get
            {
                return InputDeviceClass.Controller;
            }
        }

        public override InputDeviceStyle DeviceStyle
        {
            get
            {
                return InputDeviceStyle.Unknown;
            }
        }

        public override bool Equals(BindingSource other)
        {
            if (other == null)
            {
                return false;
            }
            UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
            return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            UnknownDeviceBindingSource unknownDeviceBindingSource = other as UnknownDeviceBindingSource;
            return unknownDeviceBindingSource != null && this.Control == unknownDeviceBindingSource.Control;
        }

        public override int GetHashCode()
        {
            return this.Control.GetHashCode();
        }

        public override BindingSourceType BindingSourceType
        {
            get
            {
                return BindingSourceType.UnknownDeviceBindingSource;
            }
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
                InputDevice device = base.BoundTo.Device;
                return device == InputDevice.Null || device.HasControl(this.Control.Control);
            }
        }

        internal override void Load(BinaryReader reader, ushort dataFormatVersion)
        {
            UnknownDeviceControl control = default(UnknownDeviceControl);
            control.Load(reader);
            this.Control = control;
        }

        internal override void Save(BinaryWriter writer)
        {
            this.Control.Save(writer);
        }
    }
}
