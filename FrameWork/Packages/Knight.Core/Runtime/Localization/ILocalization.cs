//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Game;
using UnityEngine;

namespace Knight.Core
{
    public interface ILocalization
    {
        void InitCurLanType();
        string GetMultiLanguage(long nMultiLangID);
        string GetMultiLanguage(long nMultiLangID, params object[] args);
        string GetMultiLanIcon(string rImageName);

        Font GetMultiLanFont(string rFontId);
        bool SwitchLanguage(MutilLanguageType rLanguage);
        void SwitchLanguageCompleted();
        string GetMutilLanguageSuffix();
        string GetMutilLanguageSuffixLower();
        string GetMutilLanguageSuffixLower(MutilLanguageType rLanguage);

        void InitPackFirstLanguage();

        bool IsFirstEnterGame();
    }
    public class PreMultiLanRow
    {
        public long ID;
        public string Chinese;
        public string English;
        public string Japanese;
        public string Indonesian;
        public string Korean;
        public string Malay;
        public string Thai;
        public string Arabic;
        public string Traditional;
        public string GetString(MutilLanguageType rType)
        {
            switch (rType)
            {
                case MutilLanguageType.ChineseSimplified:
                    return this.Chinese;
                case MutilLanguageType.ChineseTraditional:
                    return this.Traditional;
                case MutilLanguageType.English:
                    return this.English;
                case MutilLanguageType.Japanese:
                    return this.Japanese;
                case MutilLanguageType.Indonesian:
                    return this.Indonesian;
                case MutilLanguageType.Korean:
                    return this.Korean;
                case MutilLanguageType.Malay:
                    return this.Malay;
                case MutilLanguageType.Thai:
                    return this.Thai;
                case MutilLanguageType.Arabic:
                    return this.Arabic;
                default:
                    return this.ID.ToString();
            }
        }
    }
    public enum MutilLanguageType
    {
        [InspectorName("无效(不要选)")]
        Invalid,
        /// <summary>
        /// 简体中文
        /// </summary>
        [InspectorName("简体中文")]
        ChineseSimplified = 1,
        /// <summary>
        /// 英文
        /// </summary>
        [InspectorName("英文")]
        English = 2,
        /// <summary>
        /// 日文
        /// </summary>
        [InspectorName("日文")]
        Japanese = 3,
        /// <summary>
        /// 印尼语
        /// </summary>
        [InspectorName("印尼语")]
        Indonesian = 4,
        /// <summary>
        /// 韩语
        /// </summary>
        [InspectorName("韩语")]
        Korean = 5,
        /// <summary>
        /// 马来语
        /// </summary>
        [InspectorName("马来语")]
        Malay = 6,
        /// <summary>
        /// 泰语
        /// </summary>
        [InspectorName("泰语")]
        Thai = 7,
        /// <summary>
        /// 阿拉伯语
        /// </summary>
        [InspectorName("阿拉伯语")]
        Arabic = 8,
        /// <summary>
        /// 繁体中文
        /// </summary>
        [InspectorName("繁体中文")]
        ChineseTraditional = 9,
        [InspectorName("最大(不要选)")]
        MaxType,
    }

    public class PreMultiLanTable
    {
        public List<PreMultiLanRow> Table;
    }
    public class LocalizationManager
    {
        public const string PrefMultiLanuageSaveKey = "MultiLanuageType";

        private ILocalization mLocal;

        static LocalizationManager GInstance = null;
        public static LocalizationManager Instance
        {
            get
            {
                if (GInstance == null)
                    GInstance = new LocalizationManager();
                return GInstance;
            }
        }
        
        public Action<MutilLanguageType> OnLocalize { get; set; }
        #region Static

        //当前的系统语言
        public static MutilLanguageType CurLanguageType { get; set; } = (MutilLanguageType)PlayerPrefs.GetInt(PrefMultiLanuageSaveKey,(int)MutilLanguageType.ChineseSimplified);

        //包体的默认语言
        public static MutilLanguageType SystemLanguageType
        {
            get
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Afrikaans:
                        break;
                    case SystemLanguage.Arabic:
                        return MutilLanguageType.Arabic;
                    case SystemLanguage.Basque:
                        break;
                    case SystemLanguage.Belarusian:
                        break;
                    case SystemLanguage.Bulgarian:
                        break;
                    case SystemLanguage.Catalan:
                        break;
                    case SystemLanguage.Chinese:
                        break;
                    case SystemLanguage.Czech:
                        break;
                    case SystemLanguage.Danish:
                        break;
                    case SystemLanguage.Dutch:
                        break;
                    case SystemLanguage.English:
                        return MutilLanguageType.English;
                    case SystemLanguage.Estonian:
                        break;
                    case SystemLanguage.Faroese:
                        break;
                    case SystemLanguage.Finnish:
                        break;
                    case SystemLanguage.French:
                        break;
                    case SystemLanguage.German:
                        break;
                    case SystemLanguage.Greek:
                        break;
                    case SystemLanguage.Hebrew:
                        break;
                    case SystemLanguage.Icelandic:
                        break;
                    case SystemLanguage.Indonesian:
                        return MutilLanguageType.Indonesian;
                    case SystemLanguage.Italian:
                        break;
                    case SystemLanguage.Japanese:
                        return MutilLanguageType.Japanese;
                    case SystemLanguage.Korean:
                        return MutilLanguageType.Korean;
                    case SystemLanguage.Latvian:
                        break;
                    case SystemLanguage.Lithuanian:
                        break;
                    case SystemLanguage.Norwegian:
                        break;
                    case SystemLanguage.Polish:
                        break;
                    case SystemLanguage.Portuguese:
                        break;
                    case SystemLanguage.Romanian:
                        break;
                    case SystemLanguage.Russian:
                        break;
                    case SystemLanguage.SerboCroatian:
                        break;
                    case SystemLanguage.Slovak:
                        break;
                    case SystemLanguage.Slovenian:
                        break;
                    case SystemLanguage.Spanish:
                        break;
                    case SystemLanguage.Swedish:
                        break;
                    case SystemLanguage.Thai:
                        return MutilLanguageType.Thai;
                    case SystemLanguage.Turkish:
                        break;
                    case SystemLanguage.Ukrainian:
                        break;
                    case SystemLanguage.Vietnamese:
                        break;
                    case SystemLanguage.ChineseSimplified:
                        return MutilLanguageType.ChineseSimplified;
                    case SystemLanguage.ChineseTraditional:
                        return MutilLanguageType.ChineseTraditional;
                    case SystemLanguage.Unknown:
                        break;
                }

