using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knight.Core;

namespace Game
{
    [TypeResolveCache]
    public class GameEvent
    {
        public const ulong kEventSortLoopRectButtonDown = 400002; //UI拖拽排序无限列表 拖拽按钮按下或松开
        public const ulong kEventExchangeQuickBroadcastItem = 400003; //UI快捷发言拖拽列表产生交换

        public const ulong kEventBackToLogin = 100001;//返回登录
        public const ulong kEventLoginSuc = 100002;//登录成功后开始心跳
        public const ulong WebSocketReconnectSuc = 100003;//
        
        public const ulong KeventClosePlayMenu = 213;//关闭玩家二级菜单
        
        
        //切换语言
        public const ulong kEventSwitchMultiLanguageSecc = 1002;//切换语言
    }
}
