using DG.Tweening;
using Knight.Core;
using System;
using System.Collections.Generic;
namespace UnityEngine.UI
{
    public class ItemFlyAnimEvent : MonoBehaviour
    {
        public RectTransform AnimEndRectTrans;
        //0.51 - 0.22 / 0.55 - 0.39 / 1.09 - 0.47 / 1.03 - 0.46 / 0.56 - 0.30 / 1.11 - 0.5 /1.03 - 0.30 / 
        private float[] mItemFlyTimerArray = new float[7] { 0.29f, 0.16f, 0.62f, 0.57f, 0.26f, 0.61f, 0.73f };
        public List<RectTransform> FlyItemGos;
        public List<RectTransform> FlyTempItemGos;
        private void Awake()
        {
        }

        private void OnEnable()
        {
            if(this.FlyItemGos != null && this.FlyItemGos.Count > 0)
            {
                foreach (var rGo in this.FlyItemGos)
                {
                    rGo.gameObject.SetActiveSafe(true);
                }
            }

            if (this.FlyTempItemGos != null && this.FlyTempItemGos.Count > 0)
            {
                foreach (var rGo in this.FlyTempItemGos)
                {
                    rGo.gameObject.SetActiveSafe(false);
                }
            }
        }

        void ItemFlyAnimStart(int nItemIndex)
        {
            var nIndex = nItemIndex - 1;
            var rItemTrans = this.FlyTempItemGos[nIndex];
            var rFollowItem = this.FlyItemGos[nIndex];
            var rTimer = this.mItemFlyTimerArray[nIndex];
            DOTween.To(() => rFollowItem.localPosition, fCurExpProgress =>
            {
                rItemTrans.localPosition = fCurExpProgress;
            }, this.AnimEndRectTrans.localPosition, rTimer);
            rItemTrans.gameObject.SetActive(true);
            rFollowItem.gameObject.SetActive(false);
        }

        private void Update()
        {
            for (int i = 0; i < this.FlyItemGos.Count; i++)
            {
                var rItemTrans = this.FlyTempItemGos[i];
                var rFollowItem = this.FlyItemGos[i];
                rItemTrans.localScale = rFollowItem.localScale;
                rItemTrans.GetComponent<Image>().color = rFollowItem.GetComponent<Image>().color;
            }

        }
    }
}

