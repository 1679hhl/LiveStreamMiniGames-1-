using System.Collections.Generic;

namespace Knight.Core
{
    /// <summary>
    /// 对象池列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PoolQueue<T> : Queue<T>, IPoolObject
    {
        public bool Use { get; set; }
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public static PoolQueue<T> Alloc()
        {
            return PoolQueueManager<T>.Instance.Alloc();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free()
        {
            PoolQueueManager<T>.Instance.Free(this);
        }
    }
}
