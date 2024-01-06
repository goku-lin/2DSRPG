using System;
using System.Collections.Generic;
using InControl;
using lib.keyEvent;
using lib.notify;
using UnityEngine;

namespace Game
{
    public class GameKey : KeyEvent
    {
        public const int KEY_REPEAT_TIME = 1;
        public const int KEY_REPEAT_STARTTIME = 10;
        public const int CUSTOMER_KEY = 100;
        public static Dictionary<int, KeyCode> KeyBoardButtonMap = new Dictionary<int, KeyCode>();
        public static Dictionary<int, InputControlType> JoyButtonMap = new Dictionary<int, InputControlType>();
        public static Dictionary<int, float> JoyButtonRepeated = new Dictionary<int, float>();
        public static Dictionary<int, float> JoyButtonRepeatedBuff = new Dictionary<int, float>();
        public static Dictionary<int, GameAxisKeyValue> JoyAxisMap = new Dictionary<int, GameAxisKeyValue>();
        public static Dictionary<int, int> JoyAxisTick = new Dictionary<int, int>();
        private static InControlManager inControlMgr = null;
        public static KeyStyle style = KeyStyle.KEYBOARD;
        private static Vector3 _mousePos = Vector3.zero;
        public static bool IsJoyPadHintMode = false;
        private static bool _isCheckedJoystick = false;
        private static bool _isBlockKey = false;

        public static void InitDefalutButtons()
        {
            GameKey.RegistKeyBoardButtons(new Dictionary<int, KeyCode>
            {
                {
                    3,
                    KeyCode.UpArrow
                },
                {
                    4,
                    KeyCode.DownArrow
                },
                {
                    5,
                    KeyCode.LeftArrow
                },
                {
                    6,
                    KeyCode.RightArrow
                },
                {
                    8,
                    KeyCode.L
                },
                {
                    9,
                    KeyCode.I
                },
                {
                    1,
                    KeyCode.Return
                },
                {
                    2,
                    KeyCode.Escape
                },
                {
                    10,
                    KeyCode.H
                },
                {
                    11,
                    KeyCode.Space
                },
                {
                    12,
                    KeyCode.Minus
                },
                {
                    13,
                    KeyCode.Equals
                },
                {
                    14,
                    KeyCode.W
                },
                {
                    15,
                    KeyCode.S
                },
                {
                    16,
                    KeyCode.A
                },
                {
                    17,
                    KeyCode.D
                }
            });
            GameKey.RegistJoyButtons(new Dictionary<int, InputControlType>
            {
                {
                    3,
                    InputControlType.DPadUp
                },
                {
                    4,
                    InputControlType.DPadDown
                },
                {
                    5,
                    InputControlType.DPadLeft
                },
                {
                    6,
                    InputControlType.DPadRight
                },
                {
                    8,
                    InputControlType.Action3
                },
                {
                    9,
                    InputControlType.Action4
                },
                {
                    1,
                    InputControlType.Action1
                },
                {
                    2,
                    InputControlType.Action2
                },
                {
                    10,
                    InputControlType.Back
                },
                {
                    11,
                    InputControlType.Start
                },
                {
                    12,
                    InputControlType.LeftBumper
                },
                {
                    13,
                    InputControlType.RightBumper
                }
            });
            GameKey.RegistJoyAxis(new Dictionary<int, GameAxisKeyValue>
            {
                {
                    18,
                    new GameAxisKeyValue(InputControlType.LeftStickY, 1f)
                },
                {
                    19,
                    new GameAxisKeyValue(InputControlType.LeftStickY, -1f)
                },
                {
                    20,
                    new GameAxisKeyValue(InputControlType.LeftStickX, -1f)
                },
                {
                    21,
                    new GameAxisKeyValue(InputControlType.LeftStickX, 1f)
                },
                {
                    12,
                    new GameAxisKeyValue(InputControlType.LeftTrigger, 1f)
                },
                {
                    13,
                    new GameAxisKeyValue(InputControlType.RightTrigger, 1f)
                },
                {
                    14,
                    new GameAxisKeyValue(InputControlType.RightStickY, 1f)
                },
                {
                    15,
                    new GameAxisKeyValue(InputControlType.RightStickY, -1f)
                },
                {
                    16,
                    new GameAxisKeyValue(InputControlType.RightStickX, -1f)
                },
                {
                    17,
                    new GameAxisKeyValue(InputControlType.RightStickX, 1f)
                }
            });
        }

