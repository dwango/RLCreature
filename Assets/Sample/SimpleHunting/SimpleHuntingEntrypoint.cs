using System.Collections.Generic;
using BodyGenerator;
using BodyGenerator.Manipulatables;
using MotionGenerator;
using UnityEngine;

namespace RLCreature.Sample
{
    public class SimpleHuntingEntrypoint : MonoBehaviour
    {
        private void Start()
        {
            SpawnOne();
        }

        private void SpawnOne()
        {
            var rootObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            SuperFlexibleMove.CreateComponent(rootObject, speed: 1f);
            rootObject.AddComponent<Rigidbody>().freezeRotation = true;
            rootObject.GetComponent<Renderer>().material.color = Color.red;

            var agent = rootObject.AddComponent<Agent>();
            var actions = new List<IAction>()
            {
                LocomotionAction.GoStraight()
            };
            var sequenceMaker = new SimpleBanditSequenceMaker(epsilon: 0.3f, minimumCandidates: 100);


            var brain = new Brain(
                new ReinforcementDecisionMaker(discountRatio: 0.9f, keyOrder: new[]
                {
                    State.BasicKeys.Forward,
                }),
                sequenceMaker
            );
            agent.Init(brain, new Body(rootObject), actions);
        }
    }
}