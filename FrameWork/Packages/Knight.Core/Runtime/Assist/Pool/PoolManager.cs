using System.Collections.Generic;

namespace Knight.Core
{
    public class PoolManager : TSingleton<PoolManager>
    {
        private PoolManager() { }

        private List<IPoolClear> mPoolClearList = new List<IPoolClear>();
        public void AddPoolClear(IPoolClear rPoolClear)
        {
            this.mPoolClearList.Add(rPoolClear);
        }
        public void ClearPool()
        {
            for (int i = 0; i < this.mPoolClearList.Count; i++)
            {
                this.mPoolClearList[i].ClearPool();
            }
        }
    }
}