using UnityEngine;
using Knight.Core;
using UnityEngine.UI;

namespace Game
{
    public class LoadingView_Knight : MonoBehaviour, ILoadingView
    {
        private static LoadingView_Knight __instance;
        public static LoadingView_Knight Instance { get { return __instance; } }

        public bool IsInLoading { get; set; }
        public LoadingViewStyle LoadingViewStyle { get; set; }

        /// <summary>
        /// 登录背景节点
        /// </summary>
        public GameObject BackgroundGo;
        [HideInInspector]
        public GrayGraphics BtnSwitchHeroGrayGraphics;

        /// <summary>
        /// 背景图片，可能有需要切换的情况
        /// </summary>
        public RawImage Background;
        /// <summary>
        /// 加载时候的一些文字提示
        /// </summary>
        public Text TextTips;

        /// <summary>
        /// 加载时候的一些辅助文字提示
        /// </summary>
        public Text TextTips2;
        /// <summary>
        /// 加载进度显示
        /// </summary>
        public Text TextProgress;
        /// <summary>
        /// 版本号信息
        /// </summary>
        public Text VersionText;
        /// <summary>
        /// 加载信息
        /// </summary>
        public GameObject LoadingInfo;
        public Canvas Canvas;
        /// <summary>
        /// 当前进度条
        /// </summary>
        private float mCurLoadingProgress;
        /// <summary>
        /// 当前目标进度条
        /// </summary>
        private float mCurTargetLoadingProgress;
        /// <summary>
        /// 当前进度条速度
        /// </summary>
        private float mProgressSpeed;

        private GraphicRaycaster mGraphicRaycaster;

        void Awake()
        {
            if (__instance == null)
            {
                __instance = this;
                this.LoadingInfo.SetActive(false);
                this.BackgroundGo.SetActive(false);
                this.Background.gameObject.SetActive(false);
            }
            this.Canvas = this.gameObject.ReceiveComponent<Canvas>();
            this.gameObject.ReceiveComponent<GraphicRaycaster>();
            this.Canvas.overrideSorting = true;
            this.Canvas.sortingLayerName = "Default";
            this.Canvas.sortingOrder = 10000;

            this.mGraphicRaycaster = this.GetComponent<GraphicRaycaster>();
            this.VersionText.gameObject.SetActive(false);
            this.SetLoadingTips(string.Empty);
        }

        public void Update()
        {
            if (!this.IsInLoading) return;

            if (this.mCurLoadingProgress <= this.mCurTargetLoadingProgress * 0.95f)
            {
                this.mCurLoadingProgress += this.mProgressSpeed * Time.deltaTime;
            }
            else
            {
                this.mCurLoadingProgress = this.mCurTargetLoadingProgress;
            }

            this.mCurLoadingProgress = Mathf.Min(this.mCurLoadingProgress, 1.0f);
            if (this.TextProgress)
            {
                var nValue = Mathf.CeilToInt(this.mCurLoadingProgress * 100);
                this.TextProgress.text = nValue + "%";
            }
        }
        public void SetVersionTextVisible()
        {
            // this.VersionText.text = LocalizationManager.Instance.GetMultiLanguage_PreABLoader(16, GameVersion.Instance.GetShowVersion() + " " + GameVersion.Instance.GetShowVersionExtraInfo());
            // this.VersionText.gameObject.SetActive(true);
        }
        public void SetTips(string rTips, string rTips2 = "")
        {
            if (this.TextTips)
                this.TextTips.text = rTips;
            if (this.TextTips2)
                this.TextTips2.text = rTips2;
        }

        public void SetLoadingProgress(float fValue)
        {
            this.mCurLoadingProgress = fValue;
        }

