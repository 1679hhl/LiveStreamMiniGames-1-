using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherVector3 : DataBindingPropertyWatcher
    {

        public Action<Vector3> mAction;
        public DataBindingPropertyWatcherVector3(object rPropOwner, string rPropName, Action<Vector3> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }
        public void PropertyChanged(string rPropName, Vector3 rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }

    }
}