        public void InverseJoystickOkCancel(bool isInverse)
        {
            Dictionary<int, InputControlType> dictionary = new Dictionary<int, InputControlType>();
            dictionary.Add(3, InputControlType.DPadUp);
            dictionary.Add(4, InputControlType.DPadDown);
            dictionary.Add(5, InputControlType.DPadLeft);
            dictionary.Add(6, InputControlType.DPadRight);
            dictionary.Add(8, InputControlType.Action3);
            dictionary.Add(9, InputControlType.Action4);
            if (isInverse)
            {
                dictionary.Add(1, InputControlType.Action2);
                dictionary.Add(2, InputControlType.Action1);
            }
            else
            {
                dictionary.Add(1, InputControlType.Action1);
                dictionary.Add(2, InputControlType.Action2);
            }
            dictionary.Add(10, InputControlType.Back);
            dictionary.Add(11, InputControlType.Start);
            dictionary.Add(12, InputControlType.LeftBumper);
            dictionary.Add(13, InputControlType.RightBumper);
            GameKey.RegistJoyButtons(dictionary);
            Notifier.Notify(102, new object[]
            {
                isInverse
            });
        }

        public static void RegistKeyBoardSingleButton(int key, KeyCode keycode)
        {
            GameKey.KeyBoardButtonMap[key] = keycode;
        }

        public static void RegistKeyBoardButtons(Dictionary<int, KeyCode> buttons)
        {
            GameKey.KeyBoardButtonMap.Clear();
            foreach (int key in buttons.Keys)
            {
                GameKey.KeyBoardButtonMap.Add(key, buttons[key]);
            }
        }

        public static void RegistJoyButtons(Dictionary<int, InputControlType> buttons)
        {
            GameKey.JoyButtonMap.Clear();
            foreach (int key in buttons.Keys)
            {
                GameKey.JoyButtonMap.Add(key, buttons[key]);
            }
        }

        public static void RegistJoyAxis(Dictionary<int, GameAxisKeyValue> buttons)
        {
            GameKey.JoyAxisMap.Clear();
            foreach (int key in buttons.Keys)
            {
                GameAxisKeyValue value = new GameAxisKeyValue(buttons[key].key, buttons[key].value);
                GameKey.JoyAxisMap.Add(key, value);
            }
        }

        private static void LazyInitKey(int keyCode)
        {
            if (!GameKey.JoyButtonRepeated.ContainsKey(keyCode))
            {
                GameKey.JoyButtonRepeated.Add(keyCode, 0f);
            }
            if (!GameKey.JoyButtonRepeatedBuff.ContainsKey(keyCode))
            {
                GameKey.JoyButtonRepeatedBuff.Add(keyCode, 0f);
            }
        }

        private static bool CheckKeyRepeated(int keyCode)
        {
            GameKey.LazyInitKey(keyCode);
            return GameKey.JoyButtonRepeated[keyCode] > 10f && (int)GameKey.JoyButtonRepeated[keyCode] != (int)GameKey.JoyButtonRepeatedBuff[keyCode] && (int)GameKey.JoyButtonRepeated[keyCode] % 1 == 0;
        }

        public static bool IsKeyRepeated(int keyCode, int frame)
        {
            return GameKey.JoyButtonRepeated.ContainsKey(keyCode) && (int)GameKey.JoyButtonRepeated[keyCode] % frame == 0;
        }

        private static void KeyRepeat(int keyCode)
        {
            GameKey.LazyInitKey(keyCode);
            GameKey.JoyButtonRepeatedBuff[keyCode] = GameKey.JoyButtonRepeated[keyCode];
            Dictionary<int, float> joyButtonRepeated;
            (joyButtonRepeated = GameKey.JoyButtonRepeated)[keyCode] = joyButtonRepeated[keyCode] + 1f;
        }

        private static void KeyRelease(int keyCode)
        {
            GameKey.LazyInitKey(keyCode);
            GameKey.JoyButtonRepeated[keyCode] = 0f;
            GameKey.JoyButtonRepeatedBuff[keyCode] = 0f;
        }

