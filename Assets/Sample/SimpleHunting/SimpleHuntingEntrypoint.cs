using System.Collections;
using System.Collections.Generic;
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
        private void Start()
        {
            StartCoroutine(SpawnOne());
        }

        private IEnumerator SpawnOne()
        {
            var foodObject = new GameObject();
            var food = foodObject.AddComponent<Food>();


            var rootObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            SuperFlexibleMove.CreateComponent(rootObject, speed: 1f);
            rootObject.AddComponent<Rigidbody>().freezeRotation = true;
            rootObject.GetComponent<Renderer>().material.color = Color.red;

            yield return new WaitForSeconds(1f);
            
            Sensor.CreateComponent(rootObject, typeof(Food), State.BasicKeys.RelativeFoodPosition, range: 100f);
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