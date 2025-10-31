using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BestHTTP.WebSocket;
using Game;
using Google.Protobuf;
using Knight.Core;
using Pb;
using UnityEngine;
using UnityEngine.UI;
using UnityFx.Async;

public class NetCommonMsgInfo 
{
    public int msgId;
    public IMessage data;

    public NetCommonMsgInfo()
    {
    }
}

public class NetTransCommonInfo
{
    public int msgId;
    public string data;
}

class ExceptionMsgBase
{
    public IMessage Msg;
    public int MsgID;
}
public class WebSocketManager
{
    #region 单例
    private static WebSocketManager        __instance;
    private static object   __lock = new object();
    private WebSocketManager() { }
    public static WebSocketManager Instance
    {
        get
        {
            if (__instance == null)
            {
                lock (__lock)
                {
                    if (__instance == null)
                    {
                        __instance = new WebSocketManager();
                    }
                }
            }
            return __instance;
        }
    }
    #endregion

    public bool Logined = false;
    private WebSocket mWebSocket;
    /// <summary>
    /// 异常连接期间所有的消息队列
    /// </summary>
    private static Queue<ExceptionMsgBase> exceptionConnectWaitMsgs = new Queue<ExceptionMsgBase>();
    private string mWebSocketIPAddress;
    public bool mConnected;
    public Action<bool,string> OnConnectedCallBack;

    //异步消息处理
    private CancellationTokenSource mOtherThreadTaskCancelTokenSource;// 其他线程取消对象
    private Task mOtherThreadTask;// 其他线程任务对象
    private ushort mAwaitProtoMsgIndex = 0;
    
    //重连次数
    private int mReconnectCount = 0;
    private bool mReconnecting = false;

    private ConcurrentDictionary<ushort, AwaitProtoMsgInfo> mAwaitProtoMsgInfoDict =
        new ConcurrentDictionary<ushort, AwaitProtoMsgInfo>();// 等待消息字典

    public void Initialize()
    {

    }

    public void Connect(string rWebSocketIPAddress, Action<bool,string> rConnectCallBack,bool bCloseWebSocket = false)
    {
        LogManager.LogRelease("开始连接逻辑服");
        this.mWebSocketIPAddress = rWebSocketIPAddress;
        if (bCloseWebSocket)
        {
            this.mOtherThreadTaskCancelTokenSource?.Cancel();
            this.mOtherThreadTask = null;
            if (this.mWebSocket is { IsOpen: true })
            {
                try
                {
                    this.mWebSocket.Close();
                    this.mWebSocket = null;
                }
                catch (Exception e)
                {
                    Debug.LogWarning("关闭WebSocket连接异常:" + e);
                }
            }
        }
        else
        {
            if (this.mWebSocket is { IsOpen: true })
            {
                LogManager.LogRelease($"WebSocketManager.Connect:WB连接已打开");
                return;
            }
        }

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        try
        {
            //建立连接
            this.mWebSocket = new WebSocket(new Uri(rWebSocketIPAddress));
            // 建立委托
            this.OnConnectedCallBack = rConnectCallBack;
            this.mWebSocket.OnOpen += OnOpen;
            this.mWebSocket.OnError += OnError;
            this.mWebSocket.OnMessage += OnMessage;
            this.mWebSocket.OnBinary += OnBinary;
            this.mWebSocket.OnClosed += OnClose;
            this.mWebSocket.Open();
        }
        catch (Exception e)
        {
            LogManager.LogError(e);
            this.mReconnecting = false;
            throw;
        }

    }
    
    private void OnOpen(WebSocket webSocket)
    {
        LogManager.LogRelease("WebSocket is now Open!");
        this.mConnected = true;
        this.OnConnectedCallBack?.Invoke(true,string.Empty);
        this.OnConnectedCallBack = null;
    }

    public void StartHeartBeat()
    {
        // 启动一个线程来分发处理非Unity线程的包
        this.mOtherThreadTaskCancelTokenSource = new CancellationTokenSource();
        this.mOtherThreadTask = Task.Factory.StartNew(this.HeartBeatUpdate, this.mOtherThreadTaskCancelTokenSource.Token);
        this.OnConnectedCallBack = null;
    }

    private void HeartBeatUpdate()
    {
        while (this.mConnected)
        {
            PingReq rPingMsg = new PingReq(); 
            WebSocketManager.Instance.Send<PingReq>(rPingMsg);
            Thread.Sleep(2000);
        }
    }



