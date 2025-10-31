using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knight.Framework.Hotfix;
using Knight.Core;

namespace Game
{
    [TypeResolveCache]
    public class MainLogic_HotfixDLL : IGameMainLogic
    {
        private MainLogic_Hotfix mMainLogic;

        public void Initialize()
        {
            this.mMainLogic = new MainLogic_Hotfix();
            this.mMainLogic.Initialize();
        }

        public void OnDestroy()
        {
            this.mMainLogic?.OnDestroy();
        }
    }
}
