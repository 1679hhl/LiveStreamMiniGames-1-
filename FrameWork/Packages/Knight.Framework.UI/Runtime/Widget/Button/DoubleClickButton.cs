using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/DoubleClickButton")]
    public class DoubleClickButton : MonoBehaviour,IPointerClickHandler
    {
        //默认双击时间间隔
        public float doubleClickInterval = 0.5f;
        [Serializable]
        public class ButtonClickedEvent : UnityEvent<bool>
        {
        }
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
        public ButtonClickedEvent onClick
        {
            get
            {
                return this.m_OnClick;
            }
            set
            {
                this.m_OnClick = value;
            }
        }
        //是否有一次单击
        private int mClickCount = 0;
        //计时器
        private float mTempTimer = 0;

        private void Update()
        {
            if (this.mClickCount == 1)
            {
                this.mTempTimer += Time.deltaTime;
                if (this.mTempTimer >= this.doubleClickInterval)
                {
                    this.mClickCount = 0;
                    this.mTempTimer = 0;
                }
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            ++ this.mClickCount;
            //添加单击事件
            this.onClick?.Invoke(false);
            if(this.mClickCount == 2)
            {
                if(this.mTempTimer < this.doubleClickInterval)
                {
                    //完成双击
                    this.onClick?.Invoke(true);
                    this.mClickCount = 0;
                    this.mTempTimer = 0;
                }
                else
                {
                    this.mTempTimer = 0;
                    this.mClickCount = 1;
                }
            }
        }
    }
}
