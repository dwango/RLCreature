using System.Collections;
using MotionGenerator;
using RLCreature.BodyGenerator;
using RLCreature.BodyGenerator.Manipulatables;
using RLCreature.Sample.Common;
using RLCreature.Sample.Common.UI;
using RLCreature.Sample.Common.UI.Actions;
using RLCreature.Sample.Common.UI.UIComponents;
using UnityEngine;

namespace RLCreature.Sample.VariousHeights
{
    public class Food : MonoBehaviour
    {
    }


    public class VariousHeightsEntrypoint : MonoBehaviour
    {
        private Rect _size;
        public int FoodCount = 100;
        private CastUIPresenter GameUI;
        public GameObject Plane;

        private void Start()
        {
            GameUI = CastUIPresenter.CreateComponent(Camera.main, gameObject);
            CastCameraController.CreateComponent(Camera.main, GameUI.SelectedCreature,
                GameUI.FallbackedEventsObservable);
            GameUI.LeftToolBar.Add(new SystemActions());

            Plane.transform.position = Vector3.zero;
            Plane.transform.localScale = Vector3.one * 10;
            var unitPlaneSize = 10;
            _size = new Rect(
                (Plane.transform.position.x - Plane.transform.lossyScale.x * unitPlaneSize) / 2,
                (Plane.transform.position.y - Plane.transform.lossyScale.y * unitPlaneSize) / 2,
                Plane.transform.lossyScale.x * unitPlaneSize,
                Plane.transform.lossyScale.y * unitPlaneSize
            );
            SpawnCreature(reinforcement: true);
            SpawnCreature(reinforcement: true);
            SpawnCreature(reinforcement: false);
            SpawnCreature(reinforcement: false);


            StartCoroutine(Feeder());
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
            var food = foodObject.AddComponent<Food>();
            var heightRatio = Random.value;
            food.GetComponent<Renderer>().material.color = Color.green * (1 - heightRatio) + Color.red * heightRatio;
            food.GetComponent<Collider>().isTrigger = true;
            food.transform.position = new Vector3(
                x: _size.xMin + Random.value * _size.width,
                y: heightRatio * 3,
                z: _size.yMin + Random.value * _size.height
            );
            StartCoroutine(EntryPointUtility.DeleteTimer(foodObject, 60 * 5));
        }
     
        private GameObject SpawnCreature(bool reinforcement = true)
        {
            var rootObject = new GameObject();
            var creature = GameObject.CreatePrimitive(PrimitiveType.Cube);
            creature.transform.position = new Vector3(
                x: _size.xMin + Random.value * _size.width,
                y: 1,
                z: _size.yMin + Random.value * _size.height
            );
            creature.transform.parent = rootObject.transform;
            SuperFlexibleMove.CreateComponent(creature, speed: 1f);
            creature.AddComponent<Rigidbody>().freezeRotation = true;
            creature.GetComponent<Renderer>().material.color = reinforcement ? Color.red : Color.yellow;
            Sensor.CreateComponent(creature, typeof(Food), State.BasicKeys.RelativeFoodPosition, range: 100f);
            var mouth = Mouth.CreateComponent(creature, typeof(Food));
            var actions = LocomotionAction.EightDirections();
            var sequenceMaker = new EvolutionarySequenceMaker(epsilon: 0.3f, minimumCandidates: 30);
            if (reinforcement)
            {
            }

            var decisonMaker = reinforcement
                ? (IDecisionMaker) new ReinforcementDecisionMaker()
                : (IDecisionMaker) new FollowPointDecisionMaker(State.BasicKeys.RelativeFoodPosition);
            var brain = new Brain(
                decisonMaker,
                sequenceMaker
            );
            var agent = Agent.CreateComponent(rootObject, brain, new Body(creature), actions);
            var info = GameUI.AddAgent(agent);
            agent.name = reinforcement ? "Reinforce" : "Rule";
            StartCoroutine(EntryPointUtility.Rename(info, agent, mouth));
            return creature;
        }

    }
}