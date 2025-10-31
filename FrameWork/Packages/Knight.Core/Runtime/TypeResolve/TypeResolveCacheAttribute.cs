using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knight.Core
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Interface, Inherited = true)]
    public class TypeResolveCacheAttribute : Attribute
    {
        public TypeResolveCacheAttribute()
        {
        }
    }
}
