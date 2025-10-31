//The implementation is based on this article:http://rbarraza.com/html5-canvas-pageflip/
//As the rbarraza.com website is not live anymore you can get an archived version from web archive 
//or check an archived version that I uploaded on my website: https://dandarawy.com/html5-canvas-pageflip/

using Knight.Core;
using System;
using System.Collections;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public enum FlipMode
    {
        Bottom_RightToLeft,
        Bottom_LeftToRight,
        Top_RightToLeft,
        Top_LeftToRight,
    }

    public enum PageType
    {
        Left,
        Right,
        LeftNext,
        RightNext
    }
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:添加只读修饰符", Justification = "<挂起>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:使用 \"new(...)\"", Justification = "<挂起>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:命名样式", Justification = "<挂起>")]
    public class Book : MonoBehaviour
    {
        public Canvas canvas;
        [SerializeField]
        RectTransform BookPanel;
        public Sprite background;
        public bool Interactable = true;
        public bool enableShadowEffect = true;
        //represent the index of the sprite shown in the right page
        private int mCurrentPage = 1;

        public int MaxPageID;

        public int MinPageID = 1;
        public int CurrentPage
        {
            get { return this.mCurrentPage; }
            set
            {
                this.mCurrentPage = value;
                this.UpdateSprites();
            }
        }

        private int mNextPageID;

        public int NextPageID
        {
            get { return this.mNextPageID; }
            set
            {
                this.mNextPageID = value;
                LogManager.Log($"NextPageID = {value}");
            }
        }
        public Vector3 EndBottomLeft
        {
            get { return this.ebl; }
        }
        public Vector3 EndBottomRight
        {
            get { return this.ebr; }
        }
        public float Height
        {
            get
            {
                return this.BookPanel.rect.height;
            }
        }
        public Image ClippingPlane;
        public Image NextPageClip;
        public Image Shadow;
        public SoftMask ShadowMaskRight;
        public Image ShadowLTR;
        public SoftMask ShadowMaskLeft;
        public RectTransform Left;
        public RectTransform LeftNext;
        public RectTransform Right;
        public RectTransform RightNext;
        public UnityEvent OnFlip;
        float bottomRadius1, bottomRadius2, topRadius1, topRadius2;
        public Image ShadowClipingL;
        public Image ShadowClipingR;
        public Image ShadowLBorder;
        public Image ShadowRBorder;
        //Spine Bottom
        Vector3 sb;
        //Spine Top
        Vector3 st;
        //corner of the page
        Vector3 c;
        //Edge Bottom Right
        Vector3 ebr;
        //Edge Bottom Left
        Vector3 ebl;
        //Edge Top Right
        Vector3 etr;
        //Edge top Left
        Vector3 etl;
        //follow point 
        Vector3 f;
        [HideInInspector]
        public bool PageDragging = false;
        //current flip mode
        FlipMode mode;
        public UnityAction OnBeginFlip;
        public UnityAction OnEndFlip;
        public UnityAction<RectTransform, int, PageType> OnSetPageContent;
        private bool mIsPageReleasing = false;
        [Range(0.25f, 0.75f)]
        public float TurnPageRate = 0.25f;
        void Start()
        {
            if (!this.canvas) this.canvas = UtilTool.GetComponentInParent<Canvas>(this);
            if (!this.canvas) Debug.LogError("Book should be a child to canvas");

            this.Left.gameObject.SetActive(false);
            this.Right.gameObject.SetActive(false);
            if (this.ShadowClipingL.gameObject.activeSelf)
                this.ShadowClipingL.gameObject.SetActive(false);
            if (this.ShadowClipingR.gameObject.activeSelf)
                this.ShadowClipingR.gameObject.SetActive(false);
            // UpdateSprites();
            this.CalcCurlCriticalPoints();

            float pageWidth = this.BookPanel.rect.width / 2.0f;
            float pageHeight = this.BookPanel.rect.height;
            this.NextPageClip.rectTransform.sizeDelta = new Vector2(pageWidth, pageHeight + pageHeight * 2);

            this.ClippingPlane.rectTransform.sizeDelta = new Vector2(pageWidth * 2 + pageHeight, pageHeight + pageHeight * 2);

            //hypotenous (diagonal) page length
            float hyp = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
            float shadowPageHeight = pageWidth / 2 + hyp;

            this.Shadow.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
            this.Shadow.rectTransform.pivot = new Vector2(1, (pageWidth / 2) / shadowPageHeight);

            this.ShadowLTR.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
            this.ShadowLTR.rectTransform.pivot = new Vector2(0, (pageWidth / 2) / shadowPageHeight);
        }

        private void CalcCurlCriticalPoints()
        {
            this.sb = new Vector3(0, -this.BookPanel.rect.height / 2);
            this.ebr = new Vector3(this.BookPanel.rect.width / 2, -this.BookPanel.rect.height / 2);
            this.ebl = new Vector3(-this.BookPanel.rect.width / 2, -this.BookPanel.rect.height / 2);
            this.etr = new Vector3(this.BookPanel.rect.width / 2, this.BookPanel.rect.height / 2);
            this.etl = new Vector3(-this.BookPanel.rect.width / 2, this.BookPanel.rect.height / 2);
            this.st = new Vector3(0, this.BookPanel.rect.height / 2);
            this.bottomRadius1 = Vector2.Distance(this.sb, this.ebr);
            float bottomPageWidth = this.BookPanel.rect.width / 2.0f;
            float bottomPageHeight = this.BookPanel.rect.height;
            this.bottomRadius2 = Mathf.Sqrt(bottomPageWidth * bottomPageWidth + bottomPageHeight * bottomPageHeight);
            this.topRadius1 = Vector2.Distance(this.st, this.etr);
            float topPageWidth = this.BookPanel.rect.width / 2.0f;
            float topPageHeight = this.BookPanel.rect.height;
            this.topRadius2 = Mathf.Sqrt(topPageWidth * topPageWidth + topPageHeight * topPageHeight);
        }

        public Vector3 transformPoint(Vector3 mouseScreenPos)
        {
            if (this.canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                //Vector3 mouseWorldPos = this.canvas.worldCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, this.canvas.planeDistance));
                //Vector2 localPos = this.BookPanel.InverseTransformPoint(mouseWorldPos);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.BookPanel, mouseScreenPos, this.canvas.worldCamera, out var localPos);

                return localPos;
            }
            else if (this.canvas.renderMode == RenderMode.WorldSpace)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector3 globalEBR = this.transform.TransformPoint(this.ebr);
                Vector3 globalEBL = this.transform.TransformPoint(this.ebl);
                Vector3 globalSt = this.transform.TransformPoint(this.st);
                Plane p = new Plane(globalEBR, globalEBL, globalSt);
                p.Raycast(ray, out var distance);
                Vector2 localPos = this.BookPanel.InverseTransformPoint(ray.GetPoint(distance));
                return localPos;
            }
            else
            {
                //Screen Space Overlay
                Vector2 localPos = this.BookPanel.InverseTransformPoint(mouseScreenPos);
                return localPos;
            }
        }

        void Update()
        {
            if (this.PageDragging && this.Interactable)
            {
                this.UpdateBook();
            }
        }

        public void UpdateBook()
        {
            this.f = Vector3.Lerp(this.f, this.transformPoint(Input.mousePosition), Time.deltaTime * 10);
            if (this.mode == FlipMode.Bottom_RightToLeft)
                this.UpdateBookBRTLToPoint(this.f);
            else if (this.mode == FlipMode.Bottom_LeftToRight)
                this.UpdateBookBLTRToPoint(this.f);
            else if (this.mode == FlipMode.Top_RightToLeft)
                this.UpdateBookTRTLToPoint(this.f);
            else if (this.mode == FlipMode.Top_LeftToRight)
                this.UpdateBookTLTRToPoint(this.f);
        }

        public void UpdateBookBLTRToPoint(Vector3 followLocation)
        {
            this.mode = FlipMode.Bottom_LeftToRight;
            this.f = followLocation;
            this.ShadowLTR.transform.SetParent(this.ClippingPlane.transform, true);
            this.ShadowLTR.transform.localPosition = new Vector3(0, 0, 0);
            this.ShadowLTR.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.Left.transform.SetParent(this.ClippingPlane.transform, true);

            this.Right.transform.SetParent(this.BookPanel.transform, true);
            this.Right.transform.localEulerAngles = Vector3.zero;
            this.LeftNext.transform.SetParent(this.BookPanel.transform, true);

            this.c = this.Calc_C_Position(followLocation, this.sb, this.st);
            float clipAngle = this.CalcClipAngle(this.c, this.ebl, this.sb, out var t1);
            //0 < T0_T1_Angle < 180
            clipAngle = (clipAngle + 180) % 180;

            this.ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
            this.ClippingPlane.transform.position = this.BookPanel.TransformPoint(t1);

            //page position and angle
            this.Left.transform.position = this.BookPanel.TransformPoint(this.c);
            float C_T1_dy = t1.y - this.c.y;
            float C_T1_dx = t1.x - this.c.x;
            float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
            this.Left.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - 90 - clipAngle);

            this.NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
            this.NextPageClip.transform.position = this.BookPanel.TransformPoint(t1);
            this.LeftNext.transform.SetParent(this.NextPageClip.transform, true);
            this.Right.transform.SetParent(this.ClippingPlane.transform, true);
            this.Right.transform.SetAsFirstSibling();

            this.ShadowLTR.rectTransform.SetParent(this.ShadowMaskLeft.transform, true);
        }

        public void UpdateBookBRTLToPoint(Vector3 followLocation)
        {
            this.mode = FlipMode.Bottom_RightToLeft;
            this.f = followLocation;
            this.Shadow.transform.SetParent(this.ClippingPlane.transform, true);
            this.Shadow.transform.localPosition = Vector3.zero;
            this.Shadow.transform.localEulerAngles = Vector3.zero;
            this.Right.transform.SetParent(this.ClippingPlane.transform, true);

            this.Left.transform.SetParent(this.BookPanel.transform, true);
            this.Left.transform.localEulerAngles = Vector3.zero;
            this.RightNext.transform.SetParent(this.BookPanel.transform, true);
            this.c = this.Calc_C_Position(followLocation, this.sb, this.st);
            float clipAngle = this.CalcClipAngle(this.c, this.ebr, this.sb, out var t1);
            if (clipAngle > -90) clipAngle += 180;
            this.ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
            this.ClippingPlane.transform.position = this.BookPanel.TransformPoint(t1);

            //page position and angle
            this.Right.transform.position = this.BookPanel.TransformPoint(this.c);
            float C_T1_dy = t1.y - this.c.y;
            float C_T1_dx = t1.x - this.c.x;
            float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
            this.Right.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - (clipAngle + 90));

            this.NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
            this.NextPageClip.transform.position = this.BookPanel.TransformPoint(t1);
            this.RightNext.transform.SetParent(this.NextPageClip.transform, true);
            this.Left.transform.SetParent(this.ClippingPlane.transform, true);
            this.Left.transform.SetAsFirstSibling();

            this.Shadow.rectTransform.SetParent(this.ShadowMaskRight.transform, true);
        }

        public void UpdateBookTLTRToPoint(Vector3 followLocation)
        {
            this.mode = FlipMode.Top_LeftToRight;
            this.f = followLocation;
            this.ShadowLTR.transform.SetParent(this.ClippingPlane.transform, true);
            this.ShadowLTR.transform.localPosition = new Vector3(0, 0, 0);
            this.ShadowLTR.transform.localEulerAngles = new Vector3(0, 0, 0);
            this.Left.transform.SetParent(this.ClippingPlane.transform, true);

            this.Right.transform.SetParent(this.BookPanel.transform, true);
            this.Right.transform.localEulerAngles = Vector3.zero;
            this.LeftNext.transform.SetParent(this.BookPanel.transform, true);

            this.c = this.Calc_C_Position(followLocation, this.st, this.sb);
            float clipAngle = this.CalcClipAngle(this.c, this.etl, this.st, out var t1);
            //0 < T0_T1_Angle < 180
            clipAngle = (clipAngle + 180) % 180;

            this.ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
            this.ClippingPlane.transform.position = this.BookPanel.TransformPoint(t1);

            //page position and angle
            this.Left.transform.position = this.BookPanel.TransformPoint(this.c);
            float C_T1_dy = t1.y - this.c.y;
            float C_T1_dx = t1.x - this.c.x;
            float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
            this.Left.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - 90 - clipAngle);

            this.NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
            this.NextPageClip.transform.position = this.BookPanel.TransformPoint(t1);
            this.LeftNext.transform.SetParent(this.NextPageClip.transform, true);
            this.Right.transform.SetParent(this.ClippingPlane.transform, true);
            this.Right.transform.SetAsFirstSibling();

            this.ShadowLTR.rectTransform.SetParent(this.ShadowMaskLeft.transform, true);
        }

        public void UpdateBookTRTLToPoint(Vector3 followLocation)
        {
            this.mode = FlipMode.Top_RightToLeft;
            this.f = followLocation;
            this.Shadow.transform.SetParent(this.ClippingPlane.transform, true);
            this.Shadow.transform.localPosition = Vector3.zero;
            this.Shadow.transform.localEulerAngles = Vector3.zero;
            this.Right.transform.SetParent(this.ClippingPlane.transform, true);

            this.Left.transform.SetParent(this.BookPanel.transform, true);
            this.Left.transform.localEulerAngles = Vector3.zero;
            this.RightNext.transform.SetParent(this.BookPanel.transform, true);
            this.c = this.Calc_C_Position(followLocation, this.st, this.sb);
            float clipAngle = this.CalcClipAngle(this.c, this.etr, this.st, out var t1);
            if (clipAngle > -90) clipAngle += 180;
            this.ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
            this.ClippingPlane.transform.position = this.BookPanel.TransformPoint(t1);

            //page position and angle
            this.Right.transform.position = this.BookPanel.TransformPoint(this.c);
            float C_T1_dy = t1.y - this.c.y;
            float C_T1_dx = t1.x - this.c.x;
            float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
            this.Right.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - (clipAngle + 90));

            this.NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
            this.NextPageClip.transform.position = this.BookPanel.TransformPoint(t1);
            this.RightNext.transform.SetParent(this.NextPageClip.transform, true);
            this.Left.transform.SetParent(this.ClippingPlane.transform, true);
            this.Left.transform.SetAsFirstSibling();

            this.Shadow.rectTransform.SetParent(this.ShadowMaskRight.transform, true);
        }

        private float CalcClipAngle(Vector3 c, Vector3 bookCorner, Vector3 s, out Vector3 t1)
        {
            Vector3 t0 = (c + bookCorner) / 2;
            float T0_CORNER_dy = bookCorner.y - t0.y;
            float T0_CORNER_dx = bookCorner.x - t0.x;
            float T0_CORNER_Angle = Mathf.Atan2(T0_CORNER_dy, T0_CORNER_dx);

            float T1_X = t0.x - T0_CORNER_dy * Mathf.Tan(T0_CORNER_Angle);
            T1_X = this.normalizeT1X(T1_X, bookCorner, s);
            t1 = new Vector3(T1_X, s.y, 0);

            //clipping plane angle=T0_T1_Angle
            float T0_T1_dy = t1.y - t0.y;
            float T0_T1_dx = t1.x - t0.x;
            var T0_T1_Angle = Mathf.Atan2(T0_T1_dy, T0_T1_dx) * Mathf.Rad2Deg;
            return T0_T1_Angle;
        }

        private float normalizeT1X(float t1, Vector3 corner, Vector3 s)
        {
            if (t1 > s.x && s.x > corner.x)
                return s.x;
            if (t1 < s.x && s.x < corner.x)
                return s.x;
            return t1;
        }

        private Vector3 Calc_C_Position(Vector3 followLocation, Vector3 s1, Vector3 s2)
        {
            // 找到书页叫角最远可的达点
            Vector3 c;
            this.f = followLocation;
            var dir = this.f - s1;
            var dis = dir.magnitude;
            if (dis < this.bottomRadius1)
                c = this.f;
            else
                c = (dir.normalized * this.bottomRadius1) + s1;

            dir = c - s2;
            dis = dir.magnitude;
            if (dis > this.bottomRadius2)
                c = (dir.normalized * this.bottomRadius2) + s2;
            return c;
        }

        public void DragBottomRightPageToPoint(Vector3 point, int nPageID)
        {
            if (this.mIsPageReleasing) return;
            if (this.mCurrentPage + 2 > this.MaxPageID) return;
            this.PageDragging = true;
            this.NextPageID = nPageID;
            this.OnBeginFlip?.Invoke();
            this.mode = FlipMode.Bottom_RightToLeft;
            this.f = point;

            this.NextPageClip.rectTransform.pivot = new Vector2(0f, 0.12f);
            this.ClippingPlane.rectTransform.pivot = new Vector2(1f, 0.35f);
            this.Right.pivot = new Vector2(0f, 0f);
            this.Left.pivot = new Vector2(0f, 0f);
            this.Shadow.rectTransform.pivot = new Vector2(1f, 0f);
            this.ShadowLTR.rectTransform.pivot = new Vector2(0f, 0f);

            this.Left.gameObject.SetActive(true);
            this.Left.transform.position = this.RightNext.transform.position;
            this.Left.transform.eulerAngles = new Vector3(0, 0, 0);
            this.SetPageContent(this.Left, this.mCurrentPage + 1, PageType.Left);
            this.Left.transform.SetAsFirstSibling();

            this.Right.gameObject.SetActive(true);
            this.Right.transform.position = this.RightNext.transform.position;
            this.Right.transform.eulerAngles = new Vector3(0, 0, 0);
            this.SetPageContent(this.Right, this.NextPageID, PageType.Right);

            this.SetPageContent(this.RightNext, this.NextPageID + 1, PageType.RightNext);

            this.LeftNext.transform.SetAsFirstSibling();
            this.ShadowClipingR.gameObject.SetActive(true);
            if (this.enableShadowEffect)
            {
                this.Shadow.gameObject.SetActive(true);
                this.ShadowRBorder.gameObject.SetActive(true);
            }
            this.UpdateBookBRTLToPoint(this.f);
        }

        public void OnMouseDragBottomRightPage()
        {
            if (this.Interactable)
            {
                LogManager.Log($"[JumpBook] OnMouseDragBottomRightPage");
                this.DragBottomRightPageToPoint(this.transformPoint(Input.mousePosition), this.CurrentPage + 2);
            }
        }

        public void DragBottomLeftPageToPoint(Vector3 point, int nPageID)
        {
            if (this.mIsPageReleasing) return;
            if (this.mCurrentPage <= this.MinPageID) return;
            this.PageDragging = true;
            this.NextPageID = nPageID;
            this.OnBeginFlip?.Invoke();
            this.mode = FlipMode.Bottom_LeftToRight;
            this.f = point;

            this.NextPageClip.rectTransform.pivot = new Vector2(1, 0.12f);
            this.ClippingPlane.rectTransform.pivot = new Vector2(0, 0.35f);
            this.Right.pivot = new Vector2(0f, 0f);
            this.Left.pivot = new Vector2(1f, 0f);
            this.Shadow.rectTransform.pivot = new Vector2(1f, 0f);
            this.ShadowLTR.rectTransform.pivot = new Vector2(0f, 0f);

            this.Right.gameObject.SetActive(true);
            this.Right.transform.position = this.LeftNext.transform.position;
            this.SetPageContent(this.Right, this.mCurrentPage, PageType.Right);
            this.Right.transform.eulerAngles = new Vector3(0, 0, 0);
            this.Right.transform.SetAsFirstSibling();

            this.Left.gameObject.SetActive(true);
            this.Left.transform.position = this.LeftNext.transform.position;
            this.Left.transform.eulerAngles = new Vector3(0, 0, 0);
            this.SetPageContent(this.Left, this.mCurrentPage - 1, PageType.Left);

            this.SetPageContent(this.LeftNext, this.mCurrentPage - 2, PageType.LeftNext);

            this.RightNext.transform.SetAsFirstSibling();
            this.ShadowClipingL.gameObject.SetActive(true);
            if (this.enableShadowEffect)
            {
                this.ShadowLTR.gameObject.SetActive(true);
                this.ShadowLBorder.gameObject.SetActive(true);
            }
            this.UpdateBookBLTRToPoint(this.f);
        }

        public void OnMouseDragBottomLeftPage()
        {
            if (this.Interactable)
            {
                LogManager.Log("[JumpBook] OnMouseDragBottomLeftPage");
                this.DragBottomLeftPageToPoint(this.transformPoint(Input.mousePosition), this.CurrentPage - 2);
            }
        }

        public void DragTopRightPageToPoint(Vector3 point, int nPageID)
        {
            if (this.mIsPageReleasing) return;
            if (this.mCurrentPage + 2 > this.MaxPageID) return;
            this.PageDragging = true;
            this.NextPageID = nPageID;
            this.OnBeginFlip?.Invoke();
            this.mode = FlipMode.Top_RightToLeft;
            this.f = point;

            this.NextPageClip.rectTransform.pivot = new Vector2(0f, 0.88f);
            this.ClippingPlane.rectTransform.pivot = new Vector2(1f, 0.65f);
            this.Right.pivot = new Vector2(0f, 1f);
            this.Left.pivot = new Vector2(0f, 0f);
            this.Shadow.rectTransform.pivot = new Vector2(1f, 1f);
            this.ShadowLTR.rectTransform.pivot = new Vector2(0f, 1f);

            this.Left.gameObject.SetActive(true);
            this.Left.transform.position = this.RightNext.transform.position;
            this.Left.transform.eulerAngles = new Vector3(0, 0, 0);
            this.SetPageContent(this.Left, this.mCurrentPage + 1, PageType.Left);
            this.Left.transform.SetAsFirstSibling();

            this.Right.gameObject.SetActive(true);
            this.Right.transform.position = this.RightNext.transform.position;
            this.Right.transform.eulerAngles = new Vector3(0, 0, 0);
            this.SetPageContent(this.Right, this.NextPageID, PageType.Right);

            this.SetPageContent(this.RightNext, this.NextPageID + 1, PageType.RightNext);

            this.LeftNext.transform.SetAsFirstSibling();
            this.ShadowClipingR.gameObject.SetActive(true);
            if (this.enableShadowEffect)
            {
                this.Shadow.gameObject.SetActive(true);
                this.ShadowRBorder.gameObject.SetActive(true);
            }
            this.UpdateBookTRTLToPoint(this.f);
        }

        public void OnMouseDragTopRightPage()
        {
            if (this.Interactable)
            {
                LogManager.Log($"[JumpBook] OnMouseDragTopRightPage");
                this.DragTopRightPageToPoint(this.transformPoint(Input.mousePosition), this.CurrentPage + 2);
            }
        }

        public void DragTopLeftPageToPoint(Vector3 point, int nPageID)
        {
            if (this.mIsPageReleasing) return;
            if (this.mCurrentPage <= this.MinPageID) return;
            this.PageDragging = true;
            this.NextPageID = nPageID;
            this.OnBeginFlip?.Invoke();
            this.mode = FlipMode.Top_LeftToRight;
            this.f = point;

            this.NextPageClip.rectTransform.pivot = new Vector2(1f, 0.88f);
            this.ClippingPlane.rectTransform.pivot = new Vector2(0f, 0.65f);
            this.Right.pivot = new Vector2(0f, 0f);
            this.Left.pivot = new Vector2(1f, 1f);
            this.Shadow.rectTransform.pivot = new Vector2(1f, 1f);
            this.ShadowLTR.rectTransform.pivot = new Vector2(0f, 1f);

            this.Right.gameObject.SetActive(true);
            this.Right.transform.position = this.LeftNext.transform.position;
            this.SetPageContent(this.Right, this.mCurrentPage, PageType.Right);
            this.Right.transform.eulerAngles = new Vector3(0, 0, 0);
            this.Right.transform.SetAsFirstSibling();

            this.Left.gameObject.SetActive(true);
            this.Left.transform.position = this.LeftNext.transform.position;
            this.Left.transform.eulerAngles = new Vector3(0, 0, 0);
            this.SetPageContent(this.Left, this.mCurrentPage - 1, PageType.Left);

            this.SetPageContent(this.LeftNext, this.mCurrentPage - 2, PageType.LeftNext);

            this.RightNext.transform.SetAsFirstSibling();
            this.ShadowClipingL.gameObject.SetActive(true);
            if (this.enableShadowEffect)
            {
                this.ShadowLTR.gameObject.SetActive(true);
                this.ShadowLBorder.gameObject.SetActive(true);
            }
            this.UpdateBookTLTRToPoint(this.f);
        }

        public void OnMouseDragTopLeftPage()
        {
            if (this.Interactable)
            {
                LogManager.Log($"[JumpBook] OnMouseDragTopRightPage");
                this.DragTopLeftPageToPoint(this.transformPoint(Input.mousePosition), this.CurrentPage - 2);
            }
        }

        public void OnMouseRelease()
        {
            if (this.Interactable)
            {
                LogManager.Log("[JumpBook] OnMouseRelease");
                this.StartCoroutine(this.ReleasePage(false));
            }
        }

        public void OnMouseExit()
        {
            if (this.Interactable)
            {
                LogManager.Log("[JumpBook] OnMouseExit");
                this.StartCoroutine(this.ReleasePage(false));
            }
        }

        public void OnMouseUp()
        {
            if (this.Interactable)
            {
                LogManager.Log("[JumpBook] OnMouseUp");
                this.StartCoroutine(this.ReleasePage(false));
            }
        }

        public IEnumerator ReleasePage(bool bIsContinue)
        {
            if (this.mIsPageReleasing) yield break;
            this.mIsPageReleasing = true;
            LogManager.Log("[JumpBook] ReleasePage");
            if (this.PageDragging || bIsContinue)
            {
                this.PageDragging = false;
                var allLength = Vector2.Distance(this.ebl, this.ebr);
                var isBack = false;

                if (this.mode == FlipMode.Bottom_RightToLeft)
                {
                    var rightLength = Vector2.Distance(this.c, this.ebr);
                    if (rightLength < allLength * this.TurnPageRate)
                    {
                        isBack = true;
                    }
                }
                else if (this.mode == FlipMode.Bottom_LeftToRight)
                {
                    var leftLength = Vector2.Distance(this.c, this.ebl);
                    if (leftLength < allLength * this.TurnPageRate)
                    {
                        isBack = true;
                    }
                }
                else if (this.mode == FlipMode.Top_RightToLeft)
                {

                    var rightLength = Vector2.Distance(this.c, this.etr);
                    if (rightLength < allLength * this.TurnPageRate)
                    {
                        isBack = true;
                    }
                }
                else if (this.mode == FlipMode.Top_LeftToRight)
                {
                    var leftLength = Vector2.Distance(this.c, this.etl);
                    if (leftLength < allLength * this.TurnPageRate)
                    {
                        isBack = true;
                    }
                }
                if (isBack)
                {
                    yield return this.TweenBack(bIsContinue);
                }
                else
                {
                    yield return this.TweenForward(bIsContinue);
                }
            }
            this.mIsPageReleasing = false;
        }

        void UpdateSprites()
        {
            LogManager.Log($"[Jump Book] UpdateSprites + this.mCurrentPage ={this.mCurrentPage}");
            if (this.ShadowClipingL.gameObject.activeSelf)
                this.ShadowClipingL.gameObject.SetActive(false);
            if (this.ShadowClipingR.gameObject.activeSelf)
                this.ShadowClipingR.gameObject.SetActive(false);
            this.SetPageContent(this.LeftNext, this.mCurrentPage, PageType.LeftNext);
            this.SetPageContent(this.RightNext, this.mCurrentPage + 1, PageType.RightNext);
        }

        public IEnumerator TweenForward(bool bIsContinue)
        {
            LogManager.Log($"[Jump Book] TweenForward");
            float fDuring = bIsContinue ? 0f : 0.15f;
            if (this.mode == FlipMode.Bottom_RightToLeft)
            {
                yield return this.TweenTo(this.ebl, fDuring, () =>
                {
                    this.Flip();
                    if (!bIsContinue) this.OnEndFlip?.Invoke();
                });
            }
            else if (this.mode == FlipMode.Bottom_LeftToRight)
            {
                yield return this.TweenTo(this.ebr, fDuring, () =>
                {
                    this.Flip();
                    if (!bIsContinue) this.OnEndFlip?.Invoke();
                });
            }
            else if (this.mode == FlipMode.Top_RightToLeft)
            {
                yield return this.TweenTo(this.etl, fDuring, () =>
                {
                    this.Flip();
                    if (!bIsContinue) this.OnEndFlip?.Invoke();
                });
            }
            else if (this.mode == FlipMode.Top_LeftToRight)
            {
                yield return this.TweenTo(this.etr, fDuring, () =>
                {
                    this.Flip();
                    if (!bIsContinue) this.OnEndFlip?.Invoke();
                });
            }
        }

        void Flip()
        {
            this.LeftNext.transform.SetParent(this.BookPanel.transform, true);
            this.Left.transform.SetParent(this.BookPanel.transform, true);
            this.LeftNext.transform.SetParent(this.BookPanel.transform, true);
            this.Left.gameObject.SetActive(false);
            this.Right.gameObject.SetActive(false);
            this.Right.transform.SetParent(this.BookPanel.transform, true);
            this.RightNext.transform.SetParent(this.BookPanel.transform, true);
            // this.UpdateSprites();
            this.CurrentPage = this.NextPageID;
            this.Shadow.gameObject.SetActive(false);
            this.ShadowLTR.gameObject.SetActive(false);
            this.ShadowRBorder.gameObject.SetActive(false);
            this.ShadowLBorder.gameObject.SetActive(false);
            if (this.OnFlip != null)
                this.OnFlip.Invoke();
        }

        public IEnumerator TweenBack(bool bIsContinue)
        {
            LogManager.Log($"[Jump Book] TweenBack");
            if (this.mode == FlipMode.Bottom_RightToLeft)
            {
                yield return this.TweenTo(this.ebr, 0.15f,
                     () =>
                     {
                         this.UpdateSprites();
                         this.RightNext.transform.SetParent(this.BookPanel.transform);
                         this.Right.transform.SetParent(this.BookPanel.transform);

                         this.Left.gameObject.SetActive(false);
                         this.Right.gameObject.SetActive(false);
                         this.PageDragging = false;
                         this.OnEndFlip?.Invoke();
                     });
            }
            else if (this.mode == FlipMode.Bottom_LeftToRight)
            {
                yield return this.TweenTo(this.ebl, 0.15f,
                    () =>
                    {
                        this.UpdateSprites();

                        this.LeftNext.transform.SetParent(this.BookPanel.transform);
                        this.Left.transform.SetParent(this.BookPanel.transform);

                        this.Left.gameObject.SetActive(false);
                        this.Right.gameObject.SetActive(false);
                        this.PageDragging = false;
                        this.OnEndFlip?.Invoke();
                    }
                    );
            }
            else if (this.mode == FlipMode.Top_RightToLeft)
            {
                yield return this.TweenTo(this.etr, 0.15f,
                     () =>
                     {
                         this.UpdateSprites();
                         this.RightNext.transform.SetParent(this.BookPanel.transform);
                         this.Right.transform.SetParent(this.BookPanel.transform);

                         this.Left.gameObject.SetActive(false);
                         this.Right.gameObject.SetActive(false);
                         this.PageDragging = false;
                         this.OnEndFlip?.Invoke();
                     });
            }
            else if (this.mode == FlipMode.Top_LeftToRight)
            {
                yield return this.TweenTo(this.etl, 0.15f,
                    () =>
                    {
                        this.UpdateSprites();

                        this.LeftNext.transform.SetParent(this.BookPanel.transform);
                        this.Left.transform.SetParent(this.BookPanel.transform);

                        this.Left.gameObject.SetActive(false);
                        this.Right.gameObject.SetActive(false);
                        this.PageDragging = false;
                        this.OnEndFlip?.Invoke();
                    }
                    );
            }
        }

        public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish)
        {
            int steps = (int)(duration / 0.025f);
            Vector3 displacement = (to - this.f) / steps;
            for (int i = 0; i < steps - 1; i++)
            {
                if (this.mode == FlipMode.Bottom_RightToLeft)
                    this.UpdateBookBRTLToPoint(this.f + displacement);
                else if (this.mode == FlipMode.Bottom_LeftToRight)
                    this.UpdateBookBLTRToPoint(this.f + displacement);
                else if (this.mode == FlipMode.Top_RightToLeft)
                    this.UpdateBookTRTLToPoint(this.f + displacement);
                else if (this.mode == FlipMode.Top_LeftToRight)
                    this.UpdateBookTLTRToPoint(this.f + displacement);

                yield return new WaitForSeconds(0.025f);
            }
            onFinish?.Invoke();
        }

        public void SetPageContent(RectTransform rPageRoot, int pageIndex, PageType rPageType)
        {
            this.OnSetPageContent?.Invoke(rPageRoot, pageIndex, rPageType);
        }
    }
}
