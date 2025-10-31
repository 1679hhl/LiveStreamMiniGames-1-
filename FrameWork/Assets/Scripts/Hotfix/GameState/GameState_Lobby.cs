using System.Collections.Generic;
using System.Threading.Tasks;
using Knight.Core;
using Knight.Hotfix.Core;
using UnityEngine.UI;

namespace Game
{
    public class GameState_Lobby : StateBaseAsync<EGameState, float, object, GameStateMachine.SwitchParam>
    {
        public override EGameState StateType { get; } = EGameState.Lobby;
        protected override async Task OnPreEnter(object rEnterParam)
        {
        }

        protected override async Task OnReEnter(object rEnterParam)
        {
            await FrameManager.Instance.OpenPageUIAsync("UILobby1");
        }

        protected override async Task OnEnter(object rEnterParam)
        {
            await FrameManager.Instance.OpenPageUIAsync("UILobby1");
        }
        protected override async Task OnExit(EGameState nextStateType)
        {
            ViewManager.Instance.UnloadUIAssetsWithout(new List<string>(){"GlobalMessageBox"});
        }
    }
}