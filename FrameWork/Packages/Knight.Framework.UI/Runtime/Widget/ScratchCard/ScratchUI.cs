using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Serialization;
namespace UnityEngine.UI
{
   public class ScratchUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Canvas m_Canvas;
        public RectTransform m_RectTrans;
        public RenderTexture m_RT;
        public Shader shader;
        public Texture m_BrushTex;

        [Range(0.1f, 3)]
        public float m_BrushScale = 1;
        [Range(0.01f, 1)]
        public float m_ScaleSize = 0.1f;
        public int m_LRBorder = 10;
        public int m_UBBorder = 5;
        public bool previewCore = false;

        #region 
        private Material m_BrushMat;
        private RenderTexture m_BrushScaleRT;
        private Texture2D m_RT2D;
        private Texture2D m_BrushRect;
        private Rect rectRT;
        private bool m_MouseMoved = false;
        private int m_PixelNum;
        private int width;
        private int height;
        private int m_UpBorder;
        private int m_RightBorder;
        private float m_OldBrushScale;

        private Vector2 m_LastPos;
        private Vector2 m_CurrentPos;
        private List<Vector2> m_MiddlePosFloat = new List<Vector2>();
        private int m_MaxPixelNum;
        #endregion
        private bool mIsActionOver;
        [Serializable]
        public class ScratchEvent : UnityEvent
        {
        }
        [FormerlySerializedAs("onClick")]
        [SerializeField]
        private ScratchEvent m_OnScratchOver = new ScratchEvent();
        public ScratchEvent onScratchOver
        {
            get
            {
                return this.m_OnScratchOver;
            }
            set
            {
                this.m_OnScratchOver = value;
            }
        }
        private void Start()
        {
            InitRT();
            this.mIsActionOver = false;
        }

        private void InitRT()
        {
            width = m_RT.width;
            height = m_RT.height;
            Graphics.Blit(Texture2D.blackTexture, m_RT);
            m_RT2D = new Texture2D(width, height, TextureFormat.ARGB32, false);

            m_BrushScaleRT = RenderTexture.GetTemporary(Mathf.FloorToInt(m_BrushTex.width * m_BrushScale),
                Mathf.FloorToInt(m_BrushTex.height * m_BrushScale), 0, GraphicsFormat.R8G8B8A8_UNorm);

            m_MaxPixelNum = m_BrushScaleRT.width >= m_BrushScaleRT.height ?
                Mathf.FloorToInt(m_BrushScaleRT.width * m_ScaleSize) : Mathf.FloorToInt(m_BrushScaleRT.height * m_ScaleSize);
            m_MaxPixelNum = Mathf.Max(m_MaxPixelNum, 1);
            m_BrushRect = new Texture2D(m_BrushScaleRT.width, m_BrushScaleRT.height, TextureFormat.ARGB32, false);

            m_OldBrushScale = m_BrushScale;
            Graphics.Blit(m_BrushTex, m_BrushScaleRT);

            rectRT = new Rect(0, 0, width, height);
            int coreWidth = width - 2 * m_LRBorder;
            int coreHeight = height - 2 * m_UBBorder;
            m_PixelNum = coreWidth * coreHeight;
            m_UpBorder = m_UBBorder + coreHeight;
            m_RightBorder = m_LRBorder + coreWidth;

            if (previewCore)
            {
                PreviewCore();
            }

            InitMaterial();
        }

        private void InitMaterial()
        {
            if (shader == null)
            {
                Debug.LogError("please give a shader");
                return;
            }
            m_BrushMat = new Material(shader);
        }

        public void OnPointerDown(PointerEventData data)
        {
            if (this.mIsActionOver) return;
            m_MouseMoved = true;
            m_LastPos = Input.mousePosition;
        }

        public void OnPointerUp(PointerEventData data)
        {
            m_MouseMoved = false;
            if (this.mIsActionOver)
            {
                Graphics.Blit(Texture2D.whiteTexture, m_RT);
                this.m_OnScratchOver?.Invoke();
            }
        }

        private void Update()
        {
            if (m_MouseMoved)
            {
                if (m_OldBrushScale != m_BrushScale)
                {
                    RenderTexture.ReleaseTemporary(m_BrushScaleRT);
                    m_BrushScaleRT = RenderTexture.GetTemporary(Mathf.FloorToInt(m_BrushTex.width * m_BrushScale),
                        Mathf.FloorToInt(m_BrushTex.height * m_BrushScale), 0, GraphicsFormat.R8G8B8A8_UNorm);
                    m_MaxPixelNum = m_BrushScaleRT.width >= m_BrushScaleRT.height ?
                        Mathf.FloorToInt(m_BrushScaleRT.width * m_ScaleSize) : Mathf.FloorToInt(m_BrushScaleRT.height * m_ScaleSize);
                    m_MaxPixelNum = Mathf.Max(m_MaxPixelNum, 1);
                    m_BrushRect = new Texture2D(m_BrushScaleRT.width, m_BrushScaleRT.height, TextureFormat.ARGB32, false);
                    m_OldBrushScale = m_BrushScale;
                    Graphics.Blit(m_BrushTex, m_BrushScaleRT);
                }

                InitList();
                CheckRT();
            }
        }

