//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Knight.Core
{
    public static class DictExpand
    {
        public static bool TryGetValue<TKey, TValue>(this Dict<TKey, TValue> rDict, TKey key, out TValue value)
        {
            TValue result = default(TValue);
            if (rDict.TryGetValue(key, out result))
            {
                value = (TValue)result;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public static bool ContainsKey<TKey, TValue>(this Dict<TKey, TValue> rDict, TKey key)
        {
            return rDict.ContainsKey(key);
        }

        public static bool ContainValue<TKey, TValue>(this Dict<TKey, TValue> rDict, TValue value)
        {
            return rDict.ContainsValue(value);
        }

        public static TKey LastKey<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TKey)rDict.Keys.Last();
        }

        public static TValue LastValue<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TValue)rDict.Values.Last();
        }

        public static TKey FirstKey<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TKey)rDict.Keys.First();
        }

        public static TValue FirstValue<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            return (TValue)rDict.Values.First();
        }
        
        public static KeyValuePair<TKey, TValue> First<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            if (rDict.Count == 0) return new KeyValuePair<TKey, TValue>();
            return new KeyValuePair<TKey, TValue>(rDict.FirstKey(), rDict.FirstValue());
        }

        public static KeyValuePair<TKey, TValue> Last<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            if (rDict.Count == 0) return new KeyValuePair<TKey, TValue>();
            return new KeyValuePair<TKey, TValue>(rDict.LastKey(), rDict.LastValue());
        }

        public static void Clear<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            rDict.Clear();
        }

        public static bool Remove<TKey, TValue>(this Dict<TKey, TValue> rDict, TKey key)
        {
            return rDict.Remove(key);
        }

        public static void RemoveLast<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            rDict.Remove(rDict.LastKey());
        }

        public static void RemoveFirst<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            rDict.Remove(rDict.FirstKey());
        }

        public static Dict<TKey, TValue> Clone<TKey, TValue>(this Dict<TKey, TValue> rDict)
        {
            var newDict = new Dict<TKey, TValue>();
            foreach (var item in rDict)
            {
                newDict.Add((TKey)item.Key, (TValue)item.Value);
            }
            return newDict;
        }

        public static Dict<TKey, TValue> Sort<TKey, TValue>(this Dict<TKey, TValue> rDict, Comparison<KeyValuePair<TKey, TValue>> cmpAlgo)
        {
            var list = new List<KeyValuePair<TKey, TValue>>();
            foreach (var item in rDict)
            {
                list.Add(item);
            }

            list.Sort(cmpAlgo);

            rDict.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                rDict.Add(list[i].Key, list[i].Value);
            }

            return rDict;
        }
    }
    
    public class Dict<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get
            {
                base.TryGetValue(key, out var value);
                return value;
            }
            set { base[key] = value; }
        }
    }
}