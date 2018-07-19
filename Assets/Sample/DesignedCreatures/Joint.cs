using System.Collections.Generic;
using BodyGenerator.Manipulatables;
using MotionGenerator;
using UnityEngine;

namespace Sample.DesignedCreatures
{
    public class Joint : ManipulatableBase
    {
        const int maximumForce = 10000;
        const float positionDamper = 10;
        const float positionSpring = 50;

        float targetForce;
        public List<float> targetAngle;
        int consumedFrames = 0;
        bool isMoving = false;
        int manipulatableId;

        ConfigurableJoint joint;
        MotionSequence sequence = new MotionSequence();

        public void Start()
        {
            targetAngle = new List<float> {0, 0, 0};
            joint = GetComponent<ConfigurableJoint>();

//			joint.axis = new Vector3(0,0,0);
//			joint.secondaryAxis = new Vector3(0,0,1);
//			joint.anchor = new Vector3 (0.0f, 0.0f, 0.0f);

            joint.autoConfigureConnectedAnchor = true;
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimitLow = new SoftJointLimit();
            softJointLimitLow.limit = -60;
            joint.lowAngularXLimit = softJointLimitLow;
            SoftJointLimit softJointLimitHigh = new SoftJointLimit();
            softJointLimitHigh.limit = 60;
            joint.highAngularXLimit = softJointLimitHigh;
            SoftJointLimit angularYLimit = new SoftJointLimit();
            angularYLimit.limit = 45;
            joint.angularYLimit = angularYLimit;
            SoftJointLimit angularZLimit = new SoftJointLimit();
            angularZLimit.limit = 0;
            joint.angularZLimit = angularZLimit;
            joint.rotationDriveMode = RotationDriveMode.XYAndZ;
        }

        public override void Manipulate(MotionSequence sequence)
        {
            consumedFrames = 0;
            isMoving = true;
            this.sequence = new MotionSequence(sequence);
        }

        void UpdateJointMotor(List<float> targetAngle)
        {
            if (targetAngle.Count != 3)
                throw new System.ArgumentException("need 3D input");
            targetForce = 1000;
            joint.targetRotation = Quaternion.Euler(
                targetAngle[0] * (joint.highAngularXLimit.limit - joint.lowAngularXLimit.limit)
                + joint.lowAngularXLimit.limit,
                targetAngle[1] * joint.angularYLimit.limit,
                targetAngle[2] * joint.angularZLimit.limit);

            targetAngle[2] = 0;
            JointDrive jd = joint.angularXDrive;
            jd.maximumForce = maximumForce;
            jd.positionDamper = positionDamper * targetForce;
            jd.positionSpring = positionSpring * targetForce;
            joint.angularXDrive = jd;
            JointDrive jdYZ = joint.angularYZDrive;
            jdYZ.maximumForce = maximumForce;
            jdYZ.positionDamper = positionDamper * targetForce;
            jdYZ.positionSpring = positionSpring * targetForce;
            joint.angularYZDrive = jdYZ;
        }

        public void FixedUpdate()
        {
            if (sequence.Sequence.Count > 0)
            {
                this.targetAngle = sequence[0].value;
                if (sequence[0].time < consumedFrames)
                {
                    sequence.Sequence.RemoveAt(0);
                }
            }
            else
            {
                this.isMoving = false;
            }

            UpdateJointMotor(this.targetAngle);
            consumedFrames += 1;
        }

        public override int GetManipulatableDimention()
        {
            return 3;
        }
    }
}