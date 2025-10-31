using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace UnityEngine.UI
{
    public class UIDrawerButton : MonoBehaviour
    {
        public AnimationCurve m_AnimCurve;
        public RectTransform m_Content;
        public Vector3 m_MoveRange;
        [SerializeField]
        private float m_OffsetX = 220.0f;
        public Button m_Button;
        [SerializeField]
        float m_HorizontalX;
        int m_Index;
        bool m_Actived;
        UIDrawerContainer m_Parent;
        RectTransform m_Self;
        Vector2 m_DefaultPos = Vector2.zero;

        void Start()
        {
            m_Actived = false;
            m_HorizontalX = 0;
            m_Self = GetComponent<RectTransform>();
            m_Button.onClick.AddListener(OnClick);
        }

        public void Init(int index, UIDrawerContainer parent)
        {
            m_Index = index;
            m_Parent = parent;
        }

        public void SetActived(bool actived)
        {
            if (m_Actived != actived)
            {
                m_Actived = actived;
                if (!m_Actived)
                    m_HorizontalX = m_OffsetX;
                else
                    m_HorizontalX = -m_OffsetX;
            }
            else
            {
                m_HorizontalX = 0.0f;
            }
        }

        public void SetTargetY(float posY)
        {
            StartCoroutine(MoveVerticalCoroutine(posY));
        }

        void OnClick()
        {
            m_Parent.ChildClick(m_Index);
        }

        IEnumerator ContentFadeCoroutine(bool direction)
        {
            CanvasGroup canvasGroup = m_Content.GetComponent<CanvasGroup>();
            float sAlp = direction ? 0 : 1;
            float eAlp = (1 - sAlp);
            Vector2 pos1 = new Vector2(m_MoveRange.x, m_MoveRange.z);
            Vector2 pos2 = new Vector2(m_MoveRange.y, m_MoveRange.z);
            Vector2 sPos = direction ? pos1 : pos2;
            Vector2 ePos = direction ? pos2 : pos1;
            float stime = Time.time;
            float ratio = 0.0f;
            float curve = 0.0f;
            while (ratio < 1.0f)
            {
                m_Content.anchoredPosition = Vector2.Lerp(sPos, ePos, curve);
                canvasGroup.alpha = Mathf.Lerp(sAlp, eAlp, curve);
                yield return null;
                ratio = (Time.time - stime) * 2.0f;
                curve = m_AnimCurve.Evaluate(ratio);
            }
            m_Content.anchoredPosition = ePos;
            canvasGroup.alpha = eAlp;
        }

        IEnumerator MoveVerticalCoroutine(float posY)
        {
            if (m_HorizontalX < 0.0f)
            {
                StartCoroutine(ContentFadeCoroutine(true));
            }
            else if (m_HorizontalX > 0.0f)
            {
                StartCoroutine(ContentFadeCoroutine(false));
            }
            Vector2 sPos = m_Self.anchoredPosition;
            Vector2 ePos = new Vector2(m_Self.anchoredPosition.x + m_HorizontalX, posY);
            if (sPos == ePos)
            {
                yield break;
            }
            float stime = Time.time;
            float ratio = 0.0f; 
            float curve = 0.0f;
            while (ratio < 1.0f)
            {
                m_Self.anchoredPosition = Vector2.Lerp(sPos, ePos, curve);
                yield return null;
                ratio = (Time.time - stime) * 2.0f;
                curve = m_AnimCurve.Evaluate(ratio);
            }
            m_Self.anchoredPosition = ePos;
        }
    }
}
