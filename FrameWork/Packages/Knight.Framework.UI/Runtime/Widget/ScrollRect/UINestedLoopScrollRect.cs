using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using Knight.Core;

namespace UnityEngine.UI
{/// <summary>
 /// 解决嵌套使用LoopScrollRect时的Drag冲突问题。请将该脚本放置到内层ScrollRect上(外层的ScrollRect的Drag事件会被内层的拦截)
 /// </summary>
    public class UINestedLoopScrollRect : MonoBehaviour, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        /// <summary>
        /// 外层被拦截需要正常拖动的ScrollRect，可不指定，默认在父对象中找
        /// </summary>
        public LoopScrollRect ParentScrollRect;
        public LoopScrollRect SubScrollRect;

        void Awake()
        {
            this.SubScrollRect = this.GetComponent<LoopScrollRect>();
            if (this.ParentScrollRect == null)
                this.ParentScrollRect = UtilTool.GetComponentInParent<LoopScrollRect>(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.SubScrollRect?.OnBeginDrag(eventData);
            this.ParentScrollRect?.OnBeginDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!this.SubScrollRect)
            {
                this.ParentScrollRect?.OnDrag(eventData);
                return;
            }
            if (!this.SubScrollRect.horizontal && !this.SubScrollRect.vertical)
            {
                this.SubScrollRect.enabled = false;
                this.ParentScrollRect?.OnDrag(eventData);
                return;
            }
            float angle = Vector2.Angle(eventData.delta, Vector2.left);

            //判断拖动方向，防止水平与垂直方向同时响应导致的拖动时整个界面都会动
            if (angle > 45f && angle < 135f)
            {
                this.SubScrollRect.enabled = this.SubScrollRect.vertical;
                this.ParentScrollRect.enabled = this.SubScrollRect.horizontal && this.ParentScrollRect.vertical;
            }
            else
            {
                this.SubScrollRect.enabled = this.SubScrollRect.horizontal;
                this.ParentScrollRect.enabled = this.SubScrollRect.vertical && this.ParentScrollRect.horizontal;
            }

            this.SubScrollRect?.OnDrag(eventData);
            this.ParentScrollRect?.OnDrag(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.SubScrollRect)
            {
                this.SubScrollRect.OnEndDrag(eventData);
                this.SubScrollRect.enabled = true;
            }

            if (this.ParentScrollRect)
            {
                this.ParentScrollRect.OnEndDrag(eventData);
                this.ParentScrollRect.enabled = true;
            }
        }
    }
}