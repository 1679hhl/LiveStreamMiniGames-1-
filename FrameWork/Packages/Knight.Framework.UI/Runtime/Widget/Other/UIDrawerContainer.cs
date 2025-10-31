using System.Collections.Generic;
using UnityEngine;
namespace UnityEngine.UI
{
    public class UIDrawerContainer : MonoBehaviour
    {
        public UIDrawerButton[] m_Buttons;

        [SerializeField]
        private float m_TopY = 320.0f;
        [SerializeField]
        private float m_BottomY = -600.0f;
        [SerializeField]
        private float m_IntervalY = 160.0f;
        [Tooltip("第一个抽屉默认位置")]
        [SerializeField]
        private float m_DefaultY = 140.0f;

        private bool m_Restore;

        private int m_CurIndex = -1;
        List<UIDrawerButton> m_UpMoveList;
        List<UIDrawerButton> m_BottomList;

        void Start()
        {
            this.m_UpMoveList = new List<UIDrawerButton>();
            this.m_BottomList = new List<UIDrawerButton>();
            for (int i = 0, length = m_Buttons.Length; i < length; i++)
            {
                m_Buttons[i].Init(i, this);
            }
            this.m_Restore = false;
        }

        public void ChildClick(int index)
        {
            this.m_UpMoveList.Clear();
            this.m_BottomList.Clear();
            if(this.m_CurIndex == index)
            {
                for (int i = 0; i < this.m_Buttons.Length; i++)
                {
                    this.m_Buttons[i].SetActived(false);
                }
                this.m_CurIndex = -1;
            }
            else
            {
                this.m_CurIndex = index;
                for (int i = 0, length = m_Buttons.Length; i < length; i++)
                {
                    this.m_Buttons[i].SetActived(i == index);
                    if (i <= index)
                        this.m_UpMoveList.Add(m_Buttons[i]);
                    else
                        this.m_BottomList.Add(m_Buttons[i]);
                }
            }

            if(this.m_CurIndex != -1)
            {
                //上移
                for (int i = 0, count = this.m_UpMoveList.Count; i < count; i++)
                {
                    this.m_UpMoveList[i].SetTargetY(m_TopY - m_IntervalY * i);
                }
                int _index = 0;
                //下移
                for (int i = this.m_BottomList.Count - 1; i >= 0; i--)
                {
                    this.m_BottomList[i].SetTargetY(m_BottomY + m_IntervalY * _index);
                    _index++;
                }
            }
            else
            {
                for (int i = 0; i < m_Buttons.Length; i++)
                {
                    this.m_Buttons[i].SetTargetY(this.m_DefaultY - m_IntervalY * i);
                }
            }
        }
    }
}
