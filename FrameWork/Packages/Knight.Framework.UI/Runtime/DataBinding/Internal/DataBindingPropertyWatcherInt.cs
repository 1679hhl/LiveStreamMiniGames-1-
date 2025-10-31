using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherInt : DataBindingPropertyWatcher
    {

        public Action<int> mAction;
        public DataBindingPropertyWatcherInt(object rPropOwner, string rPropName, Action<int> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }

        public void PropertyChanged(string rPropName, int rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }
    }
}
