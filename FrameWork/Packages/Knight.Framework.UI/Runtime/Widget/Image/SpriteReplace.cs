using Knight.Core;
using NaughtyAttributes;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteInEditMode]
    public class SpriteReplace : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private string mSpriteName;
        [SerializeField]
        [ReadOnly]
        public SpriteRenderer Image;
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
            this.Image = this.gameObject.ReceiveComponent<SpriteRenderer>();

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
            
        }

        private void OnDestroy()
        {
        }

#if UNITY_EDITOR
        /*private void OnValidate()
        {
           if(this.Image && this.Image.sprite)
                this.mSpriteName = this.Image.sprite.name;
        }*/
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
        
        private void LoadSpriteByHttp(string rSpriteName,SpriteRenderer rImg)
        {
            HeadIconManager.Instance.LoadHeadInfo(rSpriteName,rImg);
        }
        private void LoadSprite(string rSpriteKey,SpriteRenderer rImg)
        {
            if (!string.IsNullOrEmpty(rSpriteKey))
            {
                var rSprite = Addressables.LoadAssetAsync<Sprite>(rSpriteKey);
                rSprite.WaitForCompletion();
                if (rSprite.Status == AsyncOperationStatus.Succeeded)
                {
                    rImg.sprite = rSprite.Result;
                }
              
            }
        }
        
    }
}
