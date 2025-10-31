using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LittleBaby.Framework.Core
{
    public class EventSystemContainer : MonoBehaviour
    {
        public static List<EventSystem> sEventSystemList = new List<EventSystem>();
        public static void SetEventSystemActive(bool bActive)
        {
            for (int i = 0; i < sEventSystemList.Count; i++)
            {
                sEventSystemList[i].gameObject.SetActive(bActive);
            }
        }

        public EventSystem EventSystem;
        protected void Awake()
        {
            this.EventSystem = this.GetComponent<EventSystem>();
            sEventSystemList.Add(this.EventSystem);
        }
        protected void OnDestroy()
        {
            sEventSystemList.Remove(this.EventSystem);
        }
    }
}