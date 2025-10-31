using System.Collections.Generic;

namespace Knight.Core
{
    public class PoolHashSet<T> : HashSet<T>
    {
        public bool Use { get; set; }
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public static PoolHashSet<T> Alloc()
        {
            return PoolHashSetManager<T>.Instance.Alloc();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free()
        {
            PoolHashSetManager<T>.Instance.Free(this);
        }
    }
}
