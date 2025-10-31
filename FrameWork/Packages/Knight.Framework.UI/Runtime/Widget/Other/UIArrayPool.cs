using Knight.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class UIArrayPool : MonoBehaviour
    {
        public GameObject       RootPoolObj;
        public GameObject       PrefabObj;

        private GameObjectPool  mObjectPool;
        private bool            mIsInited = false;
        public int              poolSize = 0;
        public int              useSize = 0;

        public GameObject GetObject()
        {
            if (!this.mIsInited)
            {
                if(this.RootPoolObj.activeSelf)
                {
                    this.RootPoolObj.SetActive(false);
                }
                this.mObjectPool = new GameObjectPool(this.RootPoolObj, this.PrefabObj, 0);
                this.mIsInited = true;
            }
            return this.mObjectPool.Alloc();
        }

        public virtual void Free(GameObject rGo)
        {
            if (this.mObjectPool == null) return;
            this.mObjectPool.Free(rGo);
        }
        public void Preload()
        {
            if (this.poolSize == 0) return;
            this.Preload(this.poolSize, this.useSize, this.RootPoolObj.GetComponent<RectTransform>());
        }

        public void Preload(int nCount, int nUseCount, RectTransform rTrans)
        {
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
