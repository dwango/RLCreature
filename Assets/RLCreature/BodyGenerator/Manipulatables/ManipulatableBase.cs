using MotionGenerator;
using UnityEngine;

namespace BodyGenerator.Manipulatables
{
    public abstract class ManipulatableBase : MonoBehaviour, IManipulatable
    {
        private int _manipulatableId;
        protected readonly State EmptyState = new State();

        protected bool _isMoving = false;

        // TODO(ogaki): Monobehaviourを外して抽象化したい。。。
        protected virtual GameObject ThisGameObject()
        {
            return gameObject;
        }

        public virtual void Init()
        {
        }

        public void SetManipulatableId(int value)
        {
            _manipulatableId = value;
        }

        public virtual int GetManipulatableDimention()
        {
            return 0;
        }

        public int GetManipulatableId()
        {
            return _manipulatableId;
        }

        public virtual State GetState()
        {
            return EmptyState;
        }

        public bool IsMoving()
        {
            return _isMoving;
        }

        public virtual void Manipulate(MotionSequence sequence)
        {
        }

        public virtual void UpdateFixedFrame()
        {
        }
    }
}