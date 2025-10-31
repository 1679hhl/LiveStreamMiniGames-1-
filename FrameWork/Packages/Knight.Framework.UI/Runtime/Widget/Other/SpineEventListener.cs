using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class SpineAnimEvent : UnityEvent<string>
    {

    }
    public class SpineEventListener : MonoBehaviour
    {
        public SpineAnimEvent OnTriggerSpineEvent = new SpineAnimEvent();

        public void TriggerSpineEvent(string rEventName)
        {
            this.OnTriggerSpineEvent?.Invoke(rEventName);
        }
    }
}
