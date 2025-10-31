using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knight.Hotfix.Core;
using System.Threading.Tasks;
using Knight.Core;

namespace Game
{
    public class GameStateMachine : StateMachineBaseAsync<EGameState, float, object, GameStateMachine.SwitchParam>
    {
        public class SwitchParam : SwitchParamBase
        {
            public float ProgressValue = 1f;
            public string ProgressText = LocalizationManager.Instance.GetMultiLanguage(52001); // 加载中 "Loading..."
            public bool ShowLoading = true;
            public bool ShowLoadingAnim = false;
            public bool IsInitZero = true;
        }
        public static GameStateMachine Instance { get { return THotfixSingleton<GameStateMachine>.Instance; } }
        private GameStateMachine() { }
        protected override async Task OnInitialize()
        {
            await this.TryAddState(new GameState_Login());
            await this.TryAddState(new GameState_Lobby());
            await this.TryAddState(new GameState_Battle());
            await this.TryAddState(new GameState_BattleResult());
        }
        protected override void OnStartedSwitch(SwitchParam switchParam)
        {
            if (switchParam.ShowLoading)
            {
                GameLoading.Instance.StartLoading(switchParam.ProgressValue, 1.0f, switchParam.ProgressText, bIsReset: switchParam.IsInitZero,bIsShowTips:true);
                LoadingView_Knight.Instance.SetTopLeftActive(switchParam.ShowLoadingAnim);
            }
        }
        protected override void OnEndedSwitch(SwitchParam switchParam)
        {
#if LOGIC_BATTLE
            return;
#endif
            LogManager.Log($"[GameLoading] CurrentState = {this.CurrentState}");
            if (switchParam.ShowLoading)
            {
                if (this.CurrentState == EGameState.Login)
                {
                    GameLoading.Instance.Hide();
                    GameLoading.Instance.SetBackgroundLayer();
                }
                else
                {
                    GameLoading.Instance.Hide();
                }

            }
            else if (this.CurrentState == EGameState.Login)
            {
                GameLoading.Instance.ShowBackground();
                GameLoading.Instance.SetBackgroundLayer();
            }
            else
            {
                GameLoading.Instance.HideBackground();
                GameLoading.Instance.Hide();
            }
            if (this.IsAllStateSwitchEnd)
            {
                // Model.Instance.Battle.HandleReconnection_StateSwitchEnd();
            }
        }

        public EGameState GetCurrentState()
        {
            return this.CurrentState;
        }
    }
}