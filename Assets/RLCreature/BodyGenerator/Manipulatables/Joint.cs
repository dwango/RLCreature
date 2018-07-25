using System.Collections.Generic;
using MotionGenerator;
using UnityEngine;

namespace RLCreature.BodyGenerator.Manipulatables
{
    public class Joint : ManipulatableBase
    {
        private const int MaximumForce = 10000;
        private const float PositionDamper = 20;
        private const float PositionSpring = 1000;
        private float _targetForce;
        public List<float> TargetAngle = new List<float> {0, 0, 0};
        private int _consumedFrames = 0;

        private ConfigurableJoint _joint;
        private MotionSequence _sequence = new MotionSequence();


        public static Joint CreateComponent(ConfigurableJoint joint, float targetForce)
        {
            return joint.gameObject.AddComponent<Joint>()._CreateComponent(joint, targetForce);
        }

        private Joint _CreateComponent(ConfigurableJoint joint, float targetForce)
        {
            _joint = joint;
            _joint.xMotion = ConfigurableJointMotion.Locked;
            _joint.yMotion = ConfigurableJointMotion.Locked;
            _joint.zMotion = ConfigurableJointMotion.Locked;
            _joint.angularXMotion = ConfigurableJointMotion.Limited;
            _joint.angularYMotion = ConfigurableJointMotion.Limited;
            _joint.angularZMotion = ConfigurableJointMotion.Limited;
            _targetForce = targetForce;
            return this;
        }

        public override void Manipulate(MotionSequence sequence)
        {
            _consumedFrames = 0;
            _isMoving = true;
            _sequence = new MotionSequence(sequence);
        }

        void UpdateJointMotor(List<float> targetAngle)
        {
            if (targetAngle.Count != 3)
                throw new System.ArgumentException("need 3D input");
            _joint.targetRotation = Quaternion.Euler(
                targetAngle[0] * (_joint.highAngularXLimit.limit - _joint.lowAngularXLimit.limit)
                + _joint.lowAngularXLimit.limit,
                (targetAngle[1] * 2f - 1f) * _joint.angularYLimit.limit,
                (targetAngle[2] * 2f - 1f) * _joint.angularZLimit.limit);

            var jd = _joint.angularXDrive;
            jd.maximumForce = MaximumForce;
            jd.positionDamper = PositionDamper * _targetForce;
            jd.positionSpring = PositionSpring * _targetForce;
            _joint.angularXDrive = jd;
            var jdYZ = _joint.angularYZDrive;
            jdYZ.maximumForce = MaximumForce;
            jdYZ.positionDamper = PositionDamper * _targetForce;
            jdYZ.positionSpring = PositionSpring * _targetForce;
            _joint.angularYZDrive = jdYZ;

            _joint.projectionMode = JointProjectionMode.PositionAndRotation;
        }

        public void FixedUpdate()
        {
            if (_sequence.Sequence.Count > 0)
            {
                this.TargetAngle = _sequence[0].value;
                if (_sequence[0].time < _consumedFrames)
                {
                    _sequence.Sequence.RemoveAt(0);
                }
            }
            else
            {
                _isMoving = false;
            }

            UpdateJointMotor(TargetAngle);
            _consumedFrames += 1;
        }

        public override int GetManipulatableDimention()
        {
            return 3;
        }
    }
}