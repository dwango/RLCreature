using BodyGenerator.Manipulatables;
using MotionGenerator;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

namespace RLCreature.Sample
{
    public class CarControlManipulatable : ManipulatableBase
    {
        private CarController _control;
        private int _consumedFrames;
        private MotionSequence _sequence = new MotionSequence();

        public static CarControlManipulatable CreateComponent(GameObject car)
        {
            return car.AddComponent<CarControlManipulatable>()._CreateComponent();
        }


        private CarControlManipulatable _CreateComponent()
        {
            _control = GetComponent<CarController>();
            GetComponent<CarUserControl>().enabled = false;
            return this;
        }

        public override int GetManipulatableDimention()
        {
            return 2;
        }

        public override void Manipulate(MotionSequence sequence)
        {
            _consumedFrames = 0;
            _isMoving = true;
            _sequence = new MotionSequence(sequence);
        }

        public override void UpdateFixedFrame()
        {
            if (_sequence.Sequence.Count > 0)
            {
                if (_sequence[0].time < _consumedFrames)
                {
                    _sequence.Sequence.RemoveAt(0);                    
                }
                else
                {
                    _control.Move(_sequence[0].value[0]*2-1, _sequence[0].value[1]*2-1, _sequence[0].value[1]*2-1, 0);
                }
            }
            else
            {
                _isMoving = false;
            }

            _consumedFrames += 1;
        }
    }
}