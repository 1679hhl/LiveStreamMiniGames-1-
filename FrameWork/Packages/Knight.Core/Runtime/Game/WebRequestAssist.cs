//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityFx.Async;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Knight.Core
{
    public class WebRequestAssist
    {
        public class LoaderRequest : AsyncRequest<LoaderRequest>
        {
            public string Text;
            public byte[] Bytes;
            public bool IsDone;

            public string Url;
            public Dictionary<string, string> FormFields;

            public LoaderRequest(string rURL)
            {
                this.IsDone = false;
                this.Url = rURL;
            }
        }

        public static IAsyncOperation<LoaderRequest> DownloadFile(string rURL, System.Action<float> rDownloadProgress = null)
        {
            var rRequest = new LoaderRequest(rURL);
            return rRequest.Start(DownloadFile(rRequest, rDownloadProgress));
        }
        public static IAsyncOperation<LoaderRequest> DownloadFile(string rURL, System.Action<float> rDownloadProgress = null, params string[] rParams)
        {
            var rURLStringBuilder = new StringBuilder(rURL);
            if (rParams != null && rParams.Length > 1)
            {
                rURLStringBuilder.Append("?");
                var rParamCount = rParams.Length / 2;
                for (int i = 0; i < rParamCount; i++)
                {
                    rURLStringBuilder.Append(rParams[i * 2]);
                    rURLStringBuilder.Append('=');
                    rURLStringBuilder.Append(rParams[i * 2 + 1]);
                    if (i != rParamCount - 1)
                    {
                        rURLStringBuilder.Append('&');
                    }
                }
            }
            return DownloadFile(rURLStringBuilder.ToString(), rDownloadProgress);
        }
        public static IAsyncOperation<LoaderRequest> DownloadFile_Post(string rURL, System.Action<float> rDownloadProgress = null, params string[] rParams)
        {
            var rRequest = new LoaderRequest(rURL);
            rRequest.FormFields = new Dictionary<string, string>();
            if (rParams != null && rParams.Length > 1)
            {
                var rParamCount = rParams.Length / 2;
                for (int i = 0; i < rParamCount; i++)
                {
                    var rKey = rParams[i * 2];
                    var rValue = rParams[i * 2 + 1];
                    LogManager.Assert(!string.IsNullOrEmpty(rKey), "Post参数key不可为空或空字符串");
                    LogManager.Assert(rValue != null, "Post参数value不可为空");
                    rRequest.FormFields.Add(rKey, rValue);
                }
            }
            return rRequest.Start(DownloadFile(rRequest, rDownloadProgress));
        }

        public static IAsyncOperation<LoaderRequest> DownloadFile_Post_Timeout(string rURL, int nTimeOut, System.Action<float> rDownloadProgress = null, params string[] rParams)
        {
            var rRequest = new LoaderRequest(rURL);
            rRequest.FormFields = new Dictionary<string, string>();
            if (rParams != null && rParams.Length > 1)
            {
                var rParamCount = rParams.Length / 2;
                for (int i = 0; i < rParamCount; i++)
                {
                    var rKey = rParams[i * 2];
                    var rValue = rParams[i * 2 + 1];
                    LogManager.Assert(!string.IsNullOrEmpty(rKey), "Post参数key不可为空或空字符串");
                    LogManager.Assert(rValue != null, "Post参数value不可为空");
                    rRequest.FormFields.Add(rKey, rValue);
                }
            }
            return rRequest.Start(DownloadFile(rRequest, rDownloadProgress, nTimeOut));
        }

        public static IAsyncOperation<LoaderRequest> UpLoadFile(string rFilePath, string rFileName, string rURL, string rMimeType)
        {
            var rRequest = new LoaderRequest(rURL);
            return rRequest.Start(UploadFile(rFilePath, rFileName, rRequest, rMimeType));
        }

        private static IEnumerator UploadFile(string rFilePath, string rFileName, LoaderRequest rRequest, string rMimeType)
        {
            byte[] rFileByte = File.ReadAllBytes(rFilePath);
            WWWForm rForm = new WWWForm();
            rForm.AddBinaryData("files", rFileByte, rFileName, rMimeType);

            UnityWebRequest rWebRequest = UnityWebRequest.Post(rRequest.Url, rForm);
            yield return rWebRequest.SendWebRequest();

            if (rWebRequest.isNetworkError || rWebRequest.isHttpError || !string.IsNullOrEmpty(rWebRequest.error))
            {
                Knight.Core.LogManager.LogRelease(rWebRequest.error + ",url:" + rRequest.Url);
            }
            else
            {
                Knight.Core.LogManager.LogRelease("file upload complete!!!");
            }
        }

        private static IEnumerator DownloadFile(LoaderRequest rRequest, System.Action<float> rDownloadProgress, int nTimeout = 5)
        {
            UnityWebRequest rWebRequest = null;
            if (rRequest.FormFields == null)
                rWebRequest = UnityWebRequest.Get(rRequest.Url);
            else
                rWebRequest = UnityWebRequest.Post(rRequest.Url, rRequest.FormFields);
            rWebRequest.timeout = nTimeout;
            if (rDownloadProgress == null)
            {
                yield return rWebRequest.SendWebRequest();
            }
            else
            {
                rWebRequest.SendWebRequest();
                while (!rWebRequest.isDone && !rWebRequest.isNetworkError && string.IsNullOrEmpty(rWebRequest.error))
                {
                    yield return new WaitForSeconds(0.033f);
                    rDownloadProgress(rWebRequest.downloadProgress);
                }
            }

            if (rWebRequest.isNetworkError || !string.IsNullOrEmpty(rWebRequest.error))
            {
                Knight.Core.LogManager.LogRelease(rWebRequest.error + ",url:" + rRequest.Url);
                rWebRequest.Dispose();
                rRequest.SetResult(rRequest);
                yield break;
            }

            var rDownloadHandler = rWebRequest.downloadHandler;
            if (rDownloadHandler == null)
            {
                rRequest.SetResult(rRequest);
                yield break;
            }

            rRequest.IsDone = true;
            rRequest.Text = rDownloadHandler.text;
            rRequest.Bytes = rDownloadHandler.data;

            rRequest.SetResult(rRequest);

            rWebRequest.Dispose();
            rDownloadHandler.Dispose();
            rWebRequest = null;
            rDownloadHandler = null;
        }
    }
}

