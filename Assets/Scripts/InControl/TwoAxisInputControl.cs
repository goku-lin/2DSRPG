using System;
using UnityEngine;

namespace InControl
{
    /// <summary>
    /// 代表一个具有两个轴控制的输入控制器。
    /// </summary>
    public class TwoAxisInputControl : IInputControl
    {
        public TwoAxisInputControl()
        {
            this.Left = new OneAxisInputControl();
            this.Right = new OneAxisInputControl();
            this.Up = new OneAxisInputControl();
            this.Down = new OneAxisInputControl();
        }

        /// <summary>
        /// 获取X轴的值。
        /// </summary>
        public float X { get; protected set; }

        /// <summary>
        /// 获取Y轴的值。
        /// </summary>
        public float Y { get; protected set; }

        /// <summary>
        /// 获取左方向的输入控制。
        /// </summary>
        public OneAxisInputControl Left { get; protected set; }

        /// <summary>
        /// 获取右方向的输入控制。
        /// </summary>
        public OneAxisInputControl Right { get; protected set; }

        /// <summary>
        /// 获取上方向的输入控制。
        /// </summary>
        public OneAxisInputControl Up { get; protected set; }

        /// <summary>
        /// 获取下方向的输入控制。
        /// </summary>
        public OneAxisInputControl Down { get; protected set; }

        /// <summary>
        /// 获取或设置输入控制的更新时间戳。
        /// </summary>
        public ulong UpdateTick { get; protected set; }

        /// <summary>
        /// 清除输入控制的状态。
        /// </summary>
        public void ClearInputState()
        {
            this.Left.ClearInputState();
            this.Right.ClearInputState();
            this.Up.ClearInputState();
            this.Down.ClearInputState();
            this.lastState = false;
            this.lastValue = Vector2.zero;
            this.thisState = false;
            this.thisValue = Vector2.zero;
            this.X = 0f;
            this.Y = 0f;
            this.clearInputState = true;
        }

        /// <summary>
        /// 通过过滤另一个输入控制器来更新这个输入控制器的状态。
        /// </summary>
        /// <param name="twoAxisInputControl">要过滤的输入控制器。</param>
        /// <param name="deltaTime">时间间隔。</param>
        public void Filter(TwoAxisInputControl twoAxisInputControl, float deltaTime)
        {
            this.UpdateWithAxes(twoAxisInputControl.X, twoAxisInputControl.Y, InputManager.CurrentTick, deltaTime);
        }

        /// <summary>
        /// 通过给定的X和Y轴值以及时间戳来更新输入控制的状态。
        /// </summary>
        /// <param name="x">X轴的值。</param>
        /// <param name="y">Y轴的值。</param>
        /// <param name="updateTick">时间戳。</param>
        /// <param name="deltaTime">时间间隔。</param>
        internal void UpdateWithAxes(float x, float y, ulong updateTick, float deltaTime)
        {
            this.lastState = this.thisState;
            this.lastValue = this.thisValue;
            this.thisValue = (!this.Raw) ? Utility.ApplyCircularDeadZone(x, y, this.LowerDeadZone, this.UpperDeadZone) : new Vector2(x, y);
            this.X = this.thisValue.x;
            this.Y = this.thisValue.y;
            this.Left.CommitWithValue(Mathf.Max(0f, -this.X), updateTick, deltaTime);
            this.Right.CommitWithValue(Mathf.Max(0f, this.X), updateTick, deltaTime);

            // 根据Y轴是否反向，决定如何更新上和下输入控制
            if (InputManager.InvertYAxis)
            {
                this.Up.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
                this.Down.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
            }
            else
            {
                this.Up.CommitWithValue(Mathf.Max(0f, this.Y), updateTick, deltaTime);
                this.Down.CommitWithValue(Mathf.Max(0f, -this.Y), updateTick, deltaTime);
            }

            this.thisState = this.Up.State || this.Down.State || this.Left.State || this.Right.State;

            // 如果需要清除输入控制的状态
            if (this.clearInputState)
            {
                this.lastState = this.thisState;
                this.lastValue = this.thisValue;
                this.clearInputState = false;
                this.HasChanged = false;
                return;
            }

            // 如果输入值发生变化
            if (this.thisValue != this.lastValue)
            {
                this.UpdateTick = updateTick;
                this.HasChanged = true;
            }
            else
            {
                this.HasChanged = false;
            }
        }

