using Knight.Core;
using NaughtyAttributes;
using System.Text.RegularExpressions;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RawImage))]
    [ExecuteInEditMode]
    public class RawImageReplace : MonoBehaviour
    {
        [SerializeField][ReadOnly]
        private string                  mSpriteName;
        [SerializeField]
        [ReadOnly]
        private MutilLanguageType       mCurLanguageType;
        public  RawImage                RawImage;
        public  bool                    IsNativeSize;

        [SerializeField]
        [OnValueChanged("OnIsUseMultiLangValueChangedCallback")]
        private bool mIsUseMultiLang;
        public bool IsUseMultiLang
        {
            get { return this.mIsUseMultiLang; }
            set
            {
                this.mIsUseMultiLang = value;
                if (this.mIsUseMultiLang)
                {
                    this.RawImage.texture = null;
                    this.OnLocalize(LocalizationManager.CurLanguageType);
                }
                else
                {
                    this.LoadSprite(this.mSpriteName);
                }
            }
        }
        private void Awake()
        {
            this.RawImage = this.gameObject.ReceiveComponent<RawImage>();
            if (this.RawImage && this.RawImage.texture)
            {
                this.mSpriteName = this.RawImage.texture.name;
            }
            if (this.mIsUseMultiLang)
            {
                if (string.IsNullOrEmpty(this.mSpriteName))
                {
                    return;
                }
                this.RawImage.texture = null;
            }
            else if (this.RawImage && this.RawImage.texture)
            {
                this.mSpriteName = this.RawImage.texture.name;

            }
            else
            {
                this.mSpriteName = string.Empty;
            }
        }
        private void OnIsUseMultiLangValueChangedCallback()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(this.mSpriteName)) return;

            if (!this.mIsUseMultiLang)
            {
                this.RawImage.texture = UIAtlasManager.Instance.LoadTexture_Editor(this.mSpriteName); 
            }
#endif
        }
        private void Start()
        {
            if (Application.isPlaying && this.mIsUseMultiLang)
            {
                LocalizationManager.Instance.OnLocalize += this.OnLocalize;
                if (string.IsNullOrEmpty(this.mSpriteName))
                {
                    return;
                }
                this.OnLocalize(LocalizationManager.CurLanguageType);
            }
        }

        private void OnDestroy()
        {
            if (Application.isPlaying && this.mIsUseMultiLang)
            {
                LocalizationManager.Instance.OnLocalize -= this.OnLocalize;
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (this.RawImage && this.RawImage.texture)
            {
                this.mSpriteName = this.RawImage.texture.name;
            }
        }
#endif
        public string SpriteName
        {
            get 
            { 
                return this.mSpriteName; 
            }
            set
            {
                if (this.mCurLanguageType == LocalizationManager.CurLanguageType && this.mSpriteName == value && this.RawImage.texture)
                {
                    return;
                }
                this.mSpriteName = value;
                if (string.IsNullOrEmpty(value))
                {
                    this.RawImage.texture = null;
                    return;
                }
                this.LoadSprite(this.mSpriteName);
                if (this.IsNativeSize)
                {
                    this.RawImage.SetNativeSize();
                }
            }
        }
        public void OnLocalize(MutilLanguageType rMutilLanguageType)
        {
            this.LoadSprite(this.mSpriteName);
        }
        private void LoadSprite(string rSpriteName)
        {
            if (string.IsNullOrEmpty(rSpriteName))
            {
                this.RawImage.texture = null;
                return;
            }
            this.mCurLanguageType = LocalizationManager.CurLanguageType;
            // 指定了图集
            if (this.mIsUseMultiLang)
            {
                this.RawImage.texture = UIAtlasManager.Instance.LoadMultiLanTexture(rSpriteName, LocalizationManager.Instance.GetMutilLanguageSuffixLower());
                if (this.RawImage.texture == null)
                {
                    this.RawImage.texture = UIAtlasManager.Instance.LoadTexture(this.mSpriteName);
                    if(this.RawImage.texture == null)
                        Knight.Core.LogManager.LogFormat("not find texture: {0} in {1}(Path:{2})", rSpriteName, LocalizationManager.Instance.GetMutilLanguageSuffixLower(), UtilTool.GetTransformPath(this.transform));
                }
            }
            else
            {
                this.RawImage.texture = UIAtlasManager.Instance.LoadTexture(this.mSpriteName);
                if (this.RawImage.texture == null)
                    Knight.Core.LogManager.LogFormat("not find texture: {0}", this.mSpriteName);
                else
                {
                    UIAtlasManager.Instance.SetNativeSize(this.RawImage, this.mSpriteName);
                }
            }
        }
    }
}
