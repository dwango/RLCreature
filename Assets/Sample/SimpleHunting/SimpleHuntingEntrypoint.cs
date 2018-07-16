using System.Collections;
using BodyGenerator;
using BodyGenerator.Manipulatables;
using MotionGenerator;
using UnityEngine;

namespace RLCreature.Sample
{
    public class Food : MonoBehaviour
    {
    }


    public class SimpleHuntingEntrypoint : MonoBehaviour
    {
        private Rect _size;

        private void Start()
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = Vector3.zero;
            plane.transform.localScale = Vector3.one * 30;
            var unitPlaneSize = 10;
            _size = new Rect(
                (plane.transform.position.x - plane.transform.lossyScale.x * unitPlaneSize) / 2,
                (plane.transform.position.y - plane.transform.lossyScale.y * unitPlaneSize) / 2,
                plane.transform.lossyScale.x * unitPlaneSize,
                plane.transform.lossyScale.y * unitPlaneSize
            );
            StartCoroutine(SpawnOne());
            for (int i = 0; i < 100; i++)
            {
                Feed();
            }
        }

        private void Feed()
        {
            var foodObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            var food = foodObject.AddComponent<Food>();
            food.GetComponent<Renderer>().material.color = Color.green;
            food.transform.position = new Vector3(
                _size.left + Random.value * _size.width,
                0,
                _size.top + Random.value * _size.height
            );
        }

        private IEnumerator SpawnOne()
        {
            var rootObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            SuperFlexibleMove.CreateComponent(rootObject, speed: 1f);
            rootObject.AddComponent<Rigidbody>().freezeRotation = true;
            rootObject.GetComponent<Renderer>().material.color = Color.red;

            yield return new WaitForSeconds(1f);

            Sensor.CreateComponent(rootObject, typeof(Food), State.BasicKeys.RelativeFoodPosition, range: 100f);
            Mouth.CreateComponent(rootObject, typeof(Food));
            var agent = rootObject.AddComponent<Agent>();
            var actions = LocomotionAction.EightDirections();
            var sequenceMaker = new EvolutionarySequenceMaker(epsilon: 0.3f, minimumCandidates: 30);
//            var brain = new Brain(
//                new ReinforcementDecisionMaker(discountRatio: 0.9f, keyOrder: new[]
//                {
//                    State.BasicKeys.RelativeFoodPosition,
//                }),
//                sequenceMaker
//            );
            var brain = new Brain(
                new FollowPointDecisionMaker(State.BasicKeys.RelativeFoodPosition),
                sequenceMaker
            );
            agent.Init(brain, new Body(rootObject), actions);
        }
    }
}