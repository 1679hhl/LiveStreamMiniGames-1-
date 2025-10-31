using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knight.Core
{
    public static class ListExpand
    {
        public static string Concat<T>(this IList<T> rList, string rSplitChar = "")
        {
            if (rList == null)
            {
                return "";
            }
            var rStringBuilder = new StringBuilder();
            for (int i = 0; i < rList.Count; i++)
            {
                rStringBuilder.A(rList[i].ToString());
                if (i + 1 != rList.Count)
                {
                    rStringBuilder.A(rSplitChar);
                }
            }
            return rStringBuilder.ToString();
        }
    }
    public static class HashSetExpand
    {
        public static string Concat<T>(this ISet<T> rHashSet, string rSplitChar = "")
        {
            if (rHashSet == null)
            {
                return "";
            }
            var rStringBuilder = new StringBuilder();
            var nIndex = 0;
            foreach (var rItem in rHashSet)
            {
                rStringBuilder.A(rItem.ToString());
                if (nIndex + 1 != rHashSet.Count)
                {
                    rStringBuilder.A(rSplitChar);
                }
                nIndex++;
            }
            return rStringBuilder.ToString();
        }
    }
}
