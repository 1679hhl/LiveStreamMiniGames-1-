using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SpineListenr : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    private bool isSubscribed = false; // 记录是否已经订阅事件，防止重复订阅
    
    private void Awake()
    {
        if (this.skeletonAnimation==null)
        {
            this.skeletonAnimation=this.gameObject.GetComponent<SkeletonAnimation>();
        }
    }

    private void SubscribeEvent()
    {
        if (skeletonAnimation != null && !isSubscribed)
        {
            skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
            isSubscribed = true;
        }
    }

    private void UnsubscribeEvent()
    {
        if (skeletonAnimation != null && isSubscribed)
        {
            skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;
            isSubscribed = false;
        }
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (gameObject.activeSelf)
        {
            // **回收前先取消事件监听**
            UnsubscribeEvent();

            // 放回对象池
            BattleManager.Instance.PoolManager.Free(this.gameObject.name, this.gameObject);
        }
    }

    private void OnDisable()
    {
        // **当对象被禁用（放回对象池）时，取消订阅**
        UnsubscribeEvent();
    }

    private void OnEnable()
    {
        // **当对象被重新取出使用时，重新订阅事件**
        SubscribeEvent();
    }

    private void OnDestroy()
    {
        // **对象真正销毁时，确保取消订阅**
        UnsubscribeEvent();
    }
}
