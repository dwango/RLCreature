using System;
using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras.OperatableCameras
{
    public class FocusCamera : IOperatableCamera
    {
        private readonly Transform _focusTarget;

        private CameraAttitude _currentAttitude;

        private CameraAttitude _goalAttitude;

        private float _elapsedTime;

        private float _targetDistance;

        private readonly float _farAwayDistance;

        private readonly float _transitionTime;

        private readonly float _inSightAngle;

        private readonly float _outSightAngle;

        private float _minDamper = 0.1f;

        private float _maxDamper = 0.99f;

        private readonly PolarCoordinatesAttitude _tempPolarAttitude = new PolarCoordinatesAttitude();

        public FocusCamera(Transform focusTarget, float transitionTime = 0.6f, float initialDistance = 10.0f, float farAwayDistance = 40.0f, float inSightAngle = 10.0f, float outSightAngle = 30.0f)
        {
            _focusTarget = focusTarget;
            _transitionTime = transitionTime;
            _targetDistance = initialDistance;
            _farAwayDistance = farAwayDistance;
            _inSightAngle = inSightAngle;
            _outSightAngle = outSightAngle;

            _elapsedTime = 0.0f;
        }

        private void Dolly(Vector2 diff)
        {
            _tempPolarAttitude.Distance = _targetDistance;
            _tempPolarAttitude.Dolly(diff);
            _targetDistance = _tempPolarAttitude.Distance;
        }

        private void Rotate(Vector2 diff)
        {
            _tempPolarAttitude.SetFromAttitude(_goalAttitude);
            _tempPolarAttitude.Rotate(diff);
            _goalAttitude = _tempPolarAttitude.ToCameraAttitude();
        }

        private CameraAttitude LookAtLate(CameraAttitude attitude, Vector3 focus)
        {
            var nowV = attitude.LookAt - attitude.Position;
            var goalV = focus - attitude.Position;
            var angle = Vector3.Angle(nowV, goalV);
            var t = Mathf.Clamp((_maxDamper - _minDamper) * (angle - _inSightAngle) / (_outSightAngle - _inSightAngle) + _minDamper,
                _minDamper,
                _maxDamper);
            attitude.LookAt = Vector3.Lerp(attitude.LookAt, focus, t);
            return attitude;
        }

        private CameraAttitude LookAtImmediately(CameraAttitude attitude, Vector3 focus)
        {
            attitude.LookAt = focus;
            return attitude;
        }

        private CameraAttitude Follow(CameraAttitude attitude, float targetDistance, float transitionTime, float elapsedTime)
        {
            _tempPolarAttitude.SetFromAttitude(attitude);
            var t = Math.Abs(transitionTime) < 0.01f ? 1.0f : Mathf.Clamp01(elapsedTime / transitionTime);
            _tempPolarAttitude.Distance = Mathf.Lerp(_tempPolarAttitude.Distance, targetDistance, t);
            _tempPolarAttitude.Rotate(Vector2.zero);
            return _tempPolarAttitude.ToCameraAttitude();
        }

        private CameraAttitude FollowImmediately(CameraAttitude attitude, float distance)
        {
            _tempPolarAttitude.SetFromAttitude(attitude);
            _tempPolarAttitude.Distance = distance;
            _tempPolarAttitude.Rotate(Vector2.zero);
            return _tempPolarAttitude.ToCameraAttitude();
        }

        CameraAttitude IOperatableCamera.Update(float deltaTime)
        {
            var focus = _focusTarget != null ? _focusTarget.position : _goalAttitude.LookAt;
            _elapsedTime += deltaTime;

            if (_elapsedTime < _transitionTime && Vector3.Distance(_currentAttitude.Position, focus) > _farAwayDistance)
            {
                _goalAttitude = LookAtImmediately(_goalAttitude, focus);
                _goalAttitude = FollowImmediately(_goalAttitude, _farAwayDistance - 1.0f);
            }
            else
            {
                _goalAttitude = LookAtLate(_goalAttitude, focus);
                _goalAttitude = Follow(_goalAttitude, _targetDistance, _transitionTime, _elapsedTime);
            }

            _currentAttitude = CameraAttitude.Slerp(_currentAttitude, _goalAttitude, 0.99f);

            return _currentAttitude;
        }

        void IOperatableCamera.Initialize(CameraAttitude attitude)
        {
            _currentAttitude = attitude;
            _goalAttitude = attitude;
        }

        void IOperatableCamera.SubDragOperation(Vector2 diff)
        {
        }

        void IOperatableCamera.MainDragOperation(Vector2 diff)
        {
            Rotate(diff);
        }

        void IOperatableCamera.ScrollOperation(Vector2 diff)
        {
            Dolly(diff);
        }

        bool IOperatableCamera.NeedsSubDragOperation
        {
            get { return false; }
        }

        bool IOperatableCamera.NeedsMainDragOperation
        {
            get { return true; }
        }

        bool IOperatableCamera.NeedsScrollOperation
        {
            get { return true; }
        }
    }
}