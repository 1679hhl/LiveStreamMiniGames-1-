using Knight.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    // UIWait逻辑渲染分离接口
    public interface IUIWait
    {
        bool IsWaiting { get; }
        void StartWait(long uiWaitCode, float rWaitTime = 3f);
        void ShowWaitText(string rWaitTips);
        void HideWaitText();
        void EndWait(long uiWaitCode);
        void RemoveAll();
        bool ContainsKey(long uiWaitCode);
    }

    public class UIWait : MonoBehaviour, IUIWait
    {
        protected static IUIWait __instance = null;
        public static IUIWait Instance { get { return __instance; } }

        public GameObject WaitPanelGO = null;
        public GameObject WaitPanelTextGO;
        public Text TxtWaitTips = null;
        public float MinWaitTime = 1;        // 等几秒钟开始弹Wait的UI，锁死界面
        public float CurWaitTime = 0.0f;

        public Dictionary<long, int> WaitCodeDict = new Dictionary<long, int>();

        private float WaitTime = 1f;
        private List<Transform> mVisuals = new List<Transform>();

        public bool IsWaiting { get { return this.WaitCodeDict.Count > 0; } }
        public Canvas Canvas;

        private GraphicRaycaster mGraphicRaycaster;
        private const int mSortOrder = 10050;
        public void Awake()
        {
            if (__instance == null)
            {
                __instance = this;
                var c = this.WaitPanelGO.transform.GetChild(0);
                this.mVisuals.Add(c);
                //this.mVisuals.Add(c.Find("ImgWait"));
                //this.mVisuals.Add(c.Find("TxtWaitTips"));
                //this.mVisuals.Add(c.Find("ImgWaitBG"));
                //this.mVisuals.Add(c.Find("ImgWaitFont"));
                this.WaitPanelGO.SetActive(false);
                this.WaitPanelTextGO.SetActive(false);
                this.WaitCodeDict.Clear();
            }
            this.Canvas = this.gameObject.ReceiveComponent<Canvas>();
            this.mGraphicRaycaster = this.gameObject.ReceiveComponent<GraphicRaycaster>();
            this.mGraphicRaycaster.enabled = false;
            this.Canvas.overrideSorting = true;
            this.Canvas.sortingLayerName = "Default";
            this.Canvas.sortingOrder = mSortOrder;
        }

        private void ShowVisual(bool bVisible)
        {
            foreach (var v in this.mVisuals)
            {
                if (v)
                {
                    v.gameObject.SetActive(bVisible);
                }
            }
        }

        public void Update()
        {
            var bActive = this.WaitCodeDict.Count > 0;
            if (!bActive)
            {
                this.WaitPanelGO.SetActive(false);
                this.WaitPanelTextGO.SetActive(false);
                this.ShowVisual(false);
                this.CurWaitTime = 0.0f;
                return;
            }
            if (this.CurWaitTime >= this.WaitTime)
            {
                this.WaitPanelGO.SetActive(true);
                this.ShowVisual(true);
                this.Canvas.overrideSorting = true;
                this.Canvas.sortingLayerName = "Default";
                this.Canvas.sortingOrder = mSortOrder;
            }
            this.CurWaitTime += Time.unscaledDeltaTime;
        }

        public void StartWait(long uiWaitCode, float rWaitTime = 3f)
        {
            this.mGraphicRaycaster.enabled = true;
            if (this.WaitCodeDict.TryGetValue(uiWaitCode, out var nCount))
            {
                this.WaitCodeDict[uiWaitCode] = nCount + 1;
            }
            else
            {
                this.WaitCodeDict[uiWaitCode] = 1;
            }
            this.CurWaitTime = 0.0f;
            this.WaitTime = Mathf.Max(0, rWaitTime);
            //但是延迟显示
            if (!this.WaitPanelTextGO.activeSelf)
                this.ShowVisual(false);
            //阻挡输入
            this.WaitPanelGO.SetActive(true);
            this.Canvas.overrideSorting = true;
            this.Canvas.sortingLayerName = "Default";
            this.Canvas.sortingOrder = mSortOrder;

            Knight.Core.LogManager.Log($"StartWait....{uiWaitCode} LastCount:{this.WaitCodeDict.Count}");
        }

        public void EndWait(long uiWaitCode)
        {
            if (this.WaitCodeDict.TryGetValue(uiWaitCode, out var nCount))
            {
                nCount--;
                if (nCount <= 0)
                {
                    this.WaitCodeDict.Remove(uiWaitCode);
                }
                else
                {
                    this.WaitCodeDict[uiWaitCode] = nCount;
                }
            }
            else
            {
                if(this.WaitCodeDict.Count <= 0)
                {
                    this.mGraphicRaycaster.enabled = false;
                }
                return;
            }
            if(this.WaitCodeDict.Count <= 0)
            {
                this.mGraphicRaycaster.enabled = false;
            }
            Knight.Core.LogManager.Log($"EndWait....{uiWaitCode} LastCount:{this.WaitCodeDict.Count}");
        }

        public void RemoveAll()
        {
            this.WaitCodeDict.Clear();
        }

        private void OnDestroy()
        {
            __instance = null;
        }

        public void ShowWaitText(string rWaitTips)
        {
            this.ShowVisual(true);
            this.WaitPanelTextGO.SetActive(true);
            this.TxtWaitTips.text = rWaitTips;
        }

        public void HideWaitText()
        {
            this.WaitPanelTextGO.SetActive(false);
        }

        public bool ContainsKey(long uiWaitCode)
        {
            return this.WaitCodeDict.ContainsKey(uiWaitCode);
        }
    }
}
