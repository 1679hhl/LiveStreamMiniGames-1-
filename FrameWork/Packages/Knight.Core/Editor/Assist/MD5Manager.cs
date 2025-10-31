//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Knight.Core.Editor
{
    public class MD5Manager
    {
        public ConcurrentDictionary<string, string> MD5Dict = new ConcurrentDictionary<string, string>();
        public bool CalcMD5s(string[] rInputFiles)
        {
            return ThreadTool<string>.StartTaskWait((rFiles, nIndex, nCount) =>
            {
                for (int i = 0; i < nCount; i++)
                {
                    var rFile = rFiles[nIndex + i];
                    if (!this.MD5Dict.ContainsKey(rFile))
                    {
                        using (var rFs = File.OpenRead(rFile))
                        {
                            using (var rMD5 = MD5.Create())
                            {
                                var rMD5Hash = rMD5.ComputeHash(rFs);
                                this.MD5Dict.TryAdd(rFile, rMD5Hash.ToHex());
                            }
                        }
                    }
                }
            }, rInputFiles);
        }
        public string GetMD5(string rFile)
        {
            if (this.MD5Dict.TryGetValue(rFile, out var rMD5))
            {
                return rMD5;
            }
            return string.Empty;
        }
        public string GetMD5WithString(string[] rFiles, string rExString = null, string rIgnorePrefixPath = null)
        {
            var rStringBuilder = new StringBuilder();
            var bIgnorePrefixPath = string.IsNullOrEmpty(rIgnorePrefixPath);
            for (int i = 0; i < rFiles.Length; i++)
            {
                var rFile = rFiles[i];
                rStringBuilder.Append(this.GetMD5(rFile));
                // 将文件名添加到MD5计算中
                if (!bIgnorePrefixPath)
                {
                    rStringBuilder.Append(rFile.Replace(rIgnorePrefixPath, ""));
                }
            }
            if (!string.IsNullOrEmpty(rExString))
            {
                rStringBuilder.Append(rExString);
            }
            using (var rMD5 = MD5.Create())
            {
                var rBytes = Encoding.UTF8.GetBytes(rStringBuilder.ToString());
                var rMD5Hash = rMD5.ComputeHash(rBytes);
                return rMD5Hash.ToHEXString();
            }
        }
    }
}