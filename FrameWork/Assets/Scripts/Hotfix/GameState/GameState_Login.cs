using Knight.Core;
using Knight.Hotfix.Core;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Game
{
    public class GameState_Login : StateBaseAsync<EGameState, float, object, GameStateMachine.SwitchParam>
    {
        public override EGameState StateType { get; } = EGameState.Login;
        private ViewContainer mViewContainer;
        private View mAccountLoginView;
        protected override Task OnInitialize()
        {
            this.mViewContainer = new ViewContainer();
            return base.OnInitialize();
        }

        protected override async Task OnLoadAsset(object rEnterParam)
        {
            this.mAccountLoginView = await this.mViewContainer.OpenPageUIAsync("UIAccountLogin");
            GameLoading.Instance.Hide();
        }
        protected override void OnUnloadAsset(EGameState nextStateType)
        {
            this.mViewContainer.CloseAll();
            if (this.mAccountLoginView != null)
            {
                FrameManager.Instance.Close(this.mAccountLoginView.GUID);
            }
            this.mAccountLoginView = null;
        }
        protected override async Task OnReEnter(object rEnterParam)
        {
            // EventManager.Instance.Distribute(GameEvent_Hotfix.kEventLoginReEnterState);
            if (this.mAccountLoginView != null)
            {
                FrameManager.Instance.Close(this.mAccountLoginView.GUID);
                this.mAccountLoginView = null;
            }
            this.mAccountLoginView = await this.mViewContainer.OpenPageUIAsync("UIAccountLogin");
            GameLoading.Instance.Hide();
        }
        
        protected override Task OnEnter(object enterParam)
        {
            return base.OnEnter(enterParam);
        }

        protected override Task OnExit(EGameState nextStateType)
        {
            return base.OnExit(nextStateType);
        }
    }
}