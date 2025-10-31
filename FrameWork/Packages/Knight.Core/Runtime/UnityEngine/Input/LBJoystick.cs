using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Knight.Core;

namespace LittleBaby.Framework.Core
{


    public enum ELBJoystickType
    {
        Static,
        Dynamic,
    }
    public enum ELBJoystickLimitAreaType
    {
        Rectangle,
        Circle,
    }

    public class LBJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Serializable]
        public class TouchMoveEvent : UnityEvent<Vector2, Single>
        {
        }
        [Serializable]
        public class TouchUpEvent : UnityEvent<Vector2, Single>
        {
        }
        #region Config Params
#pragma warning disable IDE1006 // 命名样式
        [Header("摇杆类型")]
        public ELBJoystickType JoystickType = ELBJoystickType.Dynamic;
        [Header("未激活时的透明度")]
        [Range(0f, 1f)]
        public Single NonActiveAlpha = 0.5f;
        [Header("触发区")]
        [ValidateInput("Check_RectTransformIsNotNull", "触发区不可为空")]
        public RectTransform TriggerArea;
        [Header("运行时显示触发区")]
        public Boolean ShowRuntimeTriggerArea = false;
        [Header("触发区像素测试贴图")]
        public Texture2D TriggerAreaPixelTestTexture;
        [Header("限制区")]
        [ShowIf("Check_IsDynamic")]
        public RectTransform LimitArea;
        [Header("限制区类型")]
        [ShowIf("Check_IsDynamic")]
        public ELBJoystickLimitAreaType LimitAreaType = ELBJoystickLimitAreaType.Rectangle;
        [Header("运行时显示限制区")]
        [ShowIf("Check_IsDynamic")]
        public Boolean ShowRuntimeLimitArea = false;
        [Header("背景")]
        [ValidateInput("Check_RectTransformIsNotNull", "背景不可为空")]
        public RectTransform Background;
        public CanvasGroup BackgroundRedCancel;
        [Header("运行时显示背景")]
        public Boolean ShowRuntimeBackground = true;
        [Header("Thumb")]
        [ValidateInput("Check_RectTransformIsNotNull", "Thumb不可为空")]
        public RectTransform Thumb;
        public CanvasGroup ThumbHighlight;
        public CanvasGroup ThumbRedCancel;
        
        [Header("运行时显示Thumb")]
        public Boolean ShowRuntimeThumb = true;
        [Header("Thumb最大移动距离")]
        public Single ThumbMaxMoveDis = 50;

        public UnityEvent OnTouchDown;
        public TouchMoveEvent OnTouchMove;
        public TouchUpEvent OnTouchUp;
        public LBJoystickGroup LBJoystickGroup;
        public Int32 CurrentPointID = Int32.MaxValue;
        public Vector2 TouchPosition;

        [Header("延续上一次移动位置/Thumb不移动-移动Bg")] 
        public bool ContinueLastPos;
        [HideInInspector]
        public Vector2 ThumpUp_LastUpDir;
        [HideInInspector]
        public float ThumpUp_LastUpDis;

        public bool Continue_Reset;
        
