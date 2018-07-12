using System;
using System.Collections.Generic;
using System.Linq;
using BodyGenerator.Manipulatables;
using MotionGenerator;
using MotionGenerator.Entity.Soul;
using UnityEngine;

namespace BodyGenerator
{
    public class Agent : MonoBehaviour
    {
//        [SerializeField] protected GameObject rootObject;
        Body body;
        IBrain brain;
//        List<GameObject> manipulatableObjects;
        [SerializeField] bool sync = true;

//        List<IManipulatable> GetChildrenManipulatables()
//        {
//            // TODO: search into nested children ?
//            var returnList = new List<IManipulatable>();
//            foreach (Transform child in transform)
//            {
//                foreach (var component in child.gameObject.GetComponents<IManipulatable>())
//                {
//                    returnList.Add((IManipulatable) component);
//                }
//            }
//
//            return returnList;
//        }

        
        private List<IManipulatable> GetChildrenManipulatables()
        {
            return transform.gameObject.GetComponentsInChildren<IManipulatable>().ToList();
        }
//        void Start()
//        {
//            throw new NotImplementedException("need to call Init");
//        }

        public void Init(IBrain brain, Body body, List<IAction> actions)
        {
            List<IManipulatable> manipulatables = GetChildrenManipulatables();
            foreach (var manipulatable in manipulatables)
            {
                Debug.Log(manipulatable.GetManipulatableId());
            }

            for (int i = 0; i < manipulatables.Count; i++)
            {
                manipulatables[i].SetManipulatableId(i); // just an ID
            }

            body.Init(manipulatables, actions);
            brain.Init(
                manipulatables
                    .Where(m => m.GetManipulatableDimention() > 0)
                    .Select(m => m.GetManipulatableDimention()).ToList(),
                actions,
                new List<ISoul>() {new GluttonySoul()});
            this.brain = brain;
            this.body = body;
        }

        public void Init(IBrain brain, Body body, List<IAction> actions, bool sync)
        {
            this.sync = sync;
            Init(brain, body, actions);
        }

        void GoNextAction()
        {
            State state = body.GetState();
            var ms = brain.GenerateMotionSequence(state);
            body.Manipulate(ms);
        }

        void Update()
        {
            if (!this.sync)
            {
                UpdateAction();
            }
        }

        void FixedUpdate()
        {
            if (this.sync)
            {
                UpdateAction();
            }
        }

        void UpdateAction()
        {
            body.Update();
            if (body != null && !body.IsMoving())
            {
                GoNextAction();
            }
        }
    }
}