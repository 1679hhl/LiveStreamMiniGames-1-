using Google.Protobuf;
using System;
using Knight.Core;
public static class ProtoMsgEventArgExpand
{
    public static string GetErrorMsg(this ProtoMsgEventArg rProtoMsgEventArg)
    {
        if (rProtoMsgEventArg.IsTimeOut)
        {
            return $"消息等待超时";
        }
        else
        {
            return $"error code{rProtoMsgEventArg.ErrorCode}";
        }
    }
    public static T Get<T>(this ProtoMsgEventArg rProtoMsgEventArg) where T : IMessage
    {
        T rResult = default;
        var rProtoType = ProtoBufDic.PbDic[rProtoMsgEventArg.Cmd];
        if (rProtoType == null)
        {
            rProtoType = typeof(T);
        }
        if (rProtoType != null)
        {
            //解析Proto消息
            var rMsgObj = (IMessage)Activator.CreateInstance(rProtoType);
            if (rProtoMsgEventArg.MsgData != null)
            {
                //反序列化
                rMsgObj.MergeFrom(rProtoMsgEventArg.MsgData);
            }
            rResult = (T)rMsgObj;
        }
        return rResult;
    }
}