#pragma warning restore IDE1006 // 命名样式
        #endregion
        #region Unity Interface
        public virtual void OnPointerDown(PointerEventData pointerEventData)
        {
            this.OnPointerDown(pointerEventData.pressPosition, pointerEventData.pointerId);
        }
        public virtual void OnPointerDown(Vector2 pressPosition, Int32 pointID)
        {
            if (this.CurrentPointID == Int32.MaxValue)
            {
                this.CurrentPointID = pointID;
            }
            if (this.CurrentPointID != pointID)
            {
                return;
            }
            this.TouchPosition = pressPosition;
            this.CurrentPointID = pointID;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ParentRect, pressPosition, this.UICamera, out var pos);
            if (!this.CheckTriggerAreaPixelTest(pos))
            {
                return;
            }
            this.LBJoystickGroup?.CheckTouched();
            this.SetBackgroundPos(pos, true);
            this.SetIsTouched(true);
        }
        public virtual void OnPointerDown(Int32 pointID)
        {
            this.OnPointerDown(RectTransformUtility.WorldToScreenPoint(this.UICamera, this.Background.position), pointID);
        }
        public virtual void OnDrag(PointerEventData pointerEventData)
        {
            this.OnDrag(pointerEventData.position, pointerEventData.pointerId);
        }
        public virtual void OnDrag(Vector2 position, Int32 pointID)
        {
            if (this.CurrentPointID != pointID)
            {
                return;
            }
            if (!this.IsTouched)
            {
                return;
            }
            this.TouchPosition = position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.ParentRect, position, this.UICamera, out var pos);
            this.SetThumbPos(pos);
            var dir = this.Thumb.anchoredPosition - this.Background.anchoredPosition;
            var dis = Vector2.Distance(this.Thumb.anchoredPosition, this.Background.anchoredPosition) / this.ThumbMaxMoveDis;
            this.OnTouchMove?.Invoke(dir, dis);
        }
        public virtual void OnDragOffset(Vector2 rOffset, Int32 pointID)
        {
            this.OnDrag(RectTransformUtility.WorldToScreenPoint(this.UICamera, this.Background.position) + rOffset, pointID);
        }
        public virtual void OnPointerUp(PointerEventData pointerEventData)
        {
            this.OnPointerUp(pointerEventData.position, pointerEventData.pointerId);
        }
        public virtual void OnPointerUp(Vector2 position, Int32 pointID)
        {
            if (this.CurrentPointID == Int32.MaxValue)
            {
                return;
            }
            if (this.CurrentPointID != pointID)
            {
                return;
            }
            this.TouchPosition = Vector2.zero;
            this.CurrentPointID = Int32.MaxValue;
            if (!this.IsTouched)
            {
                return;
            }
            this.SetIsTouched(false);
            this.SetBackgroundPos(this.InitPos, false);
        }
        public virtual void OnPointerUp(Int32 pointID)
        {
            this.OnPointerUp(RectTransformUtility.WorldToScreenPoint(this.UICamera, this.Background.position), pointID);
        }

        public void ChangeJoySticktype(ELBJoystickType rJoystickType)
        {
            if(this.JoystickType == rJoystickType) return;
            this.JoystickType = rJoystickType;
            this.Refresh();
        }

        public virtual bool GetActive()
        {
            return this.IsTouched;
        }
        protected virtual void Start()
        {
            //this.RefreshConfig();
            //this.RefreshShowState();
        }
        protected virtual void OnEnable()
        {
            this.Refresh();
        }
        protected virtual void OnDisable()
        {
            this.SetIsTouched(false);
            this.SetBackgroundPos(this.InitPos, false);
        }

        public virtual void Refresh()
        {
            this.Initialize();
            this.RefreshConfig();
            this.RefreshShowState();
        }
        #endregion
        #region Member
        protected static Dictionary<Int32, Boolean> CurrentGroupIDDict { get; set; } = new Dictionary<Int32, Boolean>();
        protected virtual Boolean IsInit { get; set; } = false;
        protected virtual Camera UICamera { get; set; }
        protected virtual RectTransform ParentRect { get; set; }
        public virtual CanvasGroup JoystickCanvasGroup { get; set; }
        protected virtual Vector2 InitPos { get; set; }
        protected virtual Rect LimitRect { get; set; }
        public virtual Boolean IsTouched { get; set; } = false;
        public virtual Single ThumbMaxMoveDisRate { get; set; } = 1f;

        public virtual Single ThumbMoveDisValue
        {
            get
            {
                return this.ThumbMaxMoveDis * this.ThumbMaxMoveDisRate;
            }
        }
        #endregion
        #region Check Method
#pragma warning disable IDE0051 // 删除未使用的私有成员
        private Boolean Check_RectTransformIsNotNull(RectTransform rectTransform)
        {
            return rectTransform != null;
        }
        private Boolean Check_IsDynamic()
        {
            return this.JoystickType == ELBJoystickType.Dynamic;
        }
