using System.Collections;
using MotionGenerator;
using RLCreature.BodyGenerator;
using RLCreature.BodyGenerator.JointGenerator;
using RLCreature.BodyGenerator.Manipulatables;
using RLCreature.Sample.SimpleHunting;
using UnityEngine;

namespace RLCreature.Sample.RandomCreatures
{
    public class RandomCreaturesEntryPoint : MonoBehaviour
    {
        private Rect _size;
        public int FoodCount = 800;
        public GameObject Plane;

        private void Start()
        {
            Plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Plane.transform.position = Vector3.zero;
            Plane.transform.localScale = Vector3.one * 100;
            var unitPlaneSize = 10;
            _size = new Rect(
                (Plane.transform.position.x - Plane.transform.lossyScale.x * unitPlaneSize) / 2,
                (Plane.transform.position.y - Plane.transform.lossyScale.y * unitPlaneSize) / 2,
                Plane.transform.lossyScale.x * unitPlaneSize,
                Plane.transform.lossyScale.y * unitPlaneSize
            );


            var generator = new JointGenerator(new[]
            {
                Resources.Load<GameObject>("Prefabs/CubeBody"),
                Resources.Load<GameObject>("Prefabs/CubeBody2")
            }, new[]
            {
                Resources.Load<GameObject>("Prefabs/CubeArm"),
                Resources.Load<GameObject>("Prefabs/CubeArm2")
            });

            StartCoroutine(Feeder());
            StartCoroutine(SpawnSome(generator));
        }


        private IEnumerator SpawnSome(JointGenerator generator)
        {
            for (int i = 0; i < 30; i++)
            {
                SpawnCreature(generator);
                yield return new WaitForSeconds(10);
            }
        }

        private void SpawnCreature(JointGenerator generator)
        {
            var pos = new Vector3(
                x: _size.xMin + Random.value * _size.width,
                y: 3,
                z: _size.yMin + Random.value * _size.height
            );
            ;
            var centralBody = generator.Instantiate(pos);
            Sensor.CreateComponent(centralBody, typeof(Food), State.BasicKeys.RelativeFoodPosition, range: 100f);
            Mouth.CreateComponent(centralBody, typeof(Food));

            var actions = LocomotionAction.EightDirections();
            var sequenceMaker = new EvolutionarySequenceMaker(epsilon: 0.3f, minimumCandidates: 30);
            var brain = new Brain(
                new FollowPointDecisionMaker(State.BasicKeys.RelativeFoodPosition),
                sequenceMaker
            );
            Agent.CreateComponent(centralBody, brain, new Body(centralBody), actions);
        }

        private IEnumerator Feeder()
        {
            while (true)
            {
                var foodCount = FindObjectsOfType<Food>().Length;
                for (int i = 0; i < FoodCount - foodCount; i++)
                {
                    Feed();
                }

                yield return new WaitForSeconds(5);
            }
        }

        private void Feed()
        {
            var foodObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            foodObject.transform.localScale = Vector3.one * 5;
            var food = foodObject.AddComponent<Food>();
            food.GetComponent<Renderer>().material.color = Color.green;
            food.GetComponent<Collider>().isTrigger = true;
            food.transform.position = new Vector3(
                x: _size.xMin + Random.value * _size.width,
                y: 0,
                z: _size.yMin + Random.value * _size.height
            );
        }
    }
}