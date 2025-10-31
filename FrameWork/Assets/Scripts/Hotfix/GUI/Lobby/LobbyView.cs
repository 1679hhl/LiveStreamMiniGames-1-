using System.Collections.Generic;
using System.Threading.Tasks;
using Knight.Core;
using Knight.Framework.Hotfix;
using Knight.Hotfix.Core;
using Pb;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class LobbyView : ViewController
    {
        [ViewModelKey("LobbyViewModel")]
        private LobbyViewModel mModel;
        private HotfixMBContainer mMBContainer;
        private Image mHeadIcon;
        private int[] mTestTable= {101};
        protected override Task OnOpen()
        {
            if (GameInit.EGamePlatform != PlatformType.Standlone)
            {
                this.mMBContainer = this.View.GameObject.GetComponent<HotfixMBContainer>();
                this.mHeadIcon = this.mMBContainer.Get<Image>("HeadIcon");
                HeadIconManager.Instance.LoadHeadInfo(Model.Instance.Account.AnchorInfo.HeadUrl,this.mHeadIcon);
                this.mModel.Name = Model.Instance.Account.AnchorInfo.NickName;
                this.mModel.PropChanged("Name",this.mModel.Name);
                
            }
            return base.OnOpen();
        }
        
        [DataBinding]
        private void OnBtnClick_OpenRankView(EventArg rArg)
        {
            FrameManager.Instance.OpenPageUI("UIRank");
        }
        
        [DataBinding]
        public void BegainGame(EventArg rArg)
        {
                GameStateMachine.Instance.TrySwitchState(EGameState.Battle, new GameStateMachine.SwitchParam()
                {
                    ShowLoading = true,
                    EnterParam = new GameState_Battle.EnterParamInfo()
                    {
                        BattleInitData = new BattleInitData()
                        {
                            //MapId = rProIns.WaterId,
                            //LevelId = Model.Instance.Account.AnchorInfo.LevelId,
                        }
                    },
                }).WrapErrors();
        }
    }
}
