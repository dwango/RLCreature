using MotionGenerator;

namespace RLCreature.BodyGenerator.Manipulatables
{
    public interface IManipulatable
    {
        void Init();
        void SetManipulatableId(int value);
        int GetManipulatableDimention();
        int GetManipulatableId();
        State GetState();
        bool IsMoving();
        void Manipulate(MotionSequence sequence);
        void UpdateFixedFrame();
    }
}