    /// <summary>
    /// 断开连接 
    /// </summary>
    /// <param name="webSocket"></param>
    /// <param name="error"></param>
    private void OnError(WebSocket webSocket, string error)
    {
        this.mConnected = false;
        //处理连接回调
        this.OnConnectedCallBack?.Invoke(false,error);
        this.OnConnectedCallBack = null;
        if(this.mWebSocket is { State: WebSocketStates.Open })
            this.EndClient(true);
        LogManager.LogError($" web state:{webSocket.State} WebSocket is error : {error}");
    }
    /// <summary>
    /// 与服务器通信不用string
    /// </summary>
    /// <param name="webSocket"></param>
    /// <param name="message"></param>
    private void OnMessage(WebSocket webSocket, string message) { }
    
    private void OnBinary(WebSocket socket,byte[] msg)
    {
        var (msgId,encry,zip,msgBytes) = SerializeHelper.ParseMsgByMsgBytes(msg);
        if (zip)
        {
            LogManager.LogRelease("解压缩！");
            msgBytes = ZipProgram.Decompress(msgBytes);
        }

        try
        {
            var data = SerializeHelper.DeserializeMsg(msgBytes, ProtoBufDic.PbDic[msgId]);
            LogManager.Log($"接收到服务器原始消息: {msgId} {data}");
            var rProtoMsgEventArg = TSPoolObject<ProtoMsgEventArg>.Instance.Alloc();
            rProtoMsgEventArg.MsgData = msgBytes;
            rProtoMsgEventArg.Cmd = msgId;
            ProtoMsgEventManager.Instance.Distribute(rProtoMsgEventArg.Cmd, rProtoMsgEventArg);
        }
        catch (Exception e)
        {
            LogManager.LogError($"[{msgId}:解析错误！{e}]");
        }
    }
    
    private void OnClose(WebSocket webSocket, UInt16 code, string message)
    {
        this.mConnected = false;
        if (code != 1000) //非正常断开
        {
            LogManager.LogError($"非正常断开 : code {code}");
            LogManager.LogError($"网络服务异常非正常断开：code={code} message:{message}");
            if(!this.mReconnecting)
                this.EndClient(true);
        }
        else
        {
            LogManager.LogError($"客户端主动断开网络服务：code={code} message:{message}");
            if(!this.mReconnecting)
                this.EndClient(false);
        }
    }

    public void EndClient(bool tryReconnect)
    {
        Debug.LogWarning("关闭链接！");
        // 关闭线程
        this.mOtherThreadTaskCancelTokenSource?.Cancel();
        this.mOtherThreadTask = null;
        if (this.mWebSocket != null)
        {
            try
            {
                this.mWebSocket.Close();
                this.mWebSocket = null;
            }
            catch (Exception e)
            {
                Debug.LogWarning("关闭WebSocket连接异常:" + e);
            }
        }
        if (tryReconnect && !this.mReconnecting && this.Logined)
        {
            TryReconnect();
        }
    }

    /// <summary>尝试重新连接。
    /// 先释放TCP和UDP连接再重新进行连接</summary>
    private async Task TryReconnect()
    {
        this.mReconnecting = true;
        await WaitAsync.WaitForSeconds(1f);
        if (this.mAwaitProtoMsgIndex > 10)
        {
            GlobalMessageBox.Instance.Open("确定","重新连接服务器失败，请尝试重启玩法！",UtilTool.ExitApplication);
            return;
        }
        this.Connect(this.mWebSocketIPAddress, (bConnectedSuc,rErrMsg) =>
        {
            if (bConnectedSuc)
            {
                this.mAwaitProtoMsgIndex = 0;
                LogManager.LogRelease("重连成功~开始ReLogin！");
                Toast.Instance.Show("重连服务器成功！");
                this.mReconnecting = false;
                EventManager.Instance.Distribute(GameEvent.WebSocketReconnectSuc);
            }
            else
            {
                this.mAwaitProtoMsgIndex++;
                this.TryReconnect();
            }
        });
    }
    
