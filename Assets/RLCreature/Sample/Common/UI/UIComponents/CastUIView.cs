using UnityEngine;
using UnityEngine.UI;

namespace RLCreature.Sample.Common.UI.UIComponents
{
    public class CastUIView : MonoBehaviour
    {
        public Text Time;

        public Text CreatureCount;
        public Text FoodCount;

        public Button ALifeListToggleButton;
        public RectTransform ALifeListRoot;
        public RectTransform ALifeList;

        public Button SubCameraToggleButton;
        public RectTransform SubCameraRoot;
        public RawImage SubCameraRawImage;

        public RectTransform ToolBarCategoryList;
        public RectTransform ToolBarListRoot;
        public RectTransform ToolBarList;

        public RectTransform Cursor;
    }
}
