using System.Collections.Concurrent;

namespace Knight.Core
{
    public class PoolDictionaryManager<K, V> : TSingleton<PoolDictionaryManager<K, V>>, IPoolClear
    {
        private PoolDictionaryManager()
        {
            PoolManager.Instance.AddPoolClear(this);
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        /// <summary>
        /// 使用的Dictionary数量
        /// </summary>
        public int UseDictionaryCount { get; set; } = 0;
        private ConcurrentQueue<PoolDictionary<K, V>> mPoolDictionaryQueue = new ConcurrentQueue<PoolDictionary<K, V>>();
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public PoolDictionary<K, V> Alloc()
        {
            this.UseDictionaryCount++;
            if (this.mPoolDictionaryQueue.Count == 0)
            {
                return new PoolDictionary<K, V>() { Use = true };
            }
            if (!this.mPoolDictionaryQueue.TryDequeue(out var rPoolDictionary))
            {
                return new PoolDictionary<K, V>() { Use = true };
            }
            rPoolDictionary.Use = true;
            return rPoolDictionary;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free(PoolDictionary<K, V> rPoolDictionary)
        {
            if (!rPoolDictionary.Use)
            {
                Knight.Core.LogManager.Log("PoolDictionary 释放了已经释放的对象");
                return;
            }
            this.UseDictionaryCount--;
            rPoolDictionary.Use = false;
            rPoolDictionary.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolDictionaryQueue.Count < this.MaxPoolSize)
            {
                this.mPoolDictionaryQueue.Enqueue(rPoolDictionary);
            }
        }
        public void ClearPool()
        {
            while (this.mPoolDictionaryQueue.Count > 0)
            {
                this.mPoolDictionaryQueue.TryDequeue(out var _);
            }
        }
    }
}
