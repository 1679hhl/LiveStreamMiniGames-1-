//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

namespace Knight.Core
{
    public static class ObjectExpand
    {
        /************************************************************************************
         * Component的方法扩展
         *      ReceiveComponent
         *      SafeGetComponent
         ************************************************************************************/
        public static T SafeGetComponent<T>(this GameObject rGo) where T : Component
        {
            if (rGo == null) return default(T);
            return rGo.GetComponent<T>();
        }

        public static T ReceiveComponent<T>(this GameObject rGo) where T : Component
        {
            if (rGo == null) return default(T);

            T rComponent = rGo.GetComponent<T>();
            if (rComponent == null) rComponent = rGo.AddComponent<T>();

            return rComponent;
        }

        public static int ToInt32(this object obj)
        {
            if (obj is int)
                return (int)obj;
            if (obj is float)
                return (int)(float)obj;
            if (obj is long)
                return (int)(long)obj;
            if (obj is short)
                return (int)(short)obj;
            if (obj is double)
                return (int)(double)obj;
            if (obj is byte)
                return (int)(byte)obj;
            if (obj is uint)
                return (int)(uint)obj;
            if (obj is ushort)
                return (int)(ushort)obj;
            if (obj is sbyte)
                return (int)(sbyte)obj;
            return Convert.ToInt32(obj);
        }
    }
}