        public static void ClearKeyRepeated()
        {
            GameKey.JoyButtonRepeated.Clear();
            GameKey.JoyButtonRepeatedBuff.Clear();
        }

        public static bool CheckExsitJoystick()
        {
            if (GameKey._isCheckedJoystick)
            {
                return true;
            }
            InputDevice activeDevice = InputManager.ActiveDevice;
            if (activeDevice == null || activeDevice.Name == "None")
            {
                GameKey._isCheckedJoystick = true;
                return true;
            }
            if (activeDevice.Name.ToLower().Contains("playstation"))
            {
                //Debugger.Log(LogLevel.DEBUG, "ps4", new object[0]);
                if (GameKey.style != KeyStyle.PS4)
                {
                    GameKey.style = KeyStyle.PS4;
                }
            }
            else
            {
                //Debugger.Log(LogLevel.DEBUG, "xbox", new object[0]);
                if (GameKey.style != KeyStyle.XBOX)
                {
                    GameKey.style = KeyStyle.XBOX;
                }
            }
            GameKey._isCheckedJoystick = true;
            return false;
        }

        public static void CheckStyle()
        {
            InputDevice activeDevice = InputManager.ActiveDevice;
            if (activeDevice == null || activeDevice.Name == "None")
            {
                GameKey.style = KeyStyle.KEYBOARD;
                return;
            }
            GameKey.style = KeyStyle.XBOX;
            Notifier.Notify(101, new object[]
            {
                GameKey.style
            });
        }