#pragma warning restore IDE0051 // 删除未使用的私有成员
        [Button("修正配置的相关参数")]
        protected void CorrectConfigParams()
        {
            var vector2Half = Vector2.one / 2;
            if (this.TriggerArea)
            {
                this.TriggerArea.ResetAnchorAndPivot(vector2Half, vector2Half, vector2Half, true);
            }
            if (this.LimitArea)
            {
                this.LimitArea.ResetAnchorAndPivot(vector2Half, vector2Half, vector2Half, true);
            }
            if (this.Background)
            {
                this.Background.ResetAnchorAndPivot(vector2Half, vector2Half, vector2Half, true);

                var image = this.Background.gameObject.ReceiveComponent<Image>();
                image.raycastTarget = false;
                image.maskable = false;
            }
            if (this.Thumb)
            {
                this.Thumb.ResetAnchorAndPivot(vector2Half, vector2Half, vector2Half, true);

                var image = this.Thumb.gameObject.ReceiveComponent<Image>();
                image.raycastTarget = false;
                image.maskable = false;
            }
            this.ThumbMaxMoveDis = Mathf.Max(this.ThumbMaxMoveDis, 1);
            this.ThumbMaxMoveDisRate = 1f;
        }
        #endregion
        #region Handler Method
        protected virtual void Initialize()
        {
            this.UICamera = UtilTool.GetComponentInParent<Canvas>(this.gameObject)?.worldCamera;
            this.ParentRect = this.transform?.GetComponent<RectTransform>();
            this.JoystickCanvasGroup = this.gameObject.SafeGetComponent<CanvasGroup>();

            this.JoystickCanvasGroup.interactable = true;
            this.JoystickCanvasGroup.blocksRaycasts = true;
            this.JoystickCanvasGroup.ignoreParentGroups = false;

            this.InitPos = this.Background.gameObject.ReceiveComponent<RectTransform>().anchoredPosition;
            if (this.LimitArea)
            {
                this.LimitRect = new Rect(this.LimitArea.anchoredPosition - (this.LimitArea.sizeDelta / 2), this.LimitArea.sizeDelta);
            }
            this.IsTouched = false;

            this.IsInit = true;
            this.CurrentPointID = Int32.MaxValue;
        }
        protected virtual void RefreshConfig()
        {
            if (!this.IsInit)
            {
                return;
            }
            this.CorrectConfigParams();

            if (this.LimitArea)
            {
                this.LimitArea.gameObject.SetActive(this.ShowRuntimeLimitArea && this.JoystickType == ELBJoystickType.Dynamic);
            }
            this.Background.gameObject.SetActive(this.ShowRuntimeBackground);
            this.Thumb.gameObject.SetActive(this.ShowRuntimeThumb);
        }
        protected virtual void RefreshShowState()
        {
            if (!this.IsInit)
            {
                return;
            }
            //处理CanvasGroup透明度
            this.JoystickCanvasGroup.alpha = this.IsTouched ? 1f : this.NonActiveAlpha;
            if (this.ThumbHighlight)
                this.ThumbHighlight.alpha = this.IsTouched ? 1f : 0f;
        }
        protected virtual void SetIsTouched(Boolean isTouched)
        {
            if (this.IsTouched == isTouched)
            {
                this.RefreshShowState();
                return;
            }
            this.IsTouched = isTouched;
            this.RefreshShowState();
            if (this.IsTouched)
            {
                if (this.ContinueLastPos)
                {
                    var rpos = this.Thumb.anchoredPosition - this.ThumpUp_LastUpDir;
                    this.Background.anchoredPosition = rpos;
                }
                this.OnTouchDown?.Invoke();
            }
            else
            {
                var dir = this.Thumb.anchoredPosition - this.Background.anchoredPosition;
                var dis = Vector2.Distance(this.Thumb.anchoredPosition, this.Background.anchoredPosition) / this.ThumbMaxMoveDis;
                this.OnTouchUp?.Invoke(dir, dis);
                if (this.Continue_Reset)
                {
                    this.ThumpUp_LastUpDir = Vector2.zero;
                    this.ThumpUp_LastUpDis = 0;
                    this.Continue_Reset = false;
                }
                else
                {
                    this.ThumpUp_LastUpDir = dir;
                    this.ThumpUp_LastUpDis = dis;
                }
            }
        }
        
        protected virtual void SetBackgroundPos(Vector2 pos, bool bIsPointDown)
        {
            if (this.JoystickType != ELBJoystickType.Dynamic)
            {
                if (!bIsPointDown)
                {
                    this.SetThumbPos(pos);
                }
                return;
            }
            if (this.LimitArea)
            {
                if (this.LimitAreaType == ELBJoystickLimitAreaType.Rectangle)
                {
                    if (pos.x < this.LimitRect.xMin)
                    {
                        pos.x = this.LimitRect.xMin;
                    }
                    if (pos.x > this.LimitRect.xMax)
                    {
                        pos.x = this.LimitRect.xMax;
                    }
                    if (pos.y < this.LimitRect.yMin)
                    {
                        pos.y = this.LimitRect.yMin;
                    }
                    if (pos.y > this.LimitRect.yMax)
                    {
                        pos.y = this.LimitRect.yMax;
                    }
                }
                else if (this.LimitAreaType == ELBJoystickLimitAreaType.Circle)
                {
                    var radius = Mathf.Min(this.LimitRect.width, this.LimitRect.height) / 2;
                    var dis = Vector2.Distance(this.LimitArea.anchoredPosition, pos);
                    if (dis > radius)
                    {
                        pos = Vector2.Lerp(this.LimitArea.anchoredPosition, pos, radius / dis);
                    }
                }
            }

            this.Background.anchoredPosition = pos;
            this.SetThumbPos(pos);
        }
        protected virtual void SetThumbPos(Vector2 pos)
        {
            var dis = Vector2.Distance(this.Background.anchoredPosition, pos);
            if (dis > this.ThumbMoveDisValue)
            {
                pos = Vector2.Lerp(this.Background.anchoredPosition, pos, this.ThumbMoveDisValue / dis);
            }
            this.Thumb.anchoredPosition = pos;
        }
        protected virtual Boolean CheckTriggerAreaPixelTest(Vector2 pos)
        {
            if (this.TriggerArea && this.TriggerAreaPixelTestTexture)
            {
                pos -= this.TriggerArea.anchoredPosition;
                pos -= -this.TriggerArea.sizeDelta / 2;
                var uv = pos / this.TriggerArea.sizeDelta;
                var color = this.TriggerAreaPixelTestTexture.GetPixelBilinear(uv.x, uv.y);
                if (color.a <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        #region Interface
        public virtual void SetTriggerSize(Vector2 size)
        {
            this.TriggerArea.sizeDelta = size;
        }

        public virtual void SetLimitSize(Vector2 rSize)
        {
            this.LimitArea.sizeDelta = rSize;
        }

        public virtual void SetLimitPos(Vector2 rPositon)
        {
            this.LimitArea.anchoredPosition = rPositon;
        }
        #endregion
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                this.SetIsTouched(false);
                this.SetBackgroundPos(this.InitPos, false);
            }
        }
    }
}