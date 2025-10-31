using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.UI
{
    public class ScrollDragManager : MonoBehaviour
    {
        private static ScrollDragManager _Instance;

        public static ScrollDragManager Instance
        {
            get { return _Instance; }
        }

        public Vector2 DragMinValue = Vector2.zero;

        private void Awake()
        {

            _Instance = this;
        }
    }
}
