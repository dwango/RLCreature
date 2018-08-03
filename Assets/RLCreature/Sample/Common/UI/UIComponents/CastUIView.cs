using UnityEngine;
using UnityEngine.UI;

namespace RLCreature.Sample.Common.UI.UIComponents
{
    public class CastUIView : MonoBehaviour
    {
        public Text Time;

        public Text CreatureCount;
        public Text FoodCount;

        public Button CreatureListToggleButton;
        public RectTransform CreatureListRoot;
        public RectTransform CreatureList;

        public Button SubCameraToggleButton;
        public RectTransform SubCameraRoot;
        public RawImage SubCameraRawImage;

        public RectTransform ToolBarCategoryList;
        public RectTransform ToolBarListRoot;
        public RectTransform ToolBarList;

        public RectTransform Cursor;
    }
}
