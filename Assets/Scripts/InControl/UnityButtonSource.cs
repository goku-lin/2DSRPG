using System;

namespace InControl
{
    public class UnityButtonSource : InputControlSource
    {
        public UnityButtonSource(int buttonIndex)
        {
            this.ButtonIndex = buttonIndex;
        }

        public float GetValue(InputDevice inputDevice)
        {
            return (!this.GetState(inputDevice)) ? 0f : 1f;
        }

        public bool GetState(InputDevice inputDevice)
        {
            UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
            return unityInputDevice.ReadRawButtonState(this.ButtonIndex);
        }

        public int ButtonIndex;
    }
}
