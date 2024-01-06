using System;
using UnityEngine;

namespace InControl
{
    // 一维输入控制类，实现 IInputControl 接口
    public class OneAxisInputControl : IInputControl
    {
        private float sensitivity = 1f;                // 灵敏度，默认值为 1
        private float lowerDeadZone;                   // 下限死区
        private float upperDeadZone = 1f;              // 上限死区，默认值为 1
        private float stateThreshold;                  // 状态阈值
        public float FirstRepeatDelay = 0.8f;          // 第一次重复输入的延迟，默认值为 0.8
        public float RepeatDelay = 0.1f;               // 重复输入的延迟，默认值为 0.1
        public bool Raw;                               // 是否使用原始值
        internal bool Enabled = true;                  // 是否启用
        private ulong pendingTick;                     // 待处理的时间戳
        private bool pendingCommit;                    // 是否有待提交的更改
        private float nextRepeatTime;                  // 下一次重复输入的时间
        private float lastPressedTime;                 // 上一次按下的时间
        private bool wasRepeated;                     // 是否已重复输入
        private bool clearInputState;                 // 是否清除输入状态
        private InputControlState lastState;          // 上一次的输入控制状态
        private InputControlState nextState;          // 下一次的输入控制状态
        private InputControlState thisState;          // 当前的输入控制状态

        // 获取或设置最后更新的时间戳
        public ulong UpdateTick { get; protected set; }

        // 准备更新，确保在处理更新前的必要检查
        private void PrepareForUpdate(ulong updateTick)
        {
            if (this.IsNull)
            {
                return;
            }
            if (updateTick < this.pendingTick)
            {
                throw new InvalidOperationException("Cannot be updated with an earlier tick.");
            }
            if (this.pendingCommit && updateTick != this.pendingTick)
            {
                throw new InvalidOperationException("Cannot be updated for a new tick until pending tick is committed.");
            }
            if (updateTick > this.pendingTick)
            {
                this.lastState = this.thisState;
                this.nextState.Reset();
                this.pendingTick = updateTick;
                this.pendingCommit = true;
            }
        }

        // 更新状态，返回最新的状态
        public bool UpdateWithState(bool state, ulong updateTick, float deltaTime)
        {
            if (this.IsNull)
            {
                return false;
            }
            this.PrepareForUpdate(updateTick);
            this.nextState.Set(state || this.nextState.State);
            return state;
        }

        // 更新值，返回是否更新成功
        public bool UpdateWithValue(float value, ulong updateTick, float deltaTime)
        {
            if (this.IsNull)
            {
                return false;
            }
            this.PrepareForUpdate(updateTick);
            if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
            {
                this.nextState.RawValue = value;
                if (!this.Raw)
                {
                    value = Utility.ApplyDeadZone(value, this.lowerDeadZone, this.upperDeadZone);
                }
                this.nextState.Set(value, this.stateThreshold);
                return true;
            }
            return false;
        }

        // 使用原始值更新状态，返回是否更新成功
        internal bool UpdateWithRawValue(float value, ulong updateTick, float deltaTime)
        {
            if (this.IsNull)
            {
                return false;
            }
            this.Raw = true;
            this.PrepareForUpdate(updateTick);
            if (Utility.Abs(value) > Utility.Abs(this.nextState.RawValue))
            {
                this.nextState.RawValue = value;
                this.nextState.Set(value, this.stateThreshold);
                return true;
            }
            return false;
        }

        // 设置值并更新，返回是否更新成功
        internal void SetValue(float value, ulong updateTick)
        {
            if (this.IsNull)
            {
                return;
            }
            if (updateTick > this.pendingTick)
            {
                this.lastState = this.thisState;
                this.nextState.Reset();
                this.pendingTick = updateTick;
                this.pendingCommit = true;
            }
            this.nextState.RawValue = value;
            this.nextState.Set(value, this.StateThreshold);
        }

        // 清除输入状态
        public void ClearInputState()
        {
            this.lastState.Reset();
            this.thisState.Reset();
            this.nextState.Reset();
            this.wasRepeated = false;
            this.clearInputState = true;
        }

