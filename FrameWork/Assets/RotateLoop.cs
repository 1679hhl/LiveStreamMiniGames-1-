using System;
using UnityEngine;
using DG.Tweening;

public class RotateLoop : MonoBehaviour
{
    public RectTransform mRectTransform;

    void Start()
    {
        // 假设你想让 RectTransform 旋转一圈（360°），并且每 1 秒旋转一次
        RotateLoopAnimation();
    }
    
    void RotateLoopAnimation()
    {
        // 使用 DOTween 来进行旋转，循环播放
        mRectTransform.DORotate(new Vector3(0, 0, -360), 15f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)  // 设置无限循环，Restart 是每次都从头开始
            .SetEase(Ease.Linear);           // 让旋转过程保持线性平滑
    }
}