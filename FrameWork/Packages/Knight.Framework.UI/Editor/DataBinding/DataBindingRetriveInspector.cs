using NaughtyAttributes.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(DataBindingRetrive), true)]
    public class DataBindingRetriveInspector : InspectorEditor
    {
        private DataBindingRetrive mTarget;

        protected override void OnEnable()
        {
            base.OnEnable();
            this.mTarget = this.target as DataBindingRetrive;
        }

        public override void OnInspectorGUI()
        {
            this.mTarget.GetPaths();
            base.OnInspectorGUI();
        }
    }
}
