using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Background))]
    public class FancyShaderBindingScrollView : FancyBindingScrollView
    {
        [SerializeField] FancyShaderType ShaderType;
        

        [HideInInspector]
        public int CellInstanceCount => Mathf.CeilToInt(1f / Mathf.Max(this.cellInterval, 1e-3f));
        [HideInInspector]
        public Vector4[] CellState = new Vector4[1];
        [HideInInspector]
        public int CurrentSelection
        {
            get
            {
                return this.mCurrentSelection;
            }
            set
            {
                this.mOldSelection = this.mCurrentSelection;
                this.mCurrentSelection = value;
                this.mUpdateSelectionTime = Time.time;
            }
        }

        private int mCurrentSelection;
        private int mOldSelection;
        private float mUpdateSelectionTime;

        public override void Select(int nIndex)
        {
            this.CurrentSelection = nIndex;
            base.Select(nIndex);
        }

        public override void SelectEnd()
        {
            if (this.ItemsSource.Count == 0) return;

            this.CurrentSelection = this.ItemsSource.Count - 1;
            base.SelectEnd();
        }

        public override void ScrollTo(int nIndex)
        {
            this.CurrentSelection = nIndex;
            base.ScrollTo(nIndex);
        }

        protected override void UpdateSelection(int nIndex)
        {
            this.CurrentSelection = nIndex;
            base.UpdateSelection(nIndex);
        }
        public Vector4[] GetCellState()
        {
            if(this.ShaderType == FancyShaderType.Metaball)
            {
                this.UpdateCellState_Metaball();
            }
            else if(this.ShaderType == FancyShaderType.Voronoi)
            {
                this.UpdateCellState_Voronoi();
            }

            return this.CellState;
        }

        public void SetCellState(int cellIndex, int dataIndex, float x, float y, float scale)
        {
            var size = cellIndex + 1;
            if (size > this.CellState.Length)
            {
                Array.Resize(ref this.CellState, size);
            }

            this.CellState[cellIndex].x = x;
            this.CellState[cellIndex].y = y;
            this.CellState[cellIndex].z = dataIndex;
            this.CellState[cellIndex].w = scale;
        }

        public void UpdateCellState_Metaball()
        {
            for (int i = 0; i < this.Pool.Count; i++)
            {
                var rCell = this.Pool[i] as FancyShaderBindingCell;
                var rRectTransform = rCell.RectTransform;
                var nSiblingIndex = rRectTransform.GetSiblingIndex();
                var rScale = Mathf.Min(1f, 10 * (0.5f - Mathf.Abs(rCell.CurrentPosition - 0.5f)));
                var rPosition = rCell.GetPosition();
                this.SetCellState(nSiblingIndex, rCell.Index, rPosition.x, rPosition.y, rScale);
            }
        }

        private void UpdateCellState_Voronoi()
        {
            for (int i = 0; i < this.Pool.Count; i++)
            {
                var rCell = this.Pool[i] as FancyShaderBindingCell;

                var nSiblingIndex = rCell.RectTransform.GetSiblingIndex();

                var selectAnimation = 0f;
                if(rCell.Index == this.mOldSelection || rCell.Index == this.mCurrentSelection)
                {
                    var t = Mathf.Clamp01((Time.time - this.mUpdateSelectionTime) * (1f / 0.3f));
                    selectAnimation = this.mCurrentSelection == rCell.Index ? t : 1f - t;
                }

                var rPosition = rCell.GetPosition();

                this.SetCellState(nSiblingIndex, rCell.Index, rPosition.x, rPosition.y, selectAnimation);
            }
        }

    }
}
