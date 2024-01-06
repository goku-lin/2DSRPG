using System;

namespace InControl
{
    public class BindingListenOptions
    {
        public bool CallOnBindingFound(PlayerAction playerAction, BindingSource bindingSource)
        {
            return this.OnBindingFound == null || this.OnBindingFound(playerAction, bindingSource);
        }

        public void CallOnBindingAdded(PlayerAction playerAction, BindingSource bindingSource)
        {
            if (this.OnBindingAdded != null)
            {
                this.OnBindingAdded(playerAction, bindingSource);
            }
        }

        public void CallOnBindingRejected(PlayerAction playerAction, BindingSource bindingSource, BindingSourceRejectionType bindingSourceRejectionType)
        {
            if (this.OnBindingRejected != null)
            {
                this.OnBindingRejected(playerAction, bindingSource, bindingSourceRejectionType);
            }
        }

        public bool IncludeControllers = true;

        public bool IncludeUnknownControllers;

        public bool IncludeNonStandardControls = true;

        public bool IncludeMouseButtons;

        public bool IncludeMouseScrollWheel;

        public bool IncludeKeys = true;

        public bool IncludeModifiersAsFirstClassKeys;

        public uint MaxAllowedBindings;

        public uint MaxAllowedBindingsPerType;

        public bool AllowDuplicateBindingsPerSet;

        public bool UnsetDuplicateBindingsOnSet;

        public bool RejectRedundantBindings;

        public BindingSource ReplaceBinding;

        public Func<PlayerAction, BindingSource, bool> OnBindingFound;

        public Action<PlayerAction, BindingSource> OnBindingAdded;

        public Action<PlayerAction, BindingSource, BindingSourceRejectionType> OnBindingRejected;
    }
}
