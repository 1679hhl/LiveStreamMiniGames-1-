using System;
using System.Collections.Generic;
using Pb;
public class ProtoBufDic 
{
    public static Dictionary<int, Type> PbDic = new Dictionary<int, Type>()
    { 
        {0,typeof(Common)},
        {11,typeof(LoginReq)},
        {12,typeof(LoginResp)},
        {14,typeof(PlayerJoinNotify)},
        {15,typeof(PlayerLeaveNotify)},
        {19,typeof(RankReq)},
        {20,typeof(RankResp)},
        {21,typeof(UpdatePositionNotify)},
        {22,typeof(UpdatePlayerStatusNotify)},
        {23,typeof(UpdateBattleFishNotify)},
        {4,typeof(TipsNotify)},
        {5,typeof(PingReq)},
        {6,typeof(PongResp)},
    }; 
    public static Dictionary<Type, int> PbTypeDic = new Dictionary<Type, int>()
    { 
        {typeof(Common),0},
        {typeof(LoginReq),11},
        {typeof(LoginResp),12},
        {typeof(PlayerJoinNotify),14},
        {typeof(PlayerLeaveNotify),15},
        {typeof(RankReq),19},
        {typeof(RankResp),20},
        {typeof(UpdatePositionNotify),21},
        {typeof(UpdatePlayerStatusNotify),22},
        {typeof(UpdateBattleFishNotify),23},
        {typeof(TipsNotify),4},
        {typeof(PingReq),5},
        {typeof(PongResp),6},
    }; 
        public const int CommonMsgID=0;
        public const int LoginReqMsgID=11;
        public const int LoginRespMsgID=12;
        public const int PlayerJoinNotifyMsgID=14;
        public const int PlayerLeaveNotifyMsgID=15;
        public const int RankReqMsgID=19;
        public const int RankRespMsgID=20;
        public const int UpdatePositionNotifyMsgID=21;
        public const int UpdatePlayerStatusNotifyMsgID=22;
        public const int UpdateBattleFishNotifyMsgID=23;
        public const int TipsNotifyMsgID=4;
        public const int PingReqMsgID=5;
        public const int PongRespMsgID=6;
} 
