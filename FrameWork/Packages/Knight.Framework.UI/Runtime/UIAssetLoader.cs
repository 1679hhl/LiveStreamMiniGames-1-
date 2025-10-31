//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Knight.Core;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityFx.Async;

namespace UnityEngine.UI
{
    /// <summary>
    /// UI加载器
    /// </summary>
    public class UIAssetLoader : TSingleton<UIAssetLoader>
    {
        public class LoaderRequest
        {
            public GameObject                   ViewPrefabGo;
            public string                       ViewName;
            // public AssetLoaderRequest           Request;
            public AsyncOperationHandle<GameObject> handle;

            public LoaderRequest(string rViewName)
            {
                this.ViewName = rViewName;
            }
        }

        private Dictionary<string, LoaderRequest> mUILoaderRequestDict;

        private UIAssetLoader()
        {
            this.mUILoaderRequestDict = new Dictionary<string, LoaderRequest>();
        }

        public async Task<LoaderRequest> LoadUIAsync(string rViewName)
        {
            if (this.mUILoaderRequestDict.TryGetValue(rViewName, out var rRequest))
            {
                return rRequest;
            }
            rRequest = new LoaderRequest(rViewName);
            string rUIABPath =  rRequest.ViewName;
            var m_handle = Addressables.LoadAssetAsync<GameObject>(rUIABPath);
            var rRes = await m_handle.Task;
            if (rRes != null)
            {
                rRequest.handle = m_handle;
                rRequest.ViewPrefabGo = rRes;
                rRequest.ViewPrefabGo.SetActive_Effective(false);
                if (!this.mUILoaderRequestDict.ContainsKey(rViewName))
                {
                    this.mUILoaderRequestDict.Add(rViewName, rRequest);
#if UNITY_EDITOR
                    UIAtlasCheck.CheckAtlasRefUIPrefab(rViewName, rRequest.ViewPrefabGo);
#endif
                }
                else
                {
                    LogManager.LogWarning($"重复加载UI UIViewName:{rViewName}");
                }
            }
            return rRequest;
        }

        /// <summary>
        /// 同步加载UI
        /// </summary>
        public LoaderRequest LoadUI(string rViewName)
        {
            if (this.mUILoaderRequestDict.TryGetValue(rViewName, out var rRequest))
            {
                return rRequest;
            }
            rRequest = new LoaderRequest(rViewName);
            var op = Addressables.LoadAssetAsync<GameObject>(rViewName);
            GameObject go = op.WaitForCompletion();
            
            if (go != null)
            {
                rRequest.ViewPrefabGo = go;
                rRequest.ViewPrefabGo.SetActive_Effective(false);
                rRequest.handle = op;
                this.mUILoaderRequestDict.Add(rViewName, rRequest);
#if UNITY_EDITOR
                UIAtlasCheck.CheckAtlasRefUIPrefab(rViewName, rRequest.ViewPrefabGo);
#endif
            }
            return rRequest;
        }

        public void LockUnloadUI(params string[] rViewNames)
        {
            var rUIABList = new List<string>();
            for (int i = 0; i < rViewNames.Length; i++)
            {
                string rUIABPath = "game/gui/prefabs/" + rViewNames[i].ToLower() + ".ab";
                rUIABList.Add(rUIABPath);
            }
            AssetLoader.Instance.AddGlobalABEntries(rUIABList);
        }

        public void UnlockUnloadUI(params string[] rViewNames)
        {
            var rUIABList = new List<string>();
            for (int i = 0; i < rViewNames.Length; i++)
            {
                string rUIABPath = "game/gui/prefabs/" + rViewNames[i].ToLower() + ".ab";
                rUIABList.Add(rUIABPath);
            }
            AssetLoader.Instance.RemoveGlobalABEntries(rUIABList);
        }

        /// <summary>
        /// 卸载UI资源
        /// </summary>
        public void UnloadUI(string rViewName)
        {
            if (!this.mUILoaderRequestDict.TryGetValue(rViewName, out var rLoaderRequest)) return;

            // AssetLoader.Instance.UnloadAsset(rLoaderRequest.Request);
            Addressables.Release(rLoaderRequest.handle);
            rLoaderRequest.ViewName = string.Empty;
            rLoaderRequest.ViewPrefabGo = null;
            this.mUILoaderRequestDict.Remove(rViewName);
        }
        public void Reset()
        {
            this.mUILoaderRequestDict.Clear();
        }

        public void UnloadWithoutUIAssets(List<string> rWithoutUIAssets)
        {
            var rNeedRemoveUIAssets = new List<string>();
            foreach (var rPair in this.mUILoaderRequestDict)
            {
                if (rWithoutUIAssets.Contains(rPair.Key)) continue;
                rNeedRemoveUIAssets.Add(rPair.Key);
            }
            for (int i = 0; i < rNeedRemoveUIAssets.Count; i++)
            {
                this.UnloadUI(rNeedRemoveUIAssets[i]);
            }
        }
    }
}
