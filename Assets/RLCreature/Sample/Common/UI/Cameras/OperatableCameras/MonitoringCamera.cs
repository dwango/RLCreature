using System;
using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras.OperatableCameras
{
    public class MonitoringCamera : IOperatableCamera
    {
        private readonly CameraAttitude _targetAttitude;

        private readonly float _transitionTime;

        private CameraAttitude _currentAttitude;

        private float _elapsedTime;

        public MonitoringCamera(CameraAttitude targetAttitude, float transitionTime = 1.0f)
        {
            _targetAttitude = targetAttitude;
            _transitionTime = transitionTime;
        }

        public CameraAttitude Update(float deltaTime)
        {
            _elapsedTime += deltaTime;
            var t = Math.Abs(_transitionTime) < 0.01f ? 1.0f : Mathf.Clamp01(_elapsedTime / _transitionTime);
            _currentAttitude = CameraAttitude.Slerp(_currentAttitude, _targetAttitude, t);
            return _currentAttitude;
        }

        void IOperatableCamera.Initialize(CameraAttitude attitude)
        {
            _currentAttitude = attitude;
            _elapsedTime = 0.0f;
        }

        void IOperatableCamera.SubDragOperation(Vector2 diff)
        {
        }

        void IOperatableCamera.MainDragOperation(Vector2 diff)
        {
        }

        void IOperatableCamera.ScrollOperation(Vector2 diff)
        {
        }

        bool IOperatableCamera.NeedsSubDragOperation
        {
            get { return false; }
        }

        bool IOperatableCamera.NeedsMainDragOperation
        {
            get { return false; }
        }

        bool IOperatableCamera.NeedsScrollOperation
        {
            get { return false; }
        }
    }
}