using System.Collections.Generic;
using Knight.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game
{
    public class PoolManager
    {
        private BattleManager mBattleManager;
        private GameObject mMonsterPoolRoot;
        private Dictionary<string, GameObjectPool> mMonsterPools;
        
        public void Initialize(BattleManager rBattleManager,string rPoolName)
        {
            // 初始化特效对象池
            this.mMonsterPoolRoot = new GameObject(rPoolName);
            this.mMonsterPoolRoot.SetActive(false);
            this.mMonsterPools = new Dictionary<string, GameObjectPool>();
            
            this.mBattleManager = rBattleManager;
        }
        
        public void Initialize(string rPoolName)
        {
            this.mMonsterPoolRoot = new GameObject(rPoolName);
            this.mMonsterPoolRoot.SetActive(false);
            this.mMonsterPools = new Dictionary<string, GameObjectPool>();
        }
        
        public GameObject AllocGo(string rLoadPath)
        {
            if (!this.mMonsterPools.TryGetValue(rLoadPath, out var rEffectPool))
            {
                var rPrefabGo = this.mBattleManager.BattleLoader.LoadAsset(rLoadPath);
                if (rPrefabGo == null)
                {
                    LogManager.LogError("get effect prefab failed:" + rLoadPath);
                    return null;
                }
                rEffectPool = new GameObjectPool(this.mMonsterPoolRoot, rPrefabGo, 0);
                this.mMonsterPools.TryAdd(rLoadPath, rEffectPool);
            }
            
            var rGo = rEffectPool.Alloc();
            return rGo;
        }

        public void Free(string rLoadPath, GameObject rPrefabGo)
        {
            if (this.mMonsterPools == null) return;
            
            if (this.mMonsterPools.TryGetValue(rLoadPath, out var rEffectPool))
            {
                rEffectPool.Free(rPrefabGo);
            }
        }

        public void DelayRecycleEffect(string rLoadPath, GameObject rObj, float fDelayTimer, bool bIsTimePauseUnscaled)
        {
            CoroutineManager.Instance.Start(this.Recycle(rLoadPath, rObj, fDelayTimer, bIsTimePauseUnscaled));
        }

        System.Collections.IEnumerator Recycle(string rLoadPath, GameObject rObj, float fDelayTimer,
            bool bIsTimePauseUnscaled)
        {
            yield return WaitForSecondsWithTimeScale.Wait(fDelayTimer, bIsTimePauseUnscaled);
            this.Free(rLoadPath, rObj);
        }

        public void Destroy()
        {
            foreach (var rPair in this.mMonsterPools)
                rPair.Value.Destroy();
            this.mMonsterPools.Clear();
            GameObject.Destroy(this.mMonsterPoolRoot);
        }
    }
}

