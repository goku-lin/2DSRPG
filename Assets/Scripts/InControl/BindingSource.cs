using System;
using System.IO;

namespace InControl
{
    public abstract class BindingSource : InputControlSource, IEquatable<BindingSource>
    {
        public abstract float GetValue(InputDevice inputDevice);

        public abstract bool GetState(InputDevice inputDevice);

        public abstract bool Equals(BindingSource other);

        public abstract string Name { get; }

        public abstract string DeviceName { get; }

        public abstract InputDeviceClass DeviceClass { get; }

        public abstract InputDeviceStyle DeviceStyle { get; }

        public static bool operator ==(BindingSource a, BindingSource b)
        {
            return object.ReferenceEquals(a, b) || (a != null && b != null && a.BindingSourceType == b.BindingSourceType && a.Equals(b));
        }

        public static bool operator !=(BindingSource a, BindingSource b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((BindingSource)obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract BindingSourceType BindingSourceType { get; }

        internal abstract void Save(BinaryWriter writer);

        internal abstract void Load(BinaryReader reader, ushort dataFormatVersion);

        internal PlayerAction BoundTo { get; set; }

        internal virtual bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}
