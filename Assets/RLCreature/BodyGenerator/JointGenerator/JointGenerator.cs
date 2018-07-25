using UnityEngine;

namespace RLCreature.BodyGenerator.JointGenerator
{
    public class JointGenerator
    {
        private GameObject[] _prefabs;

        public JointGenerator(GameObject[] prefabs)
        {
            _prefabs = prefabs;
        }

        public GameObject Instantiate()
        {
            var rootComponent = new BodyComponent(_prefabs[Random.Range(0, _prefabs.Length)]);
            var otherComponent = new BodyComponent(_prefabs[Random.Range(0, _prefabs.Length)], 0, rootComponent);
            var otherComponent2 = new BodyComponent(_prefabs[Random.Range(0, _prefabs.Length)], 1, rootComponent);
            return rootComponent._centralBody;
        }
    }
}