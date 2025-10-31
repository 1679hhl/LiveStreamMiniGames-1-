using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Knight.Core;

namespace UnityEngine.UI
{
    public class GlobalMessageBox : MonoBehaviour, UnityFx.Async.IAsync
    {
        private static GlobalMessageBox __instance;
        public static GlobalMessageBox Instance => __instance;

        public Button CancelButton;
        public Button ConfirmButton;

        public Text TitleText;
        public Text CancelButtonText;
        public Text ConfirmButtonText;
        public Text ContentText;

        public Animator PopupAnimator;
        public bool isDone => !this.gameObject.activeSelf;

        private void Awake()
        {
            __instance = this;
            if (this.PopupAnimator != null)
                this.PopupAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
            this.gameObject.SetActive(false);
        }

        public void Open(string rConfirmText, string rCancelText, string rContentText, Action rConfirmAction, Action rCancelAction)
        {
            this.TitleText.text = LocalizationManager.Instance.GetMultiLanguage_PreABLoader(10);
            this.gameObject.SetActive(true);
            this.ConfirmButtonText.text = rConfirmText;
            this.CancelButtonText.text = rCancelText;
            this.ContentText.text = rContentText;

            this.CancelButton.onClick.RemoveAllListeners();
            this.ConfirmButton.onClick.RemoveAllListeners();

            this.ConfirmButton.onClick.AddListener(() =>
            {
                rConfirmAction?.Invoke();
                this.gameObject.SetActive(false);
            });
            this.CancelButton.onClick.AddListener(() =>
            {
                rCancelAction?.Invoke();
                this.gameObject.SetActive(false);
            });
        }

        public void Open(string rConfirmText, string rContentText, Action rConfirmAction)
        {
            this.gameObject.SetActive(true);
            //this.TitleText.text = LocalizationManager.Instance.GetMultiLanguage_PreABLoader(10);
            this.ConfirmButtonText.text = rConfirmText;
            this.ContentText.text = rContentText;
            this.CancelButton.transform.gameObject.SetActive(false);

            this.CancelButton.onClick.RemoveAllListeners();
            this.ConfirmButton.onClick.RemoveAllListeners();

            this.ConfirmButton.onClick.AddListener(() =>
            {
                this.CancelButton.transform.gameObject.SetActive(true);
                rConfirmAction?.Invoke();
                this.gameObject.SetActive(false);
            });
        }

        public void Close()
        {
            this.CancelButton.onClick.RemoveAllListeners();
            this.ConfirmButton.onClick.RemoveAllListeners();
            this.gameObject.SetActive(false);
        }
        // public Task<bool> OpenAsync(string rConfirmText, string rContentText, Action rConfirmAction)
        // {
        //     var rTCS = new TaskCompletionSource<bool>();
        //     this.Open(rConfirmText,rContentText, () =>
        //     {
        //         rTCS.SetResult(true);
        //         rConfirmAction?.Invoke();
        //     });
        //     return rTCS.Task;
        // }
        public Task<bool> OpenAsync(string rConfirmText, string rCancelText, string rContentText, Action rConfirmAction, Action rCancelAction)
        {
            var rTCS = new TaskCompletionSource<bool>();
            this.OpenForAsync(rConfirmText,rCancelText,rContentText, () =>
            {
                rConfirmAction?.Invoke();
                this.gameObject.SetActive(false);
                rTCS.SetResult(true);
            }, () =>
            {
                rCancelAction?.Invoke();
                this.gameObject.SetActive(false);
                rTCS.SetResult(false);
            });
            return rTCS.Task;
        }
        
        public void OpenForAsync(string rConfirmText, string rCancelText, string rContentText, Action rConfirmAction, Action rCancelAction)
        {
            this.gameObject.SetActive(true);
            this.TitleText.text = LocalizationManager.Instance.GetMultiLanguage_PreABLoader(10);
            this.ConfirmButtonText.text = rConfirmText;
            this.CancelButtonText.text = rCancelText;
            this.ContentText.text = rContentText;

            this.CancelButton.onClick.RemoveAllListeners();
            this.ConfirmButton.onClick.RemoveAllListeners();

            this.ConfirmButton.onClick.AddListener(() =>
            {
                rConfirmAction?.Invoke();
            });
            this.CancelButton.onClick.AddListener(() =>
            {
                rCancelAction?.Invoke();
            });
        }
    }
}
