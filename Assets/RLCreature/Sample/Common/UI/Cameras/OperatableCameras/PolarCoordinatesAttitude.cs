using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras.OperatableCameras
{
    public class PolarCoordinatesAttitude
    {
        public Vector3 LookAt;
        public float Theta;
        public float Phi;
        public float Distance;

        public PolarCoordinatesAttitude() { }

        public PolarCoordinatesAttitude(CameraAttitude attitude)
        {
            SetFromAttitude(attitude);
        }

        public void SetFromAttitude(CameraAttitude attitude)
        {
            var dir = attitude.Position - attitude.LookAt;
            Theta = Mathf.Atan2(dir.z, dir.x);
            Phi = Mathf.Atan2(dir.y, Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z));
            Distance = dir.magnitude;
            LookAt = attitude.LookAt;
        }

        public CameraAttitude ToCameraAttitude()
        {
            return new CameraAttitude
            {
                LookAt = LookAt,
                Position = LookAt + GetDir(),
            };
        }

        public void Rotate(Vector2 diff, float lowerLimitAngle = 10.0f, float higherLimitAngle = 89.0f)
        {
            diff *= 0.1f * Mathf.Deg2Rad;
            Theta -= diff.x;
            Phi -= diff.y;
            Phi = Mathf.Clamp(Phi, lowerLimitAngle * Mathf.Deg2Rad, higherLimitAngle * Mathf.Deg2Rad);
        }

        public void Grab(Vector2 diff)
        {
            diff *= 0.001f * Distance;
            var dir = GetDir();
            var right = Vector3.Cross(dir, Vector3.up).normalized;
            var up = Vector3.Cross(right, Vector3.up);
            LookAt -= right * diff.x + up * diff.y;
        }

        public void Dolly(Vector2 diff, float minDistance = 1.5f, float maxDistance = 1000.0f)
        {
            Distance = Mathf.Clamp(Distance - diff.y * Distance * 0.1f, minDistance, maxDistance);
        }

        private Vector3 GetDir()
        {
            var c = Mathf.Cos(Phi);
            return new Vector3(c * Mathf.Cos(Theta), Mathf.Sin(Phi), c * Mathf.Sin(Theta)) * Distance;
        }
    }
}