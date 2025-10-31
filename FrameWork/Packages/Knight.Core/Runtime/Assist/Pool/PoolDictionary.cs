using System.Collections.Generic;

namespace Knight.Core
{
    /// <summary>
    /// 对象池字典
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PoolDictionary<K, V> : Dictionary<K, V>, IPoolObject
    {
        public bool Use { get; set; }

        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public static PoolDictionary<K, V> Alloc()
        {
            return PoolDictionaryManager<K, V>.Instance.Alloc();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free()
        {
            PoolDictionaryManager<K, V>.Instance.Free(this);
        }
    }
}
