// using System;
// using System.IO;
// using System.Text;

using System;
using System.IO;
using System.Linq;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;
using Google.Protobuf;
using JetBrains.Annotations;

public class SerializeHelper
{
    /// <summary>
    /// Json序列化
    /// </summary>
    /// <param name="msg"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string SerializeMsg2Json<T>(T msg)
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(msg);
    }

    /// <summary>
    /// Json反序列化
    /// </summary>
    /// <param name="msg"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T DeSerializeMsgByJson<T>(string msg)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(msg);
    }

    /// <summary>
    /// Json反序列化
    /// </summary>
    /// <param name="msg"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static object DeSerializeMsgByJsonByType(string msg, Type type)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject(msg, type);
    }

    /// <summary>
    /// 序列化protobuf
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] SerializeMsg<T>(T msg) where T : IMessage
    {
        using (MemoryStream rawOutput = new MemoryStream())
        {
            msg.WriteTo(rawOutput);
            byte[] result = rawOutput.ToArray();
            return result;
        }
    }

    /// <summary>
    /// 序列化protobuf  结构:消息体长度:4byte  + msgId:2byte + encrypt:1byte + zip:1byte
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public static byte[] SerializeMsg<T>(T msg, int msgId, bool encrypt, bool zip) where T : IMessage
    {
        using (MemoryStream rawOutput = new MemoryStream())
        {
            msg.WriteTo(rawOutput);
            byte[] msgByte = rawOutput.ToArray();
            //return result;

            int len = msgByte.Length;

            var msgLenByte = BitConverter.GetBytes(len);
            var msgIdByte = BitConverter.GetBytes((short)msgId);
            var mencryptByte = BitConverter.GetBytes(encrypt);
            var zipByte = BitConverter.GetBytes(zip);

            byte[] result = new byte[msgByte.Length + MsgHeadLen];
            for (int i = 0; i < result.Length; ++i)
            {
                if (i < 4) //总长度
                {
                    result[i] = msgLenByte[i];
                }
                else if (i < 6) //消息id
                {
                    result[i] = msgIdByte[i - 4];
                }
                else if (i < 7) //是否加密
                {
                    result[i] = mencryptByte[i - 6];
                }
                else if (i < 8) //是否压缩
                {
                    result[i] = zipByte[i - 7];
                }
                else
                {
                    result[i] = msgByte[i - MsgHeadLen];
                }
            }

            return result;
        }
    }

    /// <summary>
    ///  序列化protobuf 文本
    /// </summary>
    /// <param name="msg"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string SerializeMsg2Txt<T>(T msg) where T : IMessage
    {
        using (MemoryStream rawOutput = new MemoryStream())
        {
            msg.WriteTo(rawOutput);
            return msg.ToByteString().ToBase64();
        }
    }

    /// <summary>
    /// 反序列化protobuf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <returns></returns>
    public static T DeserializeMsgByText<T>(string str) where T : IMessage, new()
    {
        var toWrite = Convert.FromBase64String(str);
        T msg = new T();
        msg = (T)msg.Descriptor.Parser.ParseFrom(toWrite);
        return msg;
    }

    public const int MsgHeadLen = 8;
    /// <summary>
    /// 通过字节流获取消息的id,是否加密,是否压缩,真正的消息体
    /// 前4个字节总长度，5-6位是消息id，7位是否加密 8位是否压缩
    /// </summary>
    /// <returns></returns>
    public static (int,bool,bool,byte[]) ParseMsgByMsgBytes(byte[] dataBytes)
    {
        var msgLenByte = new byte[4];
        var msgIdByte = new byte[2];
        var encryptByte = new byte[1];
        var zipByte = new byte[1];

        byte[] msgByte = Array.Empty<byte>();
        for (int i = 0; i < dataBytes.Length; ++i)
        {
            if (i < 4) //总长度
            {
                msgLenByte[i] = dataBytes[i];
            }
            else if (i < 6) //消息id
            {
                msgIdByte[i - 4] = dataBytes[i];
            }
            else if (i < 7) //是否加密
            {
                encryptByte[i - 6] = dataBytes[i];
            }
            else if (i < 8) //是否压缩
            {
                zipByte[i - 7] = dataBytes[i];
            }
            else
            {
                if (msgByte.Length == 0)
                {
                    msgByte = new byte[BitConverter.ToInt32(msgLenByte)];
                }
                msgByte[i - MsgHeadLen] = dataBytes[i];
            }
        }

        // var rStr = string.Empty;
        // for(int i=0;i<msgLenByte.Length;i++)
        // {
        //     rStr += $"{msgLenByte[i]} ";
        // }
        //
        // for (int i = 0; i < msgIdByte.Length; i++)
        // {
        //     rStr += $"{msgIdByte[i]} ";
        // }
        //
        // for (int i = 0; i < encryptByte.Length; i++)
        // {
        //     rStr += $"{encryptByte[i]} ";
        // }
        //
        // for (int i = 0; i < zipByte.Length; i++)
        // {
        //     rStr += $"{zipByte[i]} ";
        // }
        //
        // LogManager.LogRelease(rStr);
        //todo 处理解压缩
        bool zip = BitConverter.ToBoolean(zipByte);
        //todo 处理解密
        bool encrypt = BitConverter.ToBoolean(encryptByte);
        
        
        return (BitConverter.ToInt16(msgIdByte), BitConverter.ToBoolean(encryptByte), BitConverter.ToBoolean(zipByte),
            msgByte);
    }

    /// <summary>
    /// 反序列化protobuf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dataBytes"></param>
    /// <returns></returns>
    public static IMessage DeserializeMsg(byte[] dataBytes, Type type)
    {
        var msgType = Activator.CreateInstance(type) as IMessage;
        var msg = msgType.Descriptor.Parser.ParseFrom(dataBytes);
        return msg;
    }
}