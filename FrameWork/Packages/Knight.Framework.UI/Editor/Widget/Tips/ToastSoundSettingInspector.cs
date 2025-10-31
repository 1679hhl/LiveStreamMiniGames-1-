using NaughtyAttributes.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Editor
{
    [CustomEditor(typeof(Toast), true)]
    public class ToastSoundSettingInspector : InspectorEditor
    {
        private Toast mTarget;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.mTarget = this.target as Toast;
        }
    }
}
