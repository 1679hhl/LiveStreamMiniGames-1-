using UnityEngine;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditor.UI;

namespace Game
{
    [CustomEditor(typeof(WTextMeshPro), true)]
    public class WTextMeshProInspector : TMP_EditorPanelUI
    {
        private SerializedProperty mIsUseMultiLang;
        private SerializedProperty mMultiLangID;
        private SerializedProperty mChinese;
        protected override void OnEnable()
        {
            base.OnEnable(); 
            this.mIsUseMultiLang = this.serializedObject.FindProperty("mIsUseMultiLang");
            this.mMultiLangID = this.serializedObject.FindProperty("mMultiLangID");
            this.mChinese = this.serializedObject.FindProperty("Chinese");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.PropertyField(this.mIsUseMultiLang);
            if (this.mIsUseMultiLang.boolValue)
            {
                EditorGUILayout.PropertyField(this.mMultiLangID);
                EditorGUILayout.Space(5f);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(this.mChinese, new GUIContent("多语言搜索"));
                if (GUILayout.Button("搜索", GUILayout.Width(75f)))
                {
                    try
                    {
                        UIMultiLanguage.Instance.Init();
                        var rFindResult = UIMultiLanguage.Instance.FindDataByChinese(this.mChinese.stringValue);
                        bool bFound = rFindResult != null;
                        if(bFound)
                        {
                            this.mMultiLangID.longValue = rFindResult.Id;
                        }
            
                        if (!bFound)
                        {
                            EditorUtility.DisplayDialog("提示", $"未找到多语言 => {this.mChinese.stringValue}。 快叫策划加上吧！", "知道了");
                        }
                    }
                    catch
                    {
                        EditorUtility.DisplayDialog("提示", $"多语言数据结构有改动，请把生成的代码复制到WTextInspector中替换掉再试！", "知道了");
                        return;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}