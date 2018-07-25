﻿using MotionGenerator;
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
                Resources.Load<GameObject>("Prefabs/Cube")
            });
            var centralBody = generator.Instantiate();
            Sensor.CreateComponent(centralBody, typeof(Food), State.BasicKeys.RelativeFoodPosition, range: 100f);
            Mouth.CreateComponent(centralBody, typeof(Food));

            var actions = LocomotionAction.EightDirections();
            var sequenceMaker = new EvolutionarySequenceMaker(epsilon: 0.3f, minimumCandidates: 30);
            var brain = new Brain(
                new FollowPointDecisionMaker(State.BasicKeys.RelativeFoodPosition),
                sequenceMaker
            );
            Agent.CreateComponent(centralBody, brain, new Body(centralBody), actions);


//            StartCoroutine(Feeder());
////            StartCoroutine(SpawnSome());
//            var creature = StartCreature(CreatureRootGameObject, CentralBody);
//            var camera = Camera.main;
//            camera.transform.parent = creature.transform;
//            camera.transform.position = creature.transform.position - creature.transform.forward * 30 +
//                                        creature.transform.up * 10;
        }


//        private GameObject StartCreature(GameObject creatureRootGameObject, GameObject centralBody)
//        {
//            Sensor.CreateComponent(centralBody, typeof(Food), State.BasicKeys.RelativeFoodPosition, range: 100f);
//            Mouth.CreateComponent(centralBody, typeof(Food));
//
//            var actions = LocomotionAction.EightDirections();
//            var sequenceMaker = new EvolutionarySequenceMaker(epsilon: 0.3f, minimumCandidates: 30);
//            var brain = new Brain(
//                new FollowPointDecisionMaker(State.BasicKeys.RelativeFoodPosition),
//                sequenceMaker
//            );
//            Agent.CreateComponent(creatureRootGameObject, brain, new Body(centralBody), actions);
//
//            return centralBody;
//        }
//
//
//        private IEnumerator Feeder()
//        {
//            while (true)
//            {
//                var foodCount = FindObjectsOfType<Food>().Length;
//                for (int i = 0; i < FoodCount - foodCount; i++)
//                {
//                    Feed();
//                }
//
//                yield return new WaitForSeconds(5);
//            }
//        }
//
//        private void Feed()
//        {
//            var foodObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//            foodObject.transform.localScale = Vector3.one * 5;
//            var food = foodObject.AddComponent<Food>();
//            food.GetComponent<Renderer>().material.color = Color.green;
//            food.GetComponent<Collider>().isTrigger = true;
//            food.transform.position = new Vector3(
//                x: _size.xMin + Random.value * _size.width,
//                y: 0,
//                z: _size.yMin + Random.value * _size.height
//            );
//        }
    }
}