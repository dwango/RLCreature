using System.Linq;
using UnityEngine;

namespace RLCreature.BodyGenerator.JointGenerator
{
    public class TreeJointGenerator : JointGenerator
    {
        public TreeJointGenerator(GameObject[] bodyPrefabs, GameObject[] armPrefabs) : base(bodyPrefabs, armPrefabs)
        {
        }

        public override GameObject Instantiate(Vector3 pos)
        {
            var rootObject = new GameObject();
            var rootComponent =
                new BodyComponent(BodyPrefabs[UnityEngine.Random.Range(0, BodyPrefabs.Length)], pos: pos);
            rootComponent.CentralBody.transform.parent = rootObject.transform;
            for (int i = 0; i < UnityEngine.Random.Range(1, 20); i++)
            {
                var current = rootComponent;
                while (true)
                {
                    if (current.GetSlaves().Count() == 0)
                    {
                        var component = new BodyComponent(
                            ArmPrefabs[UnityEngine.Random.Range(0, ArmPrefabs.Length)],
                            parent: current);
                        component.CentralBody.transform.parent = rootObject.transform;
                        break;
                    }

                    if (Random.value < 0.8 && current.HasAvailableConnector())
                    {
                        UnityEngine.Debug.Log("branch");
                        var component = new BodyComponent(
                            ArmPrefabs[UnityEngine.Random.Range(0, ArmPrefabs.Length)],
                            parent: current);
                        component.CentralBody.transform.parent = rootObject.transform;
                        break;
                    }
                    else
                    {
                        UnityEngine.Debug.Log("edge");
                        var candidates = current.GetSlaves().ToList();
                        current = candidates[Random.Range(0, candidates.Count)];
                    }
                }
            }
            
            rootComponent.CentralBody.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, -10f, 0f);;
            rootComponent.CentralBody.GetComponent<Rigidbody>().mass *= 1;
            
            return rootObject;
        }
    }
}