using System;

namespace InControl
{
    public class UnityAnalogSource : InputControlSource
    {
        public UnityAnalogSource(int analogIndex)
        {
            this.AnalogIndex = analogIndex;
        }

        public float GetValue(InputDevice inputDevice)
        {
            UnityInputDevice unityInputDevice = inputDevice as UnityInputDevice;
            return unityInputDevice.ReadRawAnalogValue(this.AnalogIndex);
        }

        public bool GetState(InputDevice inputDevice)
        {
            return Utility.IsNotZero(this.GetValue(inputDevice));
        }

        public int AnalogIndex;
    }
}
