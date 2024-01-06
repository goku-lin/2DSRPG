using System;

namespace InControl
{
    public class NativeButtonSource : InputControlSource
    {
        public NativeButtonSource(int buttonIndex)
        {
            this.ButtonIndex = buttonIndex;
        }

        public float GetValue(InputDevice inputDevice)
        {
            return (!this.GetState(inputDevice)) ? 0f : 1f;
        }

        public bool GetState(InputDevice inputDevice)
        {
            NativeInputDevice nativeInputDevice = inputDevice as NativeInputDevice;
            return nativeInputDevice.ReadRawButtonState(this.ButtonIndex);
        }

        public int ButtonIndex;
    }
}
