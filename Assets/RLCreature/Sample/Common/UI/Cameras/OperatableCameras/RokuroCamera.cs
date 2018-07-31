using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras.OperatableCameras
{
    public class RokuroCamera : IOperatableCamera
    {
        private CameraAttitude _currentAttitude;

        private PolarCoordinatesAttitude _goalAttitude;

        CameraAttitude IOperatableCamera.Update(float deltaTime)
        {
            _currentAttitude = CameraAttitude.Lerp(_currentAttitude, _goalAttitude.ToCameraAttitude(), 0.98f);
            return _currentAttitude;
        }

        void IOperatableCamera.Initialize(CameraAttitude attitude)
        {
            _currentAttitude = attitude;
            _goalAttitude = new PolarCoordinatesAttitude(_currentAttitude);
        }

        void IOperatableCamera.SubDragOperation(Vector2 diff)
        {
            _goalAttitude.Grab(diff);
        }

        void IOperatableCamera.MainDragOperation(Vector2 diff)
        {
            _goalAttitude.Rotate(diff);
        }

        void IOperatableCamera.ScrollOperation(Vector2 diff)
        {
            _goalAttitude.Dolly(diff);
        }

        bool IOperatableCamera.NeedsSubDragOperation
        {
            get { return true; }
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