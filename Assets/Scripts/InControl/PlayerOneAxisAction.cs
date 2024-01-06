using System;
using System.Diagnostics;

namespace InControl
{
    public class PlayerOneAxisAction : OneAxisInputControl
    {
        internal PlayerOneAxisAction(PlayerAction negativeAction, PlayerAction positiveAction)
        {
            this.negativeAction = negativeAction;
            this.positiveAction = positiveAction;
            this.Raw = true;
        }

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public event Action<BindingSourceType> OnLastInputTypeChanged;

        public object UserData { get; set; }

        internal void Update(ulong updateTick, float deltaTime)
        {
            this.ProcessActionUpdate(this.negativeAction);
            this.ProcessActionUpdate(this.positiveAction);
            float value = Utility.ValueFromSides(this.negativeAction, this.positiveAction);
            base.CommitWithValue(value, updateTick, deltaTime);
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

        private PlayerAction negativeAction;

        private PlayerAction positiveAction;

        public BindingSourceType LastInputType;
    }
}
