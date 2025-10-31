using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherObject : DataBindingPropertyWatcher
    {

        public Action<object> mAction;
        public DataBindingPropertyWatcherObject(object rPropOwner, string rPropName, Action<object> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }
        public void PropertyChanged(string rPropName, object rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }

    }
}
