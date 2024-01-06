using System;

namespace InControl
{
    // 输入控制类，继承自 OneAxisInputControl，用于表示输入控制器上的一个按钮或轴
    public class InputControl : OneAxisInputControl
    {
        // 私有构造函数，用于创建空的输入控制
        private InputControl()
        {
            this.Handle = "None";           // 控制的名称，默认为 "None"
            this.Target = InputControlType.None; // 控制的类型，默认为 None
            this.Passive = false;            // 是否为被动控制，默认为 false
            this.IsButton = false;           // 是否为按钮控制，默认为 false
            this.IsAnalog = false;           // 是否为模拟控制，默认为 false
        }

        // 构造函数，用于创建具有指定名称和类型的输入控制
        public InputControl(string handle, InputControlType target)
        {
            this.Handle = handle;                    // 设置控制的名称
            this.Target = target;                    // 设置控制的类型
            this.Passive = false;                    // 默认不是被动控制
            this.IsButton = Utility.TargetIsButton(target); // 根据控制类型判断是否为按钮控制
            this.IsAnalog = !this.IsButton;           // 根据按钮控制的结果判断是否为模拟控制
        }

        // 构造函数，用于创建具有指定名称、类型和被动状态的输入控制
        public InputControl(string handle, InputControlType target, bool passive) : this(handle, target)
        {
            this.Passive = passive; // 设置被动状态
        }

        // 输入控制的名称
        public string Handle { get; protected set; }

        // 输入控制的类型
        public InputControlType Target { get; protected set; }

        // 是否为按钮控制
        public bool IsButton { get; protected set; }

        // 是否为模拟控制
        public bool IsAnalog { get; protected set; }

        // 设置当前时间为零刻
        internal void SetZeroTick()
        {
            this.zeroTick = base.UpdateTick;
        }

        // 检查是否处于零刻
        internal bool IsOnZeroTick
        {
            get
            {
                return base.UpdateTick == this.zeroTick;
            }
        }

        // 检查是否为标准控制
        public bool IsStandard
        {
            get
            {
                return Utility.TargetIsStandard(this.Target);
            }
        }

        // 空的输入控制对象
        public static readonly InputControl Null = new InputControl();

        // 是否为被动控制
        public bool Passive;

        private ulong zeroTick;
    }
}
