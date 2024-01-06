using System;
using UnityEngine;

namespace InControl
{
    public struct InputRange
    {
        private InputRange(float value0, float value1, InputRangeType type)
        {
            this.Value0 = value0;
            this.Value1 = value1;
            this.Type = type;
        }

        public InputRange(InputRangeType type)
        {
            this.Value0 = InputRange.TypeToRange[(int)type].Value0;
            this.Value1 = InputRange.TypeToRange[(int)type].Value1;
            this.Type = type;
        }

        public bool Includes(float value)
        {
            return !this.Excludes(value);
        }

        public bool Excludes(float value)
        {
            return this.Type == InputRangeType.None || value < Mathf.Min(this.Value0, this.Value1) || value > Mathf.Max(this.Value0, this.Value1);
        }

        public static float Remap(float value, InputRange sourceRange, InputRange targetRange)
        {
            if (sourceRange.Excludes(value))
            {
                return 0f;
            }
            float t = Mathf.InverseLerp(sourceRange.Value0, sourceRange.Value1, value);
            return Mathf.Lerp(targetRange.Value0, targetRange.Value1, t);
        }

        internal static float Remap(float value, InputRangeType sourceRangeType, InputRangeType targetRangeType)
        {
            InputRange sourceRange = InputRange.TypeToRange[(int)sourceRangeType];
            InputRange targetRange = InputRange.TypeToRange[(int)targetRangeType];
            return InputRange.Remap(value, sourceRange, targetRange);
        }

        public static readonly InputRange None = new InputRange(0f, 0f, InputRangeType.None);

        public static readonly InputRange MinusOneToOne = new InputRange(-1f, 1f, InputRangeType.MinusOneToOne);

        public static readonly InputRange OneToMinusOne = new InputRange(1f, -1f, InputRangeType.OneToMinusOne);

        public static readonly InputRange ZeroToOne = new InputRange(0f, 1f, InputRangeType.ZeroToOne);

        public static readonly InputRange ZeroToMinusOne = new InputRange(0f, -1f, InputRangeType.ZeroToMinusOne);

        public static readonly InputRange OneToZero = new InputRange(1f, 0f, InputRangeType.OneToZero);

        public static readonly InputRange MinusOneToZero = new InputRange(-1f, 0f, InputRangeType.MinusOneToZero);

        public static readonly InputRange ZeroToNegativeInfinity = new InputRange(0f, float.NegativeInfinity, InputRangeType.ZeroToNegativeInfinity);

        public static readonly InputRange ZeroToPositiveInfinity = new InputRange(0f, float.PositiveInfinity, InputRangeType.ZeroToPositiveInfinity);

        public static readonly InputRange Everything = new InputRange(float.NegativeInfinity, float.PositiveInfinity, InputRangeType.Everything);

        private static readonly InputRange[] TypeToRange = new InputRange[]
        {
            InputRange.None,
            InputRange.MinusOneToOne,
            InputRange.OneToMinusOne,
            InputRange.ZeroToOne,
            InputRange.ZeroToMinusOne,
            InputRange.OneToZero,
            InputRange.MinusOneToZero,
            InputRange.ZeroToNegativeInfinity,
            InputRange.ZeroToPositiveInfinity,
            InputRange.Everything
        };

        public readonly float Value0;

        public readonly float Value1;

        public readonly InputRangeType Type;
    }
}
