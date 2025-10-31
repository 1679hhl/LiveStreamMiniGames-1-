using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knight.Core;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class TypeWriter : MonoBehaviour
    {
        public  Text        Text       = null;

        [SerializeField]    
        private float       mCurTime   = 0.0f;
        [SerializeField]    
        private float       mTotalTime = 0.0f;
        [SerializeField]    
        private string      mContent   = "";

        public float TotalTime
        {
            get { return this.mTotalTime;   }
            set { this.mTotalTime = value;  }
        }

        public string Content
        {
            get { return this.mContent;     }
            set { this.mContent = value;    }
        }

        public float CurTime
        {
            get { return this.mCurTime;     }
            set
            {
                this.mCurTime = value;
                this.UpdateAnim(this.mCurTime);
            }
        }
            
        private void Awake()
        {
            this.Text = this.GetComponent<Text>();
            this.mCurTime = 0.0f;
        }

        public void UpdateAnim(float fTime)
        {
            if (string.IsNullOrEmpty(this.mContent)) return;

            // 计算显示到第几个字
            var fIntervalTime = this.mTotalTime / this.mContent.Length;
            var nSubStrLength = (int)(fTime / fIntervalTime) + 1;

            if (nSubStrLength > this.mContent.Length) return;
            this.Text.text = this.mContent.Substring(0, nSubStrLength);
        }
    }
}

