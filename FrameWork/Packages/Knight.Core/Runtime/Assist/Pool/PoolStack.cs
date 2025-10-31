using System.Collections.Generic;

namespace Knight.Core
{
    /// <summary>
    /// 对象池堆栈
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PoolStack<T> : Stack<T>, IPoolObject
    {
        public bool Use { get; set; }

        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public static PoolStack<T> Alloc()
        {
            return PoolStackManager<T>.Instance.Alloc();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free()
        {
            PoolStackManager<T>.Instance.Free(this);
        }
    }
}
