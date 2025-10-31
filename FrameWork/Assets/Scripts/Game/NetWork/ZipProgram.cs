using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;


public static class ZipProgram
{
    /// <summary>
    /// 压缩
    /// </summary>
    /// <param name="plaintext"></param>
    /// <returns></returns>
    public static string Compress(string content)
    {
        var CompressDataBytes = CompressData( content );
        var zipContent = Convert.ToBase64String( CompressDataBytes );
        return zipContent;
    }

    /// <summary>
    /// 解压缩
    /// </summary>
    /// <param name="plaintext"></param>
    /// <returns></returns>
    public static string Decompress(string zipContent)
    {
        var ContentBytes = Convert.FromBase64String(zipContent);
        var DecompressDataBytes = DecompressData(ContentBytes);
        return DecompressDataBytes;
    }

    public static byte[] CompressData(string data)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(data);

        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                gzipStream.Write(buffer, 0, buffer.Length);
            }

            return memoryStream.ToArray();
        }
    }

    public static string DecompressData(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                using (StreamReader streamReader = new StreamReader(gzipStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
    
    /// <summary>
    /// 解压压缩后的数据
    /// 1.创建被压缩的数据流
    /// 2.创建zipStream对象，并传入解压的文件流
    /// 3.创建目标流
    /// 4.zipStream拷贝到目标流
    /// 5.返回目标流输出字节
    /// </summary>
    /// <param name="bytes">被压缩后的数据</param>
    /// <returns>压缩前的原始字节数据</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static byte[] Decompress(byte[] bytes)
    {
        if (bytes == null) 
            throw new ArgumentNullException("初始解压缩的字节数据流不能为null");
        using (MemoryStream compressStream = new MemoryStream(bytes))
        {
            using (GZipStream zipStream = new GZipStream(compressStream, CompressionMode.Decompress))
            {
                using (MemoryStream resultStream = new MemoryStream())
                {
                    int readLength = 1024;
                    byte[] buf = new byte[readLength];
                    int len = 0;
                    while ((len = zipStream.Read(buf, 0, readLength)) > 0)
                    {
                        resultStream.Write(buf, 0, len);
                    }
                    return resultStream.ToArray();
                }
            }
        }
    }
}