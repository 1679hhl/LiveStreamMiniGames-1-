using Game;
using Knight.Core;
using NaughtyAttributes;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TMP_Text))]
    [ExecuteInEditMode]
    public class TextMaterialReplace : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private string mMaterialPath;
        [SerializeField]
        [ReadOnly]
        private MutilLanguageType mCurLanguageType;
        public TMP_Text TMP_Text;
        public bool IsNativeSize;
        public string PrefabName;

        [SerializeField][OnValueChanged("OnIsUseMultiLangValueChangedCallback")]
        private bool mIsUseMultiLang;
        

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
            this.TMP_Text = this.gameObject.ReceiveComponent<TMP_Text>();
            if (this.TMP_Text)
            {
                this.mMaterialPath ="啥也不是";
            }
            else
            {
                this.mMaterialPath = string.Empty;
            }
        }

        private void Start()
        {
            if (Application.isPlaying && this.mIsUseMultiLang)
            {
                // LocalizationManager.Instance.OnLocalize += this.OnLocalize;
                if (string.IsNullOrEmpty(this.mMaterialPath))
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

        public string TextMaterial
        {
            get 
            { 
                return this.mMaterialPath; 
            }
            set
            {
                this.mMaterialPath = value;
                if (!string.IsNullOrEmpty(this.mMaterialPath))
                {
                    var rMaterial = Addressables.LoadAssetAsync<Material>(this.mMaterialPath);
                    rMaterial.WaitForCompletion();
                    if (rMaterial.Status == AsyncOperationStatus.Succeeded)
                    {
                        this.TMP_Text.fontSharedMaterial = rMaterial.Result;
                        Addressables.Release(rMaterial);
                    }
                }
            }
        }
    }
}
