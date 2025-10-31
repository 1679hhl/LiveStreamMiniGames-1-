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
    public class AccountModel:ModelBase
    {
        public LoginResp AnchorLoginResp;
        public AnchorDBInfo AnchorInfo;

        public string CurrRankName;
        public int CurrentFishId;
        
        
        public override Task OnInitialize()
        {
            EventManager.Instance.Binding(GameEvent.WebSocketReconnectSuc,this.ReLogin);
            return base.OnInitialize();
        }

        public override void OnDestroy()
        {
            EventManager.Instance.Unbinding(GameEvent.WebSocketReconnectSuc,this.ReLogin);
            base.OnDestroy();
        }
        

        /*[ProtoMsgEvent(ProtoBufDic.PongRespMsgID)]
        public void OnAnchorDBMapInfoUpdate(ProtoMsgEventArg rEventMsg)
        {
            var rPong = rEventMsg.Get<PongResp>();
            LogManager.LogRelease(rPong.ServerTime);
            EventManager.Instance.Distribute(GameEvent_Hotfix.kEventTest,rPong.ServerTime.ToString());
        }*/
        public override void OnLogin(EventArg rEventArg)
        {
            base.OnLogin(rEventArg);
        }

        public override void OnLogout(EventArg rEventArg)
        {
        }

        private void ReLogin(EventArg rEventArg)
        {
            if(this.AnchorInfo==null)
                return;
            if (GameInit.EGamePlatform == PlatformType.None)
            {
                LoginReq curMsg = new LoginReq();
                curMsg.GameId = AnchorInfo.GameId;
                curMsg.PlatformId = this.AnchorInfo.PlatformId;
                curMsg.ModeId = this.AnchorInfo.ModeId;
                curMsg.AccountId = this.AnchorInfo.AccountId;
                curMsg.Version = Application.version;
                curMsg.RoomId = string.Empty;
                WebSocketManager.Instance.Send<LoginReq>(curMsg);
            }
            else if (GameInit.EGamePlatform == PlatformType.KuaiShou)
            {

            }
            else if (GameInit.EGamePlatform == PlatformType.DouYin)
            {
                LoginReq curMsg = new LoginReq();
                curMsg.GameId = AnchorInfo.GameId;
                curMsg.PlatformId = this.AnchorInfo.PlatformId;
                curMsg.ModeId = this.AnchorInfo.ModeId;
                curMsg.AccountId = this.AnchorInfo.AccountId;
                curMsg.Version = Application.version;
                curMsg.RoomId = AnchorLoginResp.RoomId;
                WebSocketManager.Instance.Send<LoginReq>(curMsg);
            }
            else if (GameInit.EGamePlatform == PlatformType.Tiktok_Ye)
            {

            }
            else if(GameInit.EGamePlatform == PlatformType.YeYou)
            {

            }
        }

        [ProtoMsgEvent(ProtoBufDic.LoginRespMsgID)]
        public void LoginHandler(ProtoMsgEventArg rArg)
        {
            var rRes = rArg.Get<LoginResp>();
            if (!string.IsNullOrEmpty(rRes.ErrMsg))
            {
                LogManager.LogError(rRes.ErrMsg);
                // Toast.Instance.Show(rRes.ErrMsg);
                return;
            }

            this.mLastRecvPongTime = rRes.ServerTime;
            this.AnchorLoginResp = rRes;
            WebSocketManager.Instance.Logined = true;
            LogManager.LogRelease($"玩家{rRes.AnchorDB.NickName}登录成功！");
            TimeAssist.CalcClientServerOffset(rRes.ServerTime);
            //获取玩家的登录信息
            LogListener.Instance.AccountID = rRes.AnchorDB.AccountId + "|" + rRes.AnchorDB.NickName;
            this.AnchorInfo = rRes.AnchorDB;
            this.AnchorLoginResp = rRes;
            
            //所有玩家的位置
            //Model.Instance.AudienceModel.LoginPlayerAdd(rRes.SeatList);
            
            //登录成功开始心跳
            WebSocketManager.Instance.StartHeartBeat();
            EventManager.Instance.Distribute(GameEvent_Hotfix.kEventLogin_Login);
            HotfixUpdateManager.Instance.RemoveTickUpdate(this.PongLimitRefresh);
            HotfixUpdateManager.Instance.AddTickUpdate(1f,this.PongLimitRefresh);
            this.AnchorInfo = rRes.AnchorDB;
            LogListener.Instance.AccountID = rRes.AnchorDB.AccountId + "|" + rRes.AnchorDB.NickName;
            this.ReloadBattle().WrapErrors();
        }

        [ProtoMsgEvent(ProtoBufDic.PongRespMsgID)]
        public void PongHandler(ProtoMsgEventArg rArg)
        {
            var rRes = rArg.Get<PongResp>();
            if (!string.IsNullOrEmpty(rRes.ErrMsg))
            {
                LogManager.LogError(rRes.ErrMsg);
                return;
            }
            this.mLastRecvPongTime = rRes.ServerTime;
            TimeAssist.CalcClientServerOffset(rRes.ServerTime);
            /*TimeAssist.ServerNowSeconds();*/
        }
        
        [ProtoMsgEvent(ProtoBufDic.TipsNotifyMsgID)]//公告
        public void OpenAffiche(ProtoMsgEventArg rEventArg)
        {
            var Data = rEventArg.Get<TipsNotify>();
            if (Data.Type == TipType.Normal)
            {
                Toast.Instance.Show(Data.TipMsg);
            }

            if (Data.Type==TipType.RoomRoll)
            {
               
            }
        }

        public async Task ReloadBattle()
        {
            if (GameStateMachine.Instance.GetCurrentState()==EGameState.Battle)
            {
                GameStateMachine.Instance.TrySwitchState(EGameState.Battle,new GameStateMachine.SwitchParam()
                {
                    ShowLoading = false,
                    EnterParam = new GameState_Battle.EnterParamInfo()
                    {
                        BattleInitData = new BattleInitData()
                        {
                            MapId = Model.Instance.Map.CurrentMapId,
                            LevelId = Model.Instance.Map.CurrentLevelId,
                        }
                    }
                }).WrapErrors();
            }
        }
        
        
        private long mLastRecvPongTime = 0;
        public void PongLimitRefresh()
        {
            if (TimeAssist.ServerNowSeconds() - mLastRecvPongTime > 10F)
            {
                //WebSocketManager.Instance.EndClient(true);
                HotfixUpdateManager.Instance.RemoveTickUpdate(this.PongLimitRefresh);
            }
        }
    }
}