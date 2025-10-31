using UnityEngine.UI;
using NaughtyAttributes.Editor;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ViewModelDataSourceMasonry), true)]
    public class ViewModelDataSourceMasonryInspector : InspectorEditor
    {
        private ViewModelDataSourceMasonry mTarget;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.mTarget = this.target as ViewModelDataSourceMasonry;
        }

        public override void OnInspectorGUI()
        {
            this.mTarget.GetPaths();
            base.OnInspectorGUI();
        }
    }
}
