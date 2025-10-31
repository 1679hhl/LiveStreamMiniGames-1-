/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace FancyScrollView
{
    public enum FancyShaderType
    {
        Metaball,
        Voronoi,
    }
    class Background : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] FancyShaderType ShaderType;
        [SerializeField] Image background = default;
        [SerializeField] FancyShaderBindingScrollView scrollView = default;

        RectTransform rectTransform;
        static class Uniform
        {
            public static readonly int Resolution = Shader.PropertyToID("_Resolution");
            public static readonly int CellState = Shader.PropertyToID("_CellState");
        }

        void Start()
        {
            rectTransform = transform as RectTransform;
        }

        void LateUpdate()
        {
            if(this.ShaderType == FancyShaderType.Metaball)
            {
                this.MetaballUpdate();
            }
            else if(this.ShaderType == FancyShaderType.Voronoi)
            {
                this.VoronoiUpdate();
            }

            background.material.SetVector(Uniform.Resolution, rectTransform.rect.size);
            background.material.SetVectorArray(Uniform.CellState, scrollView.GetCellState());
        }

        bool MetaballContains(Vector2 p, Vector4[] cellState)
        {
            float f(Vector2 v) => 1f / (v.x * v.x + v.y * v.y + 0.0001f);

            const float scale = 4600f;
            var d = cellState.Sum(x => f(p - new Vector2(x.x, x.y)) * x.w);
            return d * scale > 0.46f;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.dragging)
            {
                return;
            }

            if(this.ShaderType == FancyShaderType.Metaball)
            {
                this.MetaballClick(eventData);
            }
            else if(this.ShaderType == FancyShaderType.Voronoi)
            {
                this.VoronoiClick(eventData);
            }
        }

        private void MetaballUpdate()
        {
            var offset = scrollView.CellInstanceCount;
            scrollView.SetCellState(offset + 0, -1, 500, -330 + Mathf.Sin(Time.time) * 60, 2.5f);
            scrollView.SetCellState(offset + 1, -1, -500, -330 + Mathf.Sin(Time.time) * 60, 2.5f);
        }

        private void VoronoiUpdate()
        {
            var rect = rectTransform.rect.size * 0.5f;
            var offset = scrollView.CellInstanceCount;

            scrollView.SetCellState(offset + 0, -1, rect.x, -rect.y * 1.3f, 0f);
            scrollView.SetCellState(offset + 1, -1, -rect.x, rect.y * 1.3f, 0f);
            scrollView.SetCellState(offset + 2, -1, -rect.x, -rect.y * 1.3f, 0f);
            scrollView.SetCellState(offset + 3, -1, rect.x, rect.y * 1.3f, 0f);
        }

        private void MetaballClick(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var clickPosition
            );

            var cellState = scrollView.GetCellState();
            if (!MetaballContains(clickPosition, cellState))
            {
                return;
            }

            var dataIndex = cellState
                .Take(scrollView.CellInstanceCount)
                .Select(s => (
                    index: Mathf.RoundToInt(s.z),
                    distance: (new Vector2(s.x, s.y) - clickPosition).sqrMagnitude
                ))
                .Aggregate((min, x) => x.distance < min.distance ? x : min)
                .index;

            scrollView.ScrollTo(dataIndex);
        }

        private void VoronoiClick(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var clickPosition
            );

            var cellState = scrollView.GetCellState()
                .Select((s, i) => (
                    index: i,
                    dataIndex: Mathf.RoundToInt(s.z),
                    position: new Vector2(s.x, s.y)
                ));

            var target = cellState
                .OrderBy(x => (x.position - clickPosition).sqrMagnitude)
                .First();

            var distance = cellState
                .Where(x => x.index != target.index)
                .Min(x => Vector2.Dot(
                    clickPosition - (target.position + x.position) * 0.5f,
                    (target.position - x.position).normalized
                ));

            const float borderWidth = 9;
            if (distance < borderWidth)
            {
                return;
            }

            scrollView.ScrollTo(target.dataIndex);
        }
    }
}
