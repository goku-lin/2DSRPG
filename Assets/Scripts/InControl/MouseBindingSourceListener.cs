using System;

namespace InControl
{
    public class MouseBindingSourceListener : BindingSourceListener
    {
        public void Reset()
        {
            this.detectFound = Mouse.None;
            this.detectPhase = 0;
        }

        public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
        {
            if (this.detectFound != Mouse.None && !this.IsPressed(this.detectFound) && this.detectPhase == 2)
            {
                MouseBindingSource result = new MouseBindingSource(this.detectFound);
                this.Reset();
                return result;
            }
            Mouse mouse = this.ListenForControl(listenOptions);
            if (mouse != Mouse.None)
            {
                if (this.detectPhase == 1)
                {
                    this.detectFound = mouse;
                    this.detectPhase = 2;
                }
            }
            else if (this.detectPhase == 0)
            {
                this.detectPhase = 1;
            }
            return null;
        }

        private bool IsPressed(Mouse control)
        {
            if (control == Mouse.NegativeScrollWheel)
            {
                return MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
            }
            if (control != Mouse.PositiveScrollWheel)
            {
                return MouseBindingSource.ButtonIsPressed(control);
            }
            return MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold);
        }

        private Mouse ListenForControl(BindingListenOptions listenOptions)
        {
            if (listenOptions.IncludeMouseButtons)
            {
                for (Mouse mouse = Mouse.None; mouse <= Mouse.Button9; mouse++)
                {
                    if (MouseBindingSource.ButtonIsPressed(mouse))
                    {
                        return mouse;
                    }
                }
            }
            if (listenOptions.IncludeMouseScrollWheel)
            {
                if (MouseBindingSource.NegativeScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
                {
                    return Mouse.NegativeScrollWheel;
                }
                if (MouseBindingSource.PositiveScrollWheelIsActive(MouseBindingSourceListener.ScrollWheelThreshold))
                {
                    return Mouse.PositiveScrollWheel;
                }
            }
            return Mouse.None;
        }

        public static float ScrollWheelThreshold = 0.001f;

        private Mouse detectFound;

        private int detectPhase;
    }
}
