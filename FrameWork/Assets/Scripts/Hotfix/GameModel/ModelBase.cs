using Knight.Core;
using Knight.Hotfix.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game
{
    public class ModelBase
    {
        /// <summary>
        /// 该属性只可由Mode进行赋值
        /// </summary>
        public bool IsStandalone { get; set; }

        protected List<HotfixKnightEvent> mEvents;
        protected List<HotfixProtoMsgEvent> mProtoMsgEvents;

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public virtual async Task OnInitialize()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            HotfixKnightObjectTool.BindEvent(this, out this.mEvents, out this.mProtoMsgEvents);
        }
        public virtual void OnDestroy()
        {
            HotfixKnightObjectTool.UnbindEvent(this.mEvents, this.mProtoMsgEvents);
        }
        public virtual void OnLogin(EventArg rEventArg)
        {
        }
        public virtual void OnLogout(EventArg rEventArg)
        {
        }
        public virtual void OnReLogin(EventArg rEventArg)
        {
        }
        public virtual void OnStandaloneLogin(EventArg rEventArg)
        {
        }
        public virtual void OnBattleStart(EventArg rEventArg)
        {
        }
        public virtual void OnBattleEnd(EventArg rEventArg)
        {
        }
    }
}