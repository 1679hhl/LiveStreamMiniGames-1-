
using System.Collections.Generic;
using Knight.Hotfix.Core;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Game
{
    public class GameState_BattleResult : StateBaseAsync<EGameState, float, object, GameStateMachine.SwitchParam>
    {
        public override EGameState StateType { get; } = EGameState.BattleResult;
        protected override async Task OnEnter(object enterParam)
        {
            // 加载局外模块的资源
            var rActiveScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game");
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(rActiveScene);
            await FrameManager.Instance.OpenPageUIAsync("UIBattleResult");
        }
        protected override async Task OnExit(EGameState nextStateType)
        {
            ViewManager.Instance.UnloadUIAssetsWithout(new List<string>());
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}