using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace UnityEngine.UI
{
    public class MoveAndScale : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        public RectTransform TargetTrans;


        public float MaxScale = 2f; //最大缩放
        public float MinScale = 1f;  //最小缩放

        //#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        public float ScaleSpeed = 20f;//缩放速度
                                      //#elif UNITY_ANDROID || UNITY_IPHONE
        public float FingerScaleSpeed = 0.01f;//缩放速度

        public float PreFingerDistance = -1;//手指间距

        public float LerpTime = 0.1f;

        private float mEndScale = 1.0f;   //当前缩放

        private float mCurScale = 1.0f;

        public bool IsScale = false;




        public float MoveSpeed = 1f;
        public Vector2 PositionRangeX = new Vector2(-242, 242); //Target位置X的范围
        public Vector2 PositionRangeY = new Vector2(-242, 242);  //Target位置Y的范围
        public float DragDistanceX = 0;
        public float DragDistanceY = 0;

        private Vector2 DragStartPositon = Vector2.zero;


        private Vector3 mStartAnchorPos;

        private Vector3 mEndAnchorPos;
        private void Awake()
        {
            if (!this.TargetTrans)
            {
                this.TargetTrans = this.GetComponent<RectTransform>();
            }
            this.mStartAnchorPos = this.TargetTrans.anchoredPosition;
            this.mEndAnchorPos = this.mStartAnchorPos;

            this.mEndScale = this.TargetTrans.localScale.x;
            this.mCurScale = this.mEndScale;
        }
        private void Update()
        {
            if (!this.TargetTrans) return;
            if (!this.IsScale) return;
            //缩放
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            this.mEndScale += Time.deltaTime * this.ScaleSpeed;
            if (this.mEndScale > this.MaxScale)
            {
                this.mEndScale = this.MaxScale;
            }
        }


        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            this.mEndScale -= Time.deltaTime * this.ScaleSpeed;
            if (this.mEndScale < this.MinScale)
            {
                this.mEndScale = this.MinScale;
            }
        }

#elif UNITY_ANDROID || UNITY_IPHONE
        //刚好两个指头
        if (Input.touchCount == 2)
        {
            //进行缩放
            float fCurFingerDis = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
            if (this.PreFingerDistance == -1)
            {
                this.PreFingerDistance = fCurFingerDis;
            }


            float fCurRes = fCurFingerDis - this.PreFingerDistance;//与上一帧比较变化

            this.mEndScale += fCurRes * this.FingerScaleSpeed;
            if (this.mEndScale < this.MinScale)
            {
                this.mEndScale = this.MinScale;

            }

            if (this.mEndScale > this.MaxScale)
            {

                this.mEndScale = this.MaxScale;
            }

            this.PreFingerDistance = fCurFingerDis;
        }
        else
        {
            this.PreFingerDistance = -1;
        }
#endif
            if (this.mEndScale != this.mCurScale)
            {
                this.mCurScale = Mathf.Lerp(this.mCurScale, this.mEndScale, 0.05f);

                this.TargetTrans.localScale = Vector3.one * this.mCurScale;
            }
            this.UpdatePositionLimit();
            this.TargetTrans.anchoredPosition = Vector3.Lerp(this.TargetTrans.anchoredPosition, this.mEndAnchorPos, LerpTime);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Input.touchCount == 2) return;

            this.DragDistanceX = eventData.position.x - this.DragStartPositon.x;
            this.DragDistanceY = eventData.position.y - this.DragStartPositon.y;
            var rPositionX = this.mStartAnchorPos.x + this.DragDistanceX * this.MoveSpeed;
            if (rPositionX < this.PositionRangeX.x)
            {
                rPositionX = this.PositionRangeX.x;
            }
            else if (rPositionX > this.PositionRangeX.y)
            {
                rPositionX = this.PositionRangeX.y;
            }

            var rPositionY = this.mStartAnchorPos.y + this.DragDistanceY * this.MoveSpeed;
            if (rPositionY < this.PositionRangeY.x)
            {
                rPositionY = this.PositionRangeY.x;
            }
            else if (rPositionY > this.PositionRangeY.y)
            {
                rPositionY = this.PositionRangeY.y;
            }

            this.TargetTrans.anchoredPosition = new Vector3(rPositionX, rPositionY, this.mStartAnchorPos.z);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Input.touchCount == 2) return;
            this.DragStartPositon = eventData.position;
            this.DragDistanceX = 0;
            this.DragDistanceY = 0;
            this.mStartAnchorPos = this.TargetTrans.anchoredPosition;
            this.mEndAnchorPos = this.mStartAnchorPos;
            this.UpdatePositionLimit();
        }

        public void UpdatePositionLimit()
        {
            var rSizeX = (this.TargetTrans.sizeDelta.x * this.mCurScale - Screen.width) / 2.0f;
            this.PositionRangeX = new Vector2(-rSizeX, rSizeX);


            var rSizeY = (this.TargetTrans.sizeDelta.y * this.mCurScale - Screen.height) / 2.0f;
            this.PositionRangeY = new Vector2(-rSizeY, rSizeY);

            var rPositionX = this.TargetTrans.anchoredPosition.x;
            if (rPositionX < this.PositionRangeX.x)
            {
                rPositionX = this.PositionRangeX.x;
            }
            else if (rPositionX > this.PositionRangeX.y)
            {
                rPositionX = this.PositionRangeX.y;
            }

            var rPositionY = this.TargetTrans.anchoredPosition.y;
            if (rPositionY < this.PositionRangeY.x)
            {
                rPositionY = this.PositionRangeY.x;
            }
            else if (rPositionY > this.PositionRangeY.y)
            {
                rPositionY = this.PositionRangeY.y;
            }
            this.mEndAnchorPos = new Vector3(rPositionX, rPositionY, this.mStartAnchorPos.z);
        }
    }
}
