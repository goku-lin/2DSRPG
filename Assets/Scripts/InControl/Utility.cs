using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InControl
{
    public static class Utility
    {
        // 应用死区到输入值
        public static float ApplyDeadZone(float value, float lowerDeadZone, float upperDeadZone)
        {
            if (value < 0f)
            {
                if (value > -lowerDeadZone)
                {
                    return 0f;
                }
                if (value < -upperDeadZone)
                {
                    return -1f;
                }
                return (value + lowerDeadZone) / (upperDeadZone - lowerDeadZone);
            }
            else
            {
                if (value < lowerDeadZone)
                {
                    return 0f;
                }
                if (value > upperDeadZone)
                {
                    return 1f;
                }
                return (value - lowerDeadZone) / (upperDeadZone - lowerDeadZone);
            }
        }

        // 将 X 和 Y 分别应用死区
        public static Vector2 ApplySeparateDeadZone(float x, float y, float lowerDeadZone, float upperDeadZone)
        {
            Vector2 vector = new Vector2(Utility.ApplyDeadZone(x, lowerDeadZone, upperDeadZone), Utility.ApplyDeadZone(y, lowerDeadZone, upperDeadZone));
            return vector.normalized;
        }

        // 将输入向量应用圆形死区
        public static Vector2 ApplyCircularDeadZone(Vector2 v, float lowerDeadZone, float upperDeadZone)
        {
            float magnitude = v.magnitude;
            if (magnitude < lowerDeadZone)
            {
                return Vector2.zero;
            }
            if (magnitude > upperDeadZone)
            {
                return v.normalized;
            }
            return v.normalized * ((magnitude - lowerDeadZone) / (upperDeadZone - lowerDeadZone));
        }

        // 将 X 和 Y 分别应用圆形死区
        public static Vector2 ApplyCircularDeadZone(float x, float y, float lowerDeadZone, float upperDeadZone)
        {
            return Utility.ApplyCircularDeadZone(new Vector2(x, y), lowerDeadZone, upperDeadZone);
        }

        // 获取一个数字的绝对值
        public static float Abs(float value)
        {
            return (value >= 0f) ? value : (-value);
        }

        // 检查两个浮点数是否近似相等
        public static bool Approximately(float v1, float v2)
        {
            float num = v1 - v2;
            return num >= -1E-07f && num <= 1E-07f;
        }

        // 检查两个二维向量是否近似相等
        public static bool Approximately(Vector2 v1, Vector2 v2)
        {
            return Utility.Approximately(v1.x, v2.x) && Utility.Approximately(v1.y, v2.y);
        }

        // 检查浮点数是否不接近零
        public static bool IsNotZero(float value)
        {
            return value < -1E-07f || value > 1E-07f;
        }

        // 检查浮点数是否接近零
        public static bool IsZero(float value)
        {
            return value >= -1E-07f && value <= 1E-07f;
        }

        // 检查浮点数是否大于阈值
        public static bool AbsoluteIsOverThreshold(float value, float threshold)
        {
            return value < -threshold || value > threshold;
        }

        // 根据两个输入值，计算一个单一值
        internal static float ValueFromSides(float negativeSide, float positiveSide)
        {
            float num = Utility.Abs(negativeSide);
            float num2 = Utility.Abs(positiveSide);
            if (Utility.Approximately(num, num2))
            {
                return 0f;
            }
            return (num <= num2) ? num2 : (-num);
        }

        // 根据两个输入值和反转标志，计算一个单一值
        internal static float ValueFromSides(float negativeSide, float positiveSide, bool invertSides)
        {
            if (invertSides)
            {
                return Utility.ValueFromSides(positiveSide, negativeSide);
            }
            return Utility.ValueFromSides(negativeSide, positiveSide);
        }

        // 检查输入控制是否为按钮
        internal static bool TargetIsButton(InputControlType target)
        {
            return (target >= InputControlType.Action1 && target <= InputControlType.Action12) || (target >= InputControlType.Button0 && target <= InputControlType.Button19);
        }

        // 检查输入控制是否为标准
        internal static bool TargetIsStandard(InputControlType target)
        {
            return (target >= InputControlType.LeftStickUp && target <= InputControlType.Action12) || (target >= InputControlType.Command && target <= InputControlType.DPadY);
        }
        // 将二维向量转换为角度
        public static float VectorToAngle(Vector2 vector)
        {
            if (Utility.IsZero(vector.x) && Utility.IsZero(vector.y))
            {
                return 0f;
            }
            return Utility.NormalizeAngle(Mathf.Atan2(vector.x, vector.y) * 57.29578f);
        }

        // 获取两个浮点数中的较小值
        public static float Min(float v0, float v1)
        {
            return (v0 < v1) ? v0 : v1;
        }

        // 获取两个浮点数中的较大值
        public static float Max(float v0, float v1)
        {
            return (v0 > v1) ? v0 : v1;
        }

        // 获取四个浮点数中的较小值
        public static float Min(float v0, float v1, float v2, float v3)
        {
            float num = (v0 < v1) ? v0 : v1;
            float num2 = (v2 < v3) ? v2 : v3;
            return (num < num2) ? num : num2;
        }

        // 获取四个浮点数中的较大值
        public static float Max(float v0, float v1, float v2, float v3)
        {
            float num = (v0 > v1) ? v0 : v1;
            float num2 = (v2 > v3) ? v2 : v3;
            return (num > num2) ? num : num2;
        }

        // 标准化角度值到 0-360 范围内
        public static float NormalizeAngle(float angle)
        {
            while (angle < 0f)
            {
                angle += 360f;
            }
            while (angle > 360f)
            {
                angle -= 360f;
            }
            return angle;
        }

        // 应用阈值到输入值
        public static float ApplySnapping(float value, float threshold)
        {
            if (value < -threshold)
            {
                return -1f;
            }
            if (value > threshold)
            {
                return 1f;
            }
            return 0f;
        }

        // 获取系统的版本构建号
        public static int GetSystemBuildNumber()
        {
            return Environment.OSVersion.Version.Build;
        }

        // 加载场景
        internal static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        // 获取 Windows 版本信息
        public static string GetWindowsVersion()
        {
            //string text = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");
            //if (text != null)
            //{
            //    string text2 = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CSDVersion");
            //    string text3 = (!Utility.Is32Bit) ? "64Bit" : "32Bit";
            //    int systemBuildNumber = Utility.GetSystemBuildNumber();
            //    return string.Concat(new object[]
            //    {
            //        text,
            //        (text2 == null) ? string.Empty : (" " + text2),
            //        " ",
            //        text3,
            //        " Build ",
            //        systemBuildNumber
            //    });
            //}
            return SystemInfo.operatingSystem;
        }

        // 获取插件文件扩展名
        internal static string PluginFileExtension()
        {
            return ".dll";
        }

        // 扩展数组的大小
        public static void ArrayExpand<T>(ref T[] array, int capacity)
        {
            if (array == null || capacity > array.Length)
            {
                array = new T[Utility.NextPowerOfTwo(capacity)];
            }
        }

        // 计算大于等于给定值的最小的 2 的幂
        public static int NextPowerOfTwo(int value)
        {
            if (value > 0)
            {
                value--;
                value |= value >> 1;
                value |= value >> 2;
                value |= value >> 4;
                value |= value >> 8;
                value |= value >> 16;
                value++;
                return value;
            }
            return 0;
        }

        // 从注册表获取指定路径下的字符串值
        //public static string HKLM_GetString(string path, string key)
        //{
        //    string result;
        //    try
        //    {
        //        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path);
        //        if (registryKey == null)
        //        {
        //            result = string.Empty;
        //        }
        //        else
        //        {
        //            result = (string)registryKey.GetValue(key);
        //        }
        //    }
        //    catch
        //    {
        //        result = null;
        //    }
        //    return result;
        //}
    }
}