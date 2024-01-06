using System;

namespace InControl
{
    // 定义输入控制类型的枚举
    public enum InputControlType
    {
        None,               // 无控制
        LeftStickUp,        // 左摇杆向上
        LeftStickDown,      // 左摇杆向下
        LeftStickLeft,      // 左摇杆向左
        LeftStickRight,     // 左摇杆向右
        LeftStickButton,    // 左摇杆按钮
        RightStickUp,       // 右摇杆向上
        RightStickDown,     // 右摇杆向下
        RightStickLeft,     // 右摇杆向左
        RightStickRight,    // 右摇杆向右
        RightStickButton,   // 右摇杆按钮
        DPadUp,             // 十字键向上
        DPadDown,           // 十字键向下
        DPadLeft,           // 十字键向左
        DPadRight,          // 十字键向右
        LeftTrigger,        // 左扳机
        RightTrigger,       // 右扳机
        LeftBumper,         // 左肩键
        RightBumper,        // 右肩键
        Action1,            // 动作按钮1
        Action2,            // 动作按钮2
        Action3,            // 动作按钮3
        Action4,            // 动作按钮4
        Action5,            // 动作按钮5
        Action6,            // 动作按钮6
        Action7,            // 动作按钮7
        Action8,            // 动作按钮8
        Action9,            // 动作按钮9
        Action10,           // 动作按钮10
        Action11,           // 动作按钮11
        Action12,           // 动作按钮12
        Back = 100,         // 返回按钮
        Start,              // 开始按钮
        Select,             // 选择按钮
        System,             // 系统按钮
        Options,            // 选项按钮
        Pause,              // 暂停按钮
        Menu,               // 菜单按钮
        Share,              // 共享按钮
        Home,               // 主页按钮
        View,               // 查看按钮
        Power,              // 电源按钮
        Capture,            // 捕获按钮
        Plus,               // 加号按钮
        Minus,              // 减号按钮
        PedalLeft = 150,    // 左踏板
        PedalRight,         // 右踏板
        PedalMiddle,        // 中踏板
        GearUp,             // 档位向上
        GearDown,           // 档位向下
        Pitch = 200,        // 倾斜
        Roll,               // 翻滚
        Yaw,                // 偏航
        ThrottleUp,         // 油门向上
        ThrottleDown,       // 油门向下
        ThrottleLeft,       // 油门向左
        ThrottleRight,      // 油门向右
        POVUp,              // POV向上
        POVDown,            // POV向下
        POVLeft,            // POV向左
        POVRight,           // POV向右
        TiltX = 250,        // 倾斜X轴
        TiltY,              // 倾斜Y轴
        TiltZ,              // 倾斜Z轴
        ScrollWheel,        // 滚轮
        TouchPadButton,     // 触摸板按钮
        TouchPadXAxis,      // 触摸板X轴
        TouchPadYAxis,      // 触摸板Y轴
        LeftSL,             // 左侧SL按钮
        LeftSR,             // 左侧SR按钮
        RightSL,            // 右侧SL按钮
        RightSR,            // 右侧SR按钮
        Command = 300,      // 命令按钮
        LeftStickX,         // 左摇杆X轴
        LeftStickY,         // 左摇杆Y轴
        RightStickX,        // 右摇杆X轴
        RightStickY,        // 右摇杆Y轴
        DPadX,              // 十字键X轴
        DPadY,              // 十字键Y轴
        Analog0 = 400,      // 模拟控制0
        Analog1,            // 模拟控制1
        Analog2,            // 模拟控制2
        Analog3,            // 模拟控制3
        Analog4,            // 模拟控制4
        Analog5,            // 模拟控制5
        Analog6,            // 模拟控制6
        Analog7,            // 模拟控制7
        Analog8,            // 模拟控制8
        Analog9,            // 模拟控制9
        Analog10,           // 模拟控制10
        Analog11,           // 模拟控制11
        Analog12,           // 模拟控制12
        Analog13,           // 模拟控制13
        Analog14,           // 模拟控制14
        Analog15,           // 模拟控制15
        Analog16,           // 模拟控制16
        Analog17,           // 模拟控制17
        Analog18,           // 模拟控制18
        Analog19,           // 模拟控制19
        Button0 = 500,      // 按钮0
        Button1,            // 按钮1
        Button2,            // 按钮2
        Button3,            // 按钮3
        Button4,            // 按钮4
        Button5,            // 按钮5
        Button6,            // 按钮6
        Button7,            // 按钮7
        Button8,            // 按钮8
        Button9,            // 按钮9
        Button10,           // 按钮10
        Button11,           // 按钮11
        Button12,           // 按钮12
        Button13,           // 按钮13
        Button14,           // 按钮14
        Button15,           // 按钮15
        Button16,           // 按钮16
        Button17,           // 按钮17
        Button18,           // 按钮18
        Button19,           // 按钮19
        Count               // 输入控制类型数量
    }
}
