//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Knight.Core.WindJson
{
    public class JsonNode
    {
        public virtual string Value { get; set; }
        public virtual string Key { get; set; }
        public virtual int Count { get; private set; }

        public virtual JsonNode this[int nIndex] { get { return null; } set { } }
        public virtual JsonNode this[string rKey] { get { return null; } set { } }
        public virtual JsonNode Node { get; set; }

        public virtual void Add(string rKey, JsonNode rItem) { }
        public virtual void Add(JsonNode rItem) { }

        public virtual void AddHead(string rKey, JsonNode rItem) { }
        public virtual void AddHead(JsonNode rItem) { }

        public virtual void Remove(string rKey) { }
        public virtual void Remove(int nIndex) { }

        public virtual List<string> Keys { get { return new List<string>(); } }
        public virtual bool ContainsKey(string rKey) { return false; }

        public override string ToString() { return base.ToString(); }
        public virtual object ToObject(Type rType) { return null; }
        public T ToObject<T>() { return (T)ToObject(typeof(T)); }
        public List<T> ToList<T>() { return (List<T>)ToObject(typeof(List<T>)); }
        public T[] ToArray<T>() { return (T[])ToObject(typeof(T[])); }

        public virtual object ToList(Type rListType, Type rElemType) { return null; }
        public virtual object ToDict(Type rDictType, Type rKeyType, Type rValueType) { return null; }

        public virtual bool TryGetValue(string key, out JsonNode value)
        {
            value = null;
            return false;
        }

        public Dict<TKey, TValue> ToDict<TKey, TValue>()
        {
            return (Dict<TKey, TValue>)ToObject(typeof(Dict<TKey, TValue>));
        }
        public Dictionary<TKey, TValue> ToDictionary<TKey, TValue>()
        {
            return (Dictionary<TKey, TValue>)ToObject(typeof(Dictionary<TKey, TValue>));
        }

        public virtual bool AsNull
        {
            get { return false; }
        }
        public virtual byte AsByte
        {
            get { return CastByte(Value); }
            set { Value = value.ToString(); }
        }

        public virtual short AsShort
        {
            get { return CastShort(Value); }
            set { Value = value.ToString(); }
        }

        public virtual ushort AsUShort
        {
            get { return CastUShort(Value); }
            set { Value = value.ToString(); }
        }

        public virtual int AsInt
        {
            get { return CastInt(Value); }
            set { Value = value.ToString(); }
        }

        public virtual uint AsUint
        {
            get { return CastUInt(Value); }
            set { Value = value.ToString(); }
        }

        public virtual long AsLong
        {
            get { return CastLong(Value); }
            set { Value = value.ToString(); }
        }

        public virtual ulong AsUlong
        {
            get { return CastULong(Value); }
            set { Value = value.ToString(); }
        }

        public virtual float AsFloat
        {
            get { return CastFloat(Value); }
            set { Value = value.ToString(); }
        }

        public virtual double AsDouble
        {
            get { return CastDouble(Value); }
            set { Value = value.ToString(); }
        }

        public virtual bool AsBool
        {
            get { return CastBool(Value); }
            set { Value = value.ToString(); }
        }

        public virtual string AsString
        {
            get { return Value; }
            set { Value = value; }
        }

        public byte CastByte(string value)
        {
            byte re = 0;
            if (byte.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not byte type.", value));
            return re;
        }

        public short CastShort(string value)
        {
            short re = 0;
            if (short.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not short type.", value));
            return re;
        }

        public ushort CastUShort(string value)
        {
            ushort re = 0;
            if (ushort.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not ushort type.", value));
            return re;
        }

        public int CastInt(string value)
        {
            int re = 0;
            if (int.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public uint CastUInt(string value)
        {
            uint re = 0;
            if (uint.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public long CastLong(string value)
        {
            long re = 0;
            if (long.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public ulong CastULong(string value)
        {
            ulong re = 0;
            if (ulong.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public float CastFloat(string value)
        {
            float re = 0;
            if (float.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public double CastDouble(string value)
        {
            double re = 0;
            if (double.TryParse(value, out re)) return re;
            Knight.Core.LogManager.LogError(string.Format("Value: {0} is not int type.", value));
            return re;
        }

        public bool CastBool(string value)
        {
            if (value.ToLower() == "false" || value.ToLower() == "true")
                return value.ToLower() == "false" ? false : true;
            else
                return CastInt(value) == 0 ? false : true;
        }

        public object CastEnum(Type type, string value)
        {
            //如果enum是数字，那么返回数字
#if !LOGIC_BATTLE    // 逻辑战斗解析问题
            var rBaseType = type.GetEnumUnderlyingType();
            if (rBaseType == typeof(int) || rBaseType.Name == "Int32")
            {
                if (int.TryParse(value, out var re))
                {
                    return re;
                }
            }
            else if (rBaseType == typeof(byte))
            {
                if (byte.TryParse(value, out var re))
                {
                    return re;
                }
            }
#endif

            //如果不是数字，而是字符串，直接转换为enum
            type = ITypeRedirect.GetRedirectType(type);
            return Enum.Parse(type, value, true);
        }
    }

    public class JsonArray : JsonNode, IEnumerable
    {
        private List<JsonNode> list = new List<JsonNode>();

        public override JsonNode this[int nIndex]
        {
            get
            {
                if (nIndex >= 0 && nIndex < Count) return list[nIndex];
                Knight.Core.LogManager.LogError(string.Format("Index out of size limit, Index = {0}, Count = {1}", nIndex, Count));
                return null;
            }
            set
            {
                if (nIndex >= Count)
                    list.Add(value);
                else if (nIndex >= 0 && nIndex < Count)
                    list[nIndex] = value;
            }
        }

        public override int Count { get { return list.Count; } }

        public override void Add(JsonNode rItem)
        {
            //if (!list.Contains(rItem))
            list.Add(rItem);
        }

        public override void AddHead(JsonNode rItem)
        {
            //if (!list.Contains(rItem))
            list.Insert(0, rItem);
        }

        public override void Remove(int nIndex)
        {
            if (nIndex < 0 || nIndex >= Count)
                return;
            JsonNode tmp = list[nIndex];
            list.RemoveAt(nIndex);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var rNode in list)
            {
                yield return rNode;
            }
        }

        public override string ToString()
        {
            return "";
            /*
            string jsonStr = "[";
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null)
                    jsonStr += "null";
                else
                    jsonStr += list[i].ToString();
                if (i < list.Count - 1)
                    jsonStr += ",";
            }
            jsonStr += "]";
            return jsonStr;
            */
        }

        public override object ToObject(Type rType)
        {
            rType = ITypeRedirect.GetRedirectType(rType);
            if (rType.IsArray)
            {
                Array rObject = Array.CreateInstance(rType.GetElementType(), this.Count);
                Type rArrayElemType = rType.GetElementType();
                for (int i = 0; i < this.Count; i++)
                {
                    object rValue = this.list[i].ToObject(rArrayElemType);
                    rObject.SetValue(rValue, i);
                }
                return rObject;
            }
            else if (rType.IsGenericType && typeof(IList).IsAssignableFrom(rType.GetGenericTypeDefinition()))  //是否为泛型
            {
                IList rObject = (IList)Activator.CreateInstance(rType);
                Type[] rArgsTypes = rType.GetGenericArguments();
                for (int i = 0; i < this.Count; i++)
                {
                    var rElemType = rArgsTypes[0];
                    if (this.list[i] == null)
                    {
                        rObject.Add(null);
                    }
                    else
                    {
                        object rValue = this.list[i].ToObject(rElemType);
                        rObject.Add(rValue);
                    }
                }
                return rObject;
            }
            return null;
        }

        public override object ToList(Type rListType, Type rElemType)
        {
            var rCLRType = ITypeRedirect.GetRedirectType(rListType);
            if (rCLRType.IsArray)
            {
                Array rObject = Array.CreateInstance(rCLRType.GetElementType(), this.Count);
                for (int i = 0; i < this.Count; i++)
                {
                    object rValue = this.list[i].ToObject(rElemType);
                    rObject.SetValue(rValue, i);
                }
                return rObject;
            }
            else if (rCLRType.IsGenericType && typeof(IList).IsAssignableFrom(rCLRType.GetGenericTypeDefinition()))  //是否为泛型
            {
                IList rObject = (IList)Activator.CreateInstance(rListType);
                Type[] rArgsTypes = rCLRType.GetGenericArguments();
                for (int i = 0; i < this.Count; i++)
                {
                    object rValue = this.list[i].ToObject(rElemType);
                    rObject.Add(rValue);
                }
                return rObject;
            }
            return null;
        }
    }

    public class JsonClass : JsonNode, IEnumerable
    {
        private Dict<string, JsonNode> dict = new Dict<string, JsonNode>();
        private string firstKey = null;

        public override JsonNode this[string rKey]
        {
            get
            {
                JsonNode rNode = null;
                dict.TryGetValue(rKey, out rNode);
                return rNode;
            }
            set
            {
                if (dict.ContainsKey(rKey))
                    dict[rKey] = value;
                else
                    dict.Add(rKey, value);
            }
        }

        public override List<string> Keys
        {
            get
            {
                List<string> rKeys = new List<string>();
                foreach (var rItem in dict)
                {
                    rKeys.Add(rItem.Key);
                }
                return rKeys;
            }
        }

        public override string Key
        {
            get
            {
                return firstKey == null ? "" : firstKey;
            }
            set
            {
            }
        }

        public override bool ContainsKey(string rKey)
        {
            return dict.ContainsKey(rKey);
        }

        public override int Count { get { return dict.Count; } }

        public override void Add(string rKey, JsonNode rItem)
        {
            dict[rKey] = rItem;
            if (firstKey == null)
                firstKey = rKey;
        }

        public override void AddHead(string rKey, JsonNode rItem)
        {
            dict[rKey] = rItem;
            firstKey = rKey;
        }

        public override void Remove(string rKey)
        {
            dict.Remove(rKey);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var rItem in dict)
                yield return rItem;
        }

        public override string ToString()
        {
            return "";
            /*
            string jsonStr = "{";
            int i = 0;
            foreach (var rItem in dict)
            {
                var rValue = rItem.Value == null ? "" : rItem.Value.ToString();
                jsonStr += "\"" + rItem.Key + "\":" + rValue;
                if (i < Count - 1) jsonStr += ",";
                i++;
            }
            jsonStr += "}";
            return jsonStr;
            */
        }

        public override object ToObject(Type rType)
        {
            if (rType == null) return null;

            rType = ITypeRedirect.GetRedirectType(rType);
            if (rType.IsGenericType && typeof(IDictionary).IsAssignableFrom(rType.GetGenericTypeDefinition()))
            {
                // 特殊处理IDictionary<,>类型
                IDictionary rObject = (IDictionary)ReflectionAssist.CreateInstance(rType, ReflectionAssist.flags_all);
                Type[] rArgsTypes = rType.GetGenericArguments();
                foreach (var rItem in this.dict)
                {
                    object rKey = GetKey_ByString(rArgsTypes[0], rItem.Key);
                    if (rItem.Value == null)
                    {
                        rObject.Add(rKey, null);
                    }
                    else
                    {
                        object rValue = rItem.Value.ToObject(rArgsTypes[1]);
                        rObject.Add(rKey, rValue);
                    }
                }
                return rObject;
            }
            else if (rType.IsClass || (!rType.IsPrimitive && !rType.IsEnum && rType.IsValueType))
            {
                BindingFlags rBindFlags = ReflectionAssist.flags_all;
                object rObject = null;
                if (!rType.IsPrimitive && !rType.IsEnum && rType.IsValueType)
                {
                    rObject = rType.Assembly.CreateInstance(rType.FullName);
                }
                else
                {
                    rObject = ReflectionAssist.CreateInstance(rType, rBindFlags);
                }
                foreach (var rItem in this.dict)
                {
                    Type rMemberType = null;
                    FieldInfo rFieldInfo = rType.GetField(rItem.Key, rBindFlags);
                    if (rFieldInfo != null)
                    {
                        if (rItem.Value == null)
                        {
                            rFieldInfo.SetValue(rObject, null);
                        }
                        else
                        {
                            rMemberType = rFieldInfo.FieldType;
                            object rValueObj = rItem.Value.ToObject(rMemberType);
                            rFieldInfo.SetValue(rObject, rValueObj);
                        }
                        continue;
                    }
                    PropertyInfo rPropInfo = rType.GetProperty(rItem.Key, rBindFlags);
                    if (rPropInfo != null)
                    {
                        if (rItem.Value == null)
                        {
                            rPropInfo.SetValue(rObject, null, null);
                        }
                        else
                        {
                            rMemberType = rPropInfo.PropertyType;
                            object rValueObj = rItem.Value.ToObject(rMemberType);
                            rPropInfo.SetValue(rObject, rValueObj, null);
                        }
                        continue;
                    }
                }
                return rObject;
            }
            return null;
        }

        public override bool TryGetValue(string key, out JsonNode value)
        {
            return dict.TryGetValue(key, out value);
        }

        /// <summary>
        /// 转化Key
        /// </summary>
        private object GetKey_ByString(Type rKeyType, string rKeyStr)
        {
            object rKey = rKeyStr;
            if (rKeyType == typeof(int))
            {
                int rIntKey = 0;
                int.TryParse(rKeyStr, out rIntKey);
                rKey = rIntKey;
            }
            else if (rKeyType == typeof(long))
            {
                long rLongKey = 0;
                long.TryParse(rKeyStr, out rLongKey);
                rKey = rLongKey;
            }
            return rKey;
        }

        public override object ToDict(Type rDictType, Type rKeyType, Type rValueType)
        {
            rDictType = ITypeRedirect.GetRedirectType(rDictType);
            if (rDictType.IsGenericType && typeof(IDictionary).IsAssignableFrom(rDictType.GetGenericTypeDefinition()))
            {
                // 特殊处理IDictionary<,>类型
                IDictionary rObject = (IDictionary)ReflectionAssist.CreateInstance(rDictType, BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (var rItem in this.dict)
                {
                    object rKey = GetKey_ByString(rKeyType, rItem.Key);
                    object rValue = rItem.Value.ToObject(rValueType);
                    rObject.Add(rKey, rValue);
                }
                return rObject;
            }
            return null;
        }
    }

    public class JsonData : JsonNode
    {
        private string value;
        private Type type;
        private bool isNull = false;
        private JsonData()
        {
        }
        public static JsonData GetNull()
        {
            return new JsonData()
            {
                value = null,
                type = null,
                isNull = true,
            };
        }

        public JsonData(string v)
        {
            this.type = typeof(string);
            this.value = v;
            //type = v.GetType();
            //if (v.Equals("null"))
            //{
            //    value = "";
            //}
            //else
            //{
            //    value = v;
            //}
        }

        public JsonData(float v)
        {
            type = v.GetType();
            AsFloat = v;
        }

        public JsonData(double v)
        {
            type = v.GetType();
            AsDouble = v;
        }

        public JsonData(int v)
        {
            type = v.GetType();
            AsInt = v;
        }

        public JsonData(uint v)
        {
            type = v.GetType();
            AsUint = v;
        }

        public JsonData(long v)
        {
            type = v.GetType();
            AsLong = v;
        }

        public JsonData(ulong v)
        {
            type = v.GetType();
            AsUlong = v;
        }

        public JsonData(bool v)
        {
            type = v.GetType();
            AsBool = v;
        }

        public JsonData(byte v)
        {
            type = v.GetType();
            AsByte = v;
        }

        public JsonData(short v)
        {
            type = v.GetType();
            AsShort = v;
        }

        public JsonData(ushort v)
        {
            type = v.GetType();
            AsUShort = v;
        }

        public override bool AsNull => this.isNull;

        public override string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public override string ToString()
        {
            if (this.AsNull)
            {
                return "null";
            }
            else
            {
                if (type.Equals(typeof(string)) || type.Equals(typeof(Boolean)))
                {
                    if (LexicalAnalysis.NeedBackslashSymbol(this.value))
                        return "\"" + LexicalAnalysis.BackslashSymbol(this.value) + "\"";
                    else
                        return "\"" + this.value + "\"";
                }
                else
                    return value.ToString();
            }
        }

        public override List<string> Keys
        {
            get
            {
                var rKeys = new List<string>();
                rKeys.Add(this.Key);
                return rKeys;
            }
        }

        public override bool ContainsKey(string rKey)
        {
            return rKey.Equals(this.Key);
        }

        public override object ToObject(Type rType)
        {
            rType = ITypeRedirect.GetRedirectType(rType);
            type = rType;
            if (rType.IsPrimitive)
            {
                if (rType == typeof(int))
                {
                    return CastInt(this.value);
                }
                else if (rType == typeof(uint))
                {
                    return CastUInt(this.value);
                }
                else if (rType == typeof(long))
                {
                    return CastLong(this.value);
                }
                else if (rType == typeof(ulong))
                {
                    return CastULong(this.value);
                }
                else if (rType == typeof(float))
                {
                    return CastFloat(this.value);
                }
                else if (rType == typeof(double))
                {
                    return CastDouble(this.value);
                }
                else if (rType == typeof(bool))
                {
                    return CastBool(this.value);
                }
                else if (rType == typeof(byte))
                {
                    return CastByte(this.value);
                }
                else if (rType == typeof(short))
                {
                    return CastShort(this.value);
                }
                else if (rType == typeof(ushort))
                {
                    return CastUShort(this.value);
                }
            }
            else if (rType.IsEnum)
            {
                return this.CastEnum(rType, this.value);
            }
            else if (rType == typeof(string))
            {
                if (string.IsNullOrEmpty(this.value))
                    return "";
                return this.value;
            }
            else if (rType == typeof(string[]) || rType == typeof(List<string>))
            {
                if (string.IsNullOrEmpty(this.value) || this.value.Equals("null"))
                    return null;
            }

            if (this.value.Equals("null") || string.IsNullOrWhiteSpace(this.value)) return null;

            Knight.Core.LogManager.LogErrorFormat("{0}不是基础类型，不能解析成为JsonData !", this.value);
            return this.value.Trim('"');
        }
    }
}
