using AOT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using UnityEngine;
public class Ipc
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public string type;

    public IpcData data;
    public string version;
}
[System.Serializable]
public class IpcData
{
    public int heartbeat_interval_ms;
    public string code;
    public string message;
}
public class IPCClient : MonoBehaviour
{
    private static string IPCPath = string.Empty;
    static IntPtr ipcHandle = IntPtr.Zero;
    private Dictionary<string, string> CommandLineArgs = null;
    public static IPCClient Instance
    {
        get
        {
            return _i;
        }
    }

    /// <summary>
    /// 版本id
    /// </summary>
    int version = 1;

    static IPCClient _i;

    public string PreviewRoomCode = "";

    private void Awake()
    {
        _i = this;
    }

    /// <summary>
    /// IPC是否连上
    /// </summary>
    public bool IsConnect
    {
        get;private set;
    }

    public delegate void _OnDataReceived(string content, uint len, IntPtr ptr);

    public delegate void _OnConnected(IntPtr ptr);

    public delegate void _OnDisconnect(IntPtr ptr);

    public delegate void _OnLog(string content, uint len, IntPtr ptr);

    static Queue<string> msgQueue = new Queue<string>();

    private void Start()
    {
        string[] commandLineArgs = System.Environment.GetCommandLineArgs();
        CommandLineArgs = new Dictionary<string, string>();
        for (int i = 0; i < commandLineArgs.Length; i++)
        {
            Debug.Log("$[kuaishou]:"+commandLineArgs[i]);
            if (commandLineArgs[i] == "-c" || commandLineArgs[i] == "-pos" || commandLineArgs[i] == "-ipc")
            {
                CommandLineArgs[commandLineArgs[i]] = commandLineArgs[i + 1];
            }
        }

        if (CommandLineArgs.TryGetValue("-c", out string str))
        {
            PreviewRoomCode = str;
        }
        if (CommandLineArgs.TryGetValue("-ipc", out string str1))
        {
            IPCPath = str1;
        }
        IsConnect = false;
        if (!string.IsNullOrEmpty(IPCPath))
        {
            Debug.Log("初始化IPC：" + IPCPath);
            ipcHandle = new IntPtr(GetHashCode());
            //设置回调
            IpcInterface.SetDataReceivedCallback(OnDataReceived, ipcHandle);
            IpcInterface.SetConnectedCallback(OnConnected, ipcHandle);
            IpcInterface.SetDisconnectCallback(OnDisconnect, ipcHandle);
            IpcInterface.SetLogCallback(OnLog, ipcHandle);
            int result = IpcInterface.InitIpc(IPCPath, (uint)IPCPath.Length, IpcInterface.IPC_CS_TYPE.CLIENT);
            if (result == 0)
            {
                IsConnect = true;
                // EventMgr.FireEvent(TEventType.IPCStateChange);
            }
            else
            {
                Debug.LogError("IPC 初始化失败 result:" + result.ToString());
            }
        }
    }
    
    public void Open(string IPCPath)
    {
        IsConnect = false;

        if (IPCPath == null || IPCPath == "") return;

        Debug.Log($"IPCPath=> {IPCPath}");

        Debug.Log("初始化IPC：" + IPCPath);

        ipcHandle = new IntPtr(GetHashCode());
        Debug.Log("ipcHandle：" + ipcHandle);

        int result = IpcInterface.InitIpc(IPCPath, (uint)IPCPath.Length, IpcInterface.IPC_CS_TYPE.CLIENT);
        if (result == 0)
        {
            IsConnect = true;

            //设置收到消息的回调
            IpcInterface.SetDataReceivedCallback(OnDataReceived, ipcHandle);
            //设置连接成功的回调
            IpcInterface.SetConnectedCallback(OnConnected, ipcHandle);
            //断开连接的回调
            IpcInterface.SetDisconnectCallback(OnDisconnect, ipcHandle);
            //设置日志输入回调
            IpcInterface.SetLogCallback(OnLog, ipcHandle);
        }
        else
        {
            Debug.LogError("IPC 初始化失败 result:" + result.ToString());
        }
    }

    private void Update()
    {
        lock (msgQueue)
        {
            if (msgQueue.Count > 0)
            {
                while (msgQueue.Count > 0)
                {
                    ParseMsg(msgQueue.Dequeue());
                }
            }
        }
    }