                return MutilLanguageType.Invalid;
            }
        }

        //游戏的设置语言
        public static string QuestionareRegionByGameSetting
        {
            get
            {
                switch (CurLanguageType)
                {
                    case MutilLanguageType.Arabic:
                        return "al";
                    case MutilLanguageType.Japanese:
                        return "jp";
                    case MutilLanguageType.Indonesian:
                        return "id";
                    case MutilLanguageType.Thai:
                        return "th";
                    case MutilLanguageType.Korean:
                        return "kor";
                    case MutilLanguageType.English:
                        return "en";
                    case MutilLanguageType.ChineseSimplified:
                        return "zh";
                    case MutilLanguageType.ChineseTraditional:
                        return "tw";
                }
                return "en";
            }
        }
        #endregion
        public Dict<long, PreMultiLanRow> PreMultiLanTable { get; private set; }

        private LocalizationManager()
        {
        }

        public void Initialize_PreABLoader()
        {
            var rJsonText = Resources.Load<TextAsset>("PreMultiLanguage").text;
            this.PreMultiLanTable = Newtonsoft.Json.JsonConvert.DeserializeObject<Dict<long, PreMultiLanRow>>(rJsonText);
        }
        public void Initialize(ILocalization rLocal)
        {
            this.mLocal = rLocal;
        }
        public string GetMultiLanguage_PreABLoader(long nMultiLangID, params object[] args)
        {
            var rStr = this.GetMultiLanguage_PreABLoader(nMultiLangID);
            if (string.IsNullOrEmpty(rStr))
            {
                return string.Empty;
            }
            if (rStr.Equals(nMultiLangID.ToString()))
            {
                return rStr;
            }
            try
            {
                rStr = string.Format(rStr, args);
            }
            catch
            {
                Debug.LogError($"PreABLoader多语言表ID= {nMultiLangID}的多语言字符串错误！！！");
            }

            return rStr;
        }

        public string GetMultiLanguage_PreABLoader(long nMultiLangID)
        {
            if (!this.PreMultiLanTable.TryGetValue(nMultiLangID, out var preMultiLanRow))
            {
                return string.Empty;
            }

            string rResult = preMultiLanRow.GetString(LocalizationManager.CurLanguageType);

            if (string.IsNullOrEmpty(rResult))
            {
                rResult = preMultiLanRow.GetString(MutilLanguageType.English);
            }

            return rResult;
        }
        public string GetMultiLanguage(long nMultiLangID, params object[] args)
        {
            if (this.mLocal == null) return nMultiLangID.ToString();
            return this.mLocal.GetMultiLanguage(nMultiLangID, args);
        }

        public string GetMultiLanguage(long nMultiLangID)
        {
            if (this.mLocal == null) return nMultiLangID.ToString();
            return this.mLocal.GetMultiLanguage(nMultiLangID);
        }

        public string GetMultiLanIcon(string rImageName)
        {
            if (this.mLocal == null) return rImageName;
            return this.mLocal.GetMultiLanIcon(rImageName);
        }

        public Font GetMultiLanFont(string rFontId)
        {
            if (this.mLocal == null) return default(Font);
            return this.mLocal.GetMultiLanFont(rFontId);
        }
        public string GetMutilLanguageSuffix()
        {
            if (this.mLocal == null) return string.Empty;
            return this.mLocal.GetMutilLanguageSuffix();
        }
        public string GetMutilLanguageSuffixLower()
        {
            if (this.mLocal == null) return string.Empty;
            return this.mLocal.GetMutilLanguageSuffixLower();
        }
        public string GetMutilLanguageSuffixLower(MutilLanguageType rLanguage)
        {
            if (this.mLocal == null) return string.Empty;
            return this.mLocal.GetMutilLanguageSuffixLower(rLanguage);
        }
        public bool SwitchLanguage(MutilLanguageType rLanguage)
        {
            if (this.mLocal == null) return false;
            if (this.mLocal.SwitchLanguage(rLanguage))
            {
                this.OnLocalize?.Invoke(rLanguage);
                this.mLocal.SwitchLanguageCompleted();
            }

            return true;
        }

        public void InitCurLanType()
        {
            if (this.mLocal == null) return;
            this.mLocal.InitCurLanType();
            this.OnLocalize?.Invoke(CurLanguageType);
        }

        public bool IsFirstEnterGame()
        {
            if (this.mLocal == null) return false;
            return this.mLocal.IsFirstEnterGame();
        }

        public void InitFirstLanguage()
        {
            if (this.mLocal == null) return;
            this.mLocal.InitPackFirstLanguage();
        }


    }
}
