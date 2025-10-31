using Knight.Core;
using NaughtyAttributes;
using UnityEngine.AddressableAssets;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class ImageReplace : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private string mSpriteName;
        [SerializeField]
        [ReadOnly]
        private MutilLanguageType mCurLanguageType;
        public Image Image;
        public bool IsNativeSize;
        public string PrefabName;

        [SerializeField][OnValueChanged("OnIsUseMultiLangValueChangedCallback")]
        private bool mIsUseMultiLang;

        public bool IsUseMultiLang
        {
            get { return this.mIsUseMultiLang; }
            set
            {
                this.mIsUseMultiLang = value;
                if (this.mIsUseMultiLang)
                {
                    this.Image.sprite = null;
                    // this.OnLocalize(LocalizationManager.CurLanguageType);
                }
                else
                {
                    // this.LoadSprite(this.mSpriteName);
                }
            }
        }

        private void OnIsUseMultiLangValueChangedCallback()
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(this.mSpriteName)) return;

            if (!this.mIsUseMultiLang)
            {
                this.Image.sprite = UIAtlasManager.Instance.LoadSprite_Editor(this.mSpriteName);
            }
#endif
        }

        private string mImgColor;
        public string ImgColor
        {
            get
            {
                return this.mImgColor;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Image.color = UtilTool.ToColor(value);
                    this.mImgColor = value;
                }
            }
        }

        private void Awake()
        {
            this.Image = this.gameObject.ReceiveComponent<Image>();

            if (this.mIsUseMultiLang)
            {
                if (string.IsNullOrEmpty(this.mSpriteName))
                {
                    return;
                }
                this.Image.sprite = null;
            }
            else if (this.Image && this.Image.sprite)
            {
                this.mSpriteName = this.Image.sprite.name;
                
            }
            else
            {
                this.mSpriteName = string.Empty;
            }
        }

        private void Start()
        {
            if (Application.isPlaying && this.mIsUseMultiLang)
            {
                // LocalizationManager.Instance.OnLocalize += this.OnLocalize;
                if (string.IsNullOrEmpty(this.mSpriteName))
                {
                    return;
                }
                // this.OnLocalize(LocalizationManager.CurLanguageType);
                if (this.IsNativeSize)
                {
                    this.Image.SetNativeSize();
                }
            }
        }

        private void OnDestroy()
        {
            if (Application.isPlaying && this.mIsUseMultiLang)
            {
                // LocalizationManager.Instance.OnLocalize -= this.OnLocalize;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
           if(this.Image && this.Image.sprite)
                this.mSpriteName = this.Image.sprite.name;
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
                this.mSpriteName = value;
                if (!string.IsNullOrEmpty(this.mSpriteName))
                {
                    if(this.mSpriteName.ToLower().StartsWith("http"))
                        this.LoadSpriteByHttp(this.mSpriteName,this.Image);
                    else
                        this.LoadSprite(this.mSpriteName,this.Image);
                }
            }
        }



        private void LoadSpriteByHttp(string rSpriteName,Image rImg)
        {
            HeadIconManager.Instance.LoadHeadInfo(rSpriteName,rImg);
        }
        
        private void LoadSprite(string rSpriteKey,Image rImg)
        {
            if (!string.IsNullOrEmpty(rSpriteKey))
            {
                var rSprite = Addressables.LoadAssetAsync<Sprite>(rSpriteKey).WaitForCompletion();
                rImg.sprite = rSprite;
            }
        }
        
        // private void LoadSprite(string rSpriteName)
        // {
        //     if (string.IsNullOrEmpty(rSpriteName) || rSpriteName == "0")
        //     {
        //         this.Image.sprite = null;
        //         return;
        //     }
        //     this.mCurLanguageType = LocalizationManager.CurLanguageType;
        //     // 指定了图集
        //     if (this.mIsUseMultiLang)
        //     {
        //         this.Image.sprite = UIAtlasManager.Instance.LoadMultiLanSprite(rSpriteName, LocalizationManager.Instance.GetMutilLanguageSuffixLower());
        //         if (this.Image.sprite == null)
        //         {
        //             this.Image.sprite = UIAtlasManager.Instance.LoadMutilIcon(rSpriteName, LocalizationManager.Instance.GetMutilLanguageSuffixLower());
        //             if (this.Image.sprite == null)
        //             {
        //                 this.Image.sprite = UIAtlasManager.Instance.LoadSprite(rSpriteName, this.PrefabName);
        //                 if(this.Image.sprite == null)
        //                 {
        //                     LogManager.LogErrorFormat("not find icon: {0}, PrefabName: {1}", rSpriteName, this.PrefabName);
        //                 }
        //             }
        //         }
        //     }
        //     else
        //     {
        //         this.Image.sprite = UIAtlasManager.Instance.LoadSprite(rSpriteName, this.PrefabName);
        //         if (this.Image.sprite == null)
        //         {
        //             Knight.Core.LogManager.LogErrorFormat("not find sprite: {0}, PrefabName: {1}", rSpriteName, this.PrefabName);
        //         }
        //     }
        // }
        //
        // public void OnLocalize(MutilLanguageType rMutilLanguageType)
        // {
        //     this.LoadSprite(this.mSpriteName);
        //     if (this.IsNativeSize)
        //     {
        //         this.Image.SetNativeSize();
        //     }
        // }
    }
}
