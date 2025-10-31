using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityFx.Async.Extensions;
using NaughtyAttributes;
using Knight.Core;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Animator))]
    public class AnimationCell : MonoBehaviour
    {
        [SerializeField] private Animator CellAnimator;
        [SerializeField] private GameObject Content;

        [ReadOnly][SerializeField] private bool mCanRun = true;
        private CoroutineHandler mHandler = null;

        private AnimationClip mClip;
        private void Awake()
        {
            var rClips = this.CellAnimator.runtimeAnimatorController?.animationClips;
            if (rClips != null)
            {
                foreach (var item in rClips)
                {
                    if (item.name.EndsWith("_Cell"))
                        this.mClip = item;
                }
            }
            this.Content.SetActive(this.mClip == null);

        }

        public void PlayAnimation(bool bEnable, float rDelayTime = 0f)
        {
            if(this.CellAnimator.runtimeAnimatorController == null)
            {
                this.Content.SetActive(true);
                LogManager.LogWarning($"{this.gameObject.name} Animator Controller is null");
                return;
            }
            if (this.mHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mHandler);
                this.mHandler = null;
                this.mCanRun = false;
            }
            this.mHandler = CoroutineManager.Instance.StartHandler(this.PlayAnimation_Cor(bEnable, rDelayTime));
        }

        private IEnumerator PlayAnimation_Cor(bool bEnable, float rDelayTime = 0f)
        {
            if (!this.mCanRun)
            {
                this.ResetCell();
            }

            if (this.mCanRun && bEnable)
            {
                this.Content.SetActive(false);
                yield return new WaitForSeconds(rDelayTime);
                if (this.Content)
                {
                    this.Content.SetActive(true);
                }
                LogManager.Log($"[AnimationCell] Condition {this.CellAnimator != null}   {this.mClip != null}  {rDelayTime} animator call..");
                if (this.CellAnimator != null && this.mClip != null)
                {
                    this.CellAnimator.Play(this.mClip.name, -1, 0f);
                    this.CellAnimator.Update(0);
                    LogManager.Log($"[AnimationCell]  {this.CellAnimator.gameObject?.name}   {this.mClip.name} animator call..");
                }
                this.mCanRun = false;

                if (this.mClip != null)
                {
                    yield return new WaitForSeconds(this.mClip.length);
                    this.mCanRun = true;
                }
                else
                {
                    this.mCanRun = true;
                }

            }
            else
            {
                this.Content.SetActive(true);

            }
            this.mHandler = null;
        }

        private void ResetCell()
        {
            if (this.CellAnimator != null && this.mClip != null)
            {
                this.CellAnimator.Play(this.mClip.name, -1, 1f);
                this.CellAnimator.Update(0);
            }
            if(this.mHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mHandler);
                this.mHandler = null;
                this.Content.SetActive(true);
            }
            this.mCanRun = true;
        }
        private void OnDestroy()
        {
            if (this.mHandler != null)
            {
                CoroutineManager.Instance.Stop(this.mHandler);
                this.mHandler = null;
            }
        }
    }
}