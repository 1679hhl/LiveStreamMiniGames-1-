using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// 配合Mask组件使用，显示Mask之外部分
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(MaskableGraphic))]
    public class MaskableOutside : MonoBehaviour, IMaterialModifier
    {
        private Material mMaterial;

        public Material GetModifiedMaterial(Material baseMaterial)
        {
            if (this.mMaterial == null)
            {
                int nStencilID = MaskUtilities.GetStencilDepth(this.transform, null);
                this.mMaterial = StencilMaterial.Add(baseMaterial, nStencilID, StencilOp.Keep, CompareFunction.NotEqual, ColorWriteMask.All, 255, 0);
            }
            return this.mMaterial;
        }
    }
}