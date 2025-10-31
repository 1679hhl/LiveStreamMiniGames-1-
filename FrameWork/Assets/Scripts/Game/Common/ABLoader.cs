using System;
using System.Collections.Generic;
using Knight.Core;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Game
{
    public class ABLoader : IAssetLoader
    {
        public void Initialize()
        {
        }

        public void UnloadAsset(AssetLoaderRequest rLoaderRequest)
        {
        }

        public void AddGlobalABEntries(List<string> rGlobalABEntries)
        {
        }

        public void RemoveGlobalABEntries(List<string> rGlobalABEntries)
        {
        }

        public void Reset()
        {
        }

        public bool ExistsAssetBundle(string rABName, bool bIsSimulate)
        {
            return false;
        }

        public bool ExistsAsset(string rABName, string rAssetName, bool bIsSimulate)
        {
            return false;
        }

        public AssetLoaderRequest LoadAssetAsync(string rABName, string rAssetName, bool bIsSimulate)
        {
            return null;
        }

        public AssetLoaderRequest LoadAllAssetsAsync(string rABName, bool bIsSimulate)
        {
            return null;
        }

        public AssetLoaderRequest LoadSceneAsync(string rABName, string rAssetName, LoadSceneMode rLoadSceneMode, bool bIsSimulate)
        {
            return null;
        }

        public AssetLoaderRequest LoadAllScenesAsync(string rABName, LoadSceneMode rLoadSceneMode, bool bIsSimulate)
        {
            return null;
        }

        public AssetLoaderRequest LoadAsset(string rABName, string rAssetName, bool bIsSimulate)
        {
            return null;
        }

        public AssetLoaderRequest LoadAsset(string rABName, string rAssetName, Type rAssetType, bool bIsSimulate)
        {
            return null;
        }

        public Object LoadAssetNoRef(string rABName, string rAssetName, bool bIsSimulate)
        {
            return null;
        }

        public T LoadAssetNoRef<T>(string rABName, string rAssetName, bool bIsSimulate) where T : Object
        {
            return null;
        }

        public AssetLoaderRequest LoadAllAssets(string rABName, bool bIsSimulate)
        {
            return null;
        }

        public AssetLoaderRequest LoadScene(string rABName, string rAssetName, LoadSceneMode rLoadSceneMode, bool bIsSimulate)
        {
            return null;
        }

        public bool IsSumilateMode_Script()
        {
            return ABPlatform.Instance.IsSumilateMode_Script();
        }

        public bool IsSumilateMode_Config()
        {
            return ABPlatform.Instance.IsSumilateMode_Config();
        }

        public bool IsSumilateMode_GUI()
        {
            return ABPlatform.Instance.IsSumilateMode_GUI();
        }

        public bool IsSumilateMode_Avatar()
        {
            return ABPlatform.Instance.IsSumilateMode_Avatar();
        }

        public bool IsSumilateMode_Scene()
        {
            return ABPlatform.Instance.IsSumilateMode_Scene();
        }

        public bool IsSumilateMode_Effect()
        {
            return ABPlatform.Instance.IsSumilateMode_Effect();
        }

        public bool IsSumilateMode_VideoAndSubtitles()
        {
            return ABPlatform.Instance.IsSumilateMode_VideoAndSubtitle();
        }

        public void UnloadAssetBundle(string rABName, bool bIsUnloadAllLoadedObjects)
        {
            throw new NotImplementedException();
        }

        public void ForceUnloadAllAssets()
        {
            throw new NotImplementedException();
        }

        public string[] GetAssetPathsFromAssetBundle(string rABName)
        {
            throw new NotImplementedException();
        }
    }
}