using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knight.Core;
using UnityEngine.UI;
using System;

namespace UnityEngine.UI
{
    public enum LayoutGroupType
    {
        GridLayoutGroup,
        HorizontalLayoutGroup,
        VerticalLayoutGroup,
    }
    public enum AdaptScreenType
    {
        NormalScreen,
        AdaptWideScreen,
        AdaptHighScreen,
    }
    [Serializable]
    public struct AdaptPadding
    {
        public int left;
        public int right;
        public int top;
        public int bottom;
    }

    [ExecuteAlways]
    public class LayoutGroupAdapt : MonoBehaviour
    {
        private const float ReferenceWide = 1920f;
        private const float ReferenceHigh = 1080f;

        [HideInInspector]
        [SerializeField]
        public LayoutGroupType mAdaptLayoutGroupType;

        [HideInInspector]
        [SerializeField]
        public AdaptScreenType mAdaptScreenType = AdaptScreenType.NormalScreen;

        [HideInInspector]
        [SerializeField]
        public GridLayoutGroup mLayoutGroup;
        [HideInInspector]
        [SerializeField]
        public HorizontalLayoutGroup mHorizontalLayoutGroup;
        [HideInInspector]
        [SerializeField]
        public VerticalLayoutGroup mVerticalLayoutGroup;

        [SerializeField]
        public bool EnableSelfScreenAdapt = false;

        #region 宽屏适配

        [HideInInspector]
        [SerializeField]
        public AdaptPadding AdaptWidePadding;

        [HideInInspector]
        [SerializeField]
        public Vector2 AdaptWideSpacing;

        [HideInInspector]
        [SerializeField]
        [Range(0, 1)]
        public float AdaptWideFactor = 1f;

        #endregion

        #region 高屏适配

        [HideInInspector]
        [SerializeField]
        public AdaptPadding AdaptHighPadding;

        [HideInInspector]
        [SerializeField]
        public Vector2 AdaptHighSpacing;

        [HideInInspector]
        [SerializeField]
        [Range(0, 1)]
        public float AdaptHighFactor = 1f;

        [HideInInspector]
        [SerializeField]
        public AdaptPadding AdaptNormalPadding;

        [HideInInspector]
        [SerializeField]
        public Vector2 AdaptNormalSpacing;
        #endregion

        private Vector2 mScalerScreenSize
        {
            get
            {
                var rScreenSize = new Vector2(Screen.width, Screen.height);
                var rReferenceSize = new Vector2(ReferenceWide, ReferenceHigh);
                return new Vector2(rScreenSize.x * (rReferenceSize.y / rScreenSize.y), rReferenceSize.y);
            }
        }

        private RectTransform mRectTransform;

        private float mPreOffset = 0f;
        private Vector3 mPreScale = Vector3.one;

        protected void Start()
        {
            this.mRectTransform = this.GetComponent<RectTransform>();
            this.AdaptScreen();
        }

