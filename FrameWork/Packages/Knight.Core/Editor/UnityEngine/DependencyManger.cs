//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.Collections.Generic;
using UnityEditor;

namespace Knight.Core.Editor
{
    public class DependencyManger
    {
        public Dictionary<string, string[]> AssetDependenciesDict = new Dictionary<string, string[]>();
        public void CalcDependencies(List<string> rAssetList)
        {
            if (rAssetList == null) return;
            foreach (var rAsset in rAssetList)
            {
                this.GetDependencies(rAsset);
            }
        }
        public string[] GetDependencies(string rAsset)
        {
            if (this.AssetDependenciesDict.TryGetValue(rAsset, out var rDependencies))
            {
                return rDependencies;
            }
            rDependencies = AssetDatabase.GetDependencies(rAsset);
            this.AssetDependenciesDict.Add(rAsset, rDependencies);
            return rDependencies;
        }
        public PoolList<string> GetDependencies(string[] rAssets)
        {
            var rDependencyList = PoolList<string>.Alloc();
            for (int i = 0; i < rAssets.Length; i++)
            {
                rDependencyList.AddRange(this.GetDependencies(rAssets[i]));
            }
            return rDependencyList;
        }
    }
}