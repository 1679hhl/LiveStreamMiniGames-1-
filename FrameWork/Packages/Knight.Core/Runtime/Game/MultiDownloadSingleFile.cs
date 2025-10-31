using Knight.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.IO;
using UnityEngine;

namespace Knight.Core
{
    public class MultiDownloadSingleFile : UnityFx.Async.IAsync
    {
        public bool isDone { get; private set; }

        /// <summary>
        /// 主线程
        /// </summary>
        private SynchronizationContext _mainThreadSynContext;

        /// <summary>
        /// 下载网址
        /// </summary>
        public string Url;

        /// <summary>
        /// 错误记录
        /// </summary>
        public string Error;
        
        public event Action<Exception> OnError;

        private static object errorlock = new object();

        public static List<MultiDownloadSingleFile> mDownloaderList = new List<MultiDownloadSingleFile>();
        
        //记录temp路径的流
        private FileStream[] tempFileFileStreams;
        public long[] DownloadTempLenthArray_Speed;
        public long[] DownloadLength;
        
        //下载速度 调一次计算一次 单位byte
        public long DownloadTempLength
        {
            get
            {
                if (this.DownloadLength == null)
                    return 0;
                long nCal = 0;
                for (int i = 0; i < this.DownloadLength.Length; i++)
                {
                    nCal += this.DownloadLength[i];
                } 
                return nCal;
            }
        }
        
        //下载速度 调一次计算一次 单位byte
        public long DownloadTempSpeed
        {
            get
            {
                if (this.DownloadTempLenthArray_Speed == null)
                    return 0;
                long nCal = 0;
                for (int i = 0; i < this.DownloadTempLenthArray_Speed.Length; i++)
                {
                    nCal += this.DownloadTempLenthArray_Speed[i];
                    this.DownloadTempLenthArray_Speed[i] = 0;
                } 
                return nCal;
            }
        }

        //Entry 
        public object Obj;
        private int mTheadCount;
        private string mFilePath;
        private Action<long, long> onDownloadingAct;
        private Action<byte[]> onTriggerAct;
        
        /// <summary>
        /// 主要用于关闭线程
        /// </summary>
        private bool _isDownload = false;

        public MultiDownloadSingleFile(string url,SynchronizationContext rMainThread = null)
        {
            // 主线程赋值
            if (rMainThread != null)
                _mainThreadSynContext = rMainThread;
            else
                this._mainThreadSynContext = SynchronizationContext.Current;
            // 突破Http协议的并发连接数限制
            System.Net.ServicePointManager.DefaultConnectionLimit = 512;
            Url = url;
            mDownloaderList.Add(this);
        }

        /// <summary>
        /// 查询文件大小
        /// </summary>
        /// <returns></returns>
        public long GetFileSize()
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.CreateHttp(new Uri(Url));
                request.Method = "HEAD";
                request.Timeout = 5000;
                response = (HttpWebResponse)request.GetResponse();
                // 获得文件长度
                long contentLength = response.ContentLength;

                response.Close();
                request.Abort();

