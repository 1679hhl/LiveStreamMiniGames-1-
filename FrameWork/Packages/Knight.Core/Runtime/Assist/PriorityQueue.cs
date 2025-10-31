using System;

namespace Knight.Core
{
    /// <summary>
    /// 大顶堆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Heap<T> where T : IComparable<T>
    {
        public static void HeapSort(T[] objects, int len)
        {
            for (int i = len / 2 - 1; i >= 0; --i)
                HeapAdjustFromTop(objects, i, len);
            for (int i = len - 1; i > 0; --i)
            {
                Swap(objects, i, 0);
                HeapAdjustFromTop(objects, 0, i);
            }
        }

        public static void HeapAdjustFromBottom(T[] objects, int n)
        {
            while (n > 0 && objects[(n - 1) >> 1].CompareTo(objects[n]) < 0)
            {
                Swap(objects, n, (n - 1) >> 1);
                n = (n - 1) >> 1;
            }
        }

        public static void HeapAdjustFromTop(T[] objects, int n, int len)
        {
            while ((n << 1) + 1 < len)
            {
                int m = (n << 1) + 1;
                if (m + 1 < len && objects[m].CompareTo(objects[m + 1]) < 0)
                    ++m;
                if (objects[n].CompareTo(objects[m]) > 0) return;
                Swap(objects, n, m);
                n = m;
            }
        }

        private static void Swap(T[] objects, int a, int b)
        {
            T tmp = objects[a];
            objects[a] = objects[b];
            objects[b] = tmp;
        }
    }

    /// <summary>
    /// 泛型优先队列，大者先出队
    /// </summary>
    /// <typeparam name="T">实现IComparable<T>的类型</typeparam>
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private const int mDefaultCapacity = 0x10;  //默认容量为16

        private int mHeapLength;
        private T[] mBuffer;

        public PriorityQueue()
        {
            this.mBuffer = new T[mDefaultCapacity];
            this.mHeapLength = 0;
        }

        public int Count()
        {
            return this.mHeapLength;
        }

        public T Get(int nIndex)
        {
            if (nIndex < 0 || nIndex > this.mHeapLength) return default(T);
            return this.mBuffer[nIndex];
        }

        public void Clear()
        {
            this.mHeapLength = 0;
        }

        public bool Empty()
        {
            return this.mHeapLength == 0;
        }

        public T Top()
        {
            if (this.mHeapLength == 0) throw new OverflowException("优先队列为空时无法执行返回队首操作");
            return this.mBuffer[0];
        }

        public void Sort()
        {
            Heap<T>.HeapSort(this.mBuffer, this.mHeapLength);
        }

        public void Push(T obj)
        {
            if (this.mHeapLength == this.mBuffer.Length) this.Expand();
            this.mBuffer[this.mHeapLength] = obj;
            Heap<T>.HeapAdjustFromBottom(this.mBuffer, this.mHeapLength);
            this.mHeapLength++;
        }

        public void Pop()
        {
            if (this.mHeapLength == 0) throw new OverflowException("优先队列为空时无法执行出队操作");
            --this.mHeapLength;
            this.Swap(0, this.mHeapLength);
            Heap<T>.HeapAdjustFromTop(this.mBuffer, 0, this.mHeapLength);
        }

        public int Find(T rObj)
        {
            for (int i = 0; i < this.mHeapLength; i++)
            {
                if (this.mBuffer[i].Equals(rObj)) return i;
            }
            return -1;
        }

        private void Expand()
        {
            Array.Resize<T>(ref this.mBuffer, this.mBuffer.Length * 2);
        }

        private void Swap(int a, int b)
        {
            T tmp = this.mBuffer[a];
            this.mBuffer[a] = this.mBuffer[b];
            this.mBuffer[b] = tmp;
        }
    }
}
