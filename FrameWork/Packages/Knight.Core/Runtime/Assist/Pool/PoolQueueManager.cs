using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Knight.Core
{
    public class PoolQueueManager<T> : TSingleton<PoolQueueManager<T>>, IPoolClear
    {
        private PoolQueueManager()
        {
            PoolManager.Instance.AddPoolClear(this);
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        public int UseQueueCount = 0;
        private ConcurrentQueue<PoolQueue<T>> mPoolQueueQueue = new ConcurrentQueue<PoolQueue<T>>();

        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public PoolQueue<T> Alloc()
        {
            this.UseQueueCount++;
            if (this.mPoolQueueQueue.Count == 0)
            {
                return new PoolQueue<T>() { Use = true };
            }
            if (!this.mPoolQueueQueue.TryDequeue(out var rPoolQueue))
            {
                return new PoolQueue<T>() { Use = true };
            }
            rPoolQueue.Use = true;
            return rPoolQueue;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free(PoolQueue<T> rPoolQueue)
        {
            if (!rPoolQueue.Use)
            {
                Knight.Core.LogManager.Log("PoolQueue 释放了已经释放的对象");
                return;
            }
            this.UseQueueCount--;
            rPoolQueue.Use = false;
            rPoolQueue.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolQueueQueue.Count < this.MaxPoolSize)
            {
                this.mPoolQueueQueue.Enqueue(rPoolQueue);
            }
        }
        public void ClearPool()
        {
            while (this.mPoolQueueQueue.Count > 0)
            {
                this.mPoolQueueQueue.TryDequeue(out var _);
            }
        }
    }
}