        //宽适配，适合4：3等超高屏幕分辨率
        private void AdaptScreen()
        {
            //当前屏幕宽高 1440:1080
            Vector2 rScreenSize = new Vector2(Screen.width, Screen.height);

            //1920:1080 = 1.777
            var rReferenveAspectRatio = (ReferenceWide / ReferenceHigh);
            // 当前屏幕宽高比
            var rScreenAspectRatio = rScreenSize.x / rScreenSize.y;

            if (rScreenAspectRatio < rReferenveAspectRatio)
            {
                //自己进行缩放
                if(this.EnableSelfScreenAdapt)
                {
                    this.mRectTransform.sizeDelta -= new Vector2(this.mPreOffset, 0);
                    this.mRectTransform.localScale = this.mPreScale;

                    //锁高后的长度
                    var rRectWide = this.mScalerScreenSize.x;
                    //适配宽度(1920)-锁高后的宽度
                    var WideOffset = ReferenceWide - rRectWide;
                    WideOffset = WideOffset < 0.001f ? 0 : WideOffset;

                    //存储上一分辨率下RectTransform信息
                    this.mPreOffset = WideOffset;

                    var rPreRectWide = this.mRectTransform.rect.size.x;
                    this.mRectTransform.sizeDelta += new Vector2(WideOffset, 0);
                    var rCurRectWide = this.mRectTransform.rect.size.x;

                    var rLocalScale = Vector3.one * (rPreRectWide / rCurRectWide);
                    this.mPreScale = rLocalScale;

                    this.mRectTransform.localScale = rLocalScale * this.AdaptHighFactor;
                }
                else
                {
                    this.mRectTransform.localScale = Vector3.one * this.AdaptHighFactor;
                }
                
                this.SetPaddingAndSpacing(this.AdaptHighPadding, this.AdaptHighSpacing);
            }
            else
            {
                this.mRectTransform.sizeDelta -= new Vector2(this.mPreOffset, 0);
                this.mPreOffset = 0;

                if (rScreenAspectRatio != rReferenveAspectRatio)
                {
                    this.mRectTransform.localScale = Vector3.one * this.AdaptWideFactor;
                    this.SetPaddingAndSpacing(this.AdaptWidePadding, this.AdaptWideSpacing);
                }
                else
                {
                    this.mRectTransform.localScale = Vector3.one;
                    this.SetPaddingAndSpacing(this.AdaptNormalPadding, this.AdaptNormalSpacing);
                }
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.mRectTransform);
        }

        
        public Vector2 CalSizeDeltaFormPadding(RectOffset rRectOffset)
        {
            return new Vector2(rRectOffset.left + rRectOffset.right, rRectOffset.top + rRectOffset.bottom);
        }

        private void SetPaddingAndSpacing(AdaptPadding rRectOffset, Vector2 rSpacing)
        {
            if(this.mAdaptLayoutGroupType == LayoutGroupType.GridLayoutGroup)
            {
                this.mLayoutGroup.padding.left = rRectOffset.left;
                this.mLayoutGroup.padding.right = rRectOffset.right;
                this.mLayoutGroup.padding.top = rRectOffset.top;
                this.mLayoutGroup.padding.bottom = rRectOffset.bottom;
                this.mLayoutGroup.spacing = rSpacing;
            }
            else if(this.mAdaptLayoutGroupType == LayoutGroupType.HorizontalLayoutGroup)
            {
                this.mHorizontalLayoutGroup.padding.left = rRectOffset.left;
                this.mHorizontalLayoutGroup.padding.right = rRectOffset.right;
                this.mHorizontalLayoutGroup.padding.top = rRectOffset.top;
                this.mHorizontalLayoutGroup.padding.bottom = rRectOffset.bottom;
                this.mHorizontalLayoutGroup.spacing = rSpacing.x;
            }
            else
            {
                this.mVerticalLayoutGroup.padding.left = rRectOffset.left;
                this.mVerticalLayoutGroup.padding.right = rRectOffset.right;
                this.mVerticalLayoutGroup.padding.top = rRectOffset.top;
                this.mVerticalLayoutGroup.padding.bottom = rRectOffset.bottom;
                this.mVerticalLayoutGroup.spacing = rSpacing.y;
            }
            
        }

        public void CopyPaddingAndSpacingValue(RectOffset rPadding,Vector2 rSpacing)
        {
            if(this.mAdaptScreenType == AdaptScreenType.NormalScreen)
            {
                this.AdaptNormalPadding.left = rPadding.left;
                this.AdaptNormalPadding.right = rPadding.right;
                this.AdaptNormalPadding.top = rPadding.top;
                this.AdaptNormalPadding.bottom = rPadding.bottom;
                this.AdaptNormalSpacing = rSpacing;
            }
            else if (this.mAdaptScreenType == AdaptScreenType.AdaptWideScreen)
            {
                this.AdaptWidePadding.left = rPadding.left;
                this.AdaptWidePadding.right = rPadding.right;
                this.AdaptWidePadding.top = rPadding.top;
                this.AdaptWidePadding.bottom = rPadding.bottom;
                this.AdaptWideSpacing = rSpacing;
            }
            else
            {
                this.AdaptHighPadding.left = rPadding.left;
                this.AdaptHighPadding.right = rPadding.right;
                this.AdaptHighPadding.top = rPadding.top;
                this.AdaptHighPadding.bottom = rPadding.bottom;
                this.AdaptHighSpacing = rSpacing;
            }
        }
        private void Update()
        {
#if UNITY_EDITOR
            this.AdaptScreen();
# endif
        }
    }

}