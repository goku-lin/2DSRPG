using System;
using UnityEngine;

namespace InControl
{
    public class InputControlMapping
    {
        public float MapValue(float value)
        {
            if (this.Raw)
            {
                value *= this.Scale;
                value = ((!this.SourceRange.Excludes(value)) ? value : 0f);
            }
            else
            {
                value = Mathf.Clamp(value * this.Scale, -1f, 1f);
                value = InputRange.Remap(value, this.SourceRange, this.TargetRange);
            }
            if (this.Invert)
            {
                value = -value;
            }
            return value;
        }

        public string Handle
        {
            get
            {
                return (!string.IsNullOrEmpty(this.handle)) ? this.handle : this.Target.ToString();
            }
            set
            {
                this.handle = value;
            }
        }

        public InputControlSource Source;

        public InputControlType Target;

        public bool Invert;

        public float Scale = 1f;

        public bool Raw;

        public bool Passive;

        public bool IgnoreInitialZeroValue;

        public float Sensitivity = 1f;

        public float LowerDeadZone;

        public float UpperDeadZone = 1f;

        public InputRange SourceRange = InputRange.MinusOneToOne;

        public InputRange TargetRange = InputRange.MinusOneToOne;

        private string handle;
    }
}
