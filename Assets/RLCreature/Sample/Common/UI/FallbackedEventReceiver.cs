using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
#if NET_4_6

#endif

namespace RLCreature.Sample.Common.UI
{
    public class FallbackedEventReceiver : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler, IPointerClickHandler
    {
        public enum FallbackedEvents
        {
            DragBegin,
            Dragging,
            DragEnd,
            Scroll,
            LeftClick,
            RightClick,
        }

        public IObservable<Tuple<FallbackedEvents, PointerEventData>> EventObservable
        {
            get { return _eventSubject; }
        }

        private readonly Subject<Tuple<FallbackedEvents, PointerEventData>> _eventSubject = new Subject<Tuple<FallbackedEvents, PointerEventData>>();

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            _eventSubject.OnNext(new Tuple<FallbackedEvents, PointerEventData>(FallbackedEvents.DragBegin, eventData));
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            _eventSubject.OnNext(new Tuple<FallbackedEvents, PointerEventData>(FallbackedEvents.Dragging, eventData));
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            _eventSubject.OnNext(new Tuple<FallbackedEvents, PointerEventData>(FallbackedEvents.DragEnd, eventData));
        }

        void IScrollHandler.OnScroll(PointerEventData eventData)
        {
            _eventSubject.OnNext(new Tuple<FallbackedEvents, PointerEventData>(FallbackedEvents.Scroll, eventData));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _eventSubject.OnNext(
                    new Tuple<FallbackedEvents, PointerEventData>(FallbackedEvents.LeftClick, eventData));
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                _eventSubject.OnNext(
                    new Tuple<FallbackedEvents, PointerEventData>(FallbackedEvents.RightClick, eventData));
            }
        }
    }
}