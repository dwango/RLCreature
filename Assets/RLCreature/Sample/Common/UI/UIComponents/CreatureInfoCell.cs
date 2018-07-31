using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace RLCreature.Sample.Common.UI.UIComponents
{
    public class CreatureInfoCell : MonoBehaviour
    {
        [SerializeField] private Color DefaultColor = Color.white;

        public RectTransform Header;

        public Text HeaderName;

        public Image BackgroundImage;

        public Color Default;

        public Color SelectedColor;

        public GameObject Creature;

        private string _displayName;

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                HeaderName.text = _displayName;
            }
        }

        public bool IsLastCreatureInFamily { get; set; }

        public readonly BoolReactiveProperty Selected = new BoolReactiveProperty(false);

        private void Awake()
        {
            Deselect();

            Selected.Subscribe(x =>
            {
                if (x)
                {
                    Select();
                }
                else
                {
                    Deselect();
                }
            }).AddTo(this);
        }

        private void Select()
        {
            BackgroundImage.color = SelectedColor;
        }

        private void Deselect()
        {
            BackgroundImage.color = Default;
        }

        public void OnPressed()
        {
            Selected.Value = true;
        }

        private Color GetHeaderColor()
        {
            return DefaultColor;
        }

        public bool IsActive()
        {
            return true;
        }

        private void Update()
        {
            HeaderName.color = GetHeaderColor();
        }
    }
}