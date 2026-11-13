#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Innersloth.DebugTool;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace AmongUsGameTools
{
    public static class MonoUICapture
    {
        public static bool Run(string action)
        {
            if (!action.StartsWith("mui")) return false;
            if (action == "muiclose") { DebugToolInstantiator.SetOverlayVisible(false); return true; }
            var overlay = UnityEngine.Object.FindFirstObjectByType<DebugToolOverlay>(FindObjectsInactive.Include);
            if (action == "muidump")
            {
                var es = EventSystem.current;
                Debug.Log("[MUI] overlay=" + (overlay != null) + " eventSystem=" + (es != null) + " module=" + (es != null && es.currentInputModule != null ? es.currentInputModule.GetType().Name : "none"));
                if (overlay != null)
                {
                    foreach (var graphic in overlay.GetComponentsInChildren<Graphic>(true))
                    {
                        var m = graphic.materialForRendering;
                        Debug.Log("[MUI] graphic " + graphic.name + " active=" + graphic.gameObject.activeInHierarchy + " raycast=" + graphic.raycastTarget + " material=" + (m != null ? m.name : "null") + " shader=" + (m != null && m.shader != null ? m.shader.name + " supported=" + m.shader.isSupported : "null"));
                    }
                    foreach (var tab in overlay.GetComponentsInChildren<DebugTabButton>(true)) Debug.Log("[MUI] tab name=" + tab.name + " page=" + tab.PageName);
                }
                return true;
            }
            if (action.StartsWith("muitab="))
            {
                var page = action.Substring(7);
                var tab = overlay.GetComponentsInChildren<DebugTabButton>(true).FirstOrDefault(t => t.PageName == page);
                if (tab != null) Click(tab.GetComponent<Button>() ?? tab.GetComponentInChildren<Button>());
                else Debug.Log("[MUI] no tab " + page);
                return true;
            }
            if (action.StartsWith("muibtn="))
            {
                var text = action.Substring(7);
                var btn = overlay.GetComponentsInChildren<Button>(true).FirstOrDefault(b => b.gameObject.activeInHierarchy && (b.name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || b.GetComponentsInChildren<TMP_Text>().Any(t => t.text.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)));
                Click(btn);
                return true;
            }
            return false;
        }

        private static void Click(Button button)
        {
            var es = EventSystem.current;
            if (button == null || es == null) { Debug.Log("[MUI] missing button/EventSystem"); return; }
            var canvas = button.GetComponentInParent<Canvas>();
            var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            var pos = RectTransformUtility.WorldToScreenPoint(camera, button.transform.position);
            var data = new PointerEventData(es) { position = pos, button = PointerEventData.InputButton.Left };
            var hits = new List<RaycastResult>();
            es.RaycastAll(data, hits);
            Debug.Log("[MUI] click button=" + button.name + " point=" + pos + " hits=" + string.Join(",", hits.Take(5).Select(h => h.gameObject.name)));
            if (hits.Count == 0) return;
            data.pointerCurrentRaycast = hits[0];
            data.pointerPressRaycast = hits[0];
            var handler = ExecuteEvents.ExecuteHierarchy(hits[0].gameObject, data, ExecuteEvents.pointerClickHandler);
            Debug.Log("[MUI] clicked=" + (handler != null ? handler.name : "none"));
        }
    }
}
#endif
