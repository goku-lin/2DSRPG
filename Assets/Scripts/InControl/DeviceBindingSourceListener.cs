using System;

namespace InControl
{
    public class DeviceBindingSourceListener : BindingSourceListener
    {
        public void Reset()
        {
            this.detectFound = InputControlType.None;
            this.detectPhase = 0;
        }

        public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
        {
            if (!listenOptions.IncludeControllers || device.IsUnknown)
            {
                return null;
            }
            if (this.detectFound != InputControlType.None && !this.IsPressed(this.detectFound, device) && this.detectPhase == 2)
            {
                DeviceBindingSource result = new DeviceBindingSource(this.detectFound);
                this.Reset();
                return result;
            }
            InputControlType inputControlType = this.ListenForControl(listenOptions, device);
            if (inputControlType != InputControlType.None)
            {
                if (this.detectPhase == 1)
                {
                    this.detectFound = inputControlType;
                    this.detectPhase = 2;
                }
            }
            else if (this.detectPhase == 0)
            {
                this.detectPhase = 1;
            }
            return null;
        }

        private bool IsPressed(InputControl control)
        {
            return Utility.AbsoluteIsOverThreshold(control.Value, 0.5f);
        }

        private bool IsPressed(InputControlType control, InputDevice device)
        {
            return this.IsPressed(device.GetControl(control));
        }

        private InputControlType ListenForControl(BindingListenOptions listenOptions, InputDevice device)
        {
            if (device.IsKnown)
            {
                int count = device.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    InputControl inputControl = device.Controls[i];
                    if (inputControl != null && this.IsPressed(inputControl) && (listenOptions.IncludeNonStandardControls || inputControl.IsStandard))
                    {
                        InputControlType target = inputControl.Target;
                        if (target != InputControlType.Command || !listenOptions.IncludeNonStandardControls)
                        {
                            return target;
                        }
                    }
                }
            }
            return InputControlType.None;
        }

        private InputControlType detectFound;

        private int detectPhase;
    }
}
