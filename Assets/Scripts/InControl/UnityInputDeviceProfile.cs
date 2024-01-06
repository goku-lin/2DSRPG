using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
    public class UnityInputDeviceProfile : UnityInputDeviceProfileBase
    {
        public UnityInputDeviceProfile()
        {
            base.Sensitivity = 1f;
            base.LowerDeadZone = 0.2f;
            base.UpperDeadZone = 0.9f;
            this.MinUnityVersion = VersionInfo.Min;
            this.MaxUnityVersion = VersionInfo.Max;
        }

        [SerializeField]
        public VersionInfo MinUnityVersion { get; protected set; }

        [SerializeField]
        public VersionInfo MaxUnityVersion { get; protected set; }

        public override bool IsJoystick
        {
            get
            {
                return this.LastResortRegex != null || (this.JoystickNames != null && this.JoystickNames.Length > 0) || (this.JoystickRegex != null && this.JoystickRegex.Length > 0);
            }
        }

        public override bool HasJoystickName(string joystickName)
        {
            if (base.IsNotJoystick)
            {
                return false;
            }
            if (this.JoystickNames != null && this.JoystickNames.Contains(joystickName, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }
            if (this.JoystickRegex != null)
            {
                for (int i = 0; i < this.JoystickRegex.Length; i++)
                {
                    if (Regex.IsMatch(joystickName, this.JoystickRegex[i], RegexOptions.IgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public override bool HasLastResortRegex(string joystickName)
        {
            return !base.IsNotJoystick && this.LastResortRegex != null && Regex.IsMatch(joystickName, this.LastResortRegex, RegexOptions.IgnoreCase);
        }

        public override bool HasJoystickOrRegexName(string joystickName)
        {
            return this.HasJoystickName(joystickName) || this.HasLastResortRegex(joystickName);
        }

        public override bool IsSupportedOnThisPlatform
        {
            get
            {
                return this.IsSupportedOnThisVersionOfUnity && base.IsSupportedOnThisPlatform;
            }
        }

        private bool IsSupportedOnThisVersionOfUnity
        {
            get
            {
                VersionInfo a = VersionInfo.UnityVersion();
                return a >= this.MinUnityVersion && a <= this.MaxUnityVersion;
            }
        }

        protected static InputControlSource Button(int index)
        {
            return new UnityButtonSource(index);
        }

        protected static InputControlSource Analog(int index)
        {
            return new UnityAnalogSource(index);
        }

        protected static InputControlMapping LeftStickLeftMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Left",
                Target = InputControlType.LeftStickLeft,
                Source = analog,
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickRightMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Right",
                Target = InputControlType.LeftStickRight,
                Source = analog,
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickUpMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Up",
                Target = InputControlType.LeftStickUp,
                Source = analog,
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickDownMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Down",
                Target = InputControlType.LeftStickDown,
                Source = analog,
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickLeftMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Left",
                Target = InputControlType.RightStickLeft,
                Source = analog,
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickRightMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Right",
                Target = InputControlType.RightStickRight,
                Source = analog,
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickUpMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Up",
                Target = InputControlType.RightStickUp,
                Source = analog,
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickDownMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Down",
                Target = InputControlType.RightStickDown,
                Source = analog,
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftTriggerMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Trigger",
                Target = InputControlType.LeftTrigger,
                Source = analog,
                SourceRange = InputRange.MinusOneToOne,
                TargetRange = InputRange.ZeroToOne,
                IgnoreInitialZeroValue = true
            };
        }

        protected static InputControlMapping RightTriggerMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Trigger",
                Target = InputControlType.RightTrigger,
                Source = analog,
                SourceRange = InputRange.MinusOneToOne,
                TargetRange = InputRange.ZeroToOne,
                IgnoreInitialZeroValue = true
            };
        }

        protected static InputControlMapping DPadLeftMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Left",
                Target = InputControlType.DPadLeft,
                Source = analog,
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadRightMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Right",
                Target = InputControlType.DPadRight,
                Source = analog,
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadUpMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Up",
                Target = InputControlType.DPadUp,
                Source = analog,
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadDownMapping(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Down",
                Target = InputControlType.DPadDown,
                Source = analog,
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadUpMapping2(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Up",
                Target = InputControlType.DPadUp,
                Source = analog,
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadDownMapping2(InputControlSource analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Down",
                Target = InputControlType.DPadDown,
                Source = analog,
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        [SerializeField]
        protected string[] JoystickNames;

        [SerializeField]
        protected string[] JoystickRegex;

        [SerializeField]
        protected string LastResortRegex;

        protected static InputControlSource Button0 = UnityInputDeviceProfile.Button(0);

        protected static InputControlSource Button1 = UnityInputDeviceProfile.Button(1);

        protected static InputControlSource Button2 = UnityInputDeviceProfile.Button(2);

        protected static InputControlSource Button3 = UnityInputDeviceProfile.Button(3);

        protected static InputControlSource Button4 = UnityInputDeviceProfile.Button(4);

        protected static InputControlSource Button5 = UnityInputDeviceProfile.Button(5);

        protected static InputControlSource Button6 = UnityInputDeviceProfile.Button(6);

        protected static InputControlSource Button7 = UnityInputDeviceProfile.Button(7);

        protected static InputControlSource Button8 = UnityInputDeviceProfile.Button(8);

        protected static InputControlSource Button9 = UnityInputDeviceProfile.Button(9);

        protected static InputControlSource Button10 = UnityInputDeviceProfile.Button(10);

        protected static InputControlSource Button11 = UnityInputDeviceProfile.Button(11);

        protected static InputControlSource Button12 = UnityInputDeviceProfile.Button(12);

        protected static InputControlSource Button13 = UnityInputDeviceProfile.Button(13);

        protected static InputControlSource Button14 = UnityInputDeviceProfile.Button(14);

        protected static InputControlSource Button15 = UnityInputDeviceProfile.Button(15);

        protected static InputControlSource Button16 = UnityInputDeviceProfile.Button(16);

        protected static InputControlSource Button17 = UnityInputDeviceProfile.Button(17);

        protected static InputControlSource Button18 = UnityInputDeviceProfile.Button(18);

        protected static InputControlSource Button19 = UnityInputDeviceProfile.Button(19);

        protected static InputControlSource Analog0 = UnityInputDeviceProfile.Analog(0);

        protected static InputControlSource Analog1 = UnityInputDeviceProfile.Analog(1);

        protected static InputControlSource Analog2 = UnityInputDeviceProfile.Analog(2);

        protected static InputControlSource Analog3 = UnityInputDeviceProfile.Analog(3);

        protected static InputControlSource Analog4 = UnityInputDeviceProfile.Analog(4);

        protected static InputControlSource Analog5 = UnityInputDeviceProfile.Analog(5);

        protected static InputControlSource Analog6 = UnityInputDeviceProfile.Analog(6);

        protected static InputControlSource Analog7 = UnityInputDeviceProfile.Analog(7);

        protected static InputControlSource Analog8 = UnityInputDeviceProfile.Analog(8);

        protected static InputControlSource Analog9 = UnityInputDeviceProfile.Analog(9);

        protected static InputControlSource Analog10 = UnityInputDeviceProfile.Analog(10);

        protected static InputControlSource Analog11 = UnityInputDeviceProfile.Analog(11);

        protected static InputControlSource Analog12 = UnityInputDeviceProfile.Analog(12);

        protected static InputControlSource Analog13 = UnityInputDeviceProfile.Analog(13);

        protected static InputControlSource Analog14 = UnityInputDeviceProfile.Analog(14);

        protected static InputControlSource Analog15 = UnityInputDeviceProfile.Analog(15);

        protected static InputControlSource Analog16 = UnityInputDeviceProfile.Analog(16);

        protected static InputControlSource Analog17 = UnityInputDeviceProfile.Analog(17);

        protected static InputControlSource Analog18 = UnityInputDeviceProfile.Analog(18);

        protected static InputControlSource Analog19 = UnityInputDeviceProfile.Analog(19);

        protected static InputControlSource MenuKey = new UnityKeyCodeSource(new KeyCode[]
{
            KeyCode.Menu
});

        protected static InputControlSource EscapeKey = new UnityKeyCodeSource(new KeyCode[]
{
            KeyCode.Escape
});

        protected static InputControlSource MouseButton0 = new UnityMouseButtonSource(0);

        protected static InputControlSource MouseButton1 = new UnityMouseButtonSource(1);

        protected static InputControlSource MouseButton2 = new UnityMouseButtonSource(2);

        protected static InputControlSource MouseXAxis = new UnityMouseAxisSource("x");

        protected static InputControlSource MouseYAxis = new UnityMouseAxisSource("y");

        protected static InputControlSource MouseScrollWheel = new UnityMouseAxisSource("z");
    }
}
