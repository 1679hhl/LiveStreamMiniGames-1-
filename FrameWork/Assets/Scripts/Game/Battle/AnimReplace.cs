using Game;
using Knight.Core;
using NaughtyAttributes;
using UnityEngine.AddressableAssets;

namespace UnityEngine.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    [ExecuteInEditMode]
    public class AnimReplace : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private string mAnimPath;
        [SerializeField]
        [ReadOnly]
        private MutilLanguageType mCurLanguageType;
        public Animation Animation;
        public bool IsNativeSize;
        /*public string PrefabName;*/

        [SerializeField][OnValueChanged("OnIsUseMultiLangValueChangedCallback")]
        private bool mIsUseMultiLang;

        public string BianHao { get; set; }


        private void Awake()
        {
            this.Animation = this.gameObject.ReceiveComponent<Animation>();
            
            if (this.Animation)
            {
                this.mAnimPath ="啥也不是";
            }
            else
            {
                this.mAnimPath = string.Empty;
            }
        }

        private void Start()
        {
            if (Application.isPlaying && this.mIsUseMultiLang)
            {
                if (string.IsNullOrEmpty(this.mAnimPath))
                {
                    return;
                }
            }
        }

        public void BoFangOver()
        {
            /*this.Animation.Stop();
            this.gameObject.SetActive(false);*/
            /*EventManager.Instance.Distribute(GameEvent.KeventTaskLinQU,this.BianHao);*/
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

        public string AnimPath
        {
            get 
            { 
                return this.mAnimPath; 
            }
            set
            {
                this.mAnimPath = value;
                if (this.mAnimPath=="Null")
                {
                    this.gameObject.SetActive(false);
                    this.Animation.Stop();
                }
                else if (!string.IsNullOrEmpty(this.mAnimPath))
                {
                    this.gameObject.SetActive(true);
                    this.Animation.Play();
                }
            }
        }
    }
}
