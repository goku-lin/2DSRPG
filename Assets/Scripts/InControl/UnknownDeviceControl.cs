using System;
using System.IO;

namespace InControl
{
    public struct UnknownDeviceControl : IEquatable<UnknownDeviceControl>
    {
        public UnknownDeviceControl(InputControlType control, InputRangeType sourceRange)
        {
            this.Control = control;
            this.SourceRange = sourceRange;
            this.IsButton = Utility.TargetIsButton(control);
            this.IsAnalog = !this.IsButton;
        }

        internal float GetValue(InputDevice device)
        {
            if (device == null)
            {
                return 0f;
            }
            float value = device.GetControl(this.Control).Value;
            return InputRange.Remap(value, this.SourceRange, InputRangeType.ZeroToOne);
        }

        public int Index
        {
            get
            {
                return this.Control - ((!this.IsButton) ? InputControlType.Analog0 : InputControlType.Button0);
            }
        }

        public static bool operator ==(UnknownDeviceControl a, UnknownDeviceControl b)
        {
            if (object.ReferenceEquals(null, a))
            {
                return object.ReferenceEquals(null, b);
            }
            return a.Equals(b);
        }

        public static bool operator !=(UnknownDeviceControl a, UnknownDeviceControl b)
        {
            return !(a == b);
        }

        public bool Equals(UnknownDeviceControl other)
        {
            return this.Control == other.Control && this.SourceRange == other.SourceRange;
        }

        public override bool Equals(object other)
        {
            return this.Equals((UnknownDeviceControl)other);
        }

        public override int GetHashCode()
        {
            return this.Control.GetHashCode() ^ this.SourceRange.GetHashCode();
        }

        public static implicit operator bool(UnknownDeviceControl control)
        {
            return control.Control != InputControlType.None;
        }

        public override string ToString()
        {
            return string.Format("UnknownDeviceControl( {0}, {1} )", this.Control.ToString(), this.SourceRange.ToString());
        }

        internal void Save(BinaryWriter writer)
        {
            writer.Write((int)this.Control);
            writer.Write((int)this.SourceRange);
        }

        internal void Load(BinaryReader reader)
        {
            this.Control = (InputControlType)reader.ReadInt32();
            this.SourceRange = (InputRangeType)reader.ReadInt32();
            this.IsButton = Utility.TargetIsButton(this.Control);
            this.IsAnalog = !this.IsButton;
        }

        public static readonly UnknownDeviceControl None = new UnknownDeviceControl(InputControlType.None, InputRangeType.None);

        public InputControlType Control;

        public InputRangeType SourceRange;

        public bool IsButton;

        public bool IsAnalog;
    }
}
