using System;
using UnityEngine;

namespace InControl
{
    public struct KeyInfo
    {
        private KeyInfo(Key key, string name, params KeyCode[] keyCodes)
        {
            this.key = key;
            this.name = name;
            this.macName = name;
            this.keyCodes = keyCodes;
        }

        private KeyInfo(Key key, string name, string macName, params KeyCode[] keyCodes)
        {
            this.key = key;
            this.name = name;
            this.macName = macName;
            this.keyCodes = keyCodes;
        }

        public bool IsPressed
        {
            get
            {
                int num = this.keyCodes.Length;
                for (int i = 0; i < num; i++)
                {
                    if (Input.GetKey(this.keyCodes[i]))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public string Name
        {
            get
            {
                if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
                {
                    return this.macName;
                }
                return this.name;
            }
        }

        public Key Key
        {
            get
            {
                return this.key;
            }
        }

        private readonly Key key;

        private readonly string name;

        private readonly string macName;

        private readonly KeyCode[] keyCodes;

        public static readonly KeyInfo[] KeyList = new KeyInfo[]
{
            new KeyInfo(Key.None, "None", new KeyCode[1]),
            new KeyInfo(Key.Shift, "Shift", new KeyCode[]
            {
                KeyCode.LeftShift,
                KeyCode.RightShift
            }),
            new KeyInfo(Key.Alt, "Alt", "Option", new KeyCode[]
            {
                KeyCode.LeftAlt,
                KeyCode.RightAlt
            }),
            new KeyInfo(Key.Command, "Command", new KeyCode[]
            {
                KeyCode.LeftCommand,
                KeyCode.RightCommand
            }),
            new KeyInfo(Key.Control, "Control", new KeyCode[]
            {
                KeyCode.LeftControl,
                KeyCode.RightControl
            }),
            new KeyInfo(Key.LeftShift, "Left Shift", new KeyCode[]
            {
                KeyCode.LeftShift
            }),
            new KeyInfo(Key.LeftAlt, "Left Alt", "Left Option", new KeyCode[]
            {
                KeyCode.LeftAlt
            }),
            new KeyInfo(Key.LeftCommand, "Left Command", new KeyCode[]
            {
                KeyCode.LeftCommand
            }),
            new KeyInfo(Key.LeftControl, "Left Control", new KeyCode[]
            {
                KeyCode.LeftControl
            }),
            new KeyInfo(Key.RightShift, "Right Shift", new KeyCode[]
            {
                KeyCode.RightShift
            }),
            new KeyInfo(Key.RightAlt, "Right Alt", "Right Option", new KeyCode[]
            {
                KeyCode.RightAlt
            }),
            new KeyInfo(Key.RightCommand, "Right Command", new KeyCode[]
            {
                KeyCode.RightCommand
            }),
            new KeyInfo(Key.RightControl, "Right Control", new KeyCode[]
            {
                KeyCode.RightControl
            }),
            new KeyInfo(Key.Escape, "Escape", new KeyCode[]
            {
                KeyCode.Escape
            }),
            new KeyInfo(Key.F1, "F1", new KeyCode[]
            {
                KeyCode.F1
            }),
            new KeyInfo(Key.F2, "F2", new KeyCode[]
            {
                KeyCode.F2
            }),
            new KeyInfo(Key.F3, "F3", new KeyCode[]
            {
                KeyCode.F3
            }),
            new KeyInfo(Key.F4, "F4", new KeyCode[]
            {
                KeyCode.F4
            }),
            new KeyInfo(Key.F5, "F5", new KeyCode[]
            {
                KeyCode.F5
            }),
            new KeyInfo(Key.F6, "F6", new KeyCode[]
            {
                KeyCode.F6
            }),
            new KeyInfo(Key.F7, "F7", new KeyCode[]
            {
                KeyCode.F7
            }),
            new KeyInfo(Key.F8, "F8", new KeyCode[]
            {
                KeyCode.F8
            }),
            new KeyInfo(Key.F9, "F9", new KeyCode[]
            {
                KeyCode.F9
            }),
            new KeyInfo(Key.F10, "F10", new KeyCode[]
            {
                KeyCode.F10
            }),
            new KeyInfo(Key.F11, "F11", new KeyCode[]
            {
                KeyCode.F11
            }),
            new KeyInfo(Key.F12, "F12", new KeyCode[]
            {
                KeyCode.F12
            }),
            new KeyInfo(Key.Key0, "Num 0", new KeyCode[]
            {
                KeyCode.Alpha0
            }),
            new KeyInfo(Key.Key1, "Num 1", new KeyCode[]
            {
                KeyCode.Alpha1
            }),
            new KeyInfo(Key.Key2, "Num 2", new KeyCode[]
            {
                KeyCode.Alpha2
            }),
            new KeyInfo(Key.Key3, "Num 3", new KeyCode[]
            {
                KeyCode.Alpha3
            }),
            new KeyInfo(Key.Key4, "Num 4", new KeyCode[]
            {
                KeyCode.Alpha4
            }),
            new KeyInfo(Key.Key5, "Num 5", new KeyCode[]
            {
                KeyCode.Alpha5
            }),
            new KeyInfo(Key.Key6, "Num 6", new KeyCode[]
            {
                KeyCode.Alpha6
            }),
            new KeyInfo(Key.Key7, "Num 7", new KeyCode[]
            {
                KeyCode.Alpha7
            }),
            new KeyInfo(Key.Key8, "Num 8", new KeyCode[]
            {
                KeyCode.Alpha8
            }),
            new KeyInfo(Key.Key9, "Num 9", new KeyCode[]
            {
                KeyCode.Alpha9
            }),
            new KeyInfo(Key.A, "A", new KeyCode[]
            {
                KeyCode.A
            }),
            new KeyInfo(Key.B, "B", new KeyCode[]
            {
                KeyCode.B
            }),
            new KeyInfo(Key.C, "C", new KeyCode[]
            {
                KeyCode.C
            }),
            new KeyInfo(Key.D, "D", new KeyCode[]
            {
                KeyCode.D
            }),
            new KeyInfo(Key.E, "E", new KeyCode[]
            {
                KeyCode.E
            }),
            new KeyInfo(Key.F, "F", new KeyCode[]
            {
                KeyCode.F
            }),
            new KeyInfo(Key.G, "G", new KeyCode[]
            {
                KeyCode.G
            }),
            new KeyInfo(Key.H, "H", new KeyCode[]
            {
                KeyCode.H
            }),
            new KeyInfo(Key.I, "I", new KeyCode[]
            {
                KeyCode.I
            }),
            new KeyInfo(Key.J, "J", new KeyCode[]
            {
                KeyCode.J
            }),
            new KeyInfo(Key.K, "K", new KeyCode[]
            {
                KeyCode.K
            }),
            new KeyInfo(Key.L, "L", new KeyCode[]
            {
                KeyCode.L
            }),
            new KeyInfo(Key.M, "M", new KeyCode[]
            {
                KeyCode.M
            }),
            new KeyInfo(Key.N, "N", new KeyCode[]
            {
                KeyCode.N
            }),
            new KeyInfo(Key.O, "O", new KeyCode[]
            {
                KeyCode.O
            }),
            new KeyInfo(Key.P, "P", new KeyCode[]
            {
                KeyCode.P
            }),
            new KeyInfo(Key.Q, "Q", new KeyCode[]
            {
                KeyCode.Q
            }),
            new KeyInfo(Key.R, "R", new KeyCode[]
            {
                KeyCode.R
            }),
            new KeyInfo(Key.S, "S", new KeyCode[]
            {
                KeyCode.S
            }),
            new KeyInfo(Key.T, "T", new KeyCode[]
            {
                KeyCode.T
            }),
            new KeyInfo(Key.U, "U", new KeyCode[]
            {
                KeyCode.U
            }),
            new KeyInfo(Key.V, "V", new KeyCode[]
            {
                KeyCode.V
            }),
            new KeyInfo(Key.W, "W", new KeyCode[]
            {
                KeyCode.W
            }),
            new KeyInfo(Key.X, "X", new KeyCode[]
            {
                KeyCode.X
            }),
            new KeyInfo(Key.Y, "Y", new KeyCode[]
            {
                KeyCode.Y
            }),
            new KeyInfo(Key.Z, "Z", new KeyCode[]
            {
                KeyCode.Z
            }),
            new KeyInfo(Key.Backquote, "Backquote", new KeyCode[]
            {
                KeyCode.BackQuote
            }),
            new KeyInfo(Key.Minus, "Minus", new KeyCode[]
            {
                KeyCode.Minus
            }),
            new KeyInfo(Key.Equals, "Equals", new KeyCode[]
            {
                KeyCode.Equals
            }),
            new KeyInfo(Key.Backspace, "Backspace", "Delete", new KeyCode[]
            {
                KeyCode.Backspace
            }),
            new KeyInfo(Key.Tab, "Tab", new KeyCode[]
            {
                KeyCode.Tab
            }),
            new KeyInfo(Key.LeftBracket, "Left Bracket", new KeyCode[]
            {
                KeyCode.LeftBracket
            }),
            new KeyInfo(Key.RightBracket, "Right Bracket", new KeyCode[]
            {
                KeyCode.RightBracket
            }),
            new KeyInfo(Key.Backslash, "Backslash", new KeyCode[]
            {
                KeyCode.Backslash
            }),
            new KeyInfo(Key.Semicolon, "Semicolon", new KeyCode[]
            {
                KeyCode.Semicolon
            }),
            new KeyInfo(Key.Quote, "Quote", new KeyCode[]
            {
                KeyCode.Quote
            }),
            new KeyInfo(Key.Return, "Return", new KeyCode[]
            {
                KeyCode.Return
            }),
            new KeyInfo(Key.Comma, "Comma", new KeyCode[]
            {
                KeyCode.Comma
            }),
            new KeyInfo(Key.Period, "Period", new KeyCode[]
            {
                KeyCode.Period
            }),
            new KeyInfo(Key.Slash, "Slash", new KeyCode[]
            {
                KeyCode.Slash
            }),
            new KeyInfo(Key.Space, "Space", new KeyCode[]
            {
                KeyCode.Space
            }),
            new KeyInfo(Key.Insert, "Insert", new KeyCode[]
            {
                KeyCode.Insert
            }),
            new KeyInfo(Key.Delete, "Delete", "Forward Delete", new KeyCode[]
            {
                KeyCode.Delete
            }),
            new KeyInfo(Key.Home, "Home", new KeyCode[]
            {
                KeyCode.Home
            }),
            new KeyInfo(Key.End, "End", new KeyCode[]
            {
                KeyCode.End
            }),
            new KeyInfo(Key.PageUp, "PageUp", new KeyCode[]
            {
                KeyCode.PageUp
            }),
            new KeyInfo(Key.PageDown, "PageDown", new KeyCode[]
            {
                KeyCode.PageDown
            }),
            new KeyInfo(Key.LeftArrow, "Left Arrow", new KeyCode[]
            {
                KeyCode.LeftArrow
            }),
            new KeyInfo(Key.RightArrow, "Right Arrow", new KeyCode[]
            {
                KeyCode.RightArrow
            }),
            new KeyInfo(Key.UpArrow, "Up Arrow", new KeyCode[]
            {
                KeyCode.UpArrow
            }),
            new KeyInfo(Key.DownArrow, "Down Arrow", new KeyCode[]
            {
                KeyCode.DownArrow
            }),
            new KeyInfo(Key.Pad0, "Pad 0", new KeyCode[]
            {
                KeyCode.Keypad0
            }),
            new KeyInfo(Key.Pad1, "Pad 1", new KeyCode[]
            {
                KeyCode.Keypad1
            }),
            new KeyInfo(Key.Pad2, "Pad 2", new KeyCode[]
            {
                KeyCode.Keypad2
            }),
            new KeyInfo(Key.Pad3, "Pad 3", new KeyCode[]
            {
                KeyCode.Keypad3
            }),
            new KeyInfo(Key.Pad4, "Pad 4", new KeyCode[]
            {
                KeyCode.Keypad4
            }),
            new KeyInfo(Key.Pad5, "Pad 5", new KeyCode[]
            {
                KeyCode.Keypad5
            }),
            new KeyInfo(Key.Pad6, "Pad 6", new KeyCode[]
            {
                KeyCode.Keypad6
            }),
            new KeyInfo(Key.Pad7, "Pad 7", new KeyCode[]
            {
                KeyCode.Keypad7
            }),
            new KeyInfo(Key.Pad8, "Pad 8", new KeyCode[]
            {
                KeyCode.Keypad8
            }),
            new KeyInfo(Key.Pad9, "Pad 9", new KeyCode[]
            {
                KeyCode.Keypad9
            }),
            new KeyInfo(Key.Numlock, "Numlock", new KeyCode[]
            {
                KeyCode.Numlock
            }),
            new KeyInfo(Key.PadDivide, "Pad Divide", new KeyCode[]
            {
                KeyCode.KeypadDivide
            }),
            new KeyInfo(Key.PadMultiply, "Pad Multiply", new KeyCode[]
            {
                KeyCode.KeypadMultiply
            }),
            new KeyInfo(Key.PadMinus, "Pad Minus", new KeyCode[]
            {
                KeyCode.KeypadMinus
            }),
            new KeyInfo(Key.PadPlus, "Pad Plus", new KeyCode[]
            {
                KeyCode.KeypadPlus
            }),
            new KeyInfo(Key.PadEnter, "Pad Enter", new KeyCode[]
            {
                KeyCode.KeypadEnter
            }),
            new KeyInfo(Key.PadPeriod, "Pad Period", new KeyCode[]
            {
                KeyCode.KeypadPeriod
            }),
            new KeyInfo(Key.Clear, "Clear", new KeyCode[]
            {
                KeyCode.Clear
            }),
            new KeyInfo(Key.PadEquals, "Pad Equals", new KeyCode[]
            {
                KeyCode.KeypadEquals
            }),
            new KeyInfo(Key.F13, "F13", new KeyCode[]
            {
                KeyCode.F13
            }),
            new KeyInfo(Key.F14, "F14", new KeyCode[]
            {
                KeyCode.F14
            }),
            new KeyInfo(Key.F15, "F15", new KeyCode[]
            {
                KeyCode.F15
            }),
            new KeyInfo(Key.AltGr, "Alt Graphic", new KeyCode[]
            {
                KeyCode.AltGr
            }),
            new KeyInfo(Key.CapsLock, "Caps Lock", new KeyCode[]
            {
                KeyCode.CapsLock
            })
};
    }
}
