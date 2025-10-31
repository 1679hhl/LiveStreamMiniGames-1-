namespace Knight.Hotfix.Core
{
    using Knight.Core;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public abstract class StateMachineBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam>
        where TStateType : Enum
        where TDeltaTime : struct
        where TEnterParam : class
        where TSwitchParam : StateMachineBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam>.SwitchParamBase, new()
    {
        public class SwitchParamBase
        {
            public TStateType NextStateType;
            public TEnterParam EnterParam;
            public TaskCompletionSource<bool> SwitchResult;
        }

        protected StateBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam> mCurrentState;
        protected Dictionary<TStateType, StateBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam>> mStateDict;
        protected List<TSwitchParam> mSwitchStateList;
        public Boolean IsSwitching { get; private set; }

        public bool IsAllStateSwitchEnd
        {
            get
            {
                if (this.mSwitchStateList == null) return true;
                return this.mSwitchStateList.Count == 0;
            }
        }

        protected TStateType CurrentState { get { return this.mCurrentState.StateType; } }
        public Task Initialize()
        {
            this.mCurrentState = null;
            this.mStateDict = new Dictionary<TStateType, StateBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam>>();
            this.mSwitchStateList = new List<TSwitchParam>();
            this.IsSwitching = false;
            return this.OnInitialize();
        }
        public void Destroy()
        {
            this.OnDestroy();
            this.mCurrentState?.Exit(default);
            this.mCurrentState?.Destroy();
            this.mCurrentState = null;
            this.mStateDict = null;
            this.mSwitchStateList = null;
            this.IsSwitching = false;
        }
        protected async Task<Boolean> TryAddState(StateBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam> rState, TEnterParam rEnterParam = null)
        {
            LogManager.Assert(rState != null, "[StateMatching]state can not null.");
            if (!this.mStateDict.ContainsKey(rState.StateType))
            {
                LogManager.Log($"[StateMatching]{rState.StateType} is added to state matching.");
                this.mStateDict.Add(rState.StateType, rState);
                var bFirst = false;
                if (this.mCurrentState == null)
                {
                    this.mCurrentState = rState;
                    bFirst = true;
                }
                await rState.Initialize(this);
                if (bFirst)
                {
                    await rState.Enter(rEnterParam);
                }
                return true;
            }
            LogManager.LogWarning($"[StateMatching]StateType {rState.StateType} exits.");
            return false;
        }
        public Boolean TryGetState<T>(TStateType stateType, out T t) where T : StateBaseAsync<TStateType, TDeltaTime, TEnterParam, TSwitchParam>
        {
            if (this.mStateDict.TryGetValue(stateType, out var state))
            {
                t = (T)state;
                return true;
            }
            t = default;
            return false;
        }
        public async Task<Boolean> TrySwitchState(TStateType nextStateType, TSwitchParam switchParam = null)
        {
            if (!this.mStateDict.TryGetValue(nextStateType, out var rNextState))
            {
                LogManager.LogWarning($"[StateMatching]Can not find StateType:{nextStateType}.");
                return false;
            }
            if (switchParam == null)
            {
                switchParam = new TSwitchParam();
            }

            if (switchParam.SwitchResult == null)
            {
                switchParam.SwitchResult = new TaskCompletionSource<bool>();
            }
            // 如果当前正在切换,则进行缓存
            if (this.IsSwitching || this.mCurrentState.IsLoading())
            {
                LogManager.Log($"[StateMatching]{nextStateType} is add to switch cache.");

                // 当前仅缓存最新的一次切换需求
                if (this.mSwitchStateList.Count > 0)
                {
                    this.mSwitchStateList[0].SwitchResult.SetResult(true);
                    this.mSwitchStateList.RemoveAt(0);
                }
                switchParam.NextStateType = nextStateType;
                this.mSwitchStateList.Add(switchParam);
                return await switchParam.SwitchResult.Task;
            }

            this.IsSwitching = true;

            if (this.mCurrentState != null && this.mCurrentState.StateType.Equals(nextStateType))
            {
                await this.mCurrentState.ReEnter(switchParam.EnterParam);
                LogManager.LogWarning($"[StateMatching]Current state type is equals next state type CurrentStateType:{this.mCurrentState.StateType} NextStateType:{nextStateType}.");
            }
            else
            {
                LogManager.Log($"[StateMatching]{nextStateType} is started switch.");
                this.OnStartedSwitch(switchParam);
                var currentState = this.mCurrentState;
                this.mCurrentState = rNextState;

                await rNextState.PreEnter(switchParam.EnterParam);
                await currentState.Exit(nextStateType);
                await rNextState.Enter(switchParam.EnterParam);

                this.OnEndedSwitch(switchParam);
                LogManager.Log($"[StateMatching]{nextStateType} is ended switch.");
            }

            await this.mCurrentState.OnSwitchStateComplete();

            switchParam.SwitchResult.SetResult(true);
            this.IsSwitching = false;
            // 切换完成后检查是否还有缓冲中的切换信息
            await this.TrySwitchCacheState();
            return true;
        }
        public async Task<bool> TrySwitchCacheState()
        {
            if (this.mSwitchStateList.Count > 0)
            {
                var rSwitchParam = this.mSwitchStateList[0];
                this.mSwitchStateList.RemoveAt(0);
                return await this.TrySwitchState(rSwitchParam.NextStateType, rSwitchParam);
            }
            return true;
        }
        public void Stay(TDeltaTime deltaTime, TDeltaTime unscaledDeltaTime)
        {
            this.mCurrentState?.Stay(deltaTime, unscaledDeltaTime);
        }
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        protected virtual async Task OnInitialize() { }
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        protected virtual void OnStartedSwitch(TSwitchParam switchParam) { }
        protected virtual void OnEndedSwitch(TSwitchParam switchParam) { }
        protected virtual void OnDestroy() { }
    }
}
