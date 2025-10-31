using Knight.Core;
using NaughtyAttributes.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(LayoutGroupAdapt),true)]
    public class LayoutGroupAdaptInspector : InspectorEditor
    {
        private LayoutGroupAdapt mLayoutGroupAdapt;

        private SerializedProperty mAdaptLayoutGroupType;
        private SerializedProperty mAdaptScreenType;

        [SerializeField]
        private AdaptPadding mAdaptNormalPadding;
        [SerializeField]
        private AdaptPadding mAdaptWidePadding;
        [SerializeField]
        private AdaptPadding mAdaptHighPadding;

        protected override void OnEnable()
        {
            base.OnEnable();

            this.mLayoutGroupAdapt = this.target as LayoutGroupAdapt;
            this.mAdaptLayoutGroupType = this.serializedObject.FindProperty("mAdaptLayoutGroupType");
            this.mAdaptScreenType = this.serializedObject.FindProperty("mAdaptScreenType");

            if (this.mLayoutGroupAdapt.mLayoutGroup = this.mLayoutGroupAdapt.GetComponent<GridLayoutGroup>())
            {
                this.mLayoutGroupAdapt.mAdaptLayoutGroupType = LayoutGroupType.GridLayoutGroup;
            }
            else if (this.mLayoutGroupAdapt.mHorizontalLayoutGroup = this.mLayoutGroupAdapt.GetComponent<HorizontalLayoutGroup>())
            {
                this.mLayoutGroupAdapt.mAdaptLayoutGroupType = LayoutGroupType.HorizontalLayoutGroup;
            }
            else if (this.mLayoutGroupAdapt.mVerticalLayoutGroup = this.mLayoutGroupAdapt.GetComponent<VerticalLayoutGroup>())
            {
                this.mLayoutGroupAdapt.mAdaptLayoutGroupType = LayoutGroupType.VerticalLayoutGroup;
            }
            //this.InitRectOffset(this.mLayoutGroupAdapt.mAdaptLayoutGroupType);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(this.mAdaptScreenType);
            if (this.mAdaptScreenType.enumValueIndex == 0)
            {
                
                var rFieldInfo = this.serializedObject.targetObject.GetType().GetField("AdaptNormalPadding");
                this.mAdaptNormalPadding = this.InitPadding((AdaptPadding)rFieldInfo.GetValue(this.mLayoutGroupAdapt));
                rFieldInfo.SetValue(this.mLayoutGroupAdapt, this.mAdaptNormalPadding);

                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("AdaptNormalSpacing"));
            }
            else if (this.mAdaptScreenType.enumValueIndex == 1)
            {
                var rFieldInfo = this.serializedObject.targetObject.GetType().GetField("AdaptWidePadding");
                this.mAdaptWidePadding = this.InitPadding((AdaptPadding)rFieldInfo.GetValue(this.mLayoutGroupAdapt));
                rFieldInfo.SetValue(this.mLayoutGroupAdapt, this.mAdaptWidePadding);

                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("AdaptWideSpacing"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("AdaptWideFactor"));
            }
            else if (this.mAdaptScreenType.enumValueIndex == 2)
            {
                var rFieldInfo = this.serializedObject.targetObject.GetType().GetField("AdaptHighPadding");
                this.mAdaptHighPadding = this.InitPadding((AdaptPadding)rFieldInfo.GetValue(this.mLayoutGroupAdapt));
                rFieldInfo.SetValue(this.mLayoutGroupAdapt, this.mAdaptHighPadding);

                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("AdaptHighSpacing"));
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty("AdaptHighFactor"));
            }

            this.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this.mLayoutGroupAdapt);
            EditorUtility.SetDirty(this.mLayoutGroupAdapt.gameObject);
        }

        private AdaptPadding InitPadding(AdaptPadding Padding)
        {
            Padding.left = EditorGUILayout.IntField(new GUIContent("Left"), Padding.left);
            Padding.right = EditorGUILayout.IntField(new GUIContent("Right"), Padding.right);
            Padding.top = EditorGUILayout.IntField(new GUIContent("Top"), Padding.top);
            Padding.bottom = EditorGUILayout.IntField(new GUIContent("Bottom"), Padding.bottom);
            return Padding;
        }

        //private void InitRectOffset(LayoutGroupType rType)
        //{
        //    if(rType == LayoutGroupType.GridLayoutGroup)
        //    {
        //        if (this.mLayoutGroupAdapt.mLayoutGroup == null)
        //            return;

        //        this.mAdaptWidePadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mLayoutGroup.padding.bottom
        //        };
        //        this.mAdaptHighPadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mLayoutGroup.padding.bottom
        //        };
        //        this.mAdaptNormalPadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mLayoutGroup.padding.bottom
        //        };
        //    }
        //    else if(rType == LayoutGroupType.HorizontalLayoutGroup)
        //    {
        //        if (this.mLayoutGroupAdapt.mHorizontalLayoutGroup == null)
        //            return;
        //        this.mAdaptWidePadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.bottom
        //        };
        //        this.mAdaptHighPadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.bottom
        //        };
        //        this.mAdaptNormalPadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mHorizontalLayoutGroup.padding.bottom
        //        };
        //    }
        //    else
        //    {
        //        if (this.mLayoutGroupAdapt.mVerticalLayoutGroup == null)
        //            return;
        //        this.mAdaptWidePadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.bottom
        //        };
        //        this.mAdaptHighPadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.bottom
        //        };
        //        this.mAdaptNormalPadding = new RectOffset()
        //        {
        //            left = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.left,
        //            right = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.right,
        //            top = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.top,
        //            bottom = this.mLayoutGroupAdapt.mVerticalLayoutGroup.padding.bottom
        //        };
        //    }
        //}
    }
}

