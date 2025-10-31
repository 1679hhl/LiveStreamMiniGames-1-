using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knight.Core
{
    public static class DictionaryExpand
    {
        public static bool ExEquals<TKey, TValue>(this Dictionary<TKey, TValue> rSource, Dictionary<TKey, TValue> rTarget)
        {
            if (rSource == null && rTarget == null)
            {
                return true;
            }
            else if (rSource == null || rTarget == null)
            {
                return false;
            }
            if (rSource.Count != rTarget.Count)
            {
                return false;
            }
            foreach (var rSourceKV in rSource)
            {
                if (!rTarget.TryGetValue(rSourceKV.Key, out var rTargetV) || !rSourceKV.Value.Equals(rTargetV))
                {
                    return false;
                }
            }
            return true;
        }
    }
}