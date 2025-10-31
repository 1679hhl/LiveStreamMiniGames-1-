namespace LittleBaby.Framework.Core
{
    using System.Reflection;

    using UnityEngine;
    using UnityEngine.UI;

    public static class GraphicRaycasterExpand
    {
        public const string GraphicRaycaster_m_BlockingMask_FieldName = "m_BlockingMask";
        public static FieldInfo GraphicRaycaster_m_BlockingMask_FieldInfo;
        public static bool GetBlockingMaskFieldInfo()
        {
            if (GraphicRaycaster_m_BlockingMask_FieldInfo == null)
            {
                GraphicRaycaster_m_BlockingMask_FieldInfo = typeof(GraphicRaycaster).GetField(GraphicRaycaster_m_BlockingMask_FieldName);
                return GraphicRaycaster_m_BlockingMask_FieldInfo != null;
            }
            return true;
        }
        public static LayerMask GetBlockingMask(this GraphicRaycaster rGraphicRaycaster)
        {
            if (GetBlockingMaskFieldInfo())
            {
                return (LayerMask)GraphicRaycaster_m_BlockingMask_FieldInfo.GetValue(rGraphicRaycaster);
            }
            return new LayerMask();
        }
        public static void SetBlockingMask(this GraphicRaycaster rGraphicRaycaster, in LayerMask rLayerMask)
        {
            if (GetBlockingMaskFieldInfo())
            {
                GraphicRaycaster_m_BlockingMask_FieldInfo.SetValue(rGraphicRaycaster, rLayerMask);
            }
        }
    }
}
