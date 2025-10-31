using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class AnimEventListener : MonoBehaviour
    {
        public class AnimEvent : UnityEvent<string>
        {
        }

        public AnimEvent TheAnimEvent = new AnimEvent();

        public void TriggerAnimEvent(string rEventName)
        {
            this.TheAnimEvent?.Invoke(rEventName);
        }
    }
}
