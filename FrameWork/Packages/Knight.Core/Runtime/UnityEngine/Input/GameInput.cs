using Knight.Core;
using System.Collections.Generic;
using UnityEngine;

namespace LittleBaby.Framework.Core
{
    public static class GameInput
    {
        static readonly HashSet<string> sInputDisabledCount = new HashSet<string>();
        static readonly HashSet<string> sAxisDisabledCount = new HashSet<string>();
        static readonly HashSet<string> sButtonDisabledCount = new HashSet<string>();
        static readonly HashSet<string> sMouseDisabledCount = new HashSet<string>();
        static readonly HashSet<string> sKeyDisabledCount = new HashSet<string>();

        static readonly HashSet<string> sEventSystemDisabledCount = new HashSet<string>();
        public static void ResetAllState()
        {
            sInputDisabledCount.Clear();
            sAxisDisabledCount.Clear();
            sButtonDisabledCount.Clear();
            sMouseDisabledCount.Clear();
            sKeyDisabledCount.Clear();
            sEventSystemDisabledCount.Clear();
        }

        #region Global
        public static bool GetInputEnabled()
        {
            return sInputDisabledCount.Count <= 0;
        }
        public static void DisableInput(string rName)
        {
            sInputDisabledCount.Add(rName);
        }
        public static void EnableInput(string rName)
        {
            sInputDisabledCount.Remove(rName);
        }
        #endregion

        #region Axis
        public static bool GetAxisEnabled()
        {
            return sInputDisabledCount.Count <= 0 && sAxisDisabledCount.Count <= 0;
        }
        public static void DisableAxis(string rName)
        {
            sAxisDisabledCount.Add(rName);
        }
        public static void EnableAxis(string rName)
        {
            sAxisDisabledCount.Remove(rName);
        }
        public static float GetAxis(string rAxisName)
        {
            if (!GetAxisEnabled()) return 0f;
            return Input.GetAxis(rAxisName);
        }
        public static float GetAxisRaw(string rAxisName)
        {
            if (!GetAxisEnabled()) return 0f;
            return Input.GetAxisRaw(rAxisName);
        }
        #endregion

        #region Button
        public static bool GetButtonEnabled()
        {
            return sInputDisabledCount.Count <= 0 && sButtonDisabledCount.Count <= 0;
        }
        public static void DisableButton(string rName)
        {
            sButtonDisabledCount.Add(rName);
        }
        public static void EnableButton(string rName)
        {
            sButtonDisabledCount.Remove(rName);
        }
        public static bool GetButton(string rButtonName)
        {
            if (!GetButtonEnabled()) return false;
            return Input.GetButton(rButtonName);
        }
        public static bool GetButtonDown(string rButtonName)
        {
            if (!GetButtonEnabled()) return false;
            return Input.GetButtonDown(rButtonName);
        }
        public static bool GetButtonUp(string rButtonName)
        {
            if (!GetButtonEnabled()) return false;
            return Input.GetButtonUp(rButtonName);
        }
        public static bool GetMouseButton(int nButton)
        {
            if (!GetButtonEnabled()) return false;
            return Input.GetMouseButton(nButton);
        }
        public static bool GetMouseButtonDown(int nButton)
        {
            if (!GetButtonEnabled()) return false;
            return Input.GetMouseButtonDown(nButton);
        }
        public static bool GetMouseButtonUp(int nButton)
        {
            if (!GetButtonEnabled()) return false;
            return Input.GetMouseButtonUp(nButton);
        }
        #endregion

        #region Mouse
        public static bool GetMouseEnabled()
        {
            return sInputDisabledCount.Count <= 0 && sMouseDisabledCount.Count <= 0;
        }
        public static void DisableMouse(string rName)
        {
            sMouseDisabledCount.Add(rName);
        }
        public static void EnableMouse(string rName)
        {
            sMouseDisabledCount.Remove(rName);
        }
        public static bool mousePresent
        {
            get
            {
                if (!GetMouseEnabled()) return false;
                return Input.mousePresent;
            }
        }
        public static Vector3 mousePosition
        {
            get
            {
                if (!GetMouseEnabled()) return Vector3.zero;
                return Input.mousePosition;
            }
        }
        public static Vector2 mouseScrollDelta
        {
            get
            {
                if (!GetMouseEnabled()) return Vector2.zero;
                return Input.mouseScrollDelta;
            }
        }
        #endregion

        #region Key
        public static bool GetKeyEnabled()
        {
            return sInputDisabledCount.Count <= 0 && sKeyDisabledCount.Count <= 0;
        }
        public static void DisableKey(string rName)
        {
            sKeyDisabledCount.Add(rName);
        }
        public static void EnableKey(string rName)
        {
            sKeyDisabledCount.Remove(rName);
        }
        public static bool GetKey(KeyCode rKey)
        {
            if (!GetKeyEnabled()) return false;
            return Input.GetKey(rKey);
        }
        public static bool GetKey(string rName)
        {
            if (!GetKeyEnabled()) return false;
            return Input.GetKey(rName);
        }
        public static bool GetKeyUp(KeyCode rKey)
        {
            if (!GetKeyEnabled()) return false;
            return Input.GetKeyUp(rKey);
        }
        public static bool GetKeyUp(string rName)
        {
            if (!GetKeyEnabled()) return false;
            return Input.GetKeyUp(rName);
        }
        public static bool GetKeyDown(KeyCode rKey)
        {
            if (!GetKeyEnabled()) return false;
            return Input.GetKeyDown(rKey);
        }
        public static bool GetKeyDown(string rName)
        {
            if (!GetKeyEnabled()) return false;
            return Input.GetKeyDown(rName);
        }
        #endregion

        #region EventSystem
        public static bool GetEventSystemEnabled()
        {
            return sEventSystemDisabledCount.Count <= 0;
        }
        public static void DisableEventSystem(string rName)
        {
            sEventSystemDisabledCount.Add(rName);
            RefreshEventSystemEventEnabled();
        }
        public static void EnableEventSystem(string rName)
        {
            sEventSystemDisabledCount.Remove(rName);
            RefreshEventSystemEventEnabled();
        }
        public static void RefreshEventSystemEventEnabled()
        {
            EventSystemContainer.SetEventSystemActive(GetEventSystemEnabled());
        }
        #endregion
    }
}