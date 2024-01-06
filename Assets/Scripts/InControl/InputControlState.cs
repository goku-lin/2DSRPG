using System;

namespace InControl
{
    /// <summary>
    /// 表示输入控制的状态，包括当前状态、值和原始值。
    /// </summary>
    public struct InputControlState
    {
        /// <summary>
        /// 重置输入控制的状态，将状态设置为未按下，值和原始值设置为0。
        /// </summary>
        public void Reset()
        {
            this.State = false;
            this.Value = 0f;
            this.RawValue = 0f;
        }

        /// <summary>
        /// 设置输入控制的状态和值。
        /// </summary>
        /// <param name="value">要设置的值。</param>
        public void Set(float value)
        {
            this.Value = value;
            this.State = Utility.IsNotZero(value);
        }

        /// <summary>
        /// 设置输入控制的状态和值，并指定阈值。
        /// </summary>
        /// <param name="value">要设置的值。</param>
        /// <param name="threshold">状态的阈值。</param>
        public void Set(float value, float threshold)
        {
            this.Value = value;
            this.State = Utility.AbsoluteIsOverThreshold(value, threshold);
        }

        /// <summary>
        /// 设置输入控制的状态和值，同时将原始值设置为相同的值。
        /// </summary>
        /// <param name="state">要设置的状态。</param>
        public void Set(bool state)
        {
            this.State = state;
            this.Value = state ? 1f : 0f;
            this.RawValue = this.Value;
        }

        /// <summary>
        /// 隐式转换将输入控制状态转换为布尔值。
        /// </summary>
        public static implicit operator bool(InputControlState state)
        {
            return state.State;
        }

        /// <summary>
        /// 隐式转换将输入控制状态转换为浮点数值。
        /// </summary>
        public static implicit operator float(InputControlState state)
        {
            return state.Value;
        }

        /// <summary>
        /// 检查两个输入控制状态是否相等。
        /// </summary>
        public static bool operator ==(InputControlState a, InputControlState b)
        {
            return a.State == b.State && Utility.Approximately(a.Value, b.Value);
        }

        /// <summary>
        /// 检查两个输入控制状态是否不相等。
        /// </summary>
        public static bool operator !=(InputControlState a, InputControlState b)
        {
            return a.State != b.State || !Utility.Approximately(a.Value, b.Value);
        }

        /// <summary>
        /// 当前输入控制的状态（按下/未按下）。
        /// </summary>
        public bool State;

        /// <summary>
        /// 当前输入控制的值。
        /// </summary>
        public float Value;

        /// <summary>
        /// 当前输入控制的原始值。
        /// </summary>
        public float RawValue;
    }
}
