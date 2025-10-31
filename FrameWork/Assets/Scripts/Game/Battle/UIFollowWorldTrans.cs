using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollowWorldTrans : MonoBehaviour
{
    public Transform WorldTarget;
    public Vector3 Offset;

    private Camera BattleCmr;

    private bool mIsUI;

    private RectTransform mRectTransform;

    private bool IsMove;
    //private RectTransform CanvasRectTrans;
    void Start()
    {
    }

    public void Init(Transform rTarget, Vector3 rOffset, RectTransform rectTransform,Vector3 rScale,bool rIsMove=true,bool rIsUi = true)
    {
        this.mRectTransform = rectTransform;
        this.mIsUI = rIsUi;
        this.IsMove = rIsMove;
        this.WorldTarget = rTarget;
        this.Offset = rOffset;
        //this.CanvasRectTrans = UIRoot.Instance.UICamera;
        this.transform.position = Vector3.zero;
        RectTransform rRectTransform = this.transform.GetComponent<RectTransform>();
        Vector3 position = rRectTransform.localPosition;
        position.z = 0;
        rRectTransform.localPosition = position;
        rRectTransform.anchoredPosition = Vector2.one * 10000f;
        BattleCmr = GameObject.Find("Main Camera").GetComponent<Camera>();
        rRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rRectTransform.localScale=rScale;
        SrtPosition();
    }
    
    public void Init(Transform rTarget, Vector3 rOffset, RectTransform rectTransform,bool rIsMove=true,bool rIsUi = true)
    {
        this.mRectTransform = rectTransform;
        this.mIsUI = rIsUi;
        this.IsMove = rIsMove;
        this.WorldTarget = rTarget;
        this.Offset = rOffset;
        //this.CanvasRectTrans = UIRoot.Instance.UICamera;
        this.transform.position = Vector3.zero;
        RectTransform rRectTransform = this.transform.GetComponent<RectTransform>();
        Vector3 position = rRectTransform.localPosition;
        position.z = 0;
        rRectTransform.localPosition = position;
        rRectTransform.anchoredPosition = Vector2.one * 10000f;
        BattleCmr = GameObject.Find("Main Camera").GetComponent<Camera>();
        rRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rRectTransform.localScale=Vector3.one;
        SrtPosition();
    }

    public void Update()
    {
        if (!IsMove)return;
        SrtPosition();
    }

    public void SrtPosition()
    {
        if (WorldTarget != null)
        {
            Vector2 screenPoint =
                RectTransformUtility.WorldToScreenPoint(BattleCmr, WorldTarget.position + this.Offset); // 先将3D坐标转换成屏幕坐标
            Vector2 localPoint; // 再将屏幕坐标转换成UGUI坐标
            if (!mIsUI)//ui相机
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTransform, screenPoint,
                        UIRoot.Instance.UICamera, out localPoint))
                {
                    //这里的camera可以是null 如果canvas是Screen Space-Overlay渲染模式的话
                    //如果是Screen Space-Camera模式的话 就是对应拖到Canvas上的相机
                    // canvasTransform 是(hp的父节点或者是canvas) hp.parent.GetComponent<RectTransform>()
                    localPoint.x += 125;
                    localPoint.y -= 210;
                    if (localPoint.x > 240)
                        localPoint.x = 190;
                    if (localPoint.y < -675)
                        localPoint.y = -675;
                    this.transform.GetComponent<RectTransform>().anchoredPosition = localPoint;
                }
            }

            if (mIsUI)//主摄像机
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mRectTransform, screenPoint, UIRoot.Instance.UICamera,
                        out localPoint))
                {
                    //这里的camera可以是null 如果canvas是Screen Space-Overlay渲染模式的话
                    //如果是Screen Space-Camera模式的话 就是对应拖到Canvas上的相机
                    // canvasTransform 是(hp的父节点或者是canvas) hp.parent.GetComponent<RectTransform>()
                    var rRcetTransForm = this.transform.GetComponent<RectTransform>();
                    this.transform.GetComponent<RectTransform>().anchoredPosition = localPoint;
                }
            }
        }
        else
        {
            GameObject.Destroy(this);
        }
    }
}