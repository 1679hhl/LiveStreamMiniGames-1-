using System.Collections.Generic;
using System.Threading.Tasks;
using Knight.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game
{
    public class BattleLoader
    {
        //LoaderDict
        public Dictionary<string, AsyncOperationHandle<Object>> LoaderDict = new Dict<string, AsyncOperationHandle<Object>>();
        
        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="rLoaderKey"></param>
        /// <returns></returns>
        public GameObject LoadAsset(string rLoaderKey)
        {
            if (this.LoaderDict.ContainsKey(rLoaderKey))
                return this.LoaderDict[rLoaderKey].Result as GameObject;
            var rLoaderHandle = Addressables.LoadAssetAsync<Object>(rLoaderKey);
            this.LoaderDict.TryAdd(rLoaderKey,rLoaderHandle);
            rLoaderHandle.WaitForCompletion();
            return this.LoaderDict[rLoaderKey].Result as GameObject;
        }
        
        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="rLoaderKey"></param>
        /// <returns></returns>
        public async Task<GameObject> LoadAssetAsync(string rLoaderKey)
        {
            if (this.LoaderDict.ContainsKey(rLoaderKey))
                return this.LoaderDict[rLoaderKey].Result as GameObject;
            var rLoaderHandle = Addressables.LoadAssetAsync<Object>(rLoaderKey);
            this.LoaderDict.TryAdd(rLoaderKey,rLoaderHandle);
            await rLoaderHandle.Task;
            return this.LoaderDict[rLoaderKey].Result as GameObject;
        }

        public void UnloadAsset(string rLoaderKey)
        {
            if (this.LoaderDict.ContainsKey(rLoaderKey))
            {
                this.LoaderDict[rLoaderKey].Release();
                LogManager.LogRelease($"[AssetRelease]:unload {rLoaderKey} suc!!!");
            }
        }

        /// <summary>
        /// Addressable release是异步的 可能在释放内存的时候会有些残留
        /// </summary>
        /// <param name="bReleaseRAM"></param>
        public void Destroy(bool bReleaseRAM)
        {
            foreach (var rKV in this.LoaderDict)
                rKV.Value.Release();
            this.LoaderDict.Clear();
            if (bReleaseRAM)
                Resources.UnloadUnusedAssets();
        }
    }
}

