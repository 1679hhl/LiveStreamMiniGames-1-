using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Knight.Core
{
    public interface ITypeResolveAssemblyProxy
    {
        void Load();
        Type[] GetAllTypes();
        object Instantiate(string rTypeName, params object[] rArgs);
        T Instantiate<T>(string rTypeName, params object[] rArgs);
        object Invoke(object rObj, string rTypeName, string rMethodName, params object[] rArgs);
        object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs);
    }

    public abstract class TypeResolveAssembly
    {
        public string                   AssemblyName;
        public bool                     IsHotfix;
        public Dictionary<int, Type>    Types;

        public TypeResolveAssembly(string rAssemblyName)
        {
            this.AssemblyName = rAssemblyName;
            this.IsHotfix     = false;
            this.Types        = new Dictionary<int, Type>();
        }

        public virtual void Load()
        {
        }

        public virtual object Instantiate(string rTypeName, params object[] rArgs)
        {
            return null;
        }

        public virtual T Instantiate<T>(string rTypeName, params object[] rArgs)
        {
            return default(T);
        }

        public virtual object Invoke(object rObj, string rTypeName, string rMethodName, params object[] rArgs)
        {
            return null;
        }

        public virtual object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
        {
            return null;
        }
    }

    public class TypeResolveAssembly_Mono : TypeResolveAssembly
    {
        private Assembly    mAssembly;

        public TypeResolveAssembly_Mono(string rAssemblyName) 
            : base(rAssemblyName)
        {
            this.IsHotfix = false;
        }

        public override void Load()
        {
            var rAllAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < rAllAssemblies.Length; i++)
            {
                if (rAllAssemblies[i].GetName().Name.Equals(this.AssemblyName))
                {
                    this.mAssembly = rAllAssemblies[i];
                    break;
                }
            }

            var rtype = string.Empty;
            if (this.mAssembly != null)
            {
                var rAllTypes = this.mAssembly.GetTypes();
                for (int i = 0; i < rAllTypes.Length; i++)
                {
                    var rType = rAllTypes[i];
                    var rTypeResolveCacheAttr = rType.GetCustomAttribute(typeof(TypeResolveCacheAttribute), true);
                    if (rTypeResolveCacheAttr != null)
                    {
                        var nHashcode = rAllTypes[i].FullName.GetHashCode();
                        rtype += rAllTypes[i].FullName+"\r\n";
                        this.Types.Add(nHashcode, rType);
                    }
                }
            }
        }

        public override object Instantiate(string rTypeName, params object[] rArgs)
        {
            if (this.mAssembly == null) return null;
            return Activator.CreateInstance(this.mAssembly.GetType(rTypeName), rArgs);
        }

        public override T Instantiate<T>(string rTypeName, params object[] rArgs)
        {
            if (this.mAssembly == null) return default(T);
            return (T)Activator.CreateInstance(this.mAssembly.GetType(rTypeName), rArgs);
        }

        public override object Invoke(object rObj, string rTypeName, string rMethodName, params object[] rArgs)
        {
            if (this.mAssembly == null) return null;
            if (rObj == null) return null;

            var rType = this.mAssembly.GetType(rTypeName);
            if (rType == null) return null;

            return rType.InvokeMember(rMethodName, ReflectionAssist.flags_method_inst, null, rObj, rArgs);
        }

        public override object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
        {
            if (this.mAssembly == null) return null;

            var rType = this.mAssembly.GetType(rTypeName);
            if (rType == null) return null;

            return rType.InvokeMember(rMethodName, ReflectionAssist.flags_method_static, null, null, rArgs);
        }
    }

    public class TypeResolveAssembly_Hotfix : TypeResolveAssembly
    {
        public ITypeResolveAssemblyProxy Proxy;

        public TypeResolveAssembly_Hotfix(string rAssemblyName)
            : base(rAssemblyName)
        {
            this.IsHotfix = true;
            var rInfos = System.AppDomain.CurrentDomain.GetAssemblies();
            
            var rProxyType = System.AppDomain.CurrentDomain.GetAssemblies()
                             .Single(rAssembly => rAssembly.GetName().Name.Equals("Framework.Hotfix"))?.GetTypes()?
                             .SingleOrDefault(rType => rType.FullName.Equals("Knight.Framework.Hotfix.HotfixTypeResolveAssemblyProxy"));

            if (rProxyType != null)
            {
                this.Proxy = ReflectionAssist.Construct(rProxyType, new Type[] { typeof(string) }, rAssemblyName) as ITypeResolveAssemblyProxy;
            }
        }

        public override void Load()
        {
            this.Proxy?.Load();

            // 初始化Type
            var rAllTypes = this.Proxy?.GetAllTypes();
            if (rAllTypes != null)
            {
                for (int i = 0; i < rAllTypes.Length; i++)
                {
                    var rType = rAllTypes[i];
                    var rTypeResolveCacheAttr = rType.GetCustomAttribute(typeof(TypeResolveCacheAttribute), true);
                    if (rTypeResolveCacheAttr != null)
                    {
                        var rFullName = rType.FullName.Replace('/', '+');
                        var nHashcode = rFullName.GetHashCode();
                        if (!this.Types.ContainsKey(nHashcode))
                        {
                            this.Types.Add(nHashcode, rType);
                        }
                    }
                }
            }
        }

        public override object Instantiate(string rTypeName, params object[] rArgs)
        {
            return this.Proxy?.Instantiate(rTypeName, rArgs);
        }

        public override T Instantiate<T>(string rTypeName, params object[] rArgs)
        {
            if (this.Proxy == null) return default(T);
            return this.Proxy.Instantiate<T>(rTypeName, rArgs);
        }

        public override object Invoke(object rObj, string rTypeName, string rMethodName, params object[] rArgs)
        {
            if (this.Proxy == null) return null;
            return this.Proxy.Invoke(rObj, rTypeName, rMethodName, rArgs);
        }

        public override object InvokeStatic(string rTypeName, string rMethodName, params object[] rArgs)
        {
            if (this.Proxy == null) return null;
            return this.Proxy.InvokeStatic(rTypeName, rMethodName, rArgs);
        }
    }
}
