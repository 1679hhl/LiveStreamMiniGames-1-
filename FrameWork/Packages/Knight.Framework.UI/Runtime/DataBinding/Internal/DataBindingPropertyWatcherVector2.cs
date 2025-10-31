using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherVector2 : DataBindingPropertyWatcher
    {

        public Action<Vector2> mAction;
        public DataBindingPropertyWatcherVector2(object rPropOwner, string rPropName, Action<Vector2> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }
        public void PropertyChanged(string rPropName, Vector2 rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }

    }
}
