using UnityEngine;
using Joint = RLCreature.BodyGenerator.Manipulatables.Joint;

namespace RLCreature.Sample.DesignedCreatures
{
    public class ManipulatableInitializer : MonoBehaviour
    {
        public float JointForcePower = 1;

        private void Awake()
        {
            var joints = transform.GetComponentsInChildren<ConfigurableJoint>();
            foreach (var joint in joints)
            {
                Joint.CreateComponent(joint, JointForcePower);
            }

            var rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
            foreach (var rigidbody in rigidbodies)
            {
                rigidbody.solverIterations = 30;
            }
        }
    }
}