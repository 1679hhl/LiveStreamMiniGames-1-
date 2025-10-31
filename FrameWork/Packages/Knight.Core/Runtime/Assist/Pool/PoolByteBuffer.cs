namespace Knight.Core
{
    public class PoolByteBuffer : ByteBuffer, IPoolObject
    {
        public bool Use { get; set; }
        public void Clear()
        {
            this.Clear(false);
        }
        /// <summary>
        /// 获取,使用完成后一定要释放
        /// </summary>
        /// <returns></returns>
        public static PoolByteBuffer Alloc()
        {
            return PoolByteBufferManager.Instance.Alloc();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Free()
        {
            PoolByteBufferManager.Instance.Free(this);
        }
    }
}