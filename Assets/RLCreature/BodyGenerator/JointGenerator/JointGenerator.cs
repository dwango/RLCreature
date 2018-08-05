using Boo.Lang;
using UnityEngine;

namespace RLCreature.BodyGenerator.JointGenerator
{
    public class JointGenerator
    {
        protected readonly GameObject[] BodyPrefabs;
        protected readonly GameObject[] ArmPrefabs;

        public JointGenerator(GameObject[] bodyPrefabs, GameObject[] armPrefabs)
        {
            BodyPrefabs = bodyPrefabs;
            ArmPrefabs = armPrefabs;
        }

        public virtual GameObject Instantiate(Vector3 pos)
        {
            var components = new List<BodyComponent>();
            var rootComponent =
                new BodyComponent(BodyPrefabs[UnityEngine.Random.Range(0, BodyPrefabs.Length)], pos: pos);
            components.Add(rootComponent);
            for (int i = 0; i < UnityEngine.Random.Range(1, 20); i++)
            {
                // choose parent component
                for (int j = 0; j < 100; j++)
                {
                    var parent = components[Random.Range(0, components.Count)];
                    if (parent.HasAvailableConnector())
                    {
                        var child = new BodyComponent(
                            ArmPrefabs[UnityEngine.Random.Range(0, ArmPrefabs.Length)],
                            parent: parent);
                        components.Add(child);
                        break;
                    }
                }
            }
            
            

            rootComponent.CentralBody.GetComponent<Rigidbody>().centerOfMass = new Vector3(0f, -10f, 0f);;
            rootComponent.CentralBody.GetComponent<Rigidbody>().mass *= 5;
            
            var rootObject = new GameObject();
            foreach (var component in components)
            {
                component.CentralBody.transform.parent = rootObject.transform;

            }
            return rootObject;
        }
    }
}