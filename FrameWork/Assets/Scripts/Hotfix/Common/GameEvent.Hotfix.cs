using Knight.Core;
using Knight.Hotfix.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class GameEvent_Hotfix
    {
        public const ulong kEventLogin_Login                = 10010001; // 登录
        public const ulong kEventLogin_Logout               = 10010002; // 登出
        public const ulong kEventLogin_ReLogin              = 10010003; // 重新登录
        public const ulong kEventBattle_OnBattleStart       = 10010005; // 战斗开始
        public const ulong kEventBattle_OnBattleEnd         = 10010006; // 战斗结算
        
        public const ulong kEventTest                       = 1001; // 战斗结算
    }
}