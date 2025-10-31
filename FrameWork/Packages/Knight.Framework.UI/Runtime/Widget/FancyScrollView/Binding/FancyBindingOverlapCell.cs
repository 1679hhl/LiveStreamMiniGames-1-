using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;
using EasingCore;
using System.Linq;

namespace UnityEngine.UI
{
    public class FancyBindingOverlapCell : FancyCell<FancyBindingItemData>
    {
        readonly EasingFunction alphaEasing = Easing.Get(Ease.OutQuint);

        [SerializeField] Image background = default;
        [SerializeField] CanvasGroup canvasGroup = default;
        [SerializeField] float popAngle = -15;
        [SerializeField] float slideAngle = 25;
        [SerializeField] float popSpan = 0.75f;
        [SerializeField] float slideSpan = 0.25f;
        public override void UpdateContent(FancyBindingItemData itemData)
        {
            this.UpdateSibling();
        }

        void UpdateSibling()
        {
            var cells = transform.parent.Cast<Transform>()
                .Select(t => t.GetComponent<FancyBindingOverlapCell>())
                .Where(cell => cell.IsVisible);

            if (Index == cells.Min(x => x.Index))
            {
                transform.SetAsLastSibling();
            }

            if (Index == cells.Max(x => x.Index))
            {
                transform.SetAsFirstSibling();
            }
        }
        public override void UpdatePosition(float t)
        {
            t = 1f - t;

            var pop = Mathf.Min(popSpan, t) / popSpan;
            var slide = Mathf.Max(0, t - popSpan) / slideSpan;

            transform.localRotation = t < popSpan
                ? Quaternion.Euler(0, 0, popAngle * (1f - pop))
                : Quaternion.Euler(0, 0, slideAngle * slide);

            transform.localPosition = Vector3.left * 500f * slide;

            canvasGroup.alpha = alphaEasing(1f - slide);

            background.color = Color.Lerp(Color.gray, Color.white, pop);
        }
    }
}
