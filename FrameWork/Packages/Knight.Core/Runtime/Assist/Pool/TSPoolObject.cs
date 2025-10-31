using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Knight.Core
{
    public class TSPoolObject<T> : TSingleton<TSPoolObject<T>>, IPoolClear where T : class, IPoolObject, new()
    {
        private TSPoolObject()
        {
            PoolManager.Instance.AddPoolClear(this);
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        /// <summary>
        /// 使用对象数量
        /// </summary>
        public int UseObjectCount { get; set; } = 0;
        private ConcurrentQueue<T> mPoolObjectQueue = new ConcurrentQueue<T>();
        private Func<T> AllocFunc;
        private Action<T> OnAllocFunc;
        private Action<T> OnFreeFunc;
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public T Alloc()
        {
            this.UseObjectCount++;
            T rTObject;
            if (this.mPoolObjectQueue.Count == 0)
            {
                if (this.AllocFunc == null)
                {
                    rTObject = new T();
                }
                else
                {
                    rTObject = this.AllocFunc.Invoke();
                }
            }
            else
            {
                if (!this.mPoolObjectQueue.TryDequeue(out rTObject))
                {

                    rTObject = new T();
                }
            }
            rTObject.Use = true;
            this.OnAllocFunc?.Invoke(rTObject);
            return rTObject;
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free(T rTObject)
        {
            if (!rTObject.Use)
            {
                Knight.Core.LogManager.Log($"TSPoolObject<{nameof(T)}> 释放了已经释放的对象");
                return;
            }
            this.OnFreeFunc?.Invoke(rTObject);
            rTObject.Use = false;
            this.UseObjectCount--;
            rTObject.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolObjectQueue.Count < this.MaxPoolSize)
            {
                this.mPoolObjectQueue.Enqueue(rTObject);
            }
        }
        public void ClearPool()
        {
            while (this.mPoolObjectQueue.Count > 0)
            {
                this.mPoolObjectQueue.TryDequeue(out var _);
            }
        }
    }
}