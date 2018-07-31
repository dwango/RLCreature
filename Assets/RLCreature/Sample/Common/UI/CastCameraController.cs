using System;
using RLCreature.Sample.Common.UI.Cameras;
using RLCreature.Sample.Common.UI.Cameras.OperatableCameras;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RLCreature.Sample.Common.UI
{
    public class CastCameraController : MonoBehaviour
    {
        [Range(0.1f, 10.0f)] public float RotateSpeed = 1.0f;

        [Range(0.1f, 10.0f)] public float GrabSpeed = 1.0f;

        [Range(0.1f, 10.0f)] public float ScrollSpeed = 1.0f;

        private IOperatableCamera _currentCamera;

        private CameraAttitude _defaultAttitude;

        private ReactiveProperty<GameObject> _selectedCreatureExtrenalProperty;

        public static CastCameraController CreateComponent(Camera target,
            ReactiveProperty<GameObject> selectedCreature,
            IObservable<Tuple<FallbackedEventReceiver.FallbackedEvents, PointerEventData>> eventObservable,
            CameraAttitude? defaultAttitude = null)
        {
            var self = target.gameObject.AddComponent<CastCameraController>();
            self.Init(target, selectedCreature, eventObservable, defaultAttitude);
            return self;
        }


        private Camera _target;

        private void Init(Camera target,
            ReactiveProperty<GameObject> selectedCreature,
            IObservable<Tuple<FallbackedEventReceiver.FallbackedEvents, PointerEventData>> eventObservable,
            CameraAttitude? defaultAttitude = null)
        {
            _selectedCreatureExtrenalProperty = selectedCreature;
            _target = target;
            _defaultAttitude = defaultAttitude ?? new CameraAttitude
            {
                LookAt = Vector3.zero,
                Position = new Vector3(0.0f, 10.0f, -40.0f),
            };

            eventObservable
                .Where(x => x.Item1 == FallbackedEventReceiver.FallbackedEvents.Dragging)
                .Select(x => x.Item2)
                .Where(x => x.button == PointerEventData.InputButton.Left)
                .Select(x => x.delta)
                .Subscribe(LeftDragOperation)
                .AddTo(this);

            eventObservable
                .Where(x => x.Item1 == FallbackedEventReceiver.FallbackedEvents.Dragging)
                .Select(x => x.Item2)
                .Where(x => x.button == PointerEventData.InputButton.Right)
                .Select(x => x.delta)
                .Subscribe(RightDragOperation)
                .AddTo(this);

            eventObservable
                .Where(x => x.Item1 == FallbackedEventReceiver.FallbackedEvents.Scroll)
                .Select(x => x.Item2.scrollDelta)
                .Subscribe(ScrollOperation)
                .AddTo(this);

            selectedCreature
                .Subscribe(SelectOperation)
                .AddTo(this);
        }

        private void Update()
        {
            if (_currentCamera == null)
            {
                Fallback();
            }

            var attitude = _currentCamera.Update(Time.unscaledDeltaTime);
            _target.transform.position = attitude.Position;
            _target.transform.LookAt(attitude.LookAt);
        }

        public void Transition(IOperatableCamera next, bool keepSelectingCreature = false)
        {
            var currentAttitude = _currentCamera != null ? _currentCamera.Update(0.0f) : _defaultAttitude;
            next.Initialize(currentAttitude);
            _currentCamera = next;

            if (!keepSelectingCreature)
            {
                _selectedCreatureExtrenalProperty.Value = null;
            }
        }

        private void LeftDragOperation(Vector2 diff)
        {
            if (_currentCamera == null || !(_currentCamera.NeedsMainDragOperation))
            {
                Fallback();
            }

            _currentCamera.MainDragOperation(diff * RotateSpeed);
        }

        private void RightDragOperation(Vector2 diff)
        {
            if (_currentCamera == null || !(_currentCamera.NeedsSubDragOperation))
            {
                Fallback();
            }

            _currentCamera.SubDragOperation(diff * GrabSpeed);
        }

        private void ScrollOperation(Vector2 diff)
        {
            if (_currentCamera == null || !(_currentCamera.NeedsScrollOperation))
            {
                Fallback();
            }

            _currentCamera.ScrollOperation(diff * ScrollSpeed);
        }

        private void SelectOperation(GameObject creatureRoot)
        {
            if (creatureRoot == null) return;
            if (creatureRoot.transform.childCount < 1) return;
            Transition(new FocusCamera(creatureRoot.transform.GetChild(0)), true);
        }

        private void Fallback()
        {
            Transition(new RokuroCamera());
        }
    }
}