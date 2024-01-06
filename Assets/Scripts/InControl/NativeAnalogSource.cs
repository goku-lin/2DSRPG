using System;

namespace InControl
{
    public class NativeAnalogSource : InputControlSource
    {
        public NativeAnalogSource(int analogIndex)
        {
            this.AnalogIndex = analogIndex;
        }

        public float GetValue(InputDevice inputDevice)
        {
            NativeInputDevice nativeInputDevice = inputDevice as NativeInputDevice;
            return nativeInputDevice.ReadRawAnalogValue(this.AnalogIndex);
        }

        public bool GetState(InputDevice inputDevice)
        {
            return Utility.IsNotZero(this.GetValue(inputDevice));
        }

        public int AnalogIndex;
    }
}
