using System;
using System.Globalization;

namespace lib.utils
{
    public class MathUtils
    {
        /// <summary>
        /// 一个整数的二进制表示中是否包含某个特定的标志位,通常与枚举类型的标志位一起使用
        /// </summary>
        /// <param name="value">待检查的整数</param>
        /// <param name="flag">要检查的标志位</param>
        /// <returns></returns>
        public static bool HasFlag(int value, int flag)
        {
            return (value & flag) == flag;
        }

        public static int AddFlag(int value, int flag)
        {
            return value | flag;
        }

        public static int RemoveFlag(int value, int flag)
        {
            return value & ~flag;
        }

        /// <summary>
        /// 尝试将输入的字符串转换为整数，如果转换成功，则返回转换后的整数值，否则返回默认值 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ParseInt(string value)
        {
            int result;
            try
            {
                result = int.Parse(value);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        /// <summary>
        /// 尝试将输入的字符串转换为浮点数，如果转换成功，则返回转换后的浮点数值，否则返回默认值 0.0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ParseFloat(string value)
        {
            float result;
            try
            {
                result = float.Parse(value, CultureInfo.InvariantCulture);
            }
            catch
            {
                result = 0f;
            }
            return result;
        }
    }
}
