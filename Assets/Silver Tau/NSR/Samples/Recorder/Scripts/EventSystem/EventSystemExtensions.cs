using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#if UNITY_2019_4_OR_NEWER
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
#endif

namespace SilverTau.NSR.Samples
{
    public static class EventSystemExtensions
    {
        public static bool IsPointerOverUI(this EventSystem eventSystem)
        {
            if (eventSystem == null) return false;

#if UNITY_2019_4_OR_NEWER
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                var pos = Mouse.current.position.ReadValue();
                if (RaycastUI(eventSystem, pos)) return true;
            }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            var legacyPos = (Vector2)Input.mousePosition;
            if (RaycastUI(eventSystem, legacyPos)) return true;
#endif
#else
            var legacyPos = (Vector2)Input.mousePosition;
            if (RaycastUI(eventSystem, legacyPos)) return true; 
#endif
            return false;
        }

        private static bool RaycastUI(EventSystem eventSystem, Vector2 screenPos)
        {
            var data = new PointerEventData(eventSystem) { position = screenPos };
            var results = new List<RaycastResult>();
            eventSystem.RaycastAll(data, results);
            return results.Count > 0;
        }
    }
}