        /// <summary>
        /// 开始出现加载界面
        /// </summary>
        public void ShowLoading(float fIntervalTime, float fTargetProgress, string rTextTips = "", LoadingViewStyle rLoadingViewStyle = LoadingViewStyle.Mutil, bool bIsReset = true,bool bIsShowTips = true)
        {
            LogManager.Log($"[LoadingView_Knight] ShowLoading rLoadingViewStyle = {rLoadingViewStyle} {fTargetProgress} {rTextTips}");

            this.LoadingViewStyle = rLoadingViewStyle;
            this.IsInLoading = true;
            this.BackgroundGo.SetActive(true);
            this.LoadingInfo.SetActive(true);
            this.Canvas.overrideSorting = true;
            this.Canvas.sortingLayerName = "Default";
            this.Canvas.sortingOrder = 10000;
            this.SetTips(rTextTips);
            if (bIsReset)
            {
                this.mCurLoadingProgress = 0;
            }
            this.mCurTargetLoadingProgress = fTargetProgress;
            this.mProgressSpeed = (this.mCurTargetLoadingProgress - this.mCurLoadingProgress) / fIntervalTime;
            this.SetLoadingProgress(this.mCurLoadingProgress);
            this.mGraphicRaycaster.enabled = true;
        }

        /// <summary>
        /// 开始出现加载界面
        /// </summary>
        public void ShowLoading(float fTargetProgress, string rTextTips, LoadingViewStyle rLoadingViewStyle = LoadingViewStyle.Mutil,bool bIsShowTips = true)
        {
            LogManager.Log($"[LoadingView_Knight] ShowLoading rLoadingViewStyle = {rLoadingViewStyle} {fTargetProgress} {rTextTips}");

            this.LoadingViewStyle = rLoadingViewStyle;
            this.IsInLoading = true;
            this.BackgroundGo.SetActive(true);
            this.LoadingInfo.SetActive(true);
            this.Canvas.overrideSorting = true;
            this.Canvas.sortingLayerName = "Default";
            this.Canvas.sortingOrder = 10000;
            this.SetTips(rTextTips);
            this.mProgressSpeed = 0;
            this.mCurTargetLoadingProgress = fTargetProgress;
            this.SetLoadingProgress(0);
            this.mGraphicRaycaster.enabled = true;
        }

        /// <summary>
        /// 加载界面
        /// </summary>
        public void HideLoading()
        {
            this.StopCoroutine("LoadingProgress");
            this.IsInLoading = false;
            this.LoadingInfo.SetActive(false);
            this.SetTips("");
            this.mGraphicRaycaster.enabled = false;
            this.SetLoadingTips(string.Empty);
        }

        /// <summary>
        /// 显示或隐藏Loadinginfo节点
        /// </summary>
        public void ShowHideLoadingInfo(bool bActive)
        {
            this.LoadingInfo.gameObject.SetActive(bActive);
        }

        public void HideLoadingImmediate()
        {
            this.IsInLoading = false;
            this.LoadingInfo.SetActive(false);
            this.SetTips("");
            this.SetLoadingTips(string.Empty);
        }

        public void HideBottomInfo()
        {
            this.Background.gameObject.SetActive(false);
            this.LoadingInfo.SetActive(false);
        }

        public void HideBackground()
        {
            this.Background.gameObject.SetActive(false);
            this.BackgroundGo.SetActive(false);
            LogManager.Log($"[LoadingView_Knight] HideBackground");
        }

        public void ShowBackground()
        {
            this.BackgroundGo.SetActive(this.LoadingViewStyle == LoadingViewStyle.Mutil);
            this.Background.gameObject.SetActive(this.LoadingViewStyle == LoadingViewStyle.Single);
            LogManager.Log($"[LoadingView_Knight] ShowBackground rLoadingViewStyle = {this.LoadingViewStyle}");
        }

        public void SetBackgroundLayer()
        {
            this.Canvas.overrideSorting = true;
            this.Canvas.sortingLayerName = "Default";
            this.Canvas.sortingOrder = -1;
        }

        public void SetTopLeftActive(bool rActive)
        {
        }

        public void SetLoadingTips(string rTips)
        {
        }
    }
}