        // 提交当前状态
        public void Commit()
        {
            if (this.IsNull)
            {
                return;
            }
            this.pendingCommit = false;
            this.thisState = this.nextState;
            if (this.clearInputState)
            {
                this.lastState = this.nextState;
                this.UpdateTick = this.pendingTick;
                this.clearInputState = false;
                return;
            }
            bool state = this.lastState.State;
            bool state2 = this.thisState.State;
            this.wasRepeated = false;
            if (state && !state2)
            {
                this.nextRepeatTime = 0f;
            }
            else if (state2)
            {
                if (state != state2)
                {
                    this.nextRepeatTime = Time.realtimeSinceStartup + this.FirstRepeatDelay;
                }
                else if (Time.realtimeSinceStartup >= this.nextRepeatTime)
                {
                    this.wasRepeated = true;
                    this.nextRepeatTime = Time.realtimeSinceStartup + this.RepeatDelay;
                }
            }
            if (this.thisState != this.lastState)
            {
                this.UpdateTick = this.pendingTick;
            }
        }

        // 提交当前状态并使用给定的状态值
        public void CommitWithState(bool state, ulong updateTick, float deltaTime)
        {
            this.UpdateWithState(state, updateTick, deltaTime);
            this.Commit();
        }

        // 提交当前状态并使用给定的值
        public void CommitWithValue(float value, ulong updateTick, float deltaTime)
        {
            this.UpdateWithValue(value, updateTick, deltaTime);
            this.Commit();
        }

        // 提交当前状态并使用两个输入控制的值
        internal void CommitWithSides(InputControl negativeSide, InputControl positiveSide, ulong updateTick, float deltaTime)
        {
            this.LowerDeadZone = Mathf.Max(negativeSide.LowerDeadZone, positiveSide.LowerDeadZone);
            this.UpperDeadZone = Mathf.Min(negativeSide.UpperDeadZone, positiveSide.UpperDeadZone);
            this.Raw = (negativeSide.Raw || positiveSide.Raw);
            float value = Utility.ValueFromSides(negativeSide.RawValue, positiveSide.RawValue);
            this.CommitWithValue(value, updateTick, deltaTime);
        }

        // 获取当前状态
        public bool State
        {
            get
            {
                return this.Enabled && this.thisState.State;
            }
        }

        // 获取上一个状态
        public bool LastState
        {
            get
            {
                return this.Enabled && this.lastState.State;
            }
        }

        // 获取当前值
        public float Value
        {
            get
            {
                return (!this.Enabled) ? 0f : this.thisState.Value;
            }
        }

        // 获取上一个值
        public float LastValue
        {
            get
            {
                return (!this.Enabled) ? 0f : this.lastState.Value;
            }
        }

        // 获取当前原始值
        public float RawValue
        {
            get
            {
                return (!this.Enabled) ? 0f : this.thisState.RawValue;
            }
        }

        // 获取下一个原始值
        internal float NextRawValue
        {
            get
            {
                return (!this.Enabled) ? 0f : this.nextState.RawValue;
            }
        }

        // 检查是否状态改变
        public bool HasChanged
        {
            get
            {
                return this.Enabled && this.thisState != this.lastState;
            }
        }

        // 检查是否按下
        public bool IsPressed
        {
            get
            {
                return this.Enabled && this.thisState.State;
            }
        }

        // 检查是否刚刚按下
        public bool WasPressed
        {
            get
            {
                return this.Enabled && this.thisState && !this.lastState;
            }
        }

        // 检查是否刚刚释放
        public bool WasReleased
        {
            get
            {
                return this.Enabled && !this.thisState && this.lastState;
            }
        }

        // 检查是否重复
        public bool WasRepeated
        {
            get
            {
                return this.Enabled && this.wasRepeated;
            }
        }

        // 获取或设置灵敏度
        public float Sensitivity
        {
            get
            {
                return this.sensitivity;
            }
            set
            {
                this.sensitivity = Mathf.Clamp01(value);
            }
        }

        // 获取或设置下限死区
        public float LowerDeadZone
        {
            get
            {
                return this.lowerDeadZone;
            }
            set
            {
                this.lowerDeadZone = Mathf.Clamp01(value);
            }
        }

        // 获取或设置上限死区
        public float UpperDeadZone
        {
            get
            {
                return this.upperDeadZone;
            }
            set
            {
                this.upperDeadZone = Mathf.Clamp01(value);
            }
        }

        // 获取或设置状态阈值
        public float StateThreshold
        {
            get
            {
                return this.stateThreshold;
            }
            set
            {
                this.stateThreshold = Mathf.Clamp01(value);
            }
        }

        // 检查是否为 Null 对象
        public bool IsNull
        {
            get
            {
                return object.ReferenceEquals(this, InputControl.Null);
            }
        }

        // 隐式转换为布尔值，用于获取当前状态
        public static implicit operator bool(OneAxisInputControl instance)
        {
            return instance.State;
        }

        // 隐式转换为浮点数，用于获取当前值
        public static implicit operator float(OneAxisInputControl instance)
        {
            return instance.Value;
        }
    }
}
