using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    /// <summary>
    /// 软裁切组件
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SoftMask : MonoBehaviour
    {
        [Button]
        public void Refresh()
        {
            SoftMaskable[] rSoftMaskable = this.GetComponents<SoftMaskable>();
            for (int i = 0; i < rSoftMaskable.Length; ++i)
                rSoftMaskable[i].Refresh();

            rSoftMaskable = this.GetComponentsInChildren<SoftMaskable>();
            for (int i = 0; i < rSoftMaskable.Length; ++i)
                rSoftMaskable[i].Refresh();
        }
    }
}