using System.Threading.Tasks;
using Knight.Core;
using UnityEngine.UI;
using Knight.Hotfix.Core;
using UnityFx.Async;
using UnityEngine.Rendering;
using UnityEngine;

namespace Game
{
    public class MainLogic_Hotfix
    {
        public void Initialize()
        {
            this.Initialize_Async().WrapErrors();
        }

        public async Task Initialize_Async()
        {
            GameLoading.Instance.StartLoading(10.0f, 1.0f, /*LocalizationManager.Instance.GetMultiLanguage(52005)*/ "游戏初始化阶段，开始加载资源...", bIsReset: false);
            ViewBindTool_Hotfix.Instance.Init();

            // 设置协议版本号
            GameLoading.Instance.SetVersionTextVisible();
            // 初始化渲染设置
            await this.InitializeRenderSetting();

            // 开始主工程的游戏主逻辑循环
            MainLogic.Instance.Initialize();
            // 初始化账号数据Model
            Model.DestroyInstance();
            await Model.Instance.Initialize();
            // UI模块初始化
            ViewModelManager.DestroyInstance();
            ViewModelManager.Instance.Initialize();
            ViewManager.DestroyInstance();
            ViewManager.Instance.Initialize();
            GameLoading.Instance.SetLoadingProgress(1f);
            LogManager.LogRelease("End init..");
            // 移除不使用的UI资源
            UIRoot.Instance.RemoveGlobalInitViews(ViewPreLoaderManager.InitViews);
            await GameStateMachine.Instance.Initialize();
            // await GameStateMachine.Instance.TrySwitchState(EGameState.Login, null);
            //登录、加载背景激活背景
            GameLoading.Instance.SetBackgroundLayer();
            GameLoading.Instance.Hide();
        }

        /// <summary>
        /// 该方法是为了能够清理完所有的对象能够跳转到init更新的地方
        /// </summary>
        public void OnClearAll()
        {
            ViewBindTool_Hotfix.Instance.Destroy();
            ViewManager.Instance.Destroy();
            //UIRoot.Instance.RemoveGlobalInitViewsWithoutKNLGloMsg(UIRoot.Instance.GlobalViewNames);
        }
        public void OnDestroy()
        {
            //清理资源
            this.OnClearAll();
            // 卸载新用户引导的资源
            // HotfixNetworkManager.Instance.Destroy().WrapErrors();
        }

        private async Task InitializeRenderSetting()
        {
        }
    }
}