    /// <summary>
    /// 解析并下发消息
    /// </summary>
    /// <param name="content"></param>
    void ParseMsg(string content)
    {
        Debug.Log($"ParseMsg = {content}");
        Ipc msg = JsonConvert.DeserializeObject<Ipc>(content);
        if (msg != null)
        {
            Debug.Log("type:" + msg.type);
            switch (msg.type)
            {
                case "SC_SET_CODE":
                    if (msg.data != null && msg.data.code != null)
                    {
                        Debug.Log("SC_SET_CODE  code:" + msg.data.code);
                        PreviewRoomCode = msg.data.code;
                    }

                    break;
                case "SC_QUIT":
                    if (msg.data != null && msg.data.message != null)
                    {
                        Debug.Log("SC_QUIT  msg:" + msg.data.message);
                    }

                    Application.Quit();
                    break;

                default:
                    Debug.Log("type：" + msg.type);
                    break;
            }
        }
    }

    /// <summary>
    /// 发送IPC消息
    /// </summary>
    /// <param name="msg"></param>
    public void SendMsg(string msgType, Dictionary<string, object> msgData)
    {
        if (ipcHandle != IntPtr.Zero)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "type" ,msgType },
                { "data" , msgData },
                { "version" , version }
            };
            string msg = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            int result = IpcInterface.SendData(msg, (uint)msg.Length);
            if (result == -1)
            {
                Debug.LogError("IPC消息发送失败！");
            }
        }
        else
        {
            Debug.LogError("IPC 初始化失败");
        }
    }

    /// <summary>
    /// 收到数据
    /// </summary>
    /// <param name="content"></param>
    /// <param name="len"></param>
    /// <param name="intPtr"></param>
    [MonoPInvokeCallback(typeof(_OnDataReceived))]
    static void OnDataReceived(IntPtr contentPtr, uint len, IntPtr ptr)
    {
        string content = Marshal.PtrToStringUTF8(contentPtr, (int)len);
        if (string.IsNullOrEmpty(content))
        {
            Debug.Log("ipc call func : OnDataReceived ptr:" + ptr.ToString() + "   curPtr:" + ipcHandle.ToString() + "   len" + len.ToString() + " content: is null or empty");
            return;
        }
        Debug.Log("ipc call func : OnDataReceived ptr:" + ptr.ToString() + "   curPtr:" + ipcHandle.ToString() + "   len" + len.ToString() + " content:" + content);
        if (ptr == ipcHandle)
        {
            lock (msgQueue)
            {
                msgQueue.Enqueue(content);
            }
        }
    }

    /// <summary>
    /// 连接
    /// </summary>
    /// <param name="ptr"></param>
    [MonoPInvokeCallback(typeof(_OnConnected))]
    static void OnConnected(IntPtr ptr)
    {
        Debug.Log("ipc call func : OnConnected ptr:" + ptr.ToString() + "   curPtr:" + ipcHandle.ToString());
        if (ptr == ipcHandle)
        {
            Debug.Log("ipc conneted");
            Instance.IsConnect = true;
            // EventMgr.FireEvent(TEventType.IPCStateChange);
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    /// <param name="ptr"></param>
    [MonoPInvokeCallback(typeof(_OnDisconnect))]
    static void OnDisconnect(IntPtr ptr)
    {
        Debug.Log("ipc call func : OnDisconnect ptr:" + ptr.ToString() + "   curPtr:" + ipcHandle.ToString());
        if (ptr == ipcHandle)
        {
            Instance.IsConnect = false;
            // EventMgr.FireEvent(TEventType.IPCStateChange);
            Debug.Log(message: "ipc disconneted");
            //断开连接后自动关闭游戏
            Application.Quit();
        }
    }

    /// <summary>
    /// 收到日志
    /// </summary>
    /// <param name="content"></param>
    /// <param name="len"></param>
    /// <param name="ptr"></param>
    [MonoPInvokeCallback(typeof(_OnLog))]
    static void OnLog(IntPtr contentPtr, uint len, IntPtr ptr)
    {
        string content = Marshal.PtrToStringUTF8(contentPtr, (int)len);
        Debug.Log("ipc call func : OnLog ptr:" + ptr.ToString() + "   curPtr:" + ipcHandle.ToString() + "   len" + len.ToString() + "  content:" + content);

        //if (ptr == ipcHandle)
        //{
        //    Debug.Log("ipc log:" + content);
        //}
    }

    /// <summary>
    /// 释放ipc信息
    /// </summary>
    public void ReleaseIPC()
    {
        if (ipcHandle != IntPtr.Zero)
        {
            ipcHandle = IntPtr.Zero;
            IpcInterface.ReleaseIpc();
        }
    }

    private void OnDestroy()
    {
        ReleaseIPC();
    }

}