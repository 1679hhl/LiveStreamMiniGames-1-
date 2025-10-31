using NaughtyAttributes.Editor;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(EventBinding), true)]
    public class EventBindingInspector : InspectorEditor
    {
        private EventBinding mTargetAbstract;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.mTargetAbstract = this.target as EventBinding;
        }

        public override void OnInspectorGUI()
        {
            this.mTargetAbstract.GetPaths();
            base.OnInspectorGUI();
        }
    }
}
