using System;
using System.Collections.Generic;
using UnityEngine;
using Knight.Core;

namespace UnityEngine.UI
{
    [ExecuteInEditMode]
    public class GrayGraphics : MonoBehaviour
    {
        [SerializeField]
        private bool mIsGray = true;
        [SerializeField]
        private bool mIsGrayGrad;
        public Material GrayMat;

        [NaughtyAttributes.ReorderableList]
        public List<Graphic> WillGrayGraphics;
        [SerializeField]
        private float mDarkNessValue = 1;
        [SerializeField]
        private float mGrayFactor = 1;

        public float DarkNessValue
        {
            get { return this.mDarkNessValue; }
            set
            {
                this.mDarkNessValue = value;
            }
        }

        public float GrayFactor
        {
            get { return this.mGrayFactor; }
            set
            {
                this.mGrayFactor = value;
                this.RuningChangePara();
            }
        }

        public bool IsGray
        {
            get { return this.mIsGray; }
            set
            {
                this.SetGray(value);
            }
        }

        public bool IsGrayDisable
        {
            get { return !this.mIsGray; }
            set
            {
                this.SetGray(!value);
            }
        }

        private void Awake()
        {
            if (this.GrayMat != null)
                GameObject.DestroyImmediate(this.GrayMat);
            this.GrayMat = new Material(Shader.Find("UI/Default_Gray"));

            var rGraphics = this.GetComponents<Graphic>();
            for (int i = 0; i < rGraphics.Length; i++)
            {
                rGraphics[i].material = this.GrayMat;
                SetGraphicGray(rGraphics[i], this.mIsGray);
            }

            if (this.WillGrayGraphics == null)
                this.WillGrayGraphics = new List<Graphic>();
            for (int i = 0; i < this.WillGrayGraphics.Count; i++)
            {
                if (this.WillGrayGraphics[i])
                {
                    this.WillGrayGraphics[i].material = this.GrayMat;
                    SetGraphicGray(this.WillGrayGraphics[i], this.mIsGray);
                }
            }
        }

        public void SetGray(bool bIsGray)
        {
            if (this.mIsGray == bIsGray)
            {
                return;
            }

            if (!this.mIsGrayGrad)
                this.mIsGray = bIsGray;
            var rGraphics = this.GetComponents<Graphic>();
            for (int i = 0; i < rGraphics.Length; i++)
            {
                rGraphics[i].material = this.GrayMat;
                SetGraphicGray(rGraphics[i], this.mIsGray);
            }

            for (int i = 0; i < this.WillGrayGraphics.Count; i++)
            {
                this.WillGrayGraphics[i].material = this.GrayMat;
                SetGraphicGray(this.WillGrayGraphics[i], this.mIsGray);
            }
        }

        private void OnDestroy()
        {
            var rGraphics = this.GetComponents<Graphic>();
            for (int i = 0; i < rGraphics.Length; i++)
            {
                rGraphics[i].material = null;
            }
            for (int i = 0; i < this.WillGrayGraphics.Count; i++)
            {
                if (this.WillGrayGraphics[i])
                    this.WillGrayGraphics[i].material = null;
            }
            this.WillGrayGraphics.Clear();

            if (this.GrayMat != null)
                GameObject.DestroyImmediate(this.GrayMat);
            this.GrayMat = null;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            var rGraphics = this.GetComponents<Graphic>();
            for (int i = 0; i < rGraphics.Length; i++)
            {
                SetGraphicGray(rGraphics[i], this.mIsGray);
            }

            for (int i = 0; i < this.WillGrayGraphics.Count; i++)
            {
                if (this.WillGrayGraphics[i])
                    this.WillGrayGraphics[i].material = this.GrayMat;
                SetGraphicGray(this.WillGrayGraphics[i], this.mIsGray);
            }
        }

        [NaughtyAttributes.Button]
        private void GetGraphicsInChildren()
        {
            this.WillGrayGraphics = new List<Graphic>(GetComponentsInChildren<Graphic>(true));
        }
#endif

        private void SetGraphicGray(Graphic rGraphic, bool bIsGray)
        {
            if (!rGraphic)
                return;

            if (bIsGray)
            {
                //rGraphic.material.EnableKeyword("UI_GRAY");
                rGraphic.material.SetFloat("_UseUIGray", 1);
                rGraphic.material.SetFloat("_Darkness", this.DarkNessValue);
                if (this.mIsGrayGrad)
                    rGraphic.material.SetFloat("_GrayFactor", this.GrayFactor);
            }
            else
            {
                //rGraphic.material.DisableKeyword("UI_GRAY");
                rGraphic.material.SetFloat("_UseUIGray", 0);
            }


            // 奇葩的Bug
            rGraphic.enabled = false;
            rGraphic.enabled = true;
        }

        private void RuningChangePara()
        {

            var rGraphics = this.GetComponents<Graphic>();
            for (int i = 0; i < rGraphics.Length; i++)
            {
                SetGraphicGray(rGraphics[i], this.mIsGray);
            }

            for (int i = 0; i < this.WillGrayGraphics.Count; i++)
            {
                this.WillGrayGraphics[i].material = this.GrayMat;
                SetGraphicGray(this.WillGrayGraphics[i], this.mIsGray);
            }
        }




    }
}
