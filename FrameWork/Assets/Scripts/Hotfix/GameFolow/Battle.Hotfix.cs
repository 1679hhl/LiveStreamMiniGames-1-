using Knight.Core;
using Knight.Hotfix.Core;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Game
{
    public class BattleHotfix : THotfixSingleton<BattleHotfix>
    {
        private bool mIsInBattle;

        public bool IsInBattle => this.mIsInBattle;

        private BattleHotfix()
        {
        }

        public async Task Initialize(BattleInitData rInitData)
        {
            this.mIsInBattle = true;
            await BattleManager.Instance.Initialize(rInitData);
            FrameManager.Instance.OpenFixedUI("UIBattlePad1");
        }

        // 逻辑战斗初始化
        public async Task InitializeLogicBattle(GameMode rGameMode)
        {
            this.mIsInBattle = true;
        }

        public async Task Destroy(Action rOnDestroyCompleted = null)
        {
            this.mIsInBattle = false;
            BattleManager.Instance.Destroy();
            rOnDestroyCompleted?.Invoke();
        }
    }
}
