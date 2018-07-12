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
        Body body;
        IBrain brain;


        private List<IManipulatable> GetChildrenManipulatables()
        {
            return transform.gameObject.GetComponentsInChildren<IManipulatable>().ToList();
        }

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

        void GoNextAction()
        {
            State state = body.GetState();
            var ms = brain.GenerateMotionSequence(state);
            body.Manipulate(ms);
        }

        void Update()
        {
            UpdateAction();
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