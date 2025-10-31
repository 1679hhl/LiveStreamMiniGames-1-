using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FancyScrollView;

namespace UnityEngine.UI
{
    public class FancyBindingRectCell : FancyScrollRectCell<FancyBindingItemData>
    {
        public override void UpdateContent(FancyBindingItemData itemData)
        {

        }

        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            base.UpdatePosition(normalizedPosition, localPosition); 
            
            var wave = Mathf.Sin(normalizedPosition * Mathf.PI * 2) * 65;
            transform.localPosition += Vector3.right * wave;
        }
    }
}