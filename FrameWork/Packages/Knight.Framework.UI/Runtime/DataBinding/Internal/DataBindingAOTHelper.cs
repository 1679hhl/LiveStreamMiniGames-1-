using Knight.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;

namespace UnityEngine.UI
{
#pragma warning restore 219
    public class DataBindingAOTHelper
    {
        public void EnsureGenericTypes()
        {
            var strEventBinder = new UnityEventBinder<string>(null, null);
            var floatEventBinder = new UnityEventBinder<float>(null, null);
            var boolEventBinder = new UnityEventBinder<bool>(null, null);
            var intEventBinder = new UnityEventBinder<int>(null, null);
            var vector2EventBinder = new UnityEventBinder<Vector2>(null, null);
            var vector3EventBinder = new UnityEventBinder<Vector3>(null, null);
            var colorEventBinder = new UnityEventBinder<Color>(null, null);
            var baseEventDataEventBinder = new UnityEventBinder<EventArg>(null, null);
            var threeEventBinder1 = new UnityEventBinder<Vector3, Vector2, bool>(null, null);
            var fourEventBinder1 = new UnityEventBinder<int, Vector3, bool, int>(null, null);
            var twoEventBinder2 = new UnityEventBinder<GameObject, bool>(null, null);
            var evtBinder_GO_Int32 = new UnityEventBinder<GameObject, System.Int32>(null, null);
            var boolBoolEventBinder = new UnityEventBinder<bool, bool>(null, null);

            var dictI32 = new Dict<System.Int32, System.Int32>();
            dictI32.Add(1, 1);
            dictI32.Add(2, 2);
            dictI32.ContainsKey(1);
            dictI32.ContainValue(1);
            dictI32.Last();
            dictI32.LastValue();
            dictI32.LastKey();
            dictI32.First();
            dictI32.FirstValue();
            dictI32.FirstKey();
            dictI32.RemoveFirst();
            dictI32.RemoveLast();
            dictI32.Remove(2);
            dictI32.Clear();
            dictI32.Clone();
            int result;
            dictI32.TryGetValue(1, out result);

            var dictStrI32 = new Dict<System.String, System.Int32>();
            dictStrI32.Add("1", 1);
            dictStrI32.Add("2", 2);
            dictStrI32.ContainsKey("1");
            dictStrI32.ContainValue(1);
            dictStrI32.Last();
            dictStrI32.LastValue();
            dictStrI32.LastKey();
            dictStrI32.First();
            dictStrI32.FirstValue();
            dictStrI32.FirstKey();
            dictStrI32.RemoveFirst();
            dictStrI32.RemoveLast();
            dictStrI32.Remove("2");
            dictStrI32.Clear();
            dictStrI32.Clone();
            dictStrI32.TryGetValue("1", out result);

            PlayableDirector p = null;
            if (p != null)
            {
                p.stopped += (rPlayableDirector) =>
                {
                };
            }

            List<Task> taskList = null;
            if (taskList != null)
            {
                taskList.TrueForAll((t) => false);
            }
        }
    }
#pragma warning restore 219
}
