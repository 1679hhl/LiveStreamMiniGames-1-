using System.Collections.Concurrent;

namespace Knight.Core
{
    public class PoolHashSetManager<T> : TSingleton<PoolHashSetManager<T>>, IPoolClear
    {
        private PoolHashSetManager()
        {
            PoolManager.Instance.AddPoolClear(this);
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        public int UseListCount { get; private set; } = 0;
        private ConcurrentQueue<PoolHashSet<T>> mPoolHashSetQueue = new ConcurrentQueue<PoolHashSet<T>>();
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public PoolHashSet<T> Alloc()
        {
            this.UseListCount++;
            if (this.mPoolHashSetQueue.Count == 0)
            {
                return new PoolHashSet<T>() { Use = true };
            }
            if (!this.mPoolHashSetQueue.TryDequeue(out var rPoolHashSet))
            {
                return new PoolHashSet<T>() { Use = true };
            }
            rPoolHashSet.Use = true;
            return rPoolHashSet;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free(PoolHashSet<T> rPoolHashSet)
        {
            if (!rPoolHashSet.Use)
            {
                Knight.Core.LogManager.Log("PoolHashSet 释放了已经释放的对象");
                return;
            }
            rPoolHashSet.Use = false;
            this.UseListCount--;
            rPoolHashSet.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolHashSetQueue.Count < this.MaxPoolSize)
            {
                this.mPoolHashSetQueue.Enqueue(rPoolHashSet);
            }
        }
        public void ClearPool()
        {
            while (this.mPoolHashSetQueue.Count > 0)
            {
                this.mPoolHashSetQueue.TryDequeue(out var _);
            }
        }
    }
}
