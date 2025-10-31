using Knight.Core;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace UnityEditor.UI
{
    // public class MultiLanguageArray
    // {
    //     /// <summary>
    //     /// 
    //     /// </summary>
    //     public string Id { get; set; }
    //     /// <summary>
    //     /// 测试C
    //     /// </summary>
    //     public string Chinese { get; set; }
    //     /// <summary>
    //     /// 测试英语
    //     /// </summary>
    //     public string English { get; set; }
    //     /// <summary>
    //     /// 测试日语
    //     /// </summary>
    //     public string Japanese { get; set; }
    //     /// <summary>
    //     /// 测试韩语
    //     /// </summary>
    //     public string Korean { get; set; }
    //     /// <summary>
    //     /// 测试泰语
    //     /// </summary>
    //     public string Thai { get; set; }
    // }

    public class MultiLanguage
    {
        /// <summary>
        /// 
        /// </summary>
        public List<MultiLanguageArray> MultiLanguageArray { get; set; }
    }

    
    public class MultiLanguageArray
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 测试C
        /// </summary>
        public string ChineseSimplified { get; set; }
        /// <summary>
        /// 测试英语
        /// </summary>
        public string English { get; set; }
        /// <summary>
        /// 测试日语
        /// </summary>
        public string Japanese { get; set; }
        /// <summary>
        /// 测试韩语
        /// </summary>
        public string Korean { get; set; }
        /// <summary>
        /// 测试泰语
        /// </summary>
        public string Thai { get; set; }
        /// <summary>
        /// 测试C
        /// </summary>
        public string ChineseTraditional { get; set; }
    }


    public class UIMultiLanguage : TSingleton<UIMultiLanguage>
    {

        public const string RofLanguageTablePath = "Assets/GameAsset/Configs/Json/MultiLanguageTable.json";

        private Dictionary<long, MultiLanguageArray> mIDMap;

        public void Init()
        {
            var rLanguageText = File.ReadAllText(RofLanguageTablePath, System.Text.Encoding.UTF8);
            this.mIDMap = new Dictionary<long, MultiLanguageArray>();
#if UNITY_EDITOR
            var rMultiLanguage = Newtonsoft.Json.JsonConvert.DeserializeObject<MultiLanguage>(rLanguageText);
            foreach (var rArray in rMultiLanguage.MultiLanguageArray)
                this.mIDMap.Add(rArray.Id,rArray);
#endif

        }
        public MultiLanguageArray GetDataByID(long nID)
        {
            if (this.mIDMap.ContainsKey(nID) == false)
            {
                return null;
            }
            return this.mIDMap[nID];
        }

        public MultiLanguageArray FindDataByChinese(string mChinese)
        {
            foreach (var rRotData in this.mIDMap)
            {
                if (rRotData.Value.ChineseSimplified == mChinese)
                {
                    return rRotData.Value;
                }
            }

            return null;
        }

        public string FindDataIDByChinese(string mChinese)
        {
            string rResult = string.Empty;
            foreach (var rRotData in this.mIDMap)
            {
                if (rRotData.Value.ChineseSimplified == mChinese)
                {
                    rResult += $" {rRotData.Key}";
                }
            }

            return rResult;
        }

        public string GetChineseDataByID(long nID)
        {
            if (this.mIDMap.ContainsKey(nID) == false)
            {
                return string.Empty;
            }
            return this.mIDMap[nID].ChineseSimplified;
        }
        public void GetChineseByID(long nID, ref string rContent)
        {
            if (rContent == null)
                rContent = "";
            var rRofData = UIMultiLanguage.Instance.GetDataByID(nID);
            if (rRofData != null)
                rContent = rRofData.ChineseSimplified;
        }

        private UIMultiLanguage()
        {
            this.Init();
        }

        public object FindDataByChinese(object stringValue)
        {
            throw new System.NotImplementedException();
        }
    }

}

