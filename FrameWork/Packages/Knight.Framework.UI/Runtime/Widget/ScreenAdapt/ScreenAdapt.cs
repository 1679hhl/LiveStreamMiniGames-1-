using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways][DisallowMultipleComponent]
public class ScreenAdapt : MonoBehaviour
{
    private const float ReferenceWide = 1080f;
    private const float ReferenceHigh = 1920f;

    private RectTransform mRectTransform;
    private Vector2 mPreOffset;
    //[Tooltip("锚点模式false  锚框模式true")]
    private bool ApplyModifyRectArea = true;

    private Vector2 mScalerScreenSize
    {
        get
        {
            var rScreenSize = new Vector2(Screen.width, Screen.height);
            var rReferenceSize = new Vector2(ReferenceWide, ReferenceHigh);
            return new Vector2(rScreenSize.x * (rReferenceSize.y / rScreenSize.y), rReferenceSize.y);
        }
    }

    private Vector2 mScalerScreenSize_FixHight
    {
        get
        {
            var rScreenSize = new Vector2(Screen.width, Screen.height);
            var rReferenceSize = new Vector2(ReferenceWide, ReferenceHigh);
            return new Vector2(rReferenceSize.x, rScreenSize.y * (rReferenceSize.x / rScreenSize.x));
        }
    }

    void Start()
    {
        this.mRectTransform = this.GetComponent<RectTransform>();
        this.ApplyModifyRectArea = !(this.mRectTransform.anchorMin == this.mRectTransform.anchorMax);
        this.AdaptScreen();
    }

    void Update()
    {
# if UNITY_EDITOR
        this.ApplyModifyRectArea = !(this.mRectTransform.anchorMin == this.mRectTransform.anchorMax);
        this.AdaptScreen();
#endif
    }

    public void ManualAdaptScreen()
    {
        this.Start();
    }
    private void AdaptScreen()
    {
        // 当前屏幕宽高
        Vector2 rScreenSize = new Vector2(Screen.width, Screen.height);

        // 默认屏幕宽高比 1920:1080 = 1.777
        var rReferenveAspectRatio = (ReferenceWide / ReferenceHigh);
        // 当前屏幕宽高比
        var rScreenAspectRatio = rScreenSize.x / rScreenSize.y;

        if (rScreenAspectRatio < rReferenveAspectRatio)
        {
            this.mRectTransform.sizeDelta -= this.mPreOffset;
            //this.mRectTransform.localScale = this.mPreScale;

            //锁高后的长度
            var rRectWide = this.mScalerScreenSize.x;
            //锁宽后的高度
            var rRectHigh = this.mScalerScreenSize_FixHight.y;

            //适配宽度(1920)-锁高后的宽度
            var WideOffset = ReferenceWide - rRectWide;
            WideOffset = WideOffset < 0.001f ? 0 : WideOffset;
            //锁宽后的高度-适配高度(1080)
            var HighOffset = rRectHigh - ReferenceHigh;

            //锚点模式下
            if (!this.ApplyModifyRectArea)
            {
                this.mRectTransform.localScale = Vector3.one * (rRectWide / ReferenceWide) ;
                this.mPreOffset = Vector2.zero;
            }
            //锚框模式下
            else
            {
                this.mPreOffset = new Vector2(WideOffset, HighOffset);
                this.mRectTransform.sizeDelta += this.mPreOffset;
                this.mRectTransform.localScale = Vector3.one * (rRectWide / ReferenceWide);
                
            }
        }
        else
        {
            this.mRectTransform.sizeDelta -= this.mPreOffset;
            this.mPreOffset = Vector2.zero;
            this.mRectTransform.localScale = Vector3.one;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.mRectTransform);
    }
}
