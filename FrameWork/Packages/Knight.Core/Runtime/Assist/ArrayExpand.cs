using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knight.Core
{
    public static class ArrayExpand
    {
        /// <summary>
        /// 获取数组的一部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] GetArray<T>(this T[] array, Int32 index, Int32 length)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (index < 0 || index + length > array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index) + "," + nameof(length));
            }
            var ts = new T[length];
            Array.Copy(array, index, ts, 0, length);
            return ts;
        }
        /// <summary>
        /// 获取数组的一部分
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] GetArray<T>(this T[] array, Int64 index, Int64 length)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (index < 0 || index + length > array.LongLength)
            {
                throw new ArgumentOutOfRangeException(nameof(index) + "," + nameof(length));
            }
            var ts = new T[length];
            Array.Copy(array, index, ts, 0, length);
            return ts;
        }
        /// <summary>
        /// 数组转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static String ArrayToString<T>(this T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            var sb = new StringBuilder();
            _ = sb.Append("[");
            var count = array.Length;
            for (var i = 0; i < count; i++)
            {
                _ = sb.Append(array[i]);
                if (i < count - 1)
                {
                    _ = sb.Append(",");
                }
            }
            _ = sb.Append("]");
            return sb.ToString();
        }
        /// <summary>
        /// 将数组中的元素连接起来
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static String Concat<T>(this T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            var sb = new StringBuilder();
            var count = array.Length;
            for (var i = 0; i < count; i++)
            {
                _ = sb.Append(array[i]);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 比较数组是否相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Boolean ArrayEquals<T>(this T[] array, T[] target)
        {
            if (array == null && target == null)
            {
                return true;
            }
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (target == null)
            {
                return false;
            }
            if (array.Length != target.Length)
            {
                return false;
            }
            for (var i = 0; i < array.Length; i++)
            {
                if (!array[i].Equals(target[i]))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 复制数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T[] CloneArray<T>(this T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            var result = new T[array.Length];
            Array.Copy(array, result, array.Length);
            return result;
        }
        public static int IndexOf<T>(this T[] array, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            for (int i = 0; i < array.Length; i++)
            {
                if (value.Equals(array[i]))
                {
                    return i;
                }
            }
            return -1;
        }
        public static Boolean ExistsIndex<T>(this T[] array, Int32 index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            return index >= 0 && index < array.Length;
        }
    }
}
