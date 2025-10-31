using System;
using Knight.Core;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class WTextMeshPro : TextMeshProUGUI
    {
        [NonSerialized] private static readonly VertexHelper s_VertexHelper = new VertexHelper();
        private TextAlignmentOptions mOriginTextAnchor;
    
        [SerializeField]
        [HideInInspector]
        public bool mIsInitialColor;
        [SerializeField]
        [HideInInspector]
        public bool mIsUseMultiLang;

        [SerializeField]
        [HideInInspector]
        public long mMultiLangID;

        public long MultiLangID
        {
            get { return this.mMultiLangID; }
            set
            {
                this.mMultiLangID = value;
                if (this.mIsUseMultiLang)
                {
                    if (this.mMultiLangID != 0)
                    {
                        this.text = LocalizationManager.Instance.GetMultiLanguage(this.mMultiLangID);
                    }
                    else
                    {
                        this.text = string.Empty;
                    }
                }

            }
        }

        public bool IsInitialColor
        {
            get { return this.mIsInitialColor; }
            set
            {
                this.mIsInitialColor = value;

            }
        }

        public bool IsUseMultiLang
        {
            get { return this.mIsUseMultiLang; }
            set
            {
                this.mIsUseMultiLang = value;

            }
        }
        public override string text
        {
            get
            {
                return this.m_text;
            }
            set
            {
                string rNewValue = value;
                if (this.mIsUseMultiLang && this.mMultiLangID != 0)
                {
                    rNewValue = LocalizationManager.Instance.GetMultiLanguage(this.mMultiLangID);
#if UNITY_EDITOR
                    if (!Application.isPlaying)
                    {
                        if (rNewValue == this.mMultiLangID.ToString())
                        {
                            var rRofData = UIMultiLanguage.Instance.GetDataByID(this.mMultiLangID);
                            if (rRofData != null)
                            {
                                rNewValue = rRofData.ChineseSimplified;
                            }

                        }
                    }
#endif                   
                }

                if (base.text == rNewValue)
                    return;
                if (this.IsInitialColor)
                {
                    rNewValue = UtilTool.ConvertFontColorFormat(rNewValue, 1);
                }
                base.text = rNewValue;
                base.OnPopulateMesh(s_VertexHelper);
                UtilTool.SafeExecute(this.OnTextValueChanged);
            }
        }

        public Action OnTextValueChanged;

        [SerializeField]
        [HideInInspector]
        public string mFontColor;

        public string FontColor
        {
            get { return this.mFontColor; }
            set
            {
                this.mFontColor = value;
                if (!string.IsNullOrEmpty(this.mFontColor))
                {
                    Color rFontColor = UtilTool.ToColor(this.mFontColor);
                    this.color = rFontColor;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (this.mIsUseMultiLang && this.mMultiLangID != 0)
            {
                if (Application.isPlaying)
                {

                    LocalizationManager.Instance.OnLocalize += this.OnLocalize;
                }
            }
            this.mOriginTextAnchor = this.alignment;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.text = this.m_text;
        }

        private void OnLocalize(MutilLanguageType rMutilLanguageType)
        {
            string rNewValue = LocalizationManager.Instance.GetMultiLanguage(this.mMultiLangID);
            base.text = rNewValue;
            UtilTool.SafeExecute(this.OnTextValueChanged);
        }

        protected override void OnDestroy()
        {
            if (Application.isPlaying)
            {
                if (this.mIsUseMultiLang && this.mMultiLangID != 0)
                {
                    LocalizationManager.Instance.OnLocalize -= this.OnLocalize;
                }
            }

            base.OnDestroy();
        }

#if UNITY_EDITOR
        public string Chinese;

        protected override void OnValidate()
        {
            base.OnValidate();
            this.text = this.m_text;
        }
#endif
    }
}

