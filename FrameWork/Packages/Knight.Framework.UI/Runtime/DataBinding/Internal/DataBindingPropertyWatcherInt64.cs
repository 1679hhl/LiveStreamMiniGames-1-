using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherInt64 : DataBindingPropertyWatcher
    {

        public Action<long> mAction;
        public DataBindingPropertyWatcherInt64(object rPropOwner, string rPropName, Action<long> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }

        public void PropertyChanged(string rPropName, long rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }
    }
}
