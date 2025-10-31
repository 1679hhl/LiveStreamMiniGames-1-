//======================================================================
//        Copyright (C) 2015-2020 Winddy He. All rights reserved
//        Email: hgplan@126.com
//======================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Knight.Core
{
    public class IndexedDict<TKey, TValue>
    {
        private Dictionary<TKey, TValue>    mDictionary;
        private List<TKey>                  mKeyList;

        public IndexedDict()
        {
            mDictionary = new Dictionary<TKey, TValue>();
            mKeyList = new List<TKey>();
        }
        public IndexedDict(int nCapacity)
        {
            mDictionary = new Dictionary<TKey, TValue>(nCapacity);
            mKeyList = new List<TKey>(nCapacity);
        }

        public void Add(TKey rKey, TValue rValue)
        {
            mDictionary.Add(rKey, rValue);
            mKeyList.Add(rKey);
        }

        public int Count
        {
            get { return mDictionary.Count;  }
        }

        public TValue this[TKey rKey]
        {
            get { return mDictionary[rKey];  }
            set { mDictionary[rKey] = value; }
        }

        public Dictionary<TKey, TValue> Dict
        {
            get { return mDictionary;        }
        }

        public List<TKey> Keys
        {
            get { return mKeyList;           }
        }
    }

    public static class IndexedDictExpand
    {
        public static bool TryGetValue<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict, TKey rKey, out TValue rValue)
        {
            return rIndexedDict.Dict.TryGetValue(rKey, out rValue);
        }
        
        public static TKey LastKey<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict)
        {
            return rIndexedDict.Keys[rIndexedDict.Keys.Count - 1];
        }

        public static TValue LastValue<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict)
        {
            return rIndexedDict.Dict[LastKey(rIndexedDict)];
        }

        public static TKey FirstKey<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict)
        {
            return rIndexedDict.Keys[0];
        }

        public static TValue FirstValue<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict)
        {
            return rIndexedDict.Dict[FirstKey(rIndexedDict)];
        }

        public static KeyValuePair<TKey, TValue> First<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict)
        {
            if (rIndexedDict.Count == 0) return new KeyValuePair<TKey, TValue>(); ;
            return new KeyValuePair<TKey, TValue>(rIndexedDict.FirstKey(), rIndexedDict.FirstValue());
        }

        public static KeyValuePair<TKey, TValue> Last<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict)
        {
            if (rIndexedDict.Count == 0) return new KeyValuePair<TKey, TValue>();
            return new KeyValuePair<TKey, TValue>(rIndexedDict.LastKey(), rIndexedDict.LastValue());
        }

        public static void Clear<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict)
        {
            rIndexedDict.Dict.Clear();
            rIndexedDict.Keys.Clear();
        }

        public static bool Remove<TKey, TValue>(this IndexedDict<TKey, TValue> rIndexedDict, TKey key)
        {
            rIndexedDict.Keys.Remove(key);
            return rIndexedDict.Dict.Remove(key);
        }
    }
}
