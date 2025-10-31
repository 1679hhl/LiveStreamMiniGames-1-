using System.Collections.Generic;
using System.Linq;
using Knight.Core;
using System.Threading.Tasks;
using Knight.Framework.Hotfix;
using Knight.Hotfix.Core;
using Pb;
using UnityEngine.UI;

namespace Game
{
    public class BattleResultView : ViewController
    {
        [ViewModelKey("BattleResultViewModel")]
        private BattleResultViewModel mModel;

        protected override Task OnOpen()
        {
            return base.OnOpen();
        }

        [DataBinding]
        public void BtnOnClick_TestFinish(EventArg rEventArg)
        {
            GameStateMachine.Instance.TrySwitchState(EGameState.Lobby, null).WrapErrors();
        }        
    }
}