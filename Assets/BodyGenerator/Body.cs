using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using BodyGenerator.Manipulatables;
using MathNet.Numerics.LinearAlgebra.Double;
using MotionGenerator;

namespace BodyGenerator
{
    public class Motor
    {
        List<IManipulatable> _manipulatables;

        public Motor(List<IManipulatable> manipulatables)
        {
            this._manipulatables = manipulatables;
        }

        public void Manipulate(List<MotionSequence> motionSequences)
        {
            UnityEngine.Assertions.Assert.AreEqual(motionSequences.Count, _manipulatables.Count);
            for (int i = 0; i < _manipulatables.Count; i++)
            {
                _manipulatables[i].Manipulate(motionSequences[i]);
            }
        }
    }

    public class Body
    {
        GameObject rootObject;
        List<IManipulatable> manipulatables;
        Motor motor;

        public Body(GameObject rootObject)
        {
            this.rootObject = rootObject;
        }

        public void Init(List<IManipulatable> manipulatables, List<IAction> actions)
        {
            this.manipulatables = manipulatables;
            foreach (var manipulatable in this.manipulatables)
            {
                manipulatable.Init();
            }
            motor = new Motor(manipulatables);
        }

        public State GetState()
        {
            var state = new State();
            foreach (var manipulatable in manipulatables)
            {
                foreach (var kv in manipulatable.GetState())
                {
                    state[kv.Key] = kv.Value;
                }
            }

            var position = rootObject.transform.position;
            var rotation = rootObject.transform.rotation;
            var forward = rootObject.transform.forward;
            state[State.BasicKeys.Position] = new DenseVector(new double[] {position.x, position.y, position.z});
            state[State.BasicKeys.Rotation] =
                new DenseVector(new double[] {rotation.x, rotation.y, rotation.z, rotation.w});
            state[State.BasicKeys.Forward] = new DenseVector(new double[] {forward.x, forward.y, forward.z});
            return state;
        }

        public List<IManipulatable> GetJoints()
        {
            return manipulatables;
        }

        public void Manipulate(List<MotionSequence> sequence)
        {
            motor.Manipulate(sequence);
        }

        public bool IsMoving()
        {
            return manipulatables.Any(manipulable => manipulable.IsMoving());
        }


        public void Update()
        {
            for (var i = 0; i < manipulatables.Count; i++)
            {
                manipulatables[i].UpdateFixedFrame();
            }
        }
    }
}