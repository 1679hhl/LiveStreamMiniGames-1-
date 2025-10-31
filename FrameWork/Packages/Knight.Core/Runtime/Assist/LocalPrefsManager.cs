using Knight.Core.WindJson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    public abstract class LocalPrefsManager
    {
        public enum ParamType
        {
            Int,
            Float,
            String,
            // 扩展类型
            Bool,
            Long,
            Double,
        }

        private const string PrefsKeyListPrefix = "__Local_PREFS_KEY_LIST";

        private Dictionary<ParamType, HashSet<string>> mParamKeyDict = new Dictionary<ParamType, HashSet<string>>();
        private Dictionary<ParamType, Dictionary<string, string>> mValueKeyCacheDict = new Dictionary<ParamType, Dictionary<string, string>>();

        public string SaveKey;
        public bool Initialized;

        public void Initialize(string rSaveKey)
        {
            this.SaveKey = rSaveKey;
            this.Initialized = true;
            this.mParamKeyDict.Clear();
            this.mValueKeyCacheDict.Clear();
            foreach (ParamType rParamType in Enum.GetValues(typeof(ParamType)))
            {
                var rParamKey = $"{PrefsKeyListPrefix}__{rParamType}__{rSaveKey}__";
                var rValue = PlayerPrefs.GetString(rParamKey, string.Empty);
                this.mParamKeyDict.Add(rParamType, new HashSet<string>());
                if (!rValue.IsNullOrWhiteSpace())
                {
                    var rList = JsonConvert.DeserializeObject<List<string>>(rValue);
                    if (rList != null)
                    {
                        for (int i = 0; i < rList.Count; i++)
                        {
                            this.mParamKeyDict[rParamType].Add(rList[i]);
                        }
                    }
                }
                this.mValueKeyCacheDict.Add(rParamType, new Dictionary<string, string>());
            }
        }

        public void Destroy()
        {
            this.SaveKey = string.Empty;
            this.Initialized = false;
        }

        /// <summary>
        /// 清理当前SaveKey对应的数据
        /// </summary>
        public void Clear()
        {
            foreach (var rDict in this.mParamKeyDict)
            {
                var rParamKey = $"{PrefsKeyListPrefix}__{rDict.Key}__{this.SaveKey}__";
                PlayerPrefs.DeleteKey(rParamKey);
                foreach (var rParamValueKey in rDict.Value)
                {
                    PlayerPrefs.DeleteKey(rParamValueKey);
                }
                rDict.Value.Clear();
            }
            foreach (var rDict in this.mValueKeyCacheDict)
            {
                rDict.Value.Clear();
            }
            PlayerPrefs.Save();
        }

        private bool TryAddParamValueKey(ParamType rParamType, string rParamValueKey)
        {
            if (this.mParamKeyDict.TryGetValue(rParamType, out var rParamValueKeyHashSet))
            {
                if (rParamValueKeyHashSet.Add(rParamValueKey))
                {
                    var rParamKey = $"{PrefsKeyListPrefix}__{rParamType}__{this.SaveKey}__";
                    var rList = new List<string>();
                    foreach (var rItem in rParamValueKeyHashSet)
                    {
                        rList.Add(rItem);
                    }
                    var rListJson = JsonConvert.SerializeObject(rList);
                    PlayerPrefs.SetString(rParamKey, rListJson);
                    return true;
                }
            }
            return false;
        }

        private string GetParamValueKey(ParamType rParamType, string rValueKey)
        {
            if (this.mValueKeyCacheDict.TryGetValue(rParamType, out var rDict))
            {
                if (!rDict.TryGetValue(rValueKey, out var rParamValueKey))
                {
                    rParamValueKey = $"{PrefsKeyListPrefix}__{rParamType}__{this.SaveKey}__{rValueKey}__";
                    rDict.Add(rValueKey, rParamValueKey);
                    this.TryAddParamValueKey(rParamType, rParamValueKey);
                }
                return rParamValueKey;
            }
            return $"{PrefsKeyListPrefix}__{rParamType}__{this.SaveKey}__{rValueKey}__";
        }

        public int GetInt(string rKey, int nDelfult = 0)
        {
            if (!this.Initialized) return nDelfult;
            return PlayerPrefs.GetInt(this.GetParamValueKey(ParamType.Int, rKey), nDelfult);
        }

        public void SetInt(string rKey, int nValue)
        {
            if (!this.Initialized) return;
            PlayerPrefs.SetInt(this.GetParamValueKey(ParamType.Int, rKey), nValue);
        }

        public float GetFloat(string rKey, float fDelfult = 0)
        {
            if (!this.Initialized) return fDelfult;
            return PlayerPrefs.GetFloat(this.GetParamValueKey(ParamType.Float, rKey), fDelfult);
        }

        public void SetFloat(string rKey, float fValue)
        {
            if (!this.Initialized) return;
            PlayerPrefs.SetFloat(this.GetParamValueKey(ParamType.Float, rKey), fValue);
        }

        public string GetString(string rKey, string rDelfult = "")
        {
            if (!this.Initialized) return rDelfult;
            return PlayerPrefs.GetString(this.GetParamValueKey(ParamType.String, rKey), rDelfult);
        }

        public void SetString(string rKey, string rValue)
        {
            if (!this.Initialized) return;
            PlayerPrefs.SetString(this.GetParamValueKey(ParamType.String, rKey), rValue);
        }

        public bool GetBool(string rKey, bool bDelfult = false)
        {
            if (!this.Initialized) return bDelfult;
            return PlayerPrefs.GetInt(this.GetParamValueKey(ParamType.Bool, rKey), bDelfult ? 1 : 0) == 1;
        }

        public void SetBool(string rKey, bool bValue)
        {
            if (!this.Initialized) return;
            PlayerPrefs.SetInt(this.GetParamValueKey(ParamType.Bool, rKey), bValue ? 1 : 0);
        }

        public long GetLong(string rKey, long nDelfult = 0)
        {
            if (!this.Initialized) return nDelfult;
            var rValue = PlayerPrefs.GetString(this.GetParamValueKey(ParamType.Long, rKey), "");
            if (!rValue.IsNullOrWhiteSpace() && long.TryParse(rValue, out var nValue))
            {
                return nValue;
            }
            else
            {
                return nDelfult;
            }
        }

        public void SetLong(string rKey, long nValue)
        {
            if (!this.Initialized) return;
            PlayerPrefs.SetString(this.GetParamValueKey(ParamType.Long, rKey), nValue.ToString());
        }

        public double GetDouble(string rKey, double fDelfult = 0)
        {
            if (!this.Initialized) return fDelfult;
            var rValue = PlayerPrefs.GetString(this.GetParamValueKey(ParamType.Double, rKey), "");
            if (!rValue.IsNullOrWhiteSpace() && double.TryParse(rValue, out var fValue))
            {
                return fValue;
            }
            else
            {
                return fDelfult;
            }
        }

        public void SetDouble(string rKey, double fValue)
        {
            if (!this.Initialized) return;
            PlayerPrefs.SetString(this.GetParamValueKey(ParamType.Double, rKey), fValue.ToString());
        }

        public void ClearKey(string rKey, ParamType paramType = ParamType.Int)
        {
            if (!this.Initialized) return;
            PlayerPrefs.DeleteKey(this.GetParamValueKey(paramType, rKey));
            PlayerPrefs.Save();
        }
        public bool HasKey(string rValueKey, ParamType rType = ParamType.Int)
        {
            if (!this.Initialized) return false;
            return this.HasKey(rType, rValueKey);
        }

        public bool HasKey(ParamType rParamType, string rValueKey)
        {
            if (!this.Initialized || this.mValueKeyCacheDict == null) return false;
            if (!this.mValueKeyCacheDict.TryGetValue(rParamType, out var rDict))
                return false;
            return rDict.TryGetValue(rValueKey, out var rParamValueKey);
        }
    }
}