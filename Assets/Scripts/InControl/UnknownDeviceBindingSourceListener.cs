using System;

namespace InControl
{
    public class UnknownDeviceBindingSourceListener : BindingSourceListener
    {
        public void Reset()
        {
            this.detectFound = UnknownDeviceControl.None;
            this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease;
            this.TakeSnapshotOnUnknownDevices();
        }

        private void TakeSnapshotOnUnknownDevices()
        {
            int count = InputManager.Devices.Count;
            for (int i = 0; i < count; i++)
            {
                InputDevice inputDevice = InputManager.Devices[i];
                if (inputDevice.IsUnknown)
                {
                    inputDevice.TakeSnapshot();
                }
            }
        }

        public BindingSource Listen(BindingListenOptions listenOptions, InputDevice device)
        {
            if (!listenOptions.IncludeUnknownControllers || device.IsKnown)
            {
                return null;
            }
            if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease && this.detectFound && !this.IsPressed(this.detectFound, device))
            {
                UnknownDeviceBindingSource result = new UnknownDeviceBindingSource(this.detectFound);
                this.Reset();
                return result;
            }
            UnknownDeviceControl control = this.ListenForControl(listenOptions, device);
            if (control)
            {
                if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress)
                {
                    this.detectFound = control;
                    this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlRelease;
                }
            }
            else if (this.detectPhase == UnknownDeviceBindingSourceListener.DetectPhase.WaitForInitialRelease)
            {
                this.detectPhase = UnknownDeviceBindingSourceListener.DetectPhase.WaitForControlPress;
            }
            return null;
        }

        private bool IsPressed(UnknownDeviceControl control, InputDevice device)
        {
            float value = control.GetValue(device);
            return Utility.AbsoluteIsOverThreshold(value, 0.5f);
        }

        private UnknownDeviceControl ListenForControl(BindingListenOptions listenOptions, InputDevice device)
        {
            if (device.IsUnknown)
            {
                UnknownDeviceControl firstPressedButton = device.GetFirstPressedButton();
                if (firstPressedButton)
                {
                    return firstPressedButton;
                }
                UnknownDeviceControl firstPressedAnalog = device.GetFirstPressedAnalog();
                if (firstPressedAnalog)
                {
                    return firstPressedAnalog;
                }
            }
            return UnknownDeviceControl.None;
        }

        private UnknownDeviceControl detectFound;

        private UnknownDeviceBindingSourceListener.DetectPhase detectPhase;

        private enum DetectPhase
        {
            WaitForInitialRelease,
            WaitForControlPress,
            WaitForControlRelease
        }
    }
}
