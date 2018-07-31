using UnityEngine;
using UnityEngine.UI;

namespace RLCreature.Sample.Common.UI.UIComponents
{
    public class ToolBarCell : MonoBehaviour
    {
        public Button Button;
        public Text Name;
        public Image Background;
        public Color OddColor;
        public Color EvenColor;

        public void SetColor(int listIndex)
        {
            if (listIndex % 2 == 0)
            {
                Background.color = EvenColor;
            }
            else
            {
                Background.color = OddColor;
            }
        }
    }
}