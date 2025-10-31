using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(MasonryLoopScrollRect), true)]
    public class MasonryLoopScrollRectInspector : ScrollRectEditor
    {
        public override void OnInspectorGUI()
        {
            var rPrefabSource = this.serializedObject.FindProperty("PrefabSource");
            EditorGUILayout.PropertyField(rPrefabSource);

            var rItemWidth = this.serializedObject.FindProperty("ItemWidth");
            EditorGUILayout.PropertyField(rItemWidth);

            var rItemHeight = this.serializedObject.FindProperty("ItemHeight");
            EditorGUILayout.PropertyField(rItemHeight);

            var rSpacing = this.serializedObject.FindProperty("Spacing");
            EditorGUILayout.PropertyField(rSpacing);

            this.serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();
        }
    }
}
