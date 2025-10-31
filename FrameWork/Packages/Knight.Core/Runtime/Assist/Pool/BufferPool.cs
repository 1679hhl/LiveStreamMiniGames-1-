using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Knight.Core
{
    public class BufferPool : TSingleton<BufferPool>
    {
        private Dictionary<int, ConcurrentStack<byte[]>> mPool = new Dictionary<int, ConcurrentStack<byte[]>>();

        private BufferPool()
        {
        }

        public byte[] Alloc(int nSize)
        {
            lock (this.mPool)
            {
                if (this.mPool.TryGetValue(nSize, out var rItem))
                {
                    if (rItem.TryPop(out var rBufferItem))
                    {
                        return rBufferItem;
                    }
                    return new byte[nSize];
                }
                else
                {
                    this.mPool.Add(nSize, new ConcurrentStack<byte[]>());
                }
            }
            return new byte[nSize];
        }

        public void Free(byte[] rBuffer)
        {
            lock (this.mPool)
            {
                this.mPool[rBuffer.Length].Push(rBuffer);
            }
        }
    }
}
