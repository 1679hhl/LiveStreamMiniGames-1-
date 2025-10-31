using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Knight.Core.Editor
{
    public class InputDialog : EditorWindow
    {
        public static bool Show(string rTitle, out string[] rFieldResults, params string[] rFieldTitles)
        {
            var rWindow = ScriptableObject.CreateInstance(typeof(InputDialog)) as InputDialog;
            rWindow.SetParams(rTitle, rFieldTitles);
            rWindow.ShowModalUtility();
            rFieldResults = rWindow.GetResult();
            return !rWindow.GetIsCancel();
        }
        public static bool Show(string rTitle, out string[] rFieldResults, string[] rDefaultFieldResults, params string[] rFieldTitles)
        {
            var rWindow = ScriptableObject.CreateInstance(typeof(InputDialog)) as InputDialog;
            rWindow.SetParams(rTitle, rFieldTitles);
            rWindow.SetDefaultResult(rDefaultFieldResults);
            rWindow.ShowModalUtility();
            rFieldResults = rWindow.GetResult();
            return !rWindow.GetIsCancel();
        }
        private string mTitle = string.Empty;
        private string[] mFieldTitles;
        private string[] mFieldResults;
        private bool mIsCancel;
        public void SetParams(string rTitle, string[] rFieldTitles)
        {
            this.mTitle = rTitle;
            this.mFieldTitles = rFieldTitles;
            this.titleContent = new GUIContent(this.mTitle);
            var nWidth = 300;
            var nHeight = 200;
            this.position = new Rect(300, 300, nWidth, nHeight);
            this.mFieldResults = new string[this.mFieldTitles.Length];
            this.mIsCancel = true;
        }
        public void SetDefaultResult(string[] rDefaultFieldResults)
        {
            this.mFieldResults = rDefaultFieldResults;
        }
        public string[] GetResult()
        {
            return this.mFieldResults;
        }
        public bool GetIsCancel()
        {
            return this.mIsCancel;
        }
        private void OnGUI()
        {
            for (int i = 0; i < this.mFieldTitles.Length; i++)
            {
                this.mFieldResults[i] = EditorGUILayout.TextField(this.mFieldTitles[i], this.mFieldResults[i]);
            }
            if (GUILayout.Button("确定"))
            {
                this.mIsCancel = false;
                this.Close();
            }
        }
        void OnInspectorUpdate()
        {
            Repaint();
        }
    }
}
