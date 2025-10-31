using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherFloat : DataBindingPropertyWatcher
    {

        public Action<float> mAction;
        public DataBindingPropertyWatcherFloat(object rPropOwner, string rPropName, Action<float> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }

        public void PropertyChanged(string rPropName, float rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }
    }
}
