using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Knight.Core.Editor
{
    public static class EditorGUITool
    {
        /// <summary>
        /// 搜索框
        /// </summary>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string SearchField(string value, params GUILayoutOption[] options)
        {
            MethodInfo info = typeof(EditorGUILayout).GetMethod("ToolbarSearchField", BindingFlags.NonPublic | BindingFlags.Static, null, new System.Type[] { typeof(string), typeof(GUILayoutOption[]) }, null);
            if (info != null)
            {
                value = (string)info.Invoke(null, new object[] { value, options });
            }
            return value;
        }
        public static string Popup(string rLabel, string rSelectedItem, string[] rDisplayedOptions, params GUILayoutOption[] rOptions)
        {
            var nSelectedIndex = 0;
            if (rDisplayedOptions == null)
            {
                rDisplayedOptions = new string[1];
                rDisplayedOptions[0] = "None";
            }
            else
            {
                var bFind = false;
                for (int i = 0; i < rDisplayedOptions.Length; i++)
                {
                    if (rDisplayedOptions[i] == rSelectedItem)
                    {
                        nSelectedIndex = i;
                        bFind = true;
                    }
                }
                if (!bFind)
                {
                    nSelectedIndex = 0;
                }
            }
            if (string.IsNullOrEmpty(rLabel))
            {
                nSelectedIndex = EditorGUILayout.Popup(nSelectedIndex, rDisplayedOptions, rOptions);
            }
            else
            {
                nSelectedIndex = EditorGUILayout.Popup(rLabel, nSelectedIndex, rDisplayedOptions, rOptions);
            }
            return rDisplayedOptions[nSelectedIndex];
        }
        public static string Popup(string rLabel, string rSelectedItem, List<string> rDisplayedOptionList, params GUILayoutOption[] rOptions)
        {
            return Popup(rLabel, rSelectedItem, rDisplayedOptionList.ToArray(), rOptions);
        }
    }
}