using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(DropdownProAttribute))]
    public class DropdownProPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);
            DropdownProAttribute dropdownAttribute = PropertyUtility.GetAttribute<DropdownProAttribute>(property);
            var rFlag = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly;
            var rFieldInfo = target.GetType().GetField(dropdownAttribute.ListName, rFlag);
            if (rFieldInfo == null) rFieldInfo = target.GetType().BaseType.GetField(dropdownAttribute.ListName, rFlag);
            if (rFieldInfo == null) return;
            
            string[] rSelections = rFieldInfo.GetValue(target) as string[];
            if (rSelections == null)
            {
                EditorGUILayout.PropertyField(property);
                property.serializedObject.ApplyModifiedProperties();
                return;
            }

            StringDropdown.Draw(property.name, rSelections, property.stringValue, property);
        }
    }

    public class StringDropdown : AdvancedDropdown
    {
        private AdvancedDropdownItem mRoot;
        private SerializedProperty mProperty;

        private StringDropdown(string rTitle, string[] rSelections, string rSelection, SerializedProperty rProperty) : base(new AdvancedDropdownState())
        {
            this.mRoot = new AdvancedDropdownItem(rTitle);
            this.mProperty = rProperty;

            for (int i = 0; i < rSelections.Length; i++)
            {
                this.mRoot.AddChild(new AdvancedDropdownItem(rSelections[i]));
            }
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            return this.mRoot;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            this.mProperty.stringValue = item.name;
            this.mProperty.serializedObject.ApplyModifiedProperties();
        }

        public static void Draw(string rTitle, string[] rSelections, string rCurrentSelection, SerializedProperty rProperty)
        {
            if (rSelections.Any(selection => selection.Equals(rCurrentSelection)) == false)
            {
                EditorGUILayout.HelpBox("当前选择无效，请重新选择", MessageType.Error);
                GUI.color = Color.red;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(rTitle, GUILayout.Width(200));
            Rect rect = GUILayoutUtility.GetRect(new GUIContent(rCurrentSelection), EditorStyles.popup);
            if (GUI.Button(rect, rCurrentSelection, EditorStyles.popup))
            {
                StringDropdown dropdown = new StringDropdown(rTitle, rSelections, rCurrentSelection, rProperty);
                dropdown.Show(rect);
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
        }
    }
}