    public void Send<T>(IMessage rMsg)
    {
        var nMsgID = ProtoBufDic.PbTypeDic[typeof(T)];
        if (this.mWebSocket == null)
        {
            LogManager.LogError("尚未建立连接！");
            return;
        }
        try
        {
            var rBytes = SerializeHelper.SerializeMsg(rMsg, (short)nMsgID, false, false);
            byte[] msgIdBytes = new[] { rBytes[4], rBytes[5] };
            LogManager.Log($"向服务器发  msgId:{BitConverter.ToInt16(msgIdBytes)} ReqType:{typeof(T)}");
            this.mWebSocket.Send(rBytes);
            return;
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            //压入等候队列
            exceptionConnectWaitMsgs.Enqueue(new ExceptionMsgBase()
            {
                Msg = rMsg,
                MsgID = nMsgID
            });
        }
    }
    
    public void Send(IMessage rMsg,int nMsgCode,int nIndex = 0)
    {
        var rType = ProtoBufDic.PbDic[nMsgCode];
        if (this.mWebSocket == null)
        {
            LogManager.LogError("尚未建立连接！");
            return;
        }
        try
        {
            var rBytes = SerializeHelper.SerializeMsg(rMsg, nMsgCode, false, false);
            byte[] msgIdBytes = new[] { rBytes[4], rBytes[5] };
            if(BitConverter.ToInt16(msgIdBytes)!=5)
                LogManager.LogRelease($"向服务器发  msgId:{BitConverter.ToInt16(msgIdBytes)} ReqType:{nMsgCode} ReqType:{rType}");
            this.mWebSocket.Send(rBytes);
            return;
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            //压入等候队列
            exceptionConnectWaitMsgs.Enqueue(new ExceptionMsgBase()
            {
                Msg = rMsg,
                MsgID = nMsgCode
            });
        }
    }
    
    private AwaitProtoMsgInfo CreateAwaitProtoMsgInfo(int nMsgCode, ushort nIndex)
    {
        var rAwaitProtoMsgInfo = TSPoolObject<AwaitProtoMsgInfo>.Instance.Alloc();
        rAwaitProtoMsgInfo.mMsgCode = nMsgCode;
        rAwaitProtoMsgInfo.mIndex = nIndex;
        if (this.mAwaitProtoMsgInfoDict.TryAdd(nIndex, rAwaitProtoMsgInfo))
        {
            return rAwaitProtoMsgInfo;
        }
        else
        {
            TSPoolObject<AwaitProtoMsgInfo>.Instance.Free(rAwaitProtoMsgInfo);
            return null;
        }
    }
    
    private AwaitProtoMsgInfo RegisterAwaitProtoMsgInfo(int nMsgCode, int nTimeOutTime)
    {
        // 检测消息是否可以等待
        if (nMsgCode == ProtoBufDic.PingReqMsgID)
        {
            LogManager.LogError($"该消息不可使用等待方式发送，nMsgCode:{nMsgCode}");
            return null;
        }
        // 异步操作事件
        var rTCS = new TaskCompletionSource<ProtoMsgEventArg>();

        var nWhileCount = 0;
        AwaitProtoMsgInfo rAwaitProtoMsgInfo = null;
        while (rAwaitProtoMsgInfo == null && nWhileCount <= 16)
        {
            rAwaitProtoMsgInfo = this.CreateAwaitProtoMsgInfo(nMsgCode, this.GetNextAwaitProtoMsgIndex());
            nWhileCount++;
        }

        if (rAwaitProtoMsgInfo == null)
        {
            LogManager.LogError($"创建 AwaitProtoMsgInfo 对象失败，已尝试{nWhileCount}次");
            return null;
        }

        rAwaitProtoMsgInfo.RequestTime = TimeAssist.ClientNowTicks();
        rAwaitProtoMsgInfo.RequestTimeOutTime = rAwaitProtoMsgInfo.RequestTime + nTimeOutTime;
        rAwaitProtoMsgInfo.TCS = rTCS;

        return rAwaitProtoMsgInfo;
    }
    
    private ushort GetNextAwaitProtoMsgIndex()
    {
        this.mAwaitProtoMsgIndex++;
        //如果索引为 0, 则将索引修改为 1, 0 作为非等待消息的保留值
        if (this.mAwaitProtoMsgIndex == 0)
        {
            this.mAwaitProtoMsgIndex = 1;
        }
        return this.mAwaitProtoMsgIndex;
    }
    
    private void PongRec(ProtoMsgEventArg rPbMsg)
    {
        if (rPbMsg.IsError || rPbMsg.IsTimeOut)
            return;
        var result = rPbMsg.Get<PongResp>();
        LogManager.LogRelease(result);
    }
}

