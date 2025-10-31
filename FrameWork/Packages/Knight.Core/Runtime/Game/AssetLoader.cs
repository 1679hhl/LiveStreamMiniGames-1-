using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityFx.Async;
using System;
using Object = UnityEngine.Object;

namespace Knight.Core
{
    public class AssetLoaderRequest
    {
        public AsyncCompletionSource<AssetLoaderRequest> AsyncResult;

        public Object Asset;
        public Object[] AllAssets;
        public Scene Scene;
        public Scene[] AllScenes;

        public string Path;
        public string AssetName;
        public Type AssetType;
        public bool IsScene;
        public LoadSceneMode SceneMode;

        public bool IsSimulate;
        public bool IsLoadAllAssets;

        public bool IsDependence;

        public bool IsUnloaded;
        public bool IsLOD;

#if UNITY_EDITOR
        public long StartTicks;
        public long EndTicks;
#endif
        /// <summary>
        /// 加载非场景资源
        /// </summary>
        public AssetLoaderRequest(string rPath, string rAssetName, bool bIsDependence, bool bIsSimulate, bool bIsLoadAllAssets, bool bIsLOD)
        {
            this.Path = rPath;
            this.AssetName = rAssetName;
            this.IsScene = false;
            this.SceneMode = LoadSceneMode.Single;
            this.IsSimulate = bIsSimulate;
            this.IsDependence = bIsDependence;
            this.IsLoadAllAssets = bIsLoadAllAssets;
            this.IsUnloaded = false;
            this.IsLOD = bIsLOD;

            if (this.IsLoadAllAssets)
            {
                this.AssetName = "AllAssets";
            }
        }

        public AssetLoaderRequest(string rPath, string rAssetName, Type rAssetType, bool bIsDependence, bool bIsSimulate, bool bIsLoadAllAssets)
        {
            this.Path = rPath;
            this.AssetName = rAssetName;
            this.AssetType = rAssetType;
            this.IsScene = false;
            this.SceneMode = LoadSceneMode.Single;
            this.IsSimulate = bIsSimulate;
            this.IsDependence = bIsDependence;
            this.IsLoadAllAssets = bIsLoadAllAssets;
            this.IsUnloaded = false;
            this.IsLOD = false;

            if (this.IsLoadAllAssets)
            {
                this.AssetName = "AllAssets";
            }
        }

        /// <summary>
        /// 加载场景资源
        /// </summary>
        public AssetLoaderRequest(string rPath, string rAssetName, LoadSceneMode rSceneMode, bool bIsAllAssets, bool bIsDependence, bool bIsSimulate)
        {
            this.Path = rPath;
            this.AssetName = rAssetName;
            this.IsScene = true;
            this.SceneMode = rSceneMode;
            this.IsSimulate = bIsSimulate;
            this.IsDependence = bIsDependence;
            this.IsLoadAllAssets = bIsAllAssets;
            this.IsUnloaded = false;
            this.IsLOD = false;

            if (this.IsLoadAllAssets)
            {
                this.AssetName = "AllAssets";
            }
        }

        public void Start(Func<AssetLoaderRequest, Task> rRequestFunc)
        {
            this.SetStartTicks(TimeAssist.ClientNowTicks());
            this.AsyncResult = new AsyncCompletionSource<AssetLoaderRequest>();
            rRequestFunc.Invoke(this);
        }

        public void SetResult(AssetLoaderRequest rAssetLoaderRequest)
        {
            this.SetEndTicks(TimeAssist.ClientNowTicks());
            if (this.AsyncResult != null)
                this.AsyncResult.SetResult(rAssetLoaderRequest);
        }

        public IAsyncOperation<AssetLoaderRequest> Wait()
        {
            return this.AsyncResult.Operation;
        }
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void SetStartTicks(long nStartTicks)
        {
#if UNITY_EDITOR
            this.StartTicks = nStartTicks;
#endif
        }
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void SetEndTicks(long nEndTicks)
        {
#if UNITY_EDITOR
            this.EndTicks = nEndTicks;
#endif
            this.PrintUseTime();
        }
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void PrintUseTime()
        {
#if UNITY_EDITOR
            LogManager.Log($"AssetLoaderRequest AssetBundlePath:{this.Path} AssetName:{this.AssetName} Time:{(this.EndTicks - this.StartTicks) / 10000f}ms");
#endif
        }
    }

    public interface IAssetLoader
    {
        void Initialize();
        void UnloadAsset(AssetLoaderRequest rLoaderRequest);
        void AddGlobalABEntries(List<string> rGlobalABEntries);
        void RemoveGlobalABEntries(List<string> rGlobalABEntries);
        void Reset();
        bool ExistsAssetBundle(string rABName, bool bIsSimulate);
        bool ExistsAsset(string rABName, string rAssetName, bool bIsSimulate);
        AssetLoaderRequest LoadAssetAsync(string rABName, string rAssetName, bool bIsSimulate);
        AssetLoaderRequest LoadAllAssetsAsync(string rABName, bool bIsSimulate);
        AssetLoaderRequest LoadSceneAsync(string rABName, string rAssetName, LoadSceneMode rLoadSceneMode, bool bIsSimulate);
        AssetLoaderRequest LoadAllScenesAsync(string rABName, LoadSceneMode rLoadSceneMode, bool bIsSimulate);

        AssetLoaderRequest LoadAsset(string rABName, string rAssetName, bool bIsSimulate);
        AssetLoaderRequest LoadAsset(string rABName, string rAssetName, Type rAssetType, bool bIsSimulate);
        UnityEngine.Object LoadAssetNoRef(string rABName, string rAssetName, bool bIsSimulate);
        T LoadAssetNoRef<T>(string rABName, string rAssetName, bool bIsSimulate) where T : Object;
        AssetLoaderRequest LoadAllAssets(string rABName, bool bIsSimulate);
        AssetLoaderRequest LoadScene(string rABName, string rAssetName, LoadSceneMode rLoadSceneMode, bool bIsSimulate);

        bool IsSumilateMode_Script();
        bool IsSumilateMode_Config();
        bool IsSumilateMode_GUI();
        bool IsSumilateMode_Avatar();
        bool IsSumilateMode_Scene();
        bool IsSumilateMode_Effect();
        bool IsSumilateMode_VideoAndSubtitles();
        void UnloadAssetBundle(string rABName, bool bIsUnloadAllLoadedObjects);
        void ForceUnloadAllAssets();

#if UNITY_EDITOR
        string[] GetAssetPathsFromAssetBundle(string rABName);
#endif

    }

    public class AssetLoader
    {
        public static IAssetLoader Instance;
    }
}
