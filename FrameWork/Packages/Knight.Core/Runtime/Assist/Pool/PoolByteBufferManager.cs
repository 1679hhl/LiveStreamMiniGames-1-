using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Knight.Core
{
    public class PoolByteBufferManager : TSingleton<PoolByteBufferManager>, IPoolClear
    {
        private PoolByteBufferManager()
        {
            PoolManager.Instance.AddPoolClear(this);
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        public int UseByteBufferCount { get; set; } = 0;
        private ConcurrentQueue<PoolByteBuffer> mPoolByteBufferQueue = new ConcurrentQueue<PoolByteBuffer>();
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public PoolByteBuffer Alloc()
        {
            this.UseByteBufferCount++;
            if (this.mPoolByteBufferQueue.Count == 0)
            {
                return new PoolByteBuffer() { Use = true };
            }
            if (!this.mPoolByteBufferQueue.TryDequeue(out var rPoolByteBuffer))
            {
                return new PoolByteBuffer() { Use = true };
            }
            rPoolByteBuffer.Use = true;
            return rPoolByteBuffer;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free(PoolByteBuffer rPoolByteBuffer)
        {
            if (!rPoolByteBuffer.Use)
            {
                Knight.Core.LogManager.Log("PoolByteBuffer 释放了已经释放的对象");
                return;
            }
            this.UseByteBufferCount--;
            rPoolByteBuffer.Use = false;
            rPoolByteBuffer.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolByteBufferQueue.Count < this.MaxPoolSize)
            {
                this.mPoolByteBufferQueue.Enqueue(rPoolByteBuffer);
            }
        }
        public void ClearPool()
        {
            while (this.mPoolByteBufferQueue.Count > 0)
            {
                this.mPoolByteBufferQueue.TryDequeue(out var _);
            }
        }
    }
}