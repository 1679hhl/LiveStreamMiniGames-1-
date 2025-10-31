using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Knight.Core;

namespace Knight.Hotfix.Core
{
    public class THotfixSPoolObject<T> : THotfixSingleton<THotfixSPoolObject<T>> where T : IHotfixPoolObject, new()
    {
        private THotfixSPoolObject()
        {
        }
        /// <summary>
        /// 对象池最大池化数量
        /// </summary>
        public int MaxPoolSize = 1024;
        /// <summary>
        /// 使用对象数量
        /// </summary>
        public int UseObjectCount { get; set; } = 0;
        private ConcurrentStack<T> mPoolObject = new ConcurrentStack<T>();
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
            if (this.mPoolObject.Count == 0)
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
                if (!this.mPoolObject.TryPop(out rTObject))
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
                LogManager.Log("PoolByteBuffer 释放了已经释放的对象");
                return;
            }
            this.OnFreeFunc?.Invoke(rTObject);
            rTObject.Use = false;
            this.UseObjectCount--;
            rTObject.Clear();
            //达到存储数量后，直接丢弃
            if (this.mPoolObject.Count < this.MaxPoolSize)
            {
                this.mPoolObject.Push(rTObject);
            }
        }
    }
}