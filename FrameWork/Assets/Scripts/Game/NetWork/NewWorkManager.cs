using System.Threading.Tasks;
using Knight.Core;

/// <summary>
/// 等待消息信息
/// </summary>
public class AwaitProtoMsgInfo : IPoolObject
{
    public int mMsgCode;
    public long RequestTime;
    public ushort mIndex;
    public long RequestTimeOutTime;
    public TaskCompletionSource<ProtoMsgEventArg> TCS;

    public bool Use { get; set; }
    public void Clear()
    {
        this.mMsgCode = 0;
        this.RequestTime = 0;
        this.RequestTimeOutTime = 0;
        this.TCS = null;
    }
}

public class NewWorkManager
{
        
}
