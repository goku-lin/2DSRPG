using System;

namespace InControl
{
    public abstract class NativeInputDeviceProfile : InputDeviceProfile
    {
        public NativeInputDeviceProfile()
        {
            base.Sensitivity = 1f;
            base.LowerDeadZone = 0.2f;
            base.UpperDeadZone = 0.9f;
        }

        internal bool Matches(NativeDeviceInfo deviceInfo)
        {
            return this.Matches(deviceInfo, this.Matchers);
        }

        internal bool LastResortMatches(NativeDeviceInfo deviceInfo)
        {
            return this.Matches(deviceInfo, this.LastResortMatchers);
        }

        private bool Matches(NativeDeviceInfo deviceInfo, NativeInputDeviceMatcher[] matchers)
        {
            if (this.Matchers != null)
            {
                int num = this.Matchers.Length;
                for (int i = 0; i < num; i++)
                {
                    if (this.Matchers[i].Matches(deviceInfo))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected static InputControlSource Button(int index)
        {
            return new NativeButtonSource(index);
        }

        protected static InputControlSource Analog(int index)
        {
            return new NativeAnalogSource(index);
        }

        protected static InputControlMapping LeftStickLeftMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Left",
                Target = InputControlType.LeftStickLeft,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickRightMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Right",
                Target = InputControlType.LeftStickRight,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickUpMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Up",
                Target = InputControlType.LeftStickUp,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickDownMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Down",
                Target = InputControlType.LeftStickDown,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickUpMapping2(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Up",
                Target = InputControlType.LeftStickUp,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftStickDownMapping2(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Stick Down",
                Target = InputControlType.LeftStickDown,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickLeftMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Left",
                Target = InputControlType.RightStickLeft,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickRightMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Right",
                Target = InputControlType.RightStickRight,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickUpMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Up",
                Target = InputControlType.RightStickUp,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickDownMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Down",
                Target = InputControlType.RightStickDown,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickUpMapping2(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Up",
                Target = InputControlType.RightStickUp,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping RightStickDownMapping2(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Stick Down",
                Target = InputControlType.RightStickDown,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping LeftTriggerMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Left Trigger",
                Target = InputControlType.LeftTrigger,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.MinusOneToOne,
                TargetRange = InputRange.ZeroToOne,
                IgnoreInitialZeroValue = true
            };
        }

        protected static InputControlMapping RightTriggerMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "Right Trigger",
                Target = InputControlType.RightTrigger,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.MinusOneToOne,
                TargetRange = InputRange.ZeroToOne,
                IgnoreInitialZeroValue = true
            };
        }

        protected static InputControlMapping DPadLeftMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Left",
                Target = InputControlType.DPadLeft,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadRightMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Right",
                Target = InputControlType.DPadRight,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadUpMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Up",
                Target = InputControlType.DPadUp,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadDownMapping(int analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Down",
                Target = InputControlType.DPadDown,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadUpMapping2(int analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Up",
                Target = InputControlType.DPadUp,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        protected static InputControlMapping DPadDownMapping2(int analog)
        {
            return new InputControlMapping
            {
                Handle = "DPad Down",
                Target = InputControlType.DPadDown,
                Source = NativeInputDeviceProfile.Analog(analog),
                SourceRange = InputRange.ZeroToMinusOne,
                TargetRange = InputRange.ZeroToOne
            };
        }

        public NativeInputDeviceMatcher[] Matchers;

        public NativeInputDeviceMatcher[] LastResortMatchers;
    }
}
