using Knight.Core;
using NaughtyAttributes;
using RenderHeads.Media.AVProVideo;
using UnityEngine.AddressableAssets;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MediaPlayer))]
    [ExecuteInEditMode]
    public class VeidoReplace : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private string mVedioPath;
        [SerializeField]
        [ReadOnly]
        private MutilLanguageType mCurLanguageType;
        public MediaPlayer MediaPlayer;
        public bool IsNativeSize;
        public string PrefabName;

        [SerializeField][OnValueChanged("OnIsUseMultiLangValueChangedCallback")]
        private bool mIsUseMultiLang;

        /*public bool IsUseMultiLang
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
        }*/

        private void OnIsUseMultiLangValueChangedCallback()
        {
#if UNITY_EDITOR
            /*if (string.IsNullOrEmpty(this.mVedioName)) return;

            if (!this.mIsUseMultiLang)
            {
                this.Image.sprite = UIAtlasManager.Instance.LoadSprite_Editor(this.mVedioName);
            }*/
#endif
        }

        private string mImgColor;
        public string ImgColor
        {
            get
            {
                return this.mImgColor;
            }
            /*set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.Image.color = UtilTool.ToColor(value);
                    this.mImgColor = value;
                }
            }*/
        }

        private void Awake()
        {
            this.MediaPlayer = this.gameObject.ReceiveComponent<MediaPlayer>();

            /*
            if (this.mIsUseMultiLang)
            {
                if (string.IsNullOrEmpty(this.mVedioName))
                {
                    return;
                }
                this.Image.sprite = null;
            }*/ 
            if (this.MediaPlayer)
            {
                this.mVedioPath ="啥也不是";
            }
            else
            {
                this.mVedioPath = string.Empty;
            }
        }

        private void Start()
        {
            if (Application.isPlaying && this.mIsUseMultiLang)
            {
                // LocalizationManager.Instance.OnLocalize += this.OnLocalize;
                if (string.IsNullOrEmpty(this.mVedioPath))
                {
                    return;
                }
                // this.OnLocalize(LocalizationManager.CurLanguageType);
                /*if (this.IsNativeSize)
                {
                    this.Image.SetNativeSize();
                }*/
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
        /*private void OnValidate()
        {
           if(this.Image && this.Image.sprite)
                this.mSpriteName = this.Image.sprite.name;
        }*/
#endif

        public string VeDioPath
        {
            get 
            { 
                return this.mVedioPath; 
            }
            set
            {
                this.mVedioPath = value;
                if (!string.IsNullOrEmpty(this.mVedioPath))
                {
                    if (this.mVedioPath=="Null")
                    {
                        this.MediaPlayer.Stop();
                        // 释放视频资源
                        this.MediaPlayer.CloseMedia();
                    }
                    else
                    {
                        var rReturn = this.MediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder,
                            this.mVedioPath, true);
                        if (!rReturn)
                            LogManager.LogError($"检查!!!这路径没有找到对应的视频:{this.mVedioPath}");
                    }
                    
                }
            }
        }



        /*private void LoadSpriteByHttp(string rSpriteName,Image rImg)
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
        }*/
        
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