        /// <summary>
        /// 获取或设置输入控制的灵敏度。
        /// </summary>
        public float Sensitivity
        {
            get
            {
                return this.sensitivity;
            }
            set
            {
                this.sensitivity = Mathf.Clamp01(value);
                this.Left.Sensitivity = this.sensitivity;
                this.Right.Sensitivity = this.sensitivity;
                this.Up.Sensitivity = this.sensitivity;
                this.Down.Sensitivity = this.sensitivity;
            }
        }

        /// <summary>
        /// 获取或设置输入控制的状态阈值。
        /// </summary>
        public float StateThreshold
        {
            get
            {
                return this.stateThreshold;
            }
            set
            {
                this.stateThreshold = Mathf.Clamp01(value);
                this.Left.StateThreshold = this.stateThreshold;
                this.Right.StateThreshold = this.stateThreshold;
                this.Up.StateThreshold = this.stateThreshold;
                this.Down.StateThreshold = this.stateThreshold;
            }
        }

        /// <summary>
        /// 获取或设置输入控制的下限死区。
        /// </summary>
        public float LowerDeadZone
        {
            get
            {
                return this.lowerDeadZone;
            }
            set
            {
                this.lowerDeadZone = Mathf.Clamp01(value);
                this.Left.LowerDeadZone = this.lowerDeadZone;
                this.Right.LowerDeadZone = this.lowerDeadZone;
                this.Up.LowerDeadZone = this.lowerDeadZone;
                this.Down.LowerDeadZone = this.lowerDeadZone;
            }
        }

        /// <summary>
        /// 获取或设置输入控制的上限死区。
        /// </summary>
        public float UpperDeadZone
        {
            get
            {
                return this.upperDeadZone;
            }
            set
            {
                this.upperDeadZone = Mathf.Clamp01(value);
                this.Left.UpperDeadZone = this.upperDeadZone;
                this.Right.UpperDeadZone = this.upperDeadZone;
                this.Up.UpperDeadZone = this.upperDeadZone;
                this.Down.UpperDeadZone = this.upperDeadZone;
            }
        }

        /// <summary>
        /// 获取输入控制的当前状态。
        /// </summary>
        public bool State
        {
            get
            {
                return this.thisState;
            }
        }

        /// <summary>
        /// 获取输入控制的上一个状态。
        /// </summary>
        public bool LastState
        {
            get
            {
                return this.lastState;
            }
        }

        /// <summary>
        /// 获取输入控制的当前值。
        /// </summary>
        public Vector2 Value
        {
            get
            {
                return this.thisValue;
            }
        }

        /// <summary>
        /// 获取输入控制的上一个值。
        /// </summary>
        public Vector2 LastValue
        {
            get
            {
                return this.lastValue;
            }
        }

        /// <summary>
        /// 获取输入控制的向量值。
        /// </summary>
        public Vector2 Vector
        {
            get
            {
                return this.thisValue;
            }
        }

        /// <summary>
        /// 获取输入控制是否发生了改变。
        /// </summary>
        public bool HasChanged { get; protected set; }

        /// <summary>
        /// 获取输入控制是否被按下。
        /// </summary>
        public bool IsPressed
        {
            get
            {
                return this.thisState;
            }
        }

        /// <summary>
        /// 获取输入控制是否被按下（之前未按下）。
        /// </summary>
        public bool WasPressed
        {
            get
            {
                return this.thisState && !this.lastState;
            }
        }

        /// <summary>
        /// 获取输入控制是否被释放（之前被按下）。
        /// </summary>
        public bool WasReleased
        {
            get
            {
                return !this.thisState && this.lastState;
            }
        }

        /// <summary>
        /// 获取输入控制的角度值。
        /// </summary>
        public float Angle
        {
            get
            {
                return Utility.VectorToAngle(this.thisValue);
            }
        }

        /// <summary>
        /// 隐式将此输入控制转换为布尔值。
        /// </summary>
        public static implicit operator bool(TwoAxisInputControl instance)
        {
            return instance.thisState;
        }

        /// <summary>
        /// 隐式将此输入控制转换为二维向量。
        /// </summary>
        public static implicit operator Vector2(TwoAxisInputControl instance)
        {
            return instance.thisValue;
        }

        /// <summary>
        /// 隐式将此输入控制转换为三维向量。
        /// </summary>
        public static implicit operator Vector3(TwoAxisInputControl instance)
        {
            return new Vector3(instance.thisValue.x, instance.thisValue.y);
        }

        public static readonly TwoAxisInputControl Null = new TwoAxisInputControl();
        private float sensitivity = 1f;
        private float lowerDeadZone;
        private float upperDeadZone = 1f;
        private float stateThreshold;
        public bool Raw;
        private bool thisState;
        private bool lastState;
        private Vector2 thisValue;
        private Vector2 lastValue;
        private bool clearInputState;
    }
}
