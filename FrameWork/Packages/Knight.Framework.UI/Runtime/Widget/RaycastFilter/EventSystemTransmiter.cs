using Knight.Core;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    /// <summary>
    /// 事件系统转发器
    /// </summary>
    public class EventSystemTransmiter : EmptyGraphics,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler,
        IInitializePotentialDragHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        IDropHandler,
        IScrollHandler,
        ISelectHandler,
        IDeselectHandler,
        IUpdateSelectedHandler,
        IMoveHandler,
        ISubmitHandler,
        ICancelHandler
    {
        public List<IPointerEnterHandler> PointerEnterHandlerList = new List<IPointerEnterHandler>();
        public List<IPointerExitHandler> PointerExitHandlerList = new List<IPointerExitHandler>();
        public List<IPointerDownHandler> PointerDownHandlerList = new List<IPointerDownHandler>();
        public List<IPointerUpHandler> PointerUpHandlerList = new List<IPointerUpHandler>();
        public List<IPointerClickHandler> PointerClickHandlerList = new List<IPointerClickHandler>();
        public List<IInitializePotentialDragHandler> InitializePotentialDragHandlerList = new List<IInitializePotentialDragHandler>();
        public List<IBeginDragHandler> BeginDragHandlerList = new List<IBeginDragHandler>();
        public List<IDragHandler> DragHandlerList = new List<IDragHandler>();
        public List<IEndDragHandler> EndDragHandlerList = new List<IEndDragHandler>();
        public List<IDropHandler> DropHandlerList = new List<IDropHandler>();
        public List<IScrollHandler> ScrollHandlerList = new List<IScrollHandler>();
        public List<ISelectHandler> SelectHandlerList = new List<ISelectHandler>();
        public List<IDeselectHandler> DeselectHandlerList = new List<IDeselectHandler>();
        public List<IUpdateSelectedHandler> UpdateSelectedHandlerList = new List<IUpdateSelectedHandler>();
        public List<IMoveHandler> MoveHandlerList = new List<IMoveHandler>();
        public List<ISubmitHandler> SubmitHandlerList = new List<ISubmitHandler>();
        public List<ICancelHandler> CancelHandlerList = new List<ICancelHandler>();

        public GameObject Target;
        public UnityEvent OnPointerClickEvent = new UnityEvent();
        public UnityEvent OnPointerUpEvent = new UnityEvent();
        public UnityEvent OnPointerDownEvent = new UnityEvent();
        public UnityEvent OnBeginDragEvent = new UnityEvent();
        public UnityEvent OnEndDragEvent = new UnityEvent();

        public void SetTarget(GameObject rTarget)
        {
            if (!rTarget) return;
            this.Target = rTarget;
            var rComponents = this.Target.GetComponents<Component>();
            foreach (var rComponent in rComponents)
            {
                if (rComponent is IPointerEnterHandler rPointerEnterHandler)
                {
                    this.PointerEnterHandlerList.Add(rPointerEnterHandler);
                }
                if (rComponent is IPointerExitHandler rPointerExitHandler)
                {
                    this.PointerExitHandlerList.Add(rPointerExitHandler);
                }
                if (rComponent is IPointerDownHandler rPointerDownHandler)
                {
                    this.PointerDownHandlerList.Add(rPointerDownHandler);
                }
                if (rComponent is IPointerUpHandler rPointerUpHandler)
                {
                    this.PointerUpHandlerList.Add(rPointerUpHandler);
                }
                if (rComponent is IPointerClickHandler rPointerClickHandler)
                {
                    this.PointerClickHandlerList.Add(rPointerClickHandler);
                }
                if (rComponent is IInitializePotentialDragHandler rInitializePotentialDragHandler)
                {
                    this.InitializePotentialDragHandlerList.Add(rInitializePotentialDragHandler);
                }
                if (rComponent is IBeginDragHandler rBeginDragHandler)
                {
                    this.BeginDragHandlerList.Add(rBeginDragHandler);
                }
                if (rComponent is IDragHandler rDragHandler)
                {
                    this.DragHandlerList.Add(rDragHandler);
                }
                if (rComponent is IEndDragHandler rEndDragHandler)
                {
                    this.EndDragHandlerList.Add(rEndDragHandler);
                }
                if (rComponent is IDropHandler rDropHandler)
                {
                    this.DropHandlerList.Add(rDropHandler);
                }
                if (rComponent is IScrollHandler rScrollHandler)
                {
                    this.ScrollHandlerList.Add(rScrollHandler);
                }
                if (rComponent is ISelectHandler rSelectHandler)
                {
                    this.SelectHandlerList.Add(rSelectHandler);
                }
                if (rComponent is IDeselectHandler rDeselectHandler)
                {
                    this.DeselectHandlerList.Add(rDeselectHandler);
                }
                if (rComponent is IUpdateSelectedHandler rUpdateSelectedHandler)
                {
                    this.UpdateSelectedHandlerList.Add(rUpdateSelectedHandler);
                }
                if (rComponent is IMoveHandler rMoveHandler)
                {
                    this.MoveHandlerList.Add(rMoveHandler);
                }
                if (rComponent is ISubmitHandler rSubmitHandler)
                {
                    this.SubmitHandlerList.Add(rSubmitHandler);
                }
                if (rComponent is ICancelHandler rCancelHandler)
                {
                    this.CancelHandlerList.Add(rCancelHandler);
                }
            }
        }

        public void ClearTarget()
        {
            this.Target = null;
            this.PointerEnterHandlerList.Clear();
            this.PointerExitHandlerList.Clear();
            this.PointerDownHandlerList.Clear();
            this.PointerUpHandlerList.Clear();
            this.PointerClickHandlerList.Clear();
            this.InitializePotentialDragHandlerList.Clear();
            this.BeginDragHandlerList.Clear();
            this.DragHandlerList.Clear();
            this.EndDragHandlerList.Clear();
            this.DropHandlerList.Clear();
            this.ScrollHandlerList.Clear();
            this.SelectHandlerList.Clear();
            this.DeselectHandlerList.Clear();
            this.UpdateSelectedHandlerList.Clear();
            this.MoveHandlerList.Clear();
            this.SubmitHandlerList.Clear();
            this.CancelHandlerList.Clear();
        }

        public void OnPointerEnter(PointerEventData rEventData)
        {
            for (int i = 0; i < this.PointerEnterHandlerList.Count; i++)
            {
                this.PointerEnterHandlerList[i].OnPointerEnter(rEventData);
            }
        }

        public void OnPointerExit(PointerEventData rEventData)
        {
            for (int i = 0; i < this.PointerExitHandlerList.Count; i++)
            {
                this.PointerExitHandlerList[i].OnPointerExit(rEventData);
            }
        }

        public void OnPointerDown(PointerEventData rEventData)
        {
            var rHandlerList = PoolList<IPointerDownHandler>.Alloc();
            rHandlerList.AddRange(this.PointerDownHandlerList);
            this.OnPointerDownEvent?.Invoke();
            for (int i = 0; i < rHandlerList.Count; i++)
            {
                rHandlerList[i].OnPointerDown(rEventData);
            }
            rHandlerList.Free();
        }

        public void OnPointerUp(PointerEventData rEventData)
        {
            var rHandlerList = PoolList<IPointerUpHandler>.Alloc();
            rHandlerList.AddRange(this.PointerUpHandlerList);
            this.OnPointerUpEvent?.Invoke();
            for (int i = 0; i < rHandlerList.Count; i++)
            {
                rHandlerList[i].OnPointerUp(rEventData);
            }
            rHandlerList.Free();
        }

        public void OnPointerClick(PointerEventData rEventData)
        {
            var rHandlerList = PoolList<IPointerClickHandler>.Alloc();
            rHandlerList.AddRange(this.PointerClickHandlerList);
            this.OnPointerClickEvent?.Invoke();
            for (int i = 0; i < rHandlerList.Count; i++)
            {
                rHandlerList[i].OnPointerClick(rEventData);
            }
            rHandlerList.Free();
        }

        public void OnInitializePotentialDrag(PointerEventData rEventData)
        {
            for (int i = 0; i < this.InitializePotentialDragHandlerList.Count; i++)
            {
                this.InitializePotentialDragHandlerList[i].OnInitializePotentialDrag(rEventData);
            }
        }

        public void OnBeginDrag(PointerEventData rEventData)
        {
            var rHandlerList = PoolList<IBeginDragHandler>.Alloc();
            rHandlerList.AddRange(this.BeginDragHandlerList);
            this.OnBeginDragEvent?.Invoke();
            for (int i = 0; i < rHandlerList.Count; i++)
            {
                rHandlerList[i].OnBeginDrag(rEventData);
            }
            rHandlerList.Free();
        }

        public void OnDrag(PointerEventData rEventData)
        {
            for (int i = 0; i < this.DragHandlerList.Count; i++)
            {
                this.DragHandlerList[i].OnDrag(rEventData);
            }
        }

        public void OnEndDrag(PointerEventData rEventData)
        {
            var rHandlerList = PoolList<IEndDragHandler>.Alloc();
            rHandlerList.AddRange(this.EndDragHandlerList);
            this.OnEndDragEvent?.Invoke();
            for (int i = 0; i < rHandlerList.Count; i++)
            {
                rHandlerList[i].OnEndDrag(rEventData);
            }
            rHandlerList.Free();
        }

        public void OnDrop(PointerEventData rEventData)
        {
            for (int i = 0; i < this.DropHandlerList.Count; i++)
            {
                this.DropHandlerList[i].OnDrop(rEventData);
            }
        }

        public void OnScroll(PointerEventData rEventData)
        {
            for (int i = 0; i < this.ScrollHandlerList.Count; i++)
            {
                this.ScrollHandlerList[i].OnScroll(rEventData);
            }
        }

        public void OnSelect(BaseEventData rEventData)
        {
            for (int i = 0; i < this.SelectHandlerList.Count; i++)
            {
                this.SelectHandlerList[i].OnSelect(rEventData);
            }
        }

        public void OnDeselect(BaseEventData rEventData)
        {
            for (int i = 0; i < this.DeselectHandlerList.Count; i++)
            {
                this.DeselectHandlerList[i].OnDeselect(rEventData);
            }
        }

        public void OnUpdateSelected(BaseEventData rEventData)
        {
            for (int i = 0; i < this.UpdateSelectedHandlerList.Count; i++)
            {
                this.UpdateSelectedHandlerList[i].OnUpdateSelected(rEventData);
            }
        }

        public void OnMove(AxisEventData rEventData)
        {
            for (int i = 0; i < this.MoveHandlerList.Count; i++)
            {
                this.MoveHandlerList[i].OnMove(rEventData);
            }
        }

        public void OnSubmit(BaseEventData rEventData)
        {
            for (int i = 0; i < this.SubmitHandlerList.Count; i++)
            {
                this.SubmitHandlerList[i].OnSubmit(rEventData);
            }
        }

        public void OnCancel(BaseEventData rEventData)
        {
            for (int i = 0; i < this.CancelHandlerList.Count; i++)
            {
                this.CancelHandlerList[i].OnCancel(rEventData);
            }
        }
    }
}