using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Knight.Core;
using UnityEngine;

namespace Knight.Framework.Hotfix
{
    public class HotfixTypeResolveAssemblyProxy : ITypeResolveAssemblyProxy
    {
        private string mAssemblyName;

        public HotfixTypeResolveAssemblyProxy(string rAssemblyName)
        {
            this.mAssemblyName = rAssemblyName;
        }

        public void Load()
        {
#if UNITY_EDITOR
            // 编辑器下初始化
            if (!Application.isPlaying)
            {
                HotfixManager.Instance.Initialize();
                HotfixManager.Instance.InitApp("Game.Hotfix",HotfixManager.Instance.HofixApp);
            }
            return;
#endif
            HotfixManager.Instance.Initialize();
            HotfixManager.Instance.InitApp("Game.Hotfix",HotfixManager.Instance.HofixApp);
        }

        public Type[] GetAllTypes()
        {
            return HotfixManager.Instance.GetTypes();
        }

        public object Instantiate(string rTypeName, params object[] rArgs)
        {
            return null;
        }

        public T Instantiate<T>(string rTypeName, params object[] rArgs)
        {
            return default(T);
        }

        public object Invoke(object rObj, string rTypeName, string rMethodName, params object[] rArgs)
        {
            return default;
        }

        public object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
        {
            return default;
        }
    }
}
