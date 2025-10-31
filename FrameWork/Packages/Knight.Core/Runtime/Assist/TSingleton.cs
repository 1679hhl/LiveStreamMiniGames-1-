//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Reflection;

namespace Knight.Core
{
    public class TSingleton<T> where T : class
    {
        protected static object   SyncRoot = new object();
        protected static T        instance;

        public static readonly Type[]   EmptyTypes = new Type[0];

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (instance == null)
                        {
                            ConstructorInfo ci = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, EmptyTypes, null);
                            if (ci == null) 
                            { 
                                throw new InvalidOperationException($"class must contain a private constructor {typeof(T)}"); 
                            }
                            instance = (T)ci.Invoke(null);
                        }
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// 谨慎调用,确保该单例对象需要释放的时候才能调用
        /// </summary>
        public static void DestroyInstance()
        {
            instance = null;
        }
    }

    public class Singleton<T> where T : new()
    {
        private static T        __instance;
        private static object   __lock = new object();

        private Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                if (__instance == null)
                {
                    lock (__lock)
                    {
                        if (__instance == null)
                        {
                            __instance = new T();
                        }
                    }
                }
                return __instance;
            }
        }

        public static void SetInstanceNull()
        {
            __instance = default(T);
        }
    }
}

