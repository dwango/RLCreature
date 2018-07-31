using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras
{
    public struct CameraAttitude
    {
        public Vector3 Position;
        public Vector3 LookAt;

        public static CameraAttitude Lerp(CameraAttitude a, CameraAttitude b, float t)
        {
            return new CameraAttitude
            {
                LookAt = Vector3.Lerp(a.LookAt, b.LookAt, t),
                Position = Vector3.Lerp(a.Position, b.Position, t),
            };
        }

        public static CameraAttitude Slerp(CameraAttitude a, CameraAttitude b, float t)
        {
            var va = a.Position - a.LookAt;
            var vb = b.Position - b.LookAt;
            var v = Vector3.Slerp(va, vb, t);

            var look = Vector3.Lerp(a.LookAt, b.LookAt, t);

            return new CameraAttitude
            {
                LookAt = look,
                Position = look + v,
            };
        }
    }
}