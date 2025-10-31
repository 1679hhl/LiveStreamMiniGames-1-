using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcherColor : DataBindingPropertyWatcher
    {

        public Action<Color> mAction;
        public DataBindingPropertyWatcherColor(object rPropOwner, string rPropName, Action<Color> rAction) : base(rPropOwner, rPropName)
        {
            this.mAction = rAction;
        }

        public void PropertyChanged(string rPropName, Color rvalue)
        {
            if (!this.mPropertyName.Equals(rPropName)) return;

            UtilTool.SafeExecute(this.mAction, rvalue);
        }
    }
}
