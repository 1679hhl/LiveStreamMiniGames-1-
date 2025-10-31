namespace Knight.Hotfix.Core
{
    using Knight.Core;
    using System;
    using System.Threading.Tasks;

    public abstract class StateBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam>
        where TStateType : Enum
        where TDeltaTime : struct
        where TEnterParam : class
        where TSwitchParam : StateMachineBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam>.SwitchParamBase, new()
    {
        public abstract TStateType StateType { get; }
        public StateMachineBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam> StateMachine { get; private set; }

        public async Task Initialize(StateMachineBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam> rStateMachine)
        {
            this.StateMachine = rStateMachine;
            LogManager.Log($"[StateMatching]{this.StateType} is initialize.");
            await this.OnInitialize();
        }

        public async Task PreEnter(TEnterParam rEnterParam)
        {
            LogManager.Log($"[StateMatching]{this.StateType} is preenter.");
            await this.OnPreEnter(rEnterParam);
        }
        public async Task Enter(TEnterParam rEnterParam)
        {
            LogManager.Log($"[StateMatching]{this.StateType} is load asset.");
            await this.OnLoadAsset(rEnterParam);
            LogManager.Log($"[StateMatching]{this.StateType} is enter.");
            await this.OnEnter(rEnterParam);
        }

        public async Task ReEnter(TEnterParam rEnterParam)
        {
            LogManager.Log($"[StateMatching]{this.StateType} is reenter.");
            await this.OnReEnter(rEnterParam);
        }

        public void Stay(TDeltaTime rDeltaTime, TDeltaTime rUnscaledDeltaTime)
        {
            this.OnStay(rDeltaTime, rUnscaledDeltaTime);
        }
        public async Task Exit(TStateType nextStateType)
        {
            LogManager.Log($"[StateMatching]{this.StateType} is exit.");
            await this.OnExit(nextStateType);
            LogManager.Log($"[StateMatching]{this.StateType} is unload asset.");
            this.OnUnloadAsset(nextStateType);
        }
        public void Destroy()
        {
            LogManager.Log($"[StateMatching]{this.StateType} is unload asset.");
            this.OnUnloadAsset(default);
            LogManager.Log($"[StateMatching]{this.StateType} is destroy.");
            this.OnDestroy();
            this.StateMachine = null;
        }
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        protected virtual async Task OnInitialize() { }
        protected virtual async Task OnLoadAsset(TEnterParam enterParam) { }
        protected virtual async Task OnPreEnter(TEnterParam rEnterParam) { }
        protected virtual async Task OnEnter(TEnterParam enterParam) { }
        protected virtual async Task OnReEnter(TEnterParam rEnterParam) { }
        protected virtual void OnStay(TDeltaTime deltaTime, TDeltaTime unscaledDeltaTime) { }
        protected virtual async Task OnExit(TStateType nextStateType) { }
        protected virtual void OnUnloadAsset(TStateType nextStateType) { }
        protected virtual void OnDestroy() { }
        /// <summary>
        /// 用于状态内部加载
        /// </summary>
        /// <returns></returns>
        public virtual Boolean IsLoading() { return false; }
        public virtual async Task OnSwitchStateComplete() { }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
    }
}