        /// <summary>
        /// 手柄设备
        /// </summary>
        protected static void UpdateDeviceControl()
        {
            if (GameKey.inControlMgr == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.name = "InControl";
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                GameKey.inControlMgr = gameObject.AddComponent<InControlManager>();
            }
            InputDevice activeDevice = InputManager.ActiveDevice;
            if (activeDevice == null || activeDevice.Name == "None")
            {
                return;
            }
            bool flag = false;
            if (activeDevice.AnyButtonIsPressed || activeDevice.AnyButtonWasPressed)
            {
                flag = true;
                GameKey.CheckStyle();
            }
            foreach (int num in GameKey.JoyButtonMap.Keys)
            {
                InputControl control = activeDevice.GetControl(GameKey.JoyButtonMap[num]);
                if (control != null)
                {
                    if (control.WasPressed)
                    {
                        bool flag2 = false;
                        for (int i = 0; i < KeyEvent.eventListenerList.Count; i++)
                        {
                            flag2 |= KeyEvent.eventListenerList[i].onGameKeyStateChanged(num, GAMEKEY_STATE.PRESSED, flag2);
                        }
                        GameKey.KeyRelease(num);
                        flag = true;
                    }
                    else if (control.IsPressed)
                    {
                        if (GameKey.CheckKeyRepeated(num))
                        {
                            bool flag3 = false;
                            for (int j = 0; j < KeyEvent.eventListenerList.Count; j++)
                            {
                                flag3 |= KeyEvent.eventListenerList[j].onGameKeyStateChanged(num, GAMEKEY_STATE.REPEATED, flag3);
                            }
                        }
                        GameKey.KeyRepeat(num);
                        if (GameKey.JoyButtonRepeated.ContainsKey(12) && GameKey.JoyButtonRepeated[12] > 0f && GameKey.JoyButtonRepeated.ContainsKey(13) && GameKey.JoyButtonRepeated[13] > 0f && GameKey.JoyButtonRepeated.ContainsKey(11) && GameKey.JoyButtonRepeated[11] > 0f)
                        {
                            GameKey.JoyButtonRepeated[12] = 0f;
                            GameKey.JoyButtonRepeated[13] = 0f;
                            GameKey.JoyButtonRepeated[11] = 0f;
                            Notifier.Notify(10274, new object[0]);
                        }
                        flag = true;
                    }
                    else if (control.WasReleased)
                    {
                        bool flag4 = false;
                        for (int k = 0; k < KeyEvent.eventListenerList.Count; k++)
                        {
                            flag4 |= KeyEvent.eventListenerList[k].onGameKeyStateChanged(num, GAMEKEY_STATE.RELEASED, flag4);
                        }
                        GameKey.KeyRelease(num);
                        flag = true;
                    }
                }
            }
            foreach (int num2 in GameKey.JoyAxisMap.Keys)
            {
                InputControlType key = GameKey.JoyAxisMap[num2].key;
                InputControl control2 = activeDevice.GetControl(key);
                if (control2 != null)
                {
                    if (control2.State && Mathf.Abs(control2.Value - GameKey.JoyAxisMap[num2].value) <= 0.3f)
                    {
                        flag = true;
                        if (!GameKey.JoyAxisTick.ContainsKey(num2))
                        {
                            GameKey.JoyAxisTick.Add(num2, 0);
                        }
                        bool flag5 = false;
                        if (!GameKey.JoyButtonRepeated.ContainsKey(num2))
                        {
                            GameKey.JoyButtonRepeated.Add(num2, 0f);
                        }
                        if (GameKey.JoyButtonRepeated[num2] == 0f)
                        {
                            for (int l = 0; l < KeyEvent.eventListenerList.Count; l++)
                            {
                                flag5 |= KeyEvent.eventListenerList[l].onGameKeyStateChanged(num2, GAMEKEY_STATE.PRESSED, flag5);
                            }
                            GameKey.JoyAxisTick[num2] = 1;
                        }
                        else if (GameKey.CheckKeyRepeated(num2))
                        {
                            for (int m = 0; m < KeyEvent.eventListenerList.Count; m++)
                            {
                                flag5 |= KeyEvent.eventListenerList[m].onGameKeyStateChanged(num2, GAMEKEY_STATE.REPEATED, flag5);
                            }
                        }
                        GameKey.KeyRepeat(num2);
                    }
                    else if (!control2.State)
                    {
                        if (!GameKey.JoyAxisTick.ContainsKey(num2))
                        {
                            GameKey.JoyAxisTick.Add(num2, 0);
                        }
                        if (GameKey.JoyAxisTick[num2] == 1)
                        {
                            bool flag6 = false;
                            for (int n = 0; n < KeyEvent.eventListenerList.Count; n++)
                            {
                                flag6 |= KeyEvent.eventListenerList[n].onGameKeyStateChanged(num2, GAMEKEY_STATE.RELEASED, flag6);
                            }
                        }
                        GameKey.JoyAxisTick[num2] = 0;
                        GameKey.KeyRelease(num2);
                    }
                }
            }
            if (GameKey.CheckKeyRepeated(1001))
            {
                TwoAxisInputControl leftStick = activeDevice.LeftStick;
                if (leftStick != null)
                {
                    bool flag7 = false;
                    for (int num3 = 0; num3 < KeyEvent.eventListenerList.Count; num3++)
                    {
                        flag7 |= KeyEvent.eventListenerList[num3].onGameAxisStickValueChanged(1001, leftStick.Angle, (float)Math.Sqrt((double)(leftStick.X * leftStick.X + leftStick.Y * leftStick.Y)), flag7);
                    }
                }
            }
            GameKey.KeyRepeat(1001);
            if (flag)
            {
                Notifier.Notify(100, new object[]
                {
                    false
                });
                GameKey.SetJoyPadHintMode(true);
            }
        }

        protected static bool Menu1WasPressed(InputDevice inputDevice)
        {
            return inputDevice.GetControl(InputControlType.Back).WasPressed || inputDevice.GetControl(InputControlType.View).WasPressed || inputDevice.GetControl(InputControlType.Select).WasPressed || inputDevice.GetControl(InputControlType.System).WasPressed || inputDevice.GetControl(InputControlType.TouchPadButton).WasPressed || inputDevice.GetControl(InputControlType.Menu).WasPressed;
        }

        protected static bool Menu1WasReleased(InputDevice inputDevice)
        {
            return inputDevice.GetControl(InputControlType.Back).WasReleased || inputDevice.GetControl(InputControlType.View).WasReleased || inputDevice.GetControl(InputControlType.Select).WasReleased || inputDevice.GetControl(InputControlType.System).WasReleased || inputDevice.GetControl(InputControlType.TouchPadButton).WasReleased || inputDevice.GetControl(InputControlType.Menu).WasReleased;
        }

        protected static bool Menu2WasPressed(InputDevice inputDevice)
        {
            return inputDevice.GetControl(InputControlType.Start).WasPressed || inputDevice.GetControl(InputControlType.Options).WasPressed || inputDevice.GetControl(InputControlType.Pause).WasPressed;
        }

