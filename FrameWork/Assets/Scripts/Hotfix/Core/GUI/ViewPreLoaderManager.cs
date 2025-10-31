using System.Threading.Tasks;
using Knight.Core;
using Knight.Hotfix.Core;
using UnityFx.Async;
using System.Collections.Generic;
using Game;

namespace UnityEngine.UI
{
    public class ViewPreLoaderManager : THotfixSingleton<ViewPreLoaderManager>
    {
        public static List<string> InitViews = new List<string>()
        {
            "UIAnnouncement",
            "UICGDisplay",
        };

        public static List<string> LoginViews = new List<string>()
        {
            "UIAccountLogin",
            "UIRoleSelGender",
            "UIRoleSelAvatar",
            "UIRoleCreate",
            "UIRoleSelHero",
            "UIRoleSelHeroInfo",
            "UITutorial",
            "UIAnnouncement_Htx",
        };

        public static List<string> BattleResultViews = new List<string>()
        {
            "UIBattleResult",
            "UIBattleResult_MVP",
            "UIBattleResult_Over",
            "UIBattleResult_Personal",
            "UIBattleResult_Rank",
            "UIBattleResult_Standings"
        };

        public static Dict<string, View.State> RepeateViews = new Dict<string, View.State>()
        {
            { "UIMessageBox",           View.State.Popup},
            { "UIRewardReceive",        View.State.Popup},
            { "UIPlayerLevelRank",      View.State.SceneMainCity3D},
            
        };

        public static Dict<string, View.State> CommonViews = new Dict<string, View.State>()
        {
            { "UIGameSetting",          View.State.Popup            },
            { "UIGameSetGraphic",       View.State.PopupFixing      },
        };

        public static Dict<string, View.State> LowMemoryMainViews = new Dict<string, View.State>()
        {
            { "UIMainFrame",            View.State.PageSwitch       },  // 主界面
        };

        public static Dict<string, View.State> MainViews = new Dict<string, View.State>()
        {
            //---------------------PageSwitch 界面-------------------------
            { "UIMainFrame",            View.State.PageSwitch  },       // 主界面
            { "UILobby_MatchMode",      View.State.PageSwitch  },       // 匹配
            { "UILobby_RankMode",       View.State.PageSwitch  },       // 排位
            { "UIBattleChooseHero",     View.State.PageSwitch  },       // 选择英雄
            { "UIBattleEquipEdit",      View.State.PageSwitch  },       // 备战
            { "UIFilter",               View.State.PageSwitch  },       // 筛选
            { "UIMission",              View.State.PageSwitch  },       // 活动
            { "UILobby_DragonMode",     View.State.PageSwitch  },       // 衍生玩法
            { "UIJLevelReward",         View.State.PageSwitch  },       // 等级奖励
            { "UIBattlePassMain",       View.State.PageSwitch  },       // 战令界面
            { "UIDirectMarketHero",     View.State.PageSwitch  },       // 直购界面
            { "UISystemShopMain",       View.State.PageSwitch  },       // 商店界面

            //---------------------------PageFixed界面---------------------------
            { "UIHomeParent",           View.State.PageSwitch  },       // 我的管理界面
            
            //---------------------------Popup界面---------------------------
            { "UIChat",                 View.State.Popup       },       // 聊天
            { "UIPlayerTaskRemind",     View.State.Popup       },       // 玩家任务提醒
            { "UIRewardReceive",        View.State.Popup       },       // 奖励弹窗

            //---------------------------PopupFixing界面---------------------------
            { "UIInteractiveDialog",    View.State.PopupFixing },       // 交互对话框
        };


        // 战斗内缓存
        public static Dict<string, View.State> BattleViews = new Dict<string, View.State>()
        {
            { "UIBattleEquipStore",     View.State.Popup       },
            { "UIBattleInfo",           View.State.Popup       },
        };
        public static Dict<string, View.State> CustomBattleViews = new Dict<string, View.State>()
        {
            { "UIBattleEquipStore",     View.State.Popup       },
            { "UICustomBattleInfo",     View.State.Popup       },
        };

        private Dict<string, View> mViewCaches;

        public Dict<string, View> ViewCaches
        {
            get => this.mViewCaches;
            set => this.mViewCaches = value;
        }
        private ViewPreLoaderManager()
        {
            this.ViewCaches = new Dict<string, View>();
        }

        public bool IsViewCanRepeat(string rViewName)
        {
            return RepeateViews.ContainsKey(rViewName);
        }

        public void Initialize()
        {
            ViewManager.Instance.FuncIsInCache = (rGUID) =>
            {
                return this.GetViewCacheByGUID(rGUID) != null;
            };
        }

        public View GetViewCache(string rViewName)
        {
            View rView = null;
            this.mViewCaches.TryGetValue(rViewName, out rView);
            return rView;
        }

        public View GetViewCacheByGUID(string rGUID)
        {
            foreach (var rPair in this.mViewCaches)
            {
                if (rPair.Value.GUID.Equals(rGUID))
                {
                    return rPair.Value;
                }
            }
            return null;
        }
    }
}
