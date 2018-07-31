using Boo.Lang;
using UnityEngine;

namespace RLCreature.BodyGenerator.JointGenerator
{
    public class JointGenerator
    {
        private readonly GameObject[] _bodyPrefabs;
        private readonly GameObject[] _armPrefabs;

        public JointGenerator(GameObject[] bodyPrefabs, GameObject[] armPrefabs)
        {
            _bodyPrefabs = bodyPrefabs;
            _armPrefabs = armPrefabs;
        }

        public GameObject Instantiate(Vector3 pos)
        {
            var components = new List<BodyComponent>();
            var rootComponent =
                new BodyComponent(_bodyPrefabs[UnityEngine.Random.Range(0, _bodyPrefabs.Length)], pos: pos);
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
                            _armPrefabs[UnityEngine.Random.Range(0, _armPrefabs.Length)],
                            parent: parent);
                        components.Add(child);
                        break;
                    }
                }
            }
            
            var rootObject = new GameObject();
            foreach (var component in components)
            {
                component.CentralBody.transform.parent = rootObject.transform;

            }
            return rootObject;
        }
    }
}