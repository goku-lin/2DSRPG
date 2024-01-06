using System;

namespace InControl
{
    public abstract class UnityInputDeviceProfileBase : InputDeviceProfile
    {
        public abstract bool IsJoystick { get; }

        public abstract bool HasJoystickName(string joystickName);

        public abstract bool HasLastResortRegex(string joystickName);

        public abstract bool HasJoystickOrRegexName(string joystickName);

        public bool IsNotJoystick
        {
            get
            {
                return !this.IsJoystick;
            }
        }
    }
}
