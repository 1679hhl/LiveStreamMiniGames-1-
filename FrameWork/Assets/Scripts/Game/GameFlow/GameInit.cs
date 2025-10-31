using System.Collections.Generic;
using System.Threading.Tasks;
using Knight.Core;
using Knight.Framework.Hotfix;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityFx.Async;

namespace Game
{
    public enum PlatformType
    {
        Standlone = -1,
        None = 0,
        Tiktok = 1,
        ShipingHao = 2,
        KuaiShou = 3,
        BZhan = 4,
        DouYin = 5,
        Tiktok_Ye = 15,
        YeYou = 16,
        QQ = 6
    }
    
    public class GameInit : MonoBehaviour
    {
        public string HotfixModule = "";

        public List<GameObject> InitsObjs;
        public int GamePlatForm = 0;
        public int GameID;
    
        public static PlatformType EGamePlatform = PlatformType.None;
        public static int StaticGameID;
        async void Start()
        {
            if (GamePlatForm == (int)PlatformType.Standlone)
            {
                EGamePlatform = PlatformType.Standlone;
            }
            else if (GamePlatForm == (int)PlatformType.DouYin)
            {
                DouYin_SDK.Instance.Initialize();
                EGamePlatform = PlatformType.DouYin;
            }
            else if (GamePlatForm == (int)PlatformType.KuaiShou)
            {
                EGamePlatform = PlatformType.KuaiShou;
            }
            else if (GamePlatForm == (int)PlatformType.YeYou)
            {
                EGamePlatform = PlatformType.YeYou;
            }
            else if (GamePlatForm == (int)PlatformType.Tiktok_Ye)
            {
                TikTok_SDK.Instance.Initialize();
                EGamePlatform = PlatformType.Tiktok_Ye;
            }
            else if (GamePlatForm == (int)PlatformType.None)
            {
                TikTok_SDK.Instance.Initialize();
                EGamePlatform = PlatformType.None;
            }

            StaticGameID = this.GameID;
            // 初始化协程管理器
            CoroutineManager.Instance.Initialize();
            //设置时区秒偏移
            TimeAssist.TimeZoneSecondsOffset = 8 * 3600;
            // 平台初始化
            ABPlatform.Instance.Initialize();
            // 初始化资源加载模块
            AssetLoader.Instance = new ABLoader();
            AssetLoader.Instance.Initialize();
            // 初始化UIRoot
            UIRoot.Instance.Initialize();
            // 加载Loading资源
            await UIRoot.Instance.LoadLoadingView_Async();
            // 初始化加载进度条
            GameLoading.Instance.LoadingView = LoadingView_Knight.Instance;
            GameLoading.Instance.StartLoading(6f, 0.5f, "GameLoading······",bIsShowTips:false); // 游戏初始化阶段，开始加载资源...
            // 防AOT裁剪
            new UnityEngine.UI.DataBindingAOTHelper().EnsureGenericTypes();
            new Game.DataBindingAOTHelper().EnsureGenericTypes();
            // 初始化事件管理器
            EventManager.Instance.Initialize();
            // 初始化网络事件管理器
            ProtoMsgEventManager.Instance.Initialize(ProtoBufDic.PbTypeDic,ProtoBufDic.PbDic);
            // 加载配置
            await GameConfig.Instance.Initialize();
            LocalizationManager.Instance.Initialize(new GameLocalization());
            

            if (this.InitsObjs != null)
            {
                foreach (var rObj in this.InitsObjs)
                    rObj.SetActive(true);
            }

            await WaitAsync.WaitForEndOfFrame();
            //预先加载MessageBox
            await UIRoot.Instance.PreLoadGlobalMessage_Async();
            // 加载其他资源
            await UIRoot.Instance.LoadEssentialView_Async(false);
            WebSocketManager.Instance.Initialize();
            
            await this.LoadMetadataForAOTAssemblies();

            await this.Start_Async(); 
            /*this.LoadHeadIcon(null, string.Empty);
            LogManager.LogRelease(">>>LogicUpdate 1");
            await this.Test();
            LogManager.LogRelease(">>>LogicUpdate 9999");
            await this.Test();
            LogManager.LogRelease(">>>LogicUpdate 8888");*/
        }

        /*public async Task Test()
        {
            await LoadHeadIcon(null, string.Empty);
            LogManager.LogRelease(">>>Test 1");
            await WaitAsync.WaitForSeconds(1f);
            LogManager.LogRelease(">>>Test 2");
        }

        public async Task LoadHeadIcon(Image rImage,string rUrlAvatar)
        {
            await WaitAsync.WaitForEndOfFrame();
            await WaitAsync.WaitForSeconds(1f);
            LogManager.LogRelease(">>>LoadHeadIcon 1");
        }*/

        private async Task Start_Async()
        {
            await WaitAsync.WaitForEndOfFrame();
            foreach (var rObj in this.InitsObjs)
                rObj.SetActive(false);
            // 初始化热更新模块
            HotfixManager.Instance.Initialize();
            // 加载热更新代码资源 
            var dllBytes = await Addressables.LoadAssetAsync<TextAsset>("Game.Hotfix").Task;
            var pdbBytes = await Addressables.LoadAssetAsync<TextAsset>("Game.Hotfix.PDB").Task;
            // 重定向Type
            await HotfixManager.Instance.Load(this.HotfixModule, HotfixManager.Instance.HofixApp,dllBytes.bytes,pdbBytes.bytes);
            
            TypeResolveManager.Instance.Initialize();
            TypeResolveManager.Instance.AddAssembly("Game");
            TypeResolveManager.Instance.AddAssembly("Game.Hotfix",true);
            // 开始热更新端的游戏主逻辑
            HotfixGameMainLogic.Instance.Initialize();
        }
        private void LateUpdate()
        {
            MainLogic.Instance.LateUpdate();
        }

        void Update()
        {
            MainLogic.Instance.Update();
        }
    
        private void OnDestroy()
        {
            HotfixManager.Instance.Dispose();
            MainLogic.Instance.Destroy();
        }
        
        /// <summary>
        /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        /// </summary>
        private async Task LoadMetadataForAOTAssemblies()
        {
            // 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            // 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            // var rRes = await Addressables.LoadAssetAsync<IList<TextAsset>>("AOT").Task;
            // if (rRes != null)
            // {
            //     for (int i = 0; i < rRes.Count; i++)
            //     {
            //         var rTextAsset = rRes[i];
            //         if (rTextAsset == null) continue;
            //
            //         byte[] rDllBytes = rTextAsset.bytes;
            //         // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            //         LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(rDllBytes, HomologousImageMode.SuperSet);
            //         LogManager.Log($"LoadMetadataForAOTAssembly {rTextAsset.name} {err}");
            //     }
            // }
        }
    }
}

