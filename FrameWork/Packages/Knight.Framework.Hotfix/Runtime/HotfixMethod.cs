using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Knight.Framework.Hotfix
{
    public class IHotfixMethod
    {
        public object[] Params;

        public void Initialize(int nParamCount)
        {
            this.Params = new object[nParamCount];
        }

        public virtual object Invoke(HotfixApp rApp, HotfixObject rHotfixObj)
        {
            return null;
        }
    }

    public class HotfixMethod_Reflect : IHotfixMethod
    {
        public MethodInfo Method;

        public override object Invoke(HotfixApp rApp, HotfixObject rHotfixObj)
        {
            return rApp.Invoke(rHotfixObj, this);
        }
    }
}