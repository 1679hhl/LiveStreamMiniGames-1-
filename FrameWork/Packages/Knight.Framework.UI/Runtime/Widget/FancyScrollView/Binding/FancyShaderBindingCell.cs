using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class FancyShaderBindingCell : FancyBindingCell
    {
        public RectTransform RectTransform => this.rectTransform;

        [SerializeField] Image image = default;
        [SerializeField] RectTransform rectTransform = default;
        [SerializeField, HideInInspector] Vector3 position = default;

        public float CurrentPosition => this.mCurrentPosition;

        float hash;
        public override void Initialize()
        {
            base.Initialize();
            hash = Random.value * 100f;
            rectTransform = this.GetComponent<RectTransform>();
        }
        private void LateUpdate()
        {
            image.rectTransform.localPosition = position + GetFluctuation();
        }
        public Vector3 GetPosition()
        {
            var rPosition = IsVisible
                       ? this.position + GetFluctuation()
                       : rectTransform.rect.size.x * 10f * Vector3.left;
            return rPosition;
        }

        Vector3 GetFluctuation()
        {
            var fluctX = Mathf.Sin(Time.time + hash * 40) * 12;
            var fluctY = Mathf.Sin(Time.time + hash) * 12;
            return new Vector3(fluctX, fluctY, 0f);
        }
    }

}