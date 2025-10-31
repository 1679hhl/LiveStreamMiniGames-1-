//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.Collections;
using Knight.Core;

namespace UnityEngine.UI
{
    // Toast逻辑渲染分离接口
    public interface IToast
    {
        void Show(string rTextTip);
        void ShowNoFunc();
        void ShowJExpToast(string rTextTip);
        void ShowBlackToast(string rTextTip);
        void ShowTaskToast(string rTextTip);
        void ShowChooseHeroToast(string rTextTip);
        void HideToast();
    }

    public class Toast : MonoBehaviour, IToast
    {
        protected static IToast         __instance;
        public  static IToast           Instance { get { return __instance; } }

        public GameObject               TipsWhite;
        public Text                     TipText;
        public GameObject               TipsBlack;
        public Text                     TipBlackText;
        [Header("J-Level Exp")]
        public GameObject               TipJExp;
        public Text                     TipJExpText;
        public CanvasGroup              TipGroup;
        public GameObject               TipsTask;
        public Text                     TipsTaskText;
        public GameObject               TipsChooseHero;
        public Text                     TipsChooseHeroText;


        private CoroutineHandler        mCoroutineHandler;
        private static float            mShowTimer = 1.1f;
        private static float            mDisappearTimer = 0.4f;
        public static string            NoFuncTips = "功能暂未开放，敬请期待";
        void Awake()
        {
            if (__instance == null)
            {
                __instance = this;
                this.gameObject.SetActive(false);
            }
        }

        public void ShowNoFunc()
        {
            this.Show(Toast.NoFuncTips);
        }
        public void Show(string rTextTip)
        {
            this.gameObject.SetActive(true);
            this.TipsWhite.SetActive(true);
            this.TipsBlack.SetActive(false);
            this.TipJExp.SetActive(false);
            this.TipsTask.SetActive(false);
            this.TipsChooseHero.SetActive(false);
            this.TipText.text = rTextTip;
            if (this.mCoroutineHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mCoroutineHandler);
                this.mCoroutineHandler = null;
            }
            this.mCoroutineHandler = CoroutineManager.Instance.StartHandler(this.StartAnim(rTextTip, Toast.mShowTimer,Toast.mDisappearTimer));
        }

        public void ShowBlackToast(string rTextTip)
        {
            this.gameObject.SetActive(true);
            this.TipsWhite.SetActive(false);
            this.TipsBlack.SetActive(true);
            this.TipJExp.SetActive(false);
            this.TipsTask.SetActive(false);
            this.TipsChooseHero.SetActive(false);
            this.TipBlackText.text = rTextTip;
            if (this.mCoroutineHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mCoroutineHandler);
                this.mCoroutineHandler = null;
            }
            this.mCoroutineHandler = CoroutineManager.Instance.StartHandler(this.StartAnim(rTextTip, Toast.mShowTimer, Toast.mDisappearTimer));
        }

        public void ShowJExpToast(string rTextTip)
        {
            this.gameObject.SetActive(true);
            this.TipsWhite.SetActive(false);
            this.TipsBlack.SetActive(false);
            this.TipJExp.SetActive(true);
            this.TipsTask.SetActive(false);
            this.TipsChooseHero.SetActive(false);
            this.TipJExpText.text = $"获得J-LEVEL值 <color=#ffc100>{rTextTip}</color>";
            if (this.mCoroutineHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mCoroutineHandler);
                this.mCoroutineHandler = null;
            }
            this.mCoroutineHandler = CoroutineManager.Instance.StartHandler(this.StartAnim(rTextTip, Toast.mShowTimer, Toast.mDisappearTimer));
        }

        public void ShowTaskToast(string rTextTip)
        {
            this.gameObject.SetActive(true);
            this.TipsWhite.SetActive(false);
            this.TipsBlack.SetActive(false);
            this.TipJExp.SetActive(false);
            this.TipsTask.SetActive(true);
            this.TipsChooseHero.SetActive(false);
            this.TipsTaskText.text = rTextTip;
            if (this.mCoroutineHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mCoroutineHandler);
                this.mCoroutineHandler = null;
            }
            this.mCoroutineHandler = CoroutineManager.Instance.StartHandler(this.StartAnim(rTextTip, 2, Toast.mDisappearTimer));
        }

        public void ShowChooseHeroToast(string rTextTip)
        {
            this.gameObject.SetActive(true);
            this.TipsWhite.SetActive(false);
            this.TipsBlack.SetActive(false);
            this.TipJExp.SetActive(false);
            this.TipsTask.SetActive(false);
            this.TipsChooseHero.SetActive(true);
            this.TipsChooseHeroText.text = rTextTip;
            if (this.mCoroutineHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mCoroutineHandler);
                this.mCoroutineHandler = null;
            }
            this.mCoroutineHandler = CoroutineManager.Instance.StartHandler(this.StartAnim(rTextTip, 4, Toast.mDisappearTimer));
        }

        public void HideToast()
        {
            this.Hide();
        }

        public void Hide()
        {
            if(this.gameObject.activeSelf == true)
            {
                this.gameObject.SetActive(false);
            }

        }

        private IEnumerator StartAnim(string rTextTip, float rShowTimer,float rDisappearTimer)
        {
            this.gameObject.SetActive(true);
            this.TipText.text = rTextTip;
            this.TipGroup.alpha = 1;

            yield return new WaitForSeconds(rShowTimer);

            float rCurTime = 0.0f;
            yield return new WaitUntil(() =>
            {
                this.TipGroup.alpha = Mathf.Lerp(1, 0, Mathf.InverseLerp(0, rDisappearTimer, rCurTime));
                rCurTime += Time.deltaTime;
                return rCurTime >= rDisappearTimer;
            });
            this.TipGroup.alpha = 0;
            this.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            __instance = null;
        }
    }
}
