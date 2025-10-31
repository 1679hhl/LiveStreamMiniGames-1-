using System;
using System.Collections.Generic;
namespace Knight.Core
{
    [AttributeUsage(AttributeTargets.Method)]
public class ProtoMsgEventAttribute : Attribute
{
    public int MsgCmd;

    public ProtoMsgEventAttribute(int nMsgCmd)
    {
        this.MsgCmd = nMsgCmd;
    }

    public ProtoMsgEventAttribute(object rMsgCmd)
    {
        this.MsgCmd = Convert.ToInt32(rMsgCmd);
    }
}

public class ProtoMsgEventArg : IPoolObject
{
    public byte[] MsgData;
    public int Cmd;
    public byte Flags;
    public int ErrorCode;
    public string NetworkName;
    public int NetworkType;
    public bool IsTimeOut;
    public bool Use { get; set; }
    public bool IsError => this.ErrorCode != 0 || this.IsTimeOut;

    public ProtoMsgEventArg()
    {
    }

    public void Clear()
    {
        this.MsgData = null;
        this.Cmd = 0;
        this.ErrorCode = 0;
        this.NetworkType = 0;
        this.IsTimeOut = false;
    }
}

public class ProtoMsgEventManager : TSingleton<ProtoMsgEventManager>
{
    public class MsgEvent
    {
        public int MsgCode;
        public List<Action<ProtoMsgEventArg>> Callbacks;
    }

    public Dictionary<int, MsgEvent> mMsgEvents;
    private object _syncObject = new object();
    private List<Action<ProtoMsgEventArg>> _tempEvents = new List<Action<ProtoMsgEventArg>>();
    private Dictionary<Type, int> mPbDicT2CMD = null;
    private Dictionary<int,Type> mPbDicCMD2T = null;
    private ProtoMsgEventManager() { }

    public void Initialize(Dictionary<Type,int> rMsgDicT2CMD,Dictionary<int,Type> rMsgDicCMD2T)
    {
        lock (this._syncObject)
        {
            this.mMsgEvents = new Dictionary<int, MsgEvent>();
        }
        this.mPbDicT2CMD = new Dictionary<Type,int>(rMsgDicT2CMD);
        this.mPbDicCMD2T = new Dictionary<int, Type>(rMsgDicCMD2T);
    }

    public void Binding(int nCMD,Action<ProtoMsgEventArg> rMsgEventCallback)
    {
        if (this.mPbDicCMD2T.TryGetValue(nCMD, out var T))
        {
            lock (this._syncObject)
            {
                if (this.mMsgEvents.TryGetValue(nCMD, out var rEvent))
                {
                    if (rEvent.Callbacks == null)
                    {
                        rEvent.Callbacks = new List<Action<ProtoMsgEventArg>>();
                    }
                    else
                    {
                        if (!rEvent.Callbacks.Contains(rMsgEventCallback))
                        {
                            rEvent.Callbacks.Add(rMsgEventCallback);
                        }
                    }
                }
                else
                {
                    rEvent = new MsgEvent()
                        { MsgCode = nCMD, Callbacks = new List<Action<ProtoMsgEventArg>>() { rMsgEventCallback } };
                    this.mMsgEvents.Add(nCMD, rEvent);
                }
            }
        }
        else
        {
            LogManager.LogError($"ProtoBufDic.PbTypeDic 不存在类型cmd:{nCMD}");
        }
    }

    // public void Binding<T>(Action<ProtoMsgEventArg> rMsgEventCallback)
    // {
    //     this.Binding<T>(rMsgEventCallback);
    // }

    public void Unbinding(int nCMD, Action<ProtoMsgEventArg> rEventCallback)
    {
        if (this.mPbDicCMD2T.TryGetValue(nCMD, out var T))
        {
            lock (this._syncObject)
            {
                if (this.mMsgEvents.TryGetValue(nCMD, out var rEvent))
                {
                    if (rEvent.Callbacks != null)
                        rEvent.Callbacks.Remove(rEventCallback);
                }
            }
        }
        else
        {
            LogManager.LogError($"ProtoBufDic.PbTypeDic 不存在类型cmd:{nCMD}");
        }
    }

    // public void Unbinding(Enum rCmd, Action<ProtoMsgEventArg> rMsgEventCallback)
    // {
    //     this.Unbinding(Convert.ToInt32(rCmd), rMsgEventCallback);
    // }

    public void Distribute(int nCmd, ProtoMsgEventArg rEventArg)
    {
        var nMsgCode = nCmd;
        lock (this._syncObject)
        {
            if (this.mMsgEvents.TryGetValue(nMsgCode, out var rEvent))
            {
                if (rEvent.Callbacks != null)
                {
                    var rCallbacks = new List<Action<ProtoMsgEventArg>>(rEvent.Callbacks);
                    for (int i = 0; i < rCallbacks.Count; i++)
                    {
                        try
                        {
                            UtilTool.SafeExecute(rCallbacks[i], rEventArg);
                        }
                        catch (Exception rException)
                        {
                            LogManager.LogException(rException);
                        }
                    }
                }
            }
        }
    }
}
}