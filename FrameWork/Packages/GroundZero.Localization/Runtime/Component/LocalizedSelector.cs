using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Knight.Core;
using System;

namespace GroundZero.Localization
{
    public class LocalizedSelector : MonoBehaviour
    {
        public LocalizedSelectorSetting[] Settings = new LocalizedSelectorSetting[9]
        {
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.ChineseSimplified, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.English, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.Japanese, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.Indonesian, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.Korean, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.Malay, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.Thai, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.Arabic, Obj = null },
            new LocalizedSelectorSetting(){ Type = MutilLanguageType.ChineseTraditional, Obj = null },
        };

        public void OnEnable()
        {
            LocalizationManager.Instance.OnLocalize += this.OnLocalize_Handler;
        }

        public void Start()
        {
            this.OnLocalize_Handler(LocalizationManager.CurLanguageType);
        }


        public void OnDisable()
        {
            LocalizationManager.Instance.OnLocalize -= this.OnLocalize_Handler;
        }


        private void OnLocalize_Handler(MutilLanguageType rMutilLanguageType)
        {
            foreach (var item in this.Settings)
            {
                if(item.Obj != null)
                {
                    if (item.Type == rMutilLanguageType)
                        item.Obj.SetActive(true);
                    else
                        item.Obj.SetActive(false);
                }
            }
        }



    }



    [System.Serializable]
    public class LocalizedSelectorSetting
    {
        public MutilLanguageType Type;
        public GameObject Obj;
    }
}