        private void InitList()
        {
            m_CurrentPos = Input.mousePosition;
            m_MiddlePosFloat.Clear();
            Vector2Int lastPos = ComputeUIPos(m_LastPos);
            Vector2Int current = ComputeUIPos(m_CurrentPos);
            int dValueX = current.x - lastPos.x;
            int dValueY = current.y - lastPos.y;
            if (Mathf.Abs(dValueX) < m_MaxPixelNum && Mathf.Abs(dValueY) < m_MaxPixelNum)
            {
                m_MiddlePosFloat.Add(m_CurrentPos);
            }
            else
            {
                //X方向的步数
                int numX = Mathf.Abs(dValueX) / m_MaxPixelNum;
                //Y方向的步数
                int numY = Mathf.Abs(dValueY) / m_MaxPixelNum;
                int maxXY = Mathf.Max(numX, numY);

                var stepX = (m_CurrentPos.x - m_LastPos.x) / maxXY;
                var stepY = (m_CurrentPos.y - m_LastPos.y) / maxXY;
                Vector2 step = new Vector2(stepX, stepY);

                Vector2 middlePos = m_LastPos;
                for (int i = 0; i < maxXY; ++i)
                {
                    middlePos += step;
                    m_MiddlePosFloat.Add(middlePos);
                }
                m_MiddlePosFloat.Add(m_CurrentPos);
            }
            for (int i = 0; i < m_MiddlePosFloat.Count; ++i)
            {
                UpdateMouseLocation(m_MiddlePosFloat[i]);
            }
            m_LastPos = m_CurrentPos;
        }

        private Vector2Int ComputeUIPos(Vector2 position)
        {
            Vector2 rectXY;
            bool obtainXY = RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTrans, position, m_Canvas.worldCamera, out rectXY);
            rectXY = obtainXY ? rectXY : Vector2.zero;
            float uvX = (m_RectTrans.sizeDelta.x / 2f + rectXY.x) / m_RectTrans.sizeDelta.x;
            float uvY = (m_RectTrans.sizeDelta.y / 2f + rectXY.y) / m_RectTrans.sizeDelta.y;
            int x = (int)(uvX * width);
            int y = (int)(height - uvY * height);
            return new Vector2Int(x, y);
        }

        private void UpdateMouseLocation(Vector2 position)
        {
            Vector2Int current = ComputeUIPos(position);
            RenderTexture.active = m_RT;
            GL.LoadPixelMatrix(0, width, height, 0);
            int x = current.x;
            int y = current.y;
            x -= (int)(m_BrushScaleRT.width * 0.5f);
            y -= (int)(m_BrushScaleRT.height * 0.5f);
            if (x < 0 || y < 0 || x + m_BrushScaleRT.width > width || y + m_BrushScaleRT.height > height)
            {
                RenderTexture.active = null;
                return;
            }

            Rect rect = new Rect(x, y, m_BrushScaleRT.width, m_BrushScaleRT.height);
            m_BrushRect.ReadPixels(rect, 0, 0, false);
            m_BrushRect.Apply();
            m_BrushMat.SetTexture("_RenderTex", m_BrushRect);
            Graphics.DrawTexture(rect, m_BrushScaleRT, m_BrushMat, 1);
            RenderTexture.active = null;
        }

        private void CheckRT()
        {
            if (m_PixelNum > 0)
            {
                RenderTexture.active = m_RT;
                m_RT2D.ReadPixels(rectRT, 0, 0);
                m_RT2D.Apply();
                RenderTexture.active = null;
                Color[] colors = m_RT2D.GetPixels();
                int num = 0;

                for (int i = 0; i < colors.Length; ++i)
                {
                    int w = i % width;
                    int h = i / width;
                    if (w >= m_LRBorder && h >= m_UBBorder && w < m_RightBorder && h < m_UpBorder)
                    {
                        if (colors[i].r >= 0.1)
                        {
                            num += 1;
                        }
                    }
                }

                float ratio = (float)num / m_PixelNum;
                if (ratio > 0.7)
                {
                    this.mIsActionOver = true;
                }
            }
        }

        private void PreviewCore()
        {
            RenderTexture.active = m_RT;
            m_RT2D.ReadPixels(rectRT, 0, 0);
            RenderTexture.active = null;
            Color[] colors = m_RT2D.GetPixels();
            for (int i = 0; i < colors.Length; ++i)
            {
                int w = i % width;
                int h = i / width;
                if (w >= m_LRBorder && h >= m_UBBorder && w < m_RightBorder && h < m_UpBorder)
                {
                    m_RT2D.SetPixel(w, h, Color.white);
                }
            }
            m_RT2D.Apply();
            Graphics.Blit(m_RT2D, m_RT);
        }

        private void OnDisable()
        {
            RenderTexture.ReleaseTemporary(m_BrushScaleRT);
        }
    }
}
