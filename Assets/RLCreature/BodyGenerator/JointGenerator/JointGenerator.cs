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
            var rootComponent = new BodyComponent(_prefabs[UnityEngine.Random.Range(0, _prefabs.Length)]);


            var otherComponent = new BodyComponent(_prefabs[UnityEngine.Random.Range(0, _prefabs.Length)]);
            rootComponent.ConnectRandom(otherComponent);

            return rootComponent._instance;
        }
    }
}