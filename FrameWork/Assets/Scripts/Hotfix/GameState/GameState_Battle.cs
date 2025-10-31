using System.Collections.Generic;
using Knight.Core;
using Knight.Hotfix.Core;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityFx.Async;

namespace Game
{
    public class GameState_Battle : StateBaseAsync<EGameState, float, object, GameStateMachine.SwitchParam>
    {
        public class EnterParamInfo
        {
            public BattleInitData BattleInitData;
        }
        public override EGameState StateType { get; } = EGameState.Battle;
        protected override async Task OnPreEnter(object rEnterParam)
        {
            var rEnterParamInfo = rEnterParam as EnterParamInfo;
            LogManager.Assert(rEnterParamInfo != null, $"Param {nameof(rEnterParam)} can not as {nameof(EnterParamInfo)}");
        }
        protected override async Task OnEnter(object rEnterParam)
        {
            var rEnterParamInfo = rEnterParam as EnterParamInfo;
            LogManager.Assert(rEnterParamInfo != null, $"Param {nameof(rEnterParam)} can not as {nameof(EnterParamInfo)}");
            if(rEnterParamInfo!=null)
                await BattleHotfix.Instance.Initialize(rEnterParamInfo.BattleInitData);
            else
                LogManager.LogError($"[GameState_Battle]:rEnterParamInfo == null!!");
        }

        protected override async Task OnReEnter(object rEnterParam)
        {
            var rEnterParamInfo = rEnterParam as EnterParamInfo;
            LogManager.Assert(rEnterParamInfo != null, $"Param {nameof(rEnterParam)} can not as {nameof(EnterParamInfo)}");
            if (rEnterParamInfo != null)
            {
                GameLoading.Instance.StartLoading(0.5f, 1.0f,"玩法重连中......", bIsReset: false);
                ViewManager.Instance.UnloadUIAssetsWithout(UIRoot.Instance.IgnoreNames);
                await BattleHotfix.Instance.Destroy();
                await BattleHotfix.Instance.Initialize(rEnterParamInfo.BattleInitData);
            }
            await WaitAsync.WaitForSeconds(0.8f);
            GameLoading.Instance.HideBackground();
            GameLoading.Instance.Hide();
        }

        protected override async Task OnExit(EGameState nextStateType)
        {
            await BattleHotfix.Instance.Destroy();
            ViewManager.Instance.UnloadUIAssetsWithout(new List<string>());
        }
    }
}