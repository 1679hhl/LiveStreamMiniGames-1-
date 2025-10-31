using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways][DisallowMultipleComponent]
public class ScreenAdapt_UnSafeArea : MonoBehaviour
{
    private const float MaxWide = 2400f;
    private const float MaxHigh = 1080f;
    private const float ReferenceWide = 1920f;
    private const float ReferenceHigh = 1080f;

    private RectTransform mRectTransform;
    private Vector3 mPreScale = Vector3.one;

    private Vector2 mScalerScreenSize
    {
        get
        {
            var rScreenSize = new Vector2(Screen.width, Screen.height);
            var rReferenceSize = new Vector2(ReferenceWide, ReferenceHigh);
            return new Vector2(rScreenSize.x * (rReferenceSize.y / rScreenSize.y), rReferenceSize.y);
        }
    }

    void Start()
    {
        this.mRectTransform = this.GetComponent<RectTransform>();
        this.AdaptScreen();
    }

    void Update()
    {
# if UNITY_EDITOR
        this.AdaptScreen();
#endif
    }

    private void AdaptScreen()
    {
        //当前屏幕宽高
        Vector2 rScreenSize = new Vector2(Screen.width, Screen.height);

        var rMaxAspectRatio = (MaxWide / MaxHigh);
        // 当前屏幕宽高比
        var rScreenAspectRatio = rScreenSize.x / rScreenSize.y;

        //在指定分辨率范围内做裁切，超出指定分辨率做缩放
        if(rScreenAspectRatio > rMaxAspectRatio)
        {
            this.mRectTransform.localScale = this.mPreScale;

            var rRectWide = this.mScalerScreenSize.x;
            this.mRectTransform.localScale = Vector3.one * (rRectWide / MaxWide);
            this.mPreScale = this.mRectTransform.localScale;

        }
        else
        {
            this.mRectTransform.localScale = Vector3.one;
            this.mPreScale = Vector3.one;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.mRectTransform);
    }
}
