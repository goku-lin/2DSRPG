using System;
//using XInputDotNetPure;

namespace InControl
{
    public class XInputDevice : InputDevice
    {
        public XInputDevice(int deviceIndex, XInputDeviceManager owner) : base("XInput Controller")
        {
            this.owner = owner;
            this.DeviceIndex = deviceIndex;
            base.SortOrder = deviceIndex;
            base.Meta = "XInput Device #" + deviceIndex;
            base.DeviceClass = InputDeviceClass.Controller;
            base.DeviceStyle = InputDeviceStyle.XboxOne;
            base.AddControl(InputControlType.LeftStickLeft, "Left Stick Left", 0.2f, 0.9f);
            base.AddControl(InputControlType.LeftStickRight, "Left Stick Right", 0.2f, 0.9f);
            base.AddControl(InputControlType.LeftStickUp, "Left Stick Up", 0.2f, 0.9f);
            base.AddControl(InputControlType.LeftStickDown, "Left Stick Down", 0.2f, 0.9f);
            base.AddControl(InputControlType.RightStickLeft, "Right Stick Left", 0.2f, 0.9f);
            base.AddControl(InputControlType.RightStickRight, "Right Stick Right", 0.2f, 0.9f);
            base.AddControl(InputControlType.RightStickUp, "Right Stick Up", 0.2f, 0.9f);
            base.AddControl(InputControlType.RightStickDown, "Right Stick Down", 0.2f, 0.9f);
            base.AddControl(InputControlType.LeftTrigger, "Left Trigger", 0.2f, 0.9f);
            base.AddControl(InputControlType.RightTrigger, "Right Trigger", 0.2f, 0.9f);
            base.AddControl(InputControlType.DPadUp, "DPad Up", 0.2f, 0.9f);
            base.AddControl(InputControlType.DPadDown, "DPad Down", 0.2f, 0.9f);
            base.AddControl(InputControlType.DPadLeft, "DPad Left", 0.2f, 0.9f);
            base.AddControl(InputControlType.DPadRight, "DPad Right", 0.2f, 0.9f);
            base.AddControl(InputControlType.Action1, "A");
            base.AddControl(InputControlType.Action2, "B");
            base.AddControl(InputControlType.Action3, "X");
            base.AddControl(InputControlType.Action4, "Y");
            base.AddControl(InputControlType.LeftBumper, "Left Bumper");
            base.AddControl(InputControlType.RightBumper, "Right Bumper");
            base.AddControl(InputControlType.LeftStickButton, "Left Stick Button");
            base.AddControl(InputControlType.RightStickButton, "Right Stick Button");
            base.AddControl(InputControlType.Start, "Start");
            base.AddControl(InputControlType.Back, "Back");
        }

        // (get) Token: 0x060006DF RID: 1759 RVA: 0x000455A9 File Offset: 0x000439A9
        // (set) Token: 0x060006E0 RID: 1760 RVA: 0x000455B1 File Offset: 0x000439B1
        public int DeviceIndex { get; private set; }

        public override void Update(ulong updateTick, float deltaTime)
        {
            this.GetState();
            //base.UpdateLeftStickWithValue(this.state.ThumbSticks.Left.Vector, updateTick, deltaTime);
            //base.UpdateRightStickWithValue(this.state.ThumbSticks.Right.Vector, updateTick, deltaTime);
            //base.UpdateWithValue(InputControlType.LeftTrigger, this.state.Triggers.Left, updateTick, deltaTime);
            //base.UpdateWithValue(InputControlType.RightTrigger, this.state.Triggers.Right, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.DPadUp, this.state.DPad.Up == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.DPadDown, this.state.DPad.Down == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.DPadLeft, this.state.DPad.Left == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.DPadRight, this.state.DPad.Right == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.Action1, this.state.Buttons.A == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.Action2, this.state.Buttons.B == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.Action3, this.state.Buttons.X == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.Action4, this.state.Buttons.Y == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.LeftBumper, this.state.Buttons.LeftShoulder == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.RightBumper, this.state.Buttons.RightShoulder == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.LeftStickButton, this.state.Buttons.LeftStick == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.RightStickButton, this.state.Buttons.RightStick == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.Start, this.state.Buttons.Start == ButtonState.Pressed, updateTick, deltaTime);
            //base.UpdateWithState(InputControlType.Back, this.state.Buttons.Back == ButtonState.Pressed, updateTick, deltaTime);
            base.Commit(updateTick, deltaTime);
        }

        public override void Vibrate(float leftMotor, float rightMotor)
        {
            //GamePad.SetVibration((PlayerIndex)this.DeviceIndex, leftMotor, rightMotor);
        }

        internal void GetState()
        {
            //this.state = this.owner.GetState(this.DeviceIndex);
        }

        //public bool IsConnected
        //{
        //    get
        //    {
        //        return this.state.IsConnected;
        //    }
        //}

        private const float LowerDeadZone = 0.2f;

        private const float UpperDeadZone = 0.9f;

        private XInputDeviceManager owner;

        //private GamePadState state;
    }
}
