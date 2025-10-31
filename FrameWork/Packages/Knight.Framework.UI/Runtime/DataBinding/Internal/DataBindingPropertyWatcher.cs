using System;
using System.Collections.Generic;
using System.Linq;
using Knight.Core;

namespace UnityEngine.UI
{
    public class DataBindingPropertyWatcher
    {
        protected object          mPropertyOwner;
        protected string          mPropertyName;
        //public Action<Vector3> mActionVector3;

        public DataBindingPropertyWatcher(object rPropOwner, string rPropName)
        {
            this.mPropertyOwner = rPropOwner;
            this.mPropertyName = rPropName;
        }
        //    public DataBindingPropertyWatcher(object rPropOwner, string rPropName, Action<int> rAction)
        //    {
        //        this.mPropertyOwner = rPropOwner;
        //        this.mPropertyName = rPropName;
        //        this.mActionInt = rAction;
        //    }

        //    public DataBindingPropertyWatcher(object rPropOwner, string rPropName, Action<bool> rAction)
        //    {
        //        this.mPropertyOwner = rPropOwner;
        //        this.mPropertyName = rPropName;
        //        this.mActionBool = rAction;
        //    }


        //    public DataBindingPropertyWatcher(object rPropOwner, string rPropName, Action<float> rAction)
        //    {
        //        this.mPropertyOwner = rPropOwner;
        //        this.mPropertyName = rPropName;
        //        this.mActionFloat = rAction;
        //    }


        //    public DataBindingPropertyWatcher(object rPropOwner, string rPropName, Action<Color> rAction)
        //    {
        //        this.mPropertyOwner = rPropOwner;
        //        this.mPropertyName = rPropName;
        //        this.mActionColor = rAction;
        //    }


        //    public DataBindingPropertyWatcher(object rPropOwner, string rPropName, Action<string> rAction)
        //    {
        //        this.mPropertyOwner = rPropOwner;
        //        this.mPropertyName = rPropName;
        //        this.mActionString = rAction;
        //    }

        //    public DataBindingPropertyWatcher(object rPropOwner, string rPropName, Action<Vector2> rAction)
        //    {
        //        this.mPropertyOwner = rPropOwner;
        //        this.mPropertyName = rPropName;
        //        this.mActionVector2 = rAction;
        //    }

        //public virtual void PropertyChanged(string rPropName, object rvalue)
        //{

        //}

        //    public void PropertyChanged(string rPropName, bool rvalue)
        //    {
        //        if (!this.mPropertyName.Equals(rPropName)) return;

        //        UtilTool.SafeExecute(this.mActionBool, rvalue);
        //    }

        //    public void PropertyChanged(string rPropName, float rvalue)
        //    {
        //        if (!this.mPropertyName.Equals(rPropName)) return;

        //        UtilTool.SafeExecute(this.mActionFloat, rvalue);
        //    }

        //    public void PropertyChanged(string rPropName, Color rvalue)
        //    {
        //        if (!this.mPropertyName.Equals(rPropName)) return;

        //        UtilTool.SafeExecute(this.mActionColor, rvalue);
        //    }
        //    public void PropertyChanged(string rPropName, string rvalue)
        //    {
        //        if (!this.mPropertyName.Equals(rPropName)) return;

        //        UtilTool.SafeExecute(this.mActionString, rvalue);
        //    }

        //    public void PropertyChanged(string rPropName, Vector2 rvalue)
        //    {
        //        if (!this.mPropertyName.Equals(rPropName)) return;

        //        UtilTool.SafeExecute(this.mActionVector2, rvalue);
        //    }
    }
}
