using System;
using System.Diagnostics;

namespace InControl
{
    public class PlayerTwoAxisAction : TwoAxisInputControl
    {
        internal PlayerTwoAxisAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
        {
            this.negativeXAction = negativeXAction;
            this.positiveXAction = positiveXAction;
            this.negativeYAction = negativeYAction;
            this.positiveYAction = positiveYAction;
            this.InvertXAxis = false;
            this.InvertYAxis = false;
            this.Raw = true;
        }

        public bool InvertXAxis { get; set; }

        public bool InvertYAxis { get; set; }

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action<BindingSourceType> OnLastInputTypeChanged;

        public object UserData { get; set; }

        internal void Update(ulong updateTick, float deltaTime)
        {
            this.ProcessActionUpdate(this.negativeXAction);
            this.ProcessActionUpdate(this.positiveXAction);
            this.ProcessActionUpdate(this.negativeYAction);
            this.ProcessActionUpdate(this.positiveYAction);
            float x = Utility.ValueFromSides(this.negativeXAction, this.positiveXAction, this.InvertXAxis);
            float y = Utility.ValueFromSides(this.negativeYAction, this.positiveYAction, InputManager.InvertYAxis || this.InvertYAxis);
            base.UpdateWithAxes(x, y, updateTick, deltaTime);
        }

        private void ProcessActionUpdate(PlayerAction action)
        {
            BindingSourceType lastInputType = this.LastInputType;
            if (action.UpdateTick > base.UpdateTick)
            {
                base.UpdateTick = action.UpdateTick;
                lastInputType = action.LastInputType;
            }
            if (this.LastInputType != lastInputType)
            {
                this.LastInputType = lastInputType;
                if (this.OnLastInputTypeChanged != null)
                {
                    this.OnLastInputTypeChanged(lastInputType);
                }
            }
        }

        [Obsolete("Please set this property on device controls directly. It does nothing here.")]
        public new float LowerDeadZone
        {
            get
            {
                return 0f;
            }
            set
            {
            }
        }

        [Obsolete("Please set this property on device controls directly. It does nothing here.")]
        public new float UpperDeadZone
        {
            get
            {
                return 0f;
            }
            set
            {
            }
        }

        private PlayerAction negativeXAction;

        private PlayerAction positiveXAction;

        private PlayerAction negativeYAction;

        private PlayerAction positiveYAction;

        public BindingSourceType LastInputType;
    }
}
