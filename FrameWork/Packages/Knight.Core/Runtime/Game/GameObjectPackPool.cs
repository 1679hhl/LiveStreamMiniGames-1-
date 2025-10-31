//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    public class GameObjectPackBase
    {
        public GameObject GameObject;
        public Transform Transform;
        public virtual void OnInstantiate() { }
        public virtual void OnDestroy() { }
        public virtual void OnAlloc() { }
        public virtual void OnFree() { }
    }
    /// <summary>
    /// 内存对象池，只针对GameObject
    /// </summary>
    public class GameObjectPackPool<T> where T : GameObjectPackBase, new()
    {
        /// <summary>
        /// 缓存，这个和rootGo节点下的对象个数一一对应
        /// </summary>
        private TObjectPool<T> mObjectPool;
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
        public GameObject RootGo { get { return this.mRootGo; } }
        public GameObject PrefabGo { get { return this.mPrefabGo; } }
        public int AllocCount => this.mObjectPool.AllocCount;
        public int FreeCount => this.mObjectPool.FreeCount;

        public GameObjectPackPool(GameObject rRootGo, GameObject rPrefabGo, int rInitCount = 0)
        {
            this.mPrefabGo = rPrefabGo;
            this.mObjectPool = new TObjectPool<T>(this.OnAlloc, this.OnFree, this.OnDestroy);

            this.mRootGo = rRootGo;
            this.mRootGoIsSelfCreate = false;

            var rInitAllocGos = new List<T>();
            for (int i = 0; i < rInitCount; i++)
            {
                rInitAllocGos.Add(this.mObjectPool.Alloc());
            }
            for (int i = 0; i < rInitAllocGos.Count; i++)
            {
                this.mObjectPool.Free(rInitAllocGos[i]);
            }
        }

        public T Alloc()
        {
            var rGoPack = this.mObjectPool.Alloc();
            rGoPack.GameObject.transform.position = Vector3.one * 10000;
            rGoPack.GameObject.transform.SetParent(null, false);
            rGoPack.OnAlloc();
            return rGoPack;
        }

        public void Free(T rGoPack)
        {
            if (rGoPack == null) return;
            rGoPack.OnFree();
            this.mObjectPool.Free(rGoPack);
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

        private T OnAlloc()
        {
            var rGoPack = new T();
            rGoPack.GameObject = Object.Instantiate(this.mPrefabGo);
            rGoPack.GameObject.name = this.mPrefabGo.name;
            rGoPack.Transform = rGoPack.GameObject.transform;
            rGoPack.Transform.localPosition = Vector3.one * 10000;
            rGoPack.Transform.localRotation = Quaternion.identity;
            rGoPack.Transform.localScale = Vector3.one;
            rGoPack.OnInstantiate();
            return rGoPack;
        }

        private void OnFree(T rGoPack)
        {
            if (!this.mRootGo) return;
            if (rGoPack == null || !rGoPack.GameObject) return;
            if (rGoPack.Transform == null) rGoPack.Transform = rGoPack.GameObject.transform;

            rGoPack.Transform.SetParent(this.mRootGo.transform);
            rGoPack.Transform.localPosition = Vector3.one * 10000;
            rGoPack.Transform.localRotation = Quaternion.identity;
            rGoPack.Transform.localScale = Vector3.one;
        }

        private void OnDestroy(T rGoPack)
        {
            if (rGoPack != null)
            {
                rGoPack.OnDestroy();
                UtilTool.SafeDestroy(rGoPack.GameObject);
                rGoPack.GameObject = null;
                rGoPack.Transform = null;
            }
        }

        //public void Remove(T rGoPack)
        //{
        //    this.mObjectPool.Remove(rGoPack);
        //}

        // 清理Pool,删除在pool中的对象
        public void ClearUnusedPool()
        {
            this.mObjectPool.Destroy();
        }

        // 已释放的效果出栈由外部释放或管理
        public T PopFreedObject()
        {
            return this.mObjectPool.PopFreedObject();
        }
    }
}