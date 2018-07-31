using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras.OperatableCameras
{
    public interface IOperatableCamera
    {
        CameraAttitude Update(float deltaTime);
        void Initialize(CameraAttitude attitude);
        void SubDragOperation(Vector2 diff);
        void MainDragOperation(Vector2 diff);
        void ScrollOperation(Vector2 diff);
        bool NeedsSubDragOperation { get; }
        bool NeedsMainDragOperation { get; }
        bool NeedsScrollOperation { get; }
    }
}