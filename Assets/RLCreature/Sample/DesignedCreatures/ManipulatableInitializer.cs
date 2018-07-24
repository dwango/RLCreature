using UnityEngine;

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
				joint.gameObject.AddComponent<Joint>().TargetForce = JointForcePower;
			}
			
			var rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
			foreach (var rigidbody in rigidbodies)
			{
				rigidbody.solverIterations = 30;
			}
		}
	}
}
