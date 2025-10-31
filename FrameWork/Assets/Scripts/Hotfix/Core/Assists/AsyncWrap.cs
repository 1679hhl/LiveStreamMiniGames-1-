using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Knight.Hotfix.Core
{
    public static class AsyncWrap
    {
        public static async void WrapErrors(this Task rTask)
        {
            await rTask;
        }
    }
}
