using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MotionGenerator;
using RLCreature.BodyGenerator.Manipulatables;
using UnityEngine;

namespace RLCreature.BodyGenerator
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
        private DenseVector _birthPosition;

        public Body(GameObject rootObject)
        {
            this.rootObject = rootObject;
        }

        public void Init(List<IManipulatable> manipulatables, List<IAction> actions)
        {
            var position = rootObject.transform.position;
            _birthPosition = new DenseVector(new double[]{position.x, position.y, position.z});
            this.manipulatables = manipulatables;
            foreach (var manipulatable in this.manipulatables)
            {
                manipulatable.Init();
            }

            var manipulatablesMovables = manipulatables.Where(m => m.GetManipulatableDimention() > 0).ToList();
            motor = new Motor(manipulatablesMovables);
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
            state[State.BasicKeys.Time] = new DenseVector(new double[]{Time.time});
            state[State.BasicKeys.BirthPosition] = _birthPosition;
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