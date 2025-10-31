using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherString : DataBindingPropertyWatcher
    {

        public Action<string> mAction;
        public DataBindingPropertyWatcherString(object rPropOwner, string rPropName, Action<string> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }

        public void PropertyChanged(string rPropName, string rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;
            UtilTool.SafeExecute(this.mAction, rvalue);
        }

    }
}
