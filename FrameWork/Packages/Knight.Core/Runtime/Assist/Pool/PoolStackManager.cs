using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Knight.Core
{
    public class PoolStackManager<T> : TSingleton<PoolStackManager<T>>, IPoolClear
    {
        private PoolStackManager()
        {
            PoolManager.Instance.AddPoolClear(this);
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        /// <summary>
        /// 使用的Stack数量
        /// </summary>
        public int UseStackCount { get; set; } = 0;
        private ConcurrentQueue<PoolStack<T>> mPoolStackQueue = new ConcurrentQueue<PoolStack<T>>();
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public PoolStack<T> Alloc()
        {
            this.UseStackCount++;
            if (this.mPoolStackQueue.Count == 0)
            {
                return new PoolStack<T>() { Use = true };
            }
            if (!this.mPoolStackQueue.TryDequeue(out var rPoolStack))
            {
                return new PoolStack<T>() { Use = true };
            }
            rPoolStack.Use = true;
            return rPoolStack;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free(PoolStack<T> rPoolStack)
        {
            if (!rPoolStack.Use)
            {
                Knight.Core.LogManager.Log("PoolStack 释放了已经释放的对象");
                return;
            }
            this.UseStackCount--;
            rPoolStack.Use = false;
            rPoolStack.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolStackQueue.Count < this.MaxPoolSize)
            {
                this.mPoolStackQueue.Enqueue(rPoolStack);
            }
        }
        public void ClearPool()
        {
            while (this.mPoolStackQueue.Count > 0)
            {
                this.mPoolStackQueue.TryDequeue(out var _);
            }
        }
    }
}