                return contentLength;
            }
            catch (Exception ex)
            {
                lock (errorlock)
                {
                    this.onError(ex);
                }
                return -1;
            }
        }

        /// <summary>
        /// 异步查询文件大小
        /// </summary>
        /// <param name="onTrigger"></param>
        public void GetFileSizeAsyn(Action<long> onTrigger = null)
        {
            ThreadStart threadStart = new ThreadStart(() =>
            {
                var nLength = GetFileSize();
                if (onTrigger != null)
                {
                    onTrigger(nLength);
                }
            });
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        public void ReStart()
        {
            this.ResetStreamings();
            if(File.Exists(this.mFilePath))
                File.Delete(this.mFilePath);
            this.DownloadToFile(this.mTheadCount,this.mFilePath,this.onDownloadingAct,this.onTriggerAct);
        }

        /// <summary>
        /// 多线程下载文件至本地
        /// </summary>
        /// <param name="threadCount">线程总数</param>
        /// <param name="filePath">保存文件路径</param>
        /// <param name="onDownloading">下载过程回调（已下载文件大小、总文件大小）</param>
        /// <param name="onTrigger">下载完毕回调（下载文件数据）</param>
        public void DownloadToFile(int threadCount, string filePath, Action<long, long> onDownloading = null,
            Action<byte[]> onTrigger = null)
        {
            this.isDone = false;
            this._isDownload = true;
            
            //记录为成员变量  用于于restart
            this.mTheadCount = threadCount;
            this.mFilePath = filePath;
            this.onDownloadingAct = onDownloading;
            this.onTriggerAct = onTrigger;
            lock (errorlock)
            {
                this.Error = string.Empty;
            }
            
            if (this.tempFileFileStreams != null)
            {
                this.ResetStreamings();
                Core.LogManager.Log("重置文件流");
            }

            // if (File.Exists(filePath))
            // {
            //     this.isDone = true;
            //     return;
            // }

            long csize = 0; //已下载大小
            int ocnt = 0; //完成线程数

            // 下载逻辑
            GetFileSizeAsyn((size) =>
            {
                if (size == -1)
                {
                    this.isDone = true;
                    return;
                }
                // 准备工作
                var tempFilePaths = new string[threadCount];
                this.tempFileFileStreams = new FileStream[threadCount];
                this.DownloadTempLenthArray_Speed = new long[threadCount];
                this.DownloadLength = new long[threadCount];
                var dirPath = Path.GetDirectoryName(filePath);
                var fileName = Path.GetFileName(filePath);
                // 下载根目录不存在则创建
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // 查看下载临时文件是否可以继续断点续传
                var fileInfos = new DirectoryInfo(dirPath).GetFiles(fileName + "*.tmp");
                if (fileInfos.Length != threadCount)
                {
                    // 下载临时文件数量不相同，则清理
                    foreach (var info in fileInfos)
                    {
                        info.Delete();
                    }
                }

                // 创建下载临时文件，并创建文件流

                for (int i = 0; i < threadCount; i++)
                {
                    tempFilePaths[i] = filePath + i + ".tmp";
                    if (!File.Exists(tempFilePaths[i]))
                    {
                        File.Create(tempFilePaths[i]).Dispose();
                    }

                    if (this.isDone) return;
                    
                    try
                    {
                        tempFileFileStreams[i] = File.OpenWrite(tempFilePaths[i]);
                        tempFileFileStreams[i].Seek(tempFileFileStreams[i].Length, System.IO.SeekOrigin.Current);
                        this.DownloadLength[i] += this.tempFileFileStreams[i].Length;
                        csize += tempFileFileStreams[i].Length;
                    }
                    catch (Exception e)
                    {
                        lock (errorlock)
                        {
                            if (this._isDownload)
                            {
                                this.onError(new Exception(e.ToString()));
                            }
                        }

                        this.isDone = true;
                        return;
                    }

                }

                // 单线程下载过程回调函数
                Action<int, long, byte[], byte[]> t_onDownloading = (index, rsize, rbytes, data) =>
                {
                    csize += rsize;
                    if (tempFileFileStreams == null||!this._isDownload) return;
                    tempFileFileStreams[index].Write(rbytes, 0, (int)rsize);
                    this.DownloadLength[index] += rsize;
                };
                // 单线程下载完毕回调函数
                Action<int, byte[]> t_onTrigger = (index, data) =>
                {
                    // 关闭文件流
                    if (tempFileFileStreams == null||!this._isDownload) return;
                    tempFileFileStreams[index].Close();
                    ocnt++;
                    if (ocnt >= threadCount)
                    {
                        // 将临时文件转为下载文件
                        if (!File.Exists(filePath))
                        {
                            File.Create(filePath).Dispose();
                        }
                        else
                        {
                            File.WriteAllBytes(filePath, new byte[] { });
                        }

                        FileStream fs = File.OpenWrite(filePath);
                        fs.Seek(fs.Length, System.IO.SeekOrigin.Current);
                        foreach (var tempPath in tempFilePaths)
                        {
                            var tempData = File.ReadAllBytes(tempPath);
                            fs.Write(tempData, 0, tempData.Length);
                            File.Delete(tempPath);
                        }

                        fs.Close();
                        this.isDone = true;
                    }
                };
                // 分割文件尺寸，多线程下载
                long[] sizes = SplitFileSize(size, threadCount);
                for (int i = 0; i < sizes.Length; i = i + 2)
                {
                    long from = sizes[i];
                    long to = sizes[i + 1];

                    // 断点续传
                    if (tempFileFileStreams == null||!this._isDownload) return;
                    from += tempFileFileStreams[i / 2].Length;
                    if (from >= to)
                    {
                        t_onTrigger(i / 2, null);
                        continue;
                    }

                    _threadDownload(i / 2, from, to, t_onDownloading, t_onTrigger);
                }
            });
        }

        /// <summary>
        /// 多线程下载文件至内存
        /// </summary>
        /// <param name="threadCount">线程总数</param>
        /// <param name="onDownloading">下载过程回调（已下载文件大小、总文件大小）</param>
        /// <param name="onTrigger">下载完毕回调（下载文件数据）</param>
        public void DownloadToMemory(int threadCount, Action<long, long> onDownloading = null,
            Action<byte[]> onTrigger = null)
        {
            _isDownload = true;

            long csize = 0; // 已下载大小
            int ocnt = 0; // 完成线程数
            byte[] cdata; // 已下载数据
            // 下载逻辑
            GetFileSizeAsyn((size) =>
            {
                cdata = new byte[size];
                // 单线程下载过程回调函数
                Action<int, long, byte[], byte[]> t_onDownloading = (index, rsize, rbytes, data) =>
                {
                    csize += rsize;
                    PostMainThreadAction<long, long>(onDownloading, csize, size);
                };
                // 单线程下载完毕回调函数
                Action<int, byte[]> t_onTrigger = (index, data) =>
                {
                    long dIndex = (long)System.Math.Ceiling((double)(size * index / threadCount));
                    Array.Copy(data, 0, cdata, dIndex, data.Length);

                    ocnt++;
                    if (ocnt >= threadCount)
                    {
                        PostMainThreadAction<byte[]>(onTrigger, cdata);
                    }
                };
                // 分割文件尺寸，多线程下载
                long[] sizes = SplitFileSize(size, threadCount);
                for (int i = 0; i < sizes.Length; i = i + 2)
                {
                    long from = sizes[i];
                    long to = sizes[i + 1];
                    _threadDownload(i / 2, from, to, t_onDownloading, t_onTrigger);
                }
            });
        }
        
        /// <summary>
        /// 单线程下载
        /// </summary>
        /// <param name="index">线程ID</param>
        /// <param name="from">下载起始位置</param>
        /// <param name="to">下载结束位置</param>
        /// <param name="onDownloading">下载过程回调（线程ID、单次下载数据大小、单次下载数据缓存区、已下载文件数据）</param>
        /// <param name="onTrigger">下载完毕回调（线程ID、下载文件数据）</param>
        private void _threadDownload(int index, long from, long to,
            Action<int, long, byte[], byte[]> onDownloading = null, Action<int, byte[]> onTrigger = null)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                HttpWebRequest request = null;
                HttpWebResponse response = null;
                try
                {
                    request = (HttpWebRequest)HttpWebRequest.Create(new Uri(this.Url));
                    request.Timeout = 5000;
                    request.AddRange(from, to);

                    response = (HttpWebResponse)request.GetResponse();
                    Stream ns = response.GetResponseStream();

                    byte[] rbytes = new byte[8 * 1024];
                    int rSize = 0;
                    MemoryStream ms = new MemoryStream();
                    while (true)
                    {
                        if (!this._isDownload)
                            break;
                        if (ns == null) break;
                        rSize = ns.Read(rbytes, 0, rbytes.Length);
                        if (rSize <= 0) break;
                        ms.Write(rbytes, 0, rSize);
                        if (this.tempFileFileStreams == null || !this._isDownload) break;
                        this.tempFileFileStreams[index].Write(rbytes, 0, rSize);
                        if (this.DownloadTempLenthArray_Speed != null)
                            this.DownloadTempLenthArray_Speed[index] += rSize;
                        if (this.DownloadLength != null)
                            this.DownloadLength[index] += rSize;
                    }
                    ns?.Close();
                    if (ms.Length == (to - from) + 1)
                    {
                        if (onTrigger != null) onTrigger(index, ms.ToArray());
                    }
                    else
                    {
                        lock (errorlock)
                        {
                            if (this._isDownload)
                            {
                                this.onError(new Exception("文件大小校验不通过"));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lock (errorlock)
                    {
                        this.onError(ex);
                    }
                }
                finally
                {
                    response?.Dispose();
                    response?.Close();
                    request?.Abort();
                    if (this.tempFileFileStreams != null && this.tempFileFileStreams[index] != null)
                    {
                        this.tempFileFileStreams[index]?.Close();
                        this.tempFileFileStreams[index]?.Dispose();
                    }
                }
            }));
            thread.Start();
        }

        public void ResetStreamings()
        {
            this.DownloadLength = null;
            this.DownloadTempLenthArray_Speed = null;
            if(mDownloaderList.Contains(this))
                mDownloaderList.Remove(this);

            // if(this.tempFileFileStreams == null) return;
            // for (int i = 0; i < this.tempFileFileStreams.Length; i++)
            // {
            //     if (this.tempFileFileStreams[i] != null)
            //     {
            //         this.tempFileFileStreams[i]?.Close();
            //         this.tempFileFileStreams[i]?.Dispose();
            //     }
            // }
        }
        
        public void Stop()
        {
            if(!this._isDownload) return;
            this._isDownload = false;
            this.isDone = true;
            
            this.DownloadLength = null;
            this.DownloadTempLenthArray_Speed = null;
            if(mDownloaderList.Contains(this))
                mDownloaderList.Remove(this);
        }

        private void DestroyClose()
        {
            this._isDownload = false;
            this.isDone = true;
            this.DownloadTempLenthArray_Speed = null;
            this.DownloadLength = null;
            if(this.tempFileFileStreams == null) return;
            for (int i = 0; i < this.tempFileFileStreams.Length; i++)
            {
                this.tempFileFileStreams[i]?.Close();
                this.tempFileFileStreams[i]?.Dispose();
            }
        }

        /// <summary>
        /// 分割文件大小
        /// </summary>
        /// <returns></returns>
        private long[] SplitFileSize(long size, int count)
        {
            long[] result = new long[count * 2];
            for (int i = 0; i < count; i++)
            {
                long from = (long)System.Math.Ceiling((double)(size * i / count));
                long to = (long)System.Math.Ceiling((double)(size * (i + 1) / count)) - 1;
                result[i * 2] = from;
                result[i * 2 + 1] = to;
            }

            return result;
        }

        private void onError(Exception ex)
        {
            LogManager.LogError($"Error:{this.Url}"+ex.ToString());
            this.Error = ex.ToString();
            this.isDone = true;
        }

        /// <summary>
        /// 通知主线程回调
        /// </summary>
        private void PostMainThreadAction(Action action)
        {
            _mainThreadSynContext.Post(new SendOrPostCallback((o) =>
            {
                Action e = (Action)o.GetType().GetProperty("action").GetValue(o);
                if (e != null) e();
            }), new {action = action});
        }

        private void PostMainThreadAction<T>(Action<T> action, T arg1)
        {
            _mainThreadSynContext.Post(new SendOrPostCallback((o) =>
            {
                Action<T> e = (Action<T>)o.GetType().GetProperty("action").GetValue(o);
                T t1 = (T)o.GetType().GetProperty("arg1").GetValue(o);
                if (e != null) e(t1);
            }), new {action = action, arg1 = arg1});
        }

        public void PostMainThreadAction<T1, T2>(Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            _mainThreadSynContext.Post(new SendOrPostCallback((o) =>
            {
                Action<T1, T2> e = (Action<T1, T2>)o.GetType().GetProperty("action").GetValue(o);
                T1 t1 = (T1)o.GetType().GetProperty("arg1").GetValue(o);
                T2 t2 = (T2)o.GetType().GetProperty("arg2").GetValue(o);
                if (e != null) e(t1, t2);
            }), new {action = action, arg1 = arg1, arg2 = arg2});
        }

        public void PostMainThreadAction<T1, T2, T3>(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            _mainThreadSynContext.Post(new SendOrPostCallback((o) =>
            {
                Action<T1, T2, T3> e = (Action<T1, T2, T3>)o.GetType().GetProperty("action").GetValue(o);
                T1 t1 = (T1)o.GetType().GetProperty("arg1").GetValue(o);
                T2 t2 = (T2)o.GetType().GetProperty("arg2").GetValue(o);
                T3 t3 = (T3)o.GetType().GetProperty("arg3").GetValue(o);
                if (e != null) e(t1, t2, t3);
            }), new {action = action, arg1 = arg1, arg2 = arg2, arg3 = arg3});
        }

        public void PostMainThreadAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3,
            T4 arg4)
        {
            _mainThreadSynContext.Post(new SendOrPostCallback((o) =>
            {
                Action<T1, T2, T3, T4> e = (Action<T1, T2, T3, T4>)o.GetType().GetProperty("action").GetValue(o);
                T1 t1 = (T1)o.GetType().GetProperty("arg1").GetValue(o);
                T2 t2 = (T2)o.GetType().GetProperty("arg2").GetValue(o);
                T3 t3 = (T3)o.GetType().GetProperty("arg3").GetValue(o);
                T4 t4 = (T4)o.GetType().GetProperty("arg4").GetValue(o);
                if (e != null) e(t1, t2, t3, t4);
            }), new
            {
                action = action,
                arg1 = arg1,
                arg2 = arg2,
                arg3 = arg3,
                arg4 = arg4
            });
        }

        public static void OnDestroy()
        {
            if (mDownloaderList == null) return;
            foreach (var rItem in mDownloaderList)
            {
                if (rItem != null)
                {
                    rItem.DestroyClose();
                }
            }
            mDownloaderList.Clear();
        }

    }
}