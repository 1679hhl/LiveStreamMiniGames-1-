//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Knight.Core.Editor
{
    public class AssetGuidModifyer
    {
        public class AssetInfo
        {
            public string AssetPath;
            public string OldGuid;
            public string NewGuid;
        }
        public List<AssetInfo> AssetInfoList = new List<AssetInfo>();
        public HashSet<string> AssetPathHashSet = new HashSet<string>();
        public Dictionary<string, string> AssetNewGuidDictionary = new Dictionary<string, string>();
        public byte[] GuidBytes = new byte[16];
        public bool AddAsset(string rAssetPath)
        {
            if (string.IsNullOrEmpty(rAssetPath))
            {
                LogManager.LogError($"AssetGuidModifyer添加资源失败，资源路径不可为空 AssetPath:{rAssetPath}");
                return false;
            }

            if (this.AssetPathHashSet.Add(rAssetPath))
            {
                var rAssetInfo = this.GenerateAssetInfo(rAssetPath);
                if (rAssetInfo != null)
                {
                    this.AssetInfoList.Add(rAssetInfo);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public void Replace2ONewGuid()
        {
            for (int i = 0; i < this.AssetInfoList.Count; i++)
            {
                var rAssetInfo = this.AssetInfoList[i];
                this.ReplaceAssetGuid(rAssetInfo.AssetPath, rAssetInfo.OldGuid, rAssetInfo.NewGuid);
            }
        }
        public void Replace2OldGuid()
        {
            for (int i = 0; i < this.AssetInfoList.Count; i++)
            {
                var rAssetInfo = this.AssetInfoList[i];
                this.ReplaceAssetGuid(rAssetInfo.AssetPath, rAssetInfo.NewGuid, rAssetInfo.OldGuid);
            }
        }
        private AssetInfo GenerateAssetInfo(string rAssetPath)
        {
            if (rAssetPath.IsNullOrEmpty())
            {
                LogManager.LogError($"AssetGuidModifyer生成AssetInfo失败，资源路径不可为空 AssetPath:{rAssetPath}");

                return null;
            }
            var rGuid = AssetDatabase.AssetPathToGUID(rAssetPath);
            if (rGuid.IsNullOrEmpty())
            {
                LogManager.LogError($"AssetGuidModifyer生成AssetInfo失败，资源路径无法转换为Guid AssetPath:{rAssetPath}");
                return null;
            }

            rAssetPath = AssetDatabase.GUIDToAssetPath(rGuid);
            if (rAssetPath.IsNullOrEmpty())
            {
                LogManager.LogError($"AssetGuidModifyer生成AssetInfo失败，Guid无法转换为新的资源路径 Guid:{rGuid}");
                return null;
            }

            var rAssetInfo = new AssetInfo()
            {
                AssetPath = rAssetPath,
                OldGuid = rGuid,
                NewGuid = string.Empty,
            };
            if (!Guid.TryParseExact(rAssetInfo.OldGuid, "N", out var rOldGuid))
            {
                LogManager.LogError("AssetGuidModifyer生成AssetInfo失败，无法转换OldGuid");
                return null;
            }
            var rGuidBytesSpan = new Span<byte>(this.GuidBytes);
            if (!rOldGuid.TryWriteBytes(rGuidBytesSpan))
            {
                LogManager.LogError("AssetGuidModifyer生成AssetInfo失败，无法Guid无法写入GuidBytes");
                return null;
            }
            var rAssetMD5Bytes = UtilTool.GetMD5ToBytes(rAssetPath);
            for (int i = 0; i < this.GuidBytes.Length; i++)
            {
                this.GuidBytes[i] ^= rAssetMD5Bytes[i];
            }
            var rNewGuid = new Guid(this.GuidBytes);
            var rFirstNewGuid = rNewGuid;
            var nIndex = 0;
            while (!AssetDatabase.GUIDToAssetPath(rNewGuid.ToString("N")).IsNullOrEmpty())
            {
                nIndex %= this.GuidBytes.Length;
                this.GuidBytes[nIndex]++;
                rNewGuid = new Guid(this.GuidBytes);
            }
            if (rNewGuid != rFirstNewGuid)
            {
                LogManager.LogWarning($"AssetGuidModifyer 生成新的Guid遇到了冲突 与已存在资源Guid冲突 解决冲突前Guid:{rFirstNewGuid:N} 解决冲突后Guid:{rNewGuid:N}");
            }
            rAssetInfo.NewGuid = rNewGuid.ToString("N");
            if (this.AssetNewGuidDictionary.TryGetValue(rAssetInfo.NewGuid, out var rPreAssetPath))
            {
                LogManager.LogWarning($"AssetGuidModifyer 新的Guid与之前生成的资源新Guid冲突 PreAssetPath:{rPreAssetPath} CurAssetPath:{rAssetPath} Guid:{rAssetInfo.NewGuid}");
                return null;
            }
            else
            {
                this.AssetNewGuidDictionary.Add(rAssetInfo.NewGuid, rAssetPath);
            }

            return rAssetInfo;
        }
        private void ReplaceAssetGuid(string rAssetPath, string rOldGuid, string rNewGuid)
        {
            var rAssetMetaPath = rAssetPath + ".meta";
            var rFileContent = File.ReadAllText(rAssetMetaPath);
            rFileContent = rFileContent.Replace(rOldGuid, rNewGuid);
            File.WriteAllText(rAssetMetaPath, rFileContent);
        }
    }
}