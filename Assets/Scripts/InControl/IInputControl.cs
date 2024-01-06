using System;

namespace InControl
{
    /// <summary>
    /// 输入控制接口，定义了检查输入状态变化以及清除输入状态的方法。
    /// </summary>
    public interface IInputControl
    {
        /// <summary>
        /// 获取一个值，指示输入控制是否已发生变化。
        /// </summary>
        bool HasChanged { get; }

        /// <summary>
        /// 获取一个值，指示输入控制当前是否处于按下状态。
        /// </summary>
        bool IsPressed { get; }

        /// <summary>
        /// 获取一个值，指示输入控制是否在上一次状态中处于未按下状态，但在当前状态中处于按下状态。
        /// </summary>
        bool WasPressed { get; }

        /// <summary>
        /// 获取一个值，指示输入控制是否在上一次状态中处于按下状态，但在当前状态中处于未按下状态。
        /// </summary>
        bool WasReleased { get; }

        /// <summary>
        /// 清除输入控制的状态，通常在希望重新检测输入状态时使用。
        /// </summary>
        void ClearInputState();
    }
}
