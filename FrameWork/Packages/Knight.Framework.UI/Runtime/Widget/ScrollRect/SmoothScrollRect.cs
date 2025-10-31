using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(ViewModelDataSourceArray))]
    public class SmoothScrollRect : UIBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public bool IsCanDrag { get; set; } = true;

        private class Cell
        {
            public RectTransform Rect;
            public int CellIndex;
            public int DataIndex;
            public Cell(RectTransform rect, int cellIndex, int dataIndex)
            {
                Rect = rect;
                CellIndex = cellIndex;
                DataIndex = dataIndex;
            }
        }

        [NaughtyAttributes.ReadOnly]
        public ViewModelDataSourceArray dataSource;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            this.dataSource = gameObject.GetComponent<ViewModelDataSourceArray>();
            if (this.dataSource) this.dataSource.HasInitData = true;
        }
#endif

        public float minSideWidth = 150f;
        [Header("左侧缩放Cell，按从左到右赋值"), NaughtyAttributes.ReorderableList]
        public RectTransform[] lefts;
        public RectTransform leftView;
        [Header("右侧缩放Cell，按从左到右赋值"), NaughtyAttributes.ReorderableList]
        public RectTransform[] rights;
        public RectTransform rightView;

        [Header("中间Cell，指定容器、模板、及最小间距")]
        public RectTransform centerView;
        public RectTransform centerTemplate;
        public float minCenterSpacing = 25f;
        private RectTransform[] mCenters;

        private Vector2 startPos;
        private Vector2 endPos;

        [Header("动效参数")]
        [Range(0, 350)]
        public float maxOffset;
        [Range(0, 350)]
        public float maxSpeed;
        [Range(0, 1)]
        public float smoothTime;
        [Range(0, 10)]
        public float alignSpeed;
        [Range(0, 10)]
        public float backSpeed;

        public Action<int, int> OnFillCell;

        private LinkedList<Cell> mCenterCellLinkedList = new LinkedList<Cell>();
        private int mFirstCenterCellDataIndex;
        private int mMaxFirstCenterCellDataIndex;

        public bool NeedToExeButtonAct = true;

        public int FirstCenterCellDataIndex => mFirstCenterCellDataIndex;

        public int MaxDataCount
        {
            get => mMaxFirstCenterCellDataIndex + mCenters.Length;
            set => mMaxFirstCenterCellDataIndex = value - mCenters.Length;
        }

        protected override void Awake()
        {
            base.Awake();
            Vector2 v = new Vector2(0f, 0.5f);
            this.centerTemplate.anchorMin = v;
            this.centerTemplate.anchorMax = v;
            this.centerTemplate.pivot = v;
        }

        public int InitializePerfectCell()
        {
            if (this.mCenters != null)
            {
                return this.dataSource.ItemTransList.Count;
            }

            float fTotalWidth = (transform as RectTransform).rect.width;
            float fCenterCellWidth = centerTemplate.rect.width;
            int nCenterCellCount = Mathf.FloorToInt((fTotalWidth - 2 * (minSideWidth - fCenterCellWidth) + minCenterSpacing) / (fCenterCellWidth + minCenterSpacing));

            InitializeViewRect();
            InitializeCenterViewCells();

            this.dataSource.ItemTransList.Clear();
            this.dataSource.ItemTransList.AddRange(lefts);
            this.dataSource.ItemTransList.AddRange(mCenters);
            this.dataSource.ItemTransList.AddRange(rights);

            return this.dataSource.ItemTransList.Count;

            void InitializeViewRect()
            {
                float fPerfectCenterViewWidth = nCenterCellCount * (fCenterCellWidth + minCenterSpacing) - minCenterSpacing;
                float fOffsetFromCenterToSide = (fTotalWidth - fPerfectCenterViewWidth) / 2f + fCenterCellWidth;

                centerView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fPerfectCenterViewWidth);
                leftView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fOffsetFromCenterToSide);
                rightView.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fOffsetFromCenterToSide);
            }

            void InitializeCenterViewCells()
            {
                nCenterCellCount--;// 少放一个，由左右固定的提供这一个
                /*if (this.mCenters != null && this.mCenters.Length != nCenterCellCount)
                {
                    for (int i = 1; i < this.mCenters.Length; i++)
                    {
                        Destroy(this.mCenters[i].gameObject);
                    }
                    this.mCenters = null;
                }*/

                this.mCenters = new RectTransform[nCenterCellCount];
                this.mCenters[0] = this.centerTemplate;
                for (int i = 1; i < nCenterCellCount; i++)
                {
                    this.mCenters[i] = Instantiate(this.centerTemplate, this.centerTemplate.parent);
                }
            }
        }

        public void InitializeFill(int nFirstIndex = 0)
        {
            mFirstCenterCellDataIndex = Mathf.Min(nFirstIndex - 3, mMaxFirstCenterCellDataIndex);
            mFirstCenterCellDataIndex = Mathf.Max(mFirstCenterCellDataIndex, 0);

            int nCellIndexOffset = lefts.Length;
            mCenterCellLinkedList.Clear();
            for (int i = 0; i < mCenters.Length; i++)
            {
                mCenterCellLinkedList.AddLast(new Cell(mCenters[i], i + nCellIndexOffset, mFirstCenterCellDataIndex + i));
            }

            LayoutCenters();

            Refresh();
        }

        public void Refresh()
        {
            FillTwoSideCellsWhenReuse();
            FillCenters();
        }

        private void LayoutCenters()
        {
            float fCenterViewWidth = centerView.rect.width;
            float fCenterCellWidth = centerTemplate.rect.width;
            startPos = new Vector2(0, 0);
            endPos = new Vector2(fCenterViewWidth - fCenterCellWidth, 0);

            int nCenterCellCount = mCenters.Length;
            mCenters[0].anchoredPosition = startPos;
            for (int i = 1; i < nCenterCellCount; i++)
            {
                mCenters[i].anchoredPosition = new Vector2(i * (fCenterCellWidth + minCenterSpacing), 0);
            }
        }

        private void FillCenters()
        {
            if (OnFillCell == null)
            {
                return;
            }

            foreach (var rCell in mCenterCellLinkedList)
            {
                OnFillCell.Invoke(rCell.CellIndex, rCell.DataIndex);
            }
        }

        private void FillReuseCell(bool direction)
        {
            /*this.FillCenters();//全部刷新，避免数据源变化导致数据错乱
            return;*/
            if (OnFillCell == null)
            {
                return;
            }

            Cell cell;
            if (direction)
            {
                cell = mCenterCellLinkedList.First.Value;
            }
            else
            {
                cell = mCenterCellLinkedList.Last.Value;
            }
            OnFillCell.Invoke(cell.CellIndex, cell.DataIndex);
        }

        private void FillTwoSideCellsWhenReuse()
        {
            if (OnFillCell == null)
            {
                return;
            }

            int nLeftCount = lefts.Length;
            int nDataIndexOffset = nLeftCount - 1;
            for (int i = 0; i < nLeftCount; i++, nDataIndexOffset--)
            {
                OnFillCell.Invoke(i, mFirstCenterCellDataIndex - nDataIndexOffset - 1);
            }

            int nCenterCount = mCenters.Length;
            int nCellIndexOffset = lefts.Length + nCenterCount;
            int nRightCount = rights.Length;
            for (int i = 0; i < nRightCount; i++)
            {
                OnFillCell.Invoke(i + nCellIndexOffset, mFirstCenterCellDataIndex + nCenterCount + i);
            }
        }

        // true表示右侧覆盖 false表示左侧覆盖
        private void FillTwoSideCellsWhenAlign(bool bOverlayFlag)
        {
            if (OnFillCell == null)
            {
                return;
            }

            if (bOverlayFlag)
            {
                int nCenterCount = mCenters.Length;
                int nCellIndexOffset = lefts.Length + nCenterCount;
                int nRightCount = rights.Length;
                for (int i = 0; i < nRightCount; i++)
                {
                    OnFillCell.Invoke(i + nCellIndexOffset, mFirstCenterCellDataIndex + nCenterCount + i - 1);
                }
            }
            else
            {
                int nLeftCount = lefts.Length;
                int nDataIndexOffset = nLeftCount - 1;
                for (int i = 0; i < nLeftCount; i++, nDataIndexOffset--)
                {
                    OnFillCell.Invoke(i, mFirstCenterCellDataIndex - nDataIndexOffset);
                }
            }
        }

        //修正位移
        private void ReviseMovement(ref Vector2 movement)
        {
            movement.y = 0;
            if (movement.x > maxSpeed)
            {
                movement.x = maxSpeed;
            }
            else if (movement.x < -maxSpeed)
            {
                movement.x = -maxSpeed;
            }

            if (GetBackFlag() * movement.x < 0)
            {
                float offset = GetOffset(movement.x > 0);
                if (offset >= maxOffset)
                {
                    movement.x = 0;
                }
                else
                {
                    movement.x = movement.x * (maxOffset - offset) / maxOffset;
                }
            }
        }

        /// <summary>
        /// 获取偏移值，始终返回正数
        /// false表示向右的差值
        /// true表示向左的差值
        /// </summary>
        private float GetOffset(bool direction)
        {
            if (direction)
            {
                return mCenterCellLinkedList.First.Value.Rect.anchoredPosition.x - startPos.x;
            }
            else
            {
                return endPos.x - mCenterCellLinkedList.Last.Value.Rect.anchoredPosition.x;
            }
        }

        //移动卡片
        private void MoveCards(Vector2 movement)
        {
            for (int i = 0; i < mCenters.Length; i++)
            {
                mCenters[i].anchoredPosition += movement;
            }
        }

        /// <summary>
        /// 标记回弹
        /// -1  向左回弹
        /// 0   不回弹
        /// 1   向右回弹 
        /// </summary>
        private int GetBackFlag()
        {
            int backFlag = 0;
            if (mFirstCenterCellDataIndex <= 0)
            {
                backFlag = -1;
            }
            else if (mFirstCenterCellDataIndex >= mMaxFirstCenterCellDataIndex)
            {
                backFlag = 1;
            }
            return backFlag;
        }

        //是否需要复用
        private bool NeedReuse(bool direction)
        {
            if (direction)
            {
                return mCenterCellLinkedList.Last.Value.Rect.anchoredPosition.x > endPos.x;
            }
            else
            {
                return mCenterCellLinkedList.First.Value.Rect.anchoredPosition.x < startPos.x;
            }
        }

        //如果需要复用，则复用卡片
        private void Reuse(bool direction)
        {
            if (direction)
            {
                mFirstCenterCellDataIndex--;
                MoveLastToFirst();
            }
            else
            {
                mFirstCenterCellDataIndex++;
                MoveFirstToLast();
            }

            FillReuseCell(direction);
            FillTwoSideCellsWhenReuse();

            void MoveLastToFirst()
            {
                var cell = mCenterCellLinkedList.Last.Value;
                cell.DataIndex = mCenterCellLinkedList.First.Value.DataIndex - 1;
                Vector2 offset = cell.Rect.anchoredPosition - endPos;
                cell.Rect.anchoredPosition = startPos + offset;
                mCenterCellLinkedList.RemoveLast();
                mCenterCellLinkedList.AddFirst(cell);
            }

            void MoveFirstToLast()
            {
                var cell = mCenterCellLinkedList.First.Value;
                cell.DataIndex = mCenterCellLinkedList.Last.Value.DataIndex + 1;
                Vector2 offset = cell.Rect.anchoredPosition - startPos;
                cell.Rect.anchoredPosition = endPos + offset;
                mCenterCellLinkedList.RemoveFirst();
                mCenterCellLinkedList.AddLast(cell);
            }
        }

        //重叠卡片切换动画
        private void DOTween(Vector2 movement)
        {
            //TODO
        }

        /// <summary>
        /// 回弹
        /// true表示向左回弹
        /// false表示向右回弹
        /// </summary>
        private IEnumerator MoveBack()
        {
            int flag = GetBackFlag();
            float offset = GetOffset(flag < 0);
            Vector2 movement = Vector2.zero;
            while (offset > 0.1f)
            {
                movement.x = Mathf.Lerp(0, flag * offset, backSpeed * Time.deltaTime);
                MoveCards(movement);
                yield return null;
                offset -= Mathf.Abs(movement.x);
            }
        }

        /// <summary>
        /// 缓慢停止
        /// </summary>
        private IEnumerator SlowlyStop(Vector2 movement)
        {
            bool direction = movement.x > 0;
            float currentSpeed = movement.x;
            while (Mathf.Abs(movement.x) > 10)
            {
                movement.x = Mathf.SmoothDamp(movement.x, 0, ref currentSpeed, smoothTime);
                ReviseMovement(ref movement);
                MoveCards(movement);
                if (NeedReuse(direction))
                {
                    Reuse(direction);
                }
                yield return null;
            }
            if (GetBackFlag() != 0)
            {
                StartCoroutine(MoveBack());
            }
            else
            {
                StartCoroutine(AlignToNearest(direction ? 1 : -1));
            }
        }

        /// <summary>
        /// true 向右靠齐  false 向左靠齐
        /// </summary>
        private IEnumerator AlignToNearest(int moveFlag = 0)
        {
            float offset;
            bool direction;
            if (moveFlag == 0)
            {
                float offset1 = GetOffset(true);
                float offset2 = GetOffset(false);
                direction = offset1 > offset2;
                offset = direction ? offset2 : offset1;
            }
            else
            {
                direction = moveFlag > 0;
                offset = GetOffset(!direction);
            }
            Vector2 movement = Vector2.zero;
            while (offset > 1)
            {
                movement.x = Mathf.Lerp(0, direction ? offset : -offset, alignSpeed * Time.deltaTime);
                MoveCards(movement);
                yield return null;
                offset -= Mathf.Abs(movement.x);
            }
            FillTwoSideCellsWhenAlign(direction);
        }

        private Vector2 lastPos;
        public void OnBeginDrag(PointerEventData eventData)
        {
            StopAllCoroutines();
            FillTwoSideCellsWhenReuse();
            lastPos = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!this.IsCanDrag)
                return;
            Vector2 movement = eventData.position - lastPos;
            if (movement.x == 0)
            {
                return;
            }
            bool direction = movement.x > 0;
            ReviseMovement(ref movement);
            MoveCards(movement);
            if (NeedReuse(direction))
            {
                Reuse(direction);
            }
            lastPos = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (/*Mathf.Abs((eventData.position - eventData.pressPosition).x) <= ScrollDragManager.Instance.DragMinValue.x&&*/  this.NeedToExeButtonAct)
            {
                var rButton = EventSystem.current.currentSelectedGameObject?.GetComponent<Button>();
                if (rButton)
                {
                    rButton.onClick.Invoke();
                }
                return;
            }
            Vector2 movement = eventData.position - lastPos;
            if (!this.IsCanDrag)
            {
                movement.x = 0;
            }
            if (movement.x == 0)
            {
                if (GetBackFlag() != 0)
                {
                    StartCoroutine(MoveBack());
                }
                else
                {
                    StartCoroutine(AlignToNearest());
                }
            }
            else
            {
                if (GetBackFlag() != 0)
                {
                    StartCoroutine(MoveBack());
                }
                else
                {
                    StartCoroutine(SlowlyStop(movement));
                }
            }
        }
    }
}