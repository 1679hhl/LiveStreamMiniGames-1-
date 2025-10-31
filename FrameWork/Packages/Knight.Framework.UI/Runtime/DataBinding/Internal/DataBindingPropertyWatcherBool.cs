using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherBool : DataBindingPropertyWatcher
    {

        public Action<bool> mAction;
        public DataBindingPropertyWatcherBool(object rPropOwner, string rPropName, Action<bool> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }
        public void PropertyChanged(string rPropName, bool rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }

    }
}