        protected static bool Menu2WasReleased(InputDevice inputDevice)
        {
            return inputDevice.GetControl(InputControlType.Start).WasReleased || inputDevice.GetControl(InputControlType.Options).WasReleased || inputDevice.GetControl(InputControlType.Pause).WasReleased;
        }

        public static void SetKeyBlock(bool isLock)
        {
            GameKey._isBlockKey = isLock;
        }

        public static void Update()
        {
            if (GameKey._isBlockKey)
            {
                return;
            }
            bool flag = false;
            //检查是否有移动鼠标
            if ((GameKey._mousePos - Input.mousePosition).sqrMagnitude > 0.01f)
            {
                flag = true;
            }
            GameKey._mousePos = Input.mousePosition;
            //是否有鼠标右键的回调
            if (Input.GetMouseButtonDown(1))
            {
                bool flag2 = false;
                for (int i = 0; i < KeyEvent.eventListenerList.Count; i++)
                {
                    flag2 |= KeyEvent.eventListenerList[i].onGameKeyStateChanged(2002, GAMEKEY_STATE.PRESSED, flag2);
                }
                flag = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                bool flag3 = false;
                for (int j = 0; j < KeyEvent.eventListenerList.Count; j++)
                {
                    flag3 |= KeyEvent.eventListenerList[j].onGameKeyStateChanged(2002, GAMEKEY_STATE.RELEASED, flag3);
                }
                flag = true;
            }
            //是否有鼠标左键的回调
            if (Input.GetMouseButtonDown(0))
            {
                bool flag4 = false;
                for (int k = 0; k < KeyEvent.eventListenerList.Count; k++)
                {
                    flag4 |= KeyEvent.eventListenerList[k].onGameKeyStateChanged(2001, GAMEKEY_STATE.PRESSED, flag4);
                }
                flag = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                bool flag5 = false;
                for (int l = 0; l < KeyEvent.eventListenerList.Count; l++)
                {
                    flag5 |= KeyEvent.eventListenerList[l].onGameKeyStateChanged(2001, GAMEKEY_STATE.RELEASED, flag5);
                }
                flag = true;
            }
            //是否有键盘的回调
            foreach (int num in GameKey.KeyBoardButtonMap.Keys)
            {
                if (Input.GetKeyDown(GameKey.KeyBoardButtonMap[num]))
                {
                    bool flag6 = false;
                    for (int m = 0; m < KeyEvent.eventListenerList.Count; m++)
                    {
                        flag6 |= KeyEvent.eventListenerList[m].onGameKeyStateChanged(num, GAMEKEY_STATE.PRESSED, flag6);
                    }
                    flag = true;
                    GameKey.KeyRelease(num);
                }
                else if (Input.GetKey(GameKey.KeyBoardButtonMap[num]))
                {
                    GameKey.KeyRepeat(num);
                    flag = true;
                    if (GameKey.CheckKeyRepeated(num))
                    {
                        bool flag7 = false;
                        for (int n = 0; n < KeyEvent.eventListenerList.Count; n++)
                        {
                            flag7 |= KeyEvent.eventListenerList[n].onGameKeyStateChanged(num, GAMEKEY_STATE.REPEATED, flag7);
                        }
                        flag = true;
                    }
                }
                else if (Input.GetKeyUp(GameKey.KeyBoardButtonMap[num]))
                {
                    bool flag8 = false;
                    for (int num2 = 0; num2 < KeyEvent.eventListenerList.Count; num2++)
                    {
                        flag8 |= KeyEvent.eventListenerList[num2].onGameKeyStateChanged(num, GAMEKEY_STATE.RELEASED, flag8);
                    }
                    flag = true;
                    GameKey.KeyRelease(num);
                }
            }
            //有按键或鼠标就隐藏手柄相关
            if (flag)
            {
                Notifier.Notify(100, new object[]
                {
                    true
                });
                GameKey.SetJoyPadHintMode(false);
            }
            //TODO:手柄的未来再说
            //GameKey.UpdateDeviceControl();
        }

        public static void SetJoyPadHintMode(bool joypadHintModeOn)
        {
            if (GameKey.IsJoyPadHintMode != joypadHintModeOn)
            {
                GameKey.IsJoyPadHintMode = joypadHintModeOn;
                Notifier.Notify(10216, new object[0]);
            }
        }
    }
}