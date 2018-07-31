using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RLCreature.Sample.Common.UI
{
    public class FallbackPhysicsRaycaster : PhysicsRaycaster
    {
        private GameObject _fallback;

        public static FallbackPhysicsRaycaster CreateComponent(GameObject attachedCamera, GameObject fallback)
        {
            var self = attachedCamera.AddComponent<FallbackPhysicsRaycaster>();
            self._fallback = fallback;
            return self;
        }

        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
        {
            var count = resultAppendList.Count;
            base.Raycast(eventData, resultAppendList);
            if (count != resultAppendList.Count && !(eventData.IsScrolling() || eventData.dragging)) return;

            var result = new RaycastResult
            {
                gameObject = _fallback,
                module = (BaseRaycaster) this,
                distance = 0,
                worldPosition = _fallback.transform.position,
                worldNormal = _fallback.transform.up,
                screenPosition = eventData.position,
                index = (float) resultAppendList.Count,
                sortingLayer = 0,
                sortingOrder = 0
            };
            resultAppendList.Add(result);
        }
    }
}