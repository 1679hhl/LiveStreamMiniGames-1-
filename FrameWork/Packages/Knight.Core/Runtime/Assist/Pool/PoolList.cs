using System.Collections.Generic;

namespace Knight.Core
{
    /// <summary>
    /// 对象池列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PoolList<T> : List<T>, IPoolObject
    {
        public bool Use { get; set; }
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public static PoolList<T> Alloc()
        {
            return PoolListManager<T>.Instance.Alloc();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free()
        {
            PoolListManager<T>.Instance.Free(this);
        }
    }
}
