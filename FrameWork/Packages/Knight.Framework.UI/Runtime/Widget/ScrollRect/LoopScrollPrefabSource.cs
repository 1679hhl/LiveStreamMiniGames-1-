using UnityEngine;
using System.Collections;
using Knight.Core;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource
    {
        public GameObject rootPoolObj;
        public GameObject prefabObj;
        [HideInInspector]
        public string prefabObjName;
        public int poolSize = 0;
        public int UseSize = 0;

        private GameObjectPool objectPool;
        private bool inited = false;

        public virtual GameObject GetObject()
        {
            if (!this.inited)
            {
                this.objectPool = new GameObjectPool(this.rootPoolObj, this.prefabObj, 0);
                this.inited = true;
                this.prefabObjName = this.prefabObj.name;
            }
            return this.objectPool.Alloc();
        }

        public virtual bool Free(GameObject rGo)
        {
            if (this.objectPool == null) return false;
            this.objectPool.Free(rGo);
            return true;
        }

        public void Preload()
        {
            if (this.poolSize == 0) return;
            this.Preload(this.poolSize, this.UseSize, this.rootPoolObj.GetComponent<RectTransform>());
        }

        public void Preload(int nCount, int nUseCount, RectTransform rTrans)
        {
            if(nUseCount > nCount)
            {
                LogManager.LogError("数量不充足！！！");
                nUseCount = nCount;
            }
            var rGoList = new List<GameObject>();

            for (int i = 0; i < nCount; i++)
            {
                var rGo = this.GetObject();
                rGoList.Add(rGo);
            }
            for (int i = 0; i < rGoList.Count; i++)
            {
                this.Free(rGoList[i]);
            }

            for (int i = 0; i < nUseCount; i++)
            {
                rGoList[i].transform.SetParent(rTrans);
            }
        }
    }
}
