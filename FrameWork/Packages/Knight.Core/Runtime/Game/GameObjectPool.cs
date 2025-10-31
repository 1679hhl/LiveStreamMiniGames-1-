//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Knight.Core
{
    /// <summary>
    /// 内存对象池，只针对GameObject
    /// </summary>
    public class GameObjectPool
    {
        /// <summary>
        /// 缓存，这个和rootGo节点下的对象个数一一对应
        /// </summary>
        private TObjectPool<GameObject> mObjectPool;
        /// <summary>
        /// 模板对象
        /// </summary>
        private GameObject mPrefabGo;
        /// <summary>
        /// 对象池的根节点
        /// </summary>
        private GameObject mRootGo;
        private bool mRootGoIsSelfCreate;
        /// <summary>
        /// 对象池的根节点
        /// </summary>
        public GameObject RootGo { get { return mRootGo; } }
        
        public GameObject PrefabGo { get { return this.mPrefabGo; } }
        public int AllocCount => this.mObjectPool.AllocCount;
        public int FreeCount => this.mObjectPool.FreeCount;

        public GameObjectPool(string rPoolName, GameObject rPrefabGo, int rInitCount = 0)
        {
            if (rPrefabGo == null) return;

            this.mPrefabGo = rPrefabGo;
            this.mObjectPool = new TObjectPool<GameObject>(OnAlloc, OnFree, OnDestroy);

            this.mRootGo = UtilTool.CreateGameObject(rPoolName);
            this.mRootGo.SetActive(false);
            this.mRootGo.transform.position = new Vector3(0, 1000, 0);
            this.mRootGoIsSelfCreate = true;

            var rInitAllocGos = new List<GameObject>();
            for (int i = 0; i < rInitCount; i++)
            {
                rInitAllocGos.Add(this.mObjectPool.Alloc());
            }
            for (int i = 0; i < rInitAllocGos.Count; i++)
            {
                this.mObjectPool.Free(rInitAllocGos[i]);
            }
        }

        public GameObjectPool(GameObject rRootGo, GameObject rPrefabGo, int rInitCount = 0)
        {
            this.mPrefabGo = rPrefabGo;
            this.mObjectPool = new TObjectPool<GameObject>(OnAlloc, OnFree, OnDestroy);

            this.mRootGo = rRootGo;
            //this.mRootGo.SetActive(false);
            this.mRootGoIsSelfCreate = false;

            var rInitAllocGos = new List<GameObject>();
            for (int i = 0; i < rInitCount; i++)
            {
                rInitAllocGos.Add(this.mObjectPool.Alloc());
            }
            for (int i = 0; i < rInitAllocGos.Count; i++)
            {
                this.mObjectPool.Free(rInitAllocGos[i]);
            }
        }

        public GameObject Alloc()
        {
            var rGo = this.mObjectPool.Alloc();
            rGo.transform.position = Vector3.one * 10000;
            rGo.transform.SetParent(null, false);
            rGo.transform.rotation = Quaternion.Euler(Vector3.zero);
            return rGo;
        }

        public void Free(GameObject rGo)
        {
            if (rGo == null) return;
            this.mObjectPool.Free(rGo);
        }

        public void Destroy()
        {
            this.mPrefabGo = null;
            this.mObjectPool.Destroy();
            if (this.mRootGoIsSelfCreate)
            {
                UtilTool.SafeDestroy(this.mRootGo);
            }
        }
        // 清理Pool,删除在pool中的对象
        public void ClearUnusedPool()
        {
            this.mObjectPool.Destroy();
        }

        private GameObject OnAlloc()
        {
            var rIns = GameObject.Instantiate(this.mPrefabGo);
            rIns.name = this.mPrefabGo.name;
            return rIns;
        }

        private void OnFree(GameObject rGo)
        {
            if (!this.mRootGo) return;
            if (!rGo) return;

            rGo.transform.SetParent(this.mRootGo.transform);
            rGo.transform.localPosition = Vector3.one * 10000;
            rGo.transform.localRotation = Quaternion.identity;
            rGo.transform.localScale = Vector3.one;
        }

        private void OnDestroy(GameObject rGo)
        {
            UtilTool.SafeDestroy(rGo);
        }
    }
}