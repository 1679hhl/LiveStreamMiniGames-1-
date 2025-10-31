using System.Collections.Concurrent;

namespace Knight.Core
{
    public class PoolListManager<T> : TSingleton<PoolListManager<T>>, IPoolClear
    {
        private PoolListManager()
        {
            PoolManager.Instance.AddPoolClear(this);
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        public int UseListCount { get; private set; } = 0;
        private ConcurrentQueue<PoolList<T>> mPoolListQueue = new ConcurrentQueue<PoolList<T>>();
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public PoolList<T> Alloc()
        {
            this.UseListCount++;
            if (this.mPoolListQueue.Count == 0)
            {
                return new PoolList<T>() { Use = true };
            }
            if (!this.mPoolListQueue.TryDequeue(out var rPoolList))
            {
                return new PoolList<T>() { Use = true };
            }
            rPoolList.Use = true;
            return rPoolList;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free(PoolList<T> rPoolList)
        {
            if (!rPoolList.Use)
            {
                Knight.Core.LogManager.Log("PoolList 释放了已经释放的对象");
                return;
            }
            rPoolList.Use = false;
            this.UseListCount--;
            rPoolList.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolListQueue.Count < this.MaxPoolSize)
            {
                this.mPoolListQueue.Enqueue(rPoolList);
            }
        }
        public void ClearPool()
        {
            while (this.mPoolListQueue.Count > 0)
            {
                this.mPoolListQueue.TryDequeue(out var _);
            }
        }
    }
}
