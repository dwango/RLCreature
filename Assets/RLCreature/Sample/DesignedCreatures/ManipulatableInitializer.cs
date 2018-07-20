using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sample.DesignedCreatures
{
	public class ManipulatableInitializer : MonoBehaviour
	{
		private void Awake()
		{
			var joints = transform.GetComponentsInChildren<ConfigurableJoint>();
			foreach (var joint in joints)
			{
				joint.gameObject.AddComponent<Joint>();
			}
			
			var rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
			foreach (var rigidbody in rigidbodies)
			{
				rigidbody.solverIterations = 30;
			}
		}
	}
}
