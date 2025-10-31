using Knight.Core;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using GroundZero.Localization;

namespace Game
{
    public class GameLocalization : ILocalization
    {
        private static MutilLanguageType mCurLanguageType
        {
            get { return LocalizationManager.CurLanguageType; }
            set { LocalizationManager.CurLanguageType = value; }
        }
        private Dictionary<string, Font> mFontCacheDictionary = new Dictionary<string, Font>();
        public string GetMultiLanguage(long nMultiLangID)
        {
            if (!GameConfig.Instance.TryGetMultiLanguage(nMultiLangID, out var rValue))
            {
                return nMultiLangID.ToString();
            }

            //此符号策划默认为空字符串
            if (rValue.Equals("&"))
            {
                return string.Empty;
            }
            if (rValue.IsNullOrEmpty() || rValue.Equals("0"))
            {
                rValue = $"{nMultiLangID}";
            }
            else
            {
                // //阿拉伯系语言特殊处理
                // if (mCurLanguageType == MutilLanguageType.Arabic)
                //     rValue = rValue.RtlFix();
            }
            return rValue;
        }

        public string GetMultiLanguage(long nMultiLangID, params object[] args)
        {
            var rStr = this.GetMultiLanguage(nMultiLangID);
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
                rStr = string.Format(PluralFormatProvider.Formatter, rStr, args);
            }
            catch
            {
                Debug.LogError($"多语言表ID= {nMultiLangID}的多语言字符串错误！！！");
            }

            return rStr;
        }

        public string GetMultiLanIcon(string rImageName)
        {
            // string rValue = $"{rImageName}";
            //
            // if (GameConfig.Instance.EnumLocation != null)
            // {
            //     var rPath = GameConfig.Instance.EnumLocation.GetParam2(EEnumType.MultiLanguage, (int)mCurLanguageType);
            //     if (!string.IsNullOrEmpty(rPath))
            //     {
            //         rValue = $"{rPath}/{rImageName}";
            //     }
            // }
            // return rValue;
            return string.Empty;
        }

        public Font GetMultiLanFont(string rFontId)
        {
            //if (this.mFontCacheDictionary.TryGetValue(rFontId, out var rFont))
            //{
            //    return rFont;
            //}
            // Font rFont;
            // if (GameConfig.Instance.MultiLangFont != null)
            // {
            //     if (GameConfig.Instance.MultiLangFont.Table.TryGetValue(rFontId, out MultiLangFontConfig rConfig))
            //     {
            //         //LogManager.Log($"[多语言] {rFontId}-{rConfig[mCurLanguageType]}");
            //         rFont = FontAssetLoader.Instance.Load(mCurLanguageType, rConfig[mCurLanguageType]);
            //         //this.mFontCacheDictionary.Add(rFontId, rFont);
            //         return rFont;
            //     }
            // }
            // rFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
            //this.mFontCacheDictionary.Add(rFontId, rFont);
            // return rFont;
            return null;
        }

        public bool IsFirstEnterGame()
        {
            // if (ABPlatform.Instance.IsDevelopeMode())
            // {
            //     return false;
            // }
            //
            // string rCache = AppPrefsManager.Instance.GetString(SplitAssetBundleDownloadManager.CheckSystemLanguageTypeKey, "");
            //
            // if (!string.IsNullOrEmpty(rCache))
            // {
            //     return false;
            // }
            //
            // LogManager.LogRelease($"========SystemLanguage======={LocalizationManager.SystemLanguageType.ToString()}=");
            //
            // //如果系统语言 和 初始语言一直  不处理
            // if (LocalizationManager.SystemLanguageType == PackageAreaManager.Instance.PackageAreaConfig.InitLanguage)
            // {
            //     return false;
            // }
            return true;
        }

        public void InitPackFirstLanguage()
        {
            // if (PackageAreaManager.Instance.PackageAreaConfig.MutilLanguageTypeIsOpen(LocalizationManager.SystemLanguageType))
            // {
            //     this.SwitchLanguage(LocalizationManager.SystemLanguageType);
            // }
            // else
            // {
            //     this.SwitchLanguage(PackageAreaManager.Instance.PackageAreaConfig.DefaultLanguage);
            // }
        }

        public void InitCurLanType()
        {
            //多语言初始化
            var rType = PlayerPrefs.GetInt(LocalizationManager.PrefMultiLanuageSaveKey,1);
            mCurLanguageType = (MutilLanguageType)rType;
        }

        public bool SwitchLanguage(MutilLanguageType rLanguageType)
        {
            PlayerPrefs.SetInt(LocalizationManager.PrefMultiLanuageSaveKey, (int)rLanguageType);
            if (mCurLanguageType == rLanguageType) return false;
            mCurLanguageType = rLanguageType;
            this.PreSwitchLanguage();
            // GameConfig.Instance.InitializeOne("game/config/gameconfig/binary.ab", "MultiLanguage", mCurLanguageType, true);
            EventManager.Instance.Distribute(GameEvent.kEventSwitchMultiLanguageSecc);
            return true;
        }
        public void PreSwitchLanguage()
        {
            this.mFontCacheDictionary.Clear();
        }
        public void SwitchLanguageCompleted()
        {
            // UIAtlasManager.Instance.UnloadUnuseMutilanguage(mCurLanguageType);
            // FontAssetLoader.Instance.UnloadAll(mCurLanguageType);
        }

        public string GetMutilLanguageSuffix()
        {
            return string.Empty;
            // return PackageAreaConst.GetMutilLanguageSuffix(mCurLanguageType);
        }

        public string GetMutilLanguageSuffixLower()
        {
            return string.Empty;
            // return PackageAreaConst.GetMutilLanguageSuffixLower(mCurLanguageType);
        }
        public string GetMutilLanguageSuffixLower(MutilLanguageType rLanguage)
        {
            return string.Empty;
            // return PackageAreaConst.GetMutilLanguageSuffixLower(rLanguage);
        }
    }
}