using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityFx.Async.Extensions
{
    public static class AsyncExtensions
    {
        public static CompilerServices.IAsyncAwaiter GetAwaiter(this IAsync op)
        {
            return new CompilerServices.IAsyncAwaiter(op);
        }
    }
}
