using System;
using RLCreature.BodyGenerator;
using RLCreature.Sample.Common.UI.Cameras.SubCameras;
using RLCreature.Sample.Common.UI.UIComponents;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RLCreature.Sample.Common.UI
{
    public class CastUIPresenter : MonoBehaviour
    {
        private const string EnergySummaryFormat = "+{0:f0} -{1:f0}";
        public static readonly string PrefabPath = "SampleServerUICanvas";

        private SubCamera _subCamera;
        private AgentFocusStrategy _subCameraStrategy;


        public static CastUIPresenter CreateComponent(Camera targetCamera, GameObject rootObject)
        {
            var es = Transform.FindObjectOfType<EventSystem>();
            if (es == null)
            {
                var obj = new GameObject("EventSystem");
                obj.AddComponent<EventSystem>();
                obj.AddComponent<StandaloneInputModule>();
            }
            var presenter = targetCamera.gameObject.AddComponent<CastUIPresenter>();
            presenter._targetCamera = targetCamera;
            presenter.Init(rootObject);
            return presenter;
        }

        public IObservable<Tuple<FallbackedEventReceiver.FallbackedEvents, PointerEventData>>
            FallbackedEventsObservable { get; private set; }

        public readonly ReactiveProperty<GameObject> SelectedCreature = new ReactiveProperty<GameObject>();

        public ToolBar LeftToolBar { get; private set; }

        private Camera _targetCamera;
        private CreatureInfoList _creatureInfoList;

        private GameObject _currentTargetCreature = null;

        private CastUIView _canvasUi;

        public bool Visible
        {
            get { return _canvasUi.gameObject.activeSelf; }
            set { _canvasUi.gameObject.SetActive(value); }
        }

        public bool SubCameraIsActive
        {
            get { return _subCamera.IsActive; }
            set
            {
                _subCamera.IsActive = value;
                _canvasUi.SubCameraRoot.gameObject.SetActive(value);
            }
        }

        public void SelectCreature(GameObject creature)
        {
            SelectedCreature.Value = creature;
        }

        public Vector3 GetCursorPosition()
        {
            return _canvasUi.Cursor.transform.position;
        }

        public void ResetCursorPosition()
        {
            _canvasUi.Cursor.anchoredPosition = Vector2.zero;
        }

        private void OnDestroy()
        {
            Destroy(_subCamera);
        }
        
        public void AddAgent(Agent agent)
        {
            var displayName = agent.name;
            _creatureInfoList.Add(agent.gameObject, displayName);
            SelectableCreature.CreateComponent(agent.gameObject, _targetCamera.transform, SelectedCreature);
            _subCameraStrategy.Add(agent);
        }


        private void Init(GameObject rootObject)
        {
            var receiver = new GameObject("FallbackedEvent").AddComponent<FallbackedEventReceiver>();
            receiver.transform.SetParent(transform, false);
            var raycaster = FallbackPhysicsRaycaster.CreateComponent(gameObject, receiver.gameObject);
            FallbackedEventsObservable = receiver.EventObservable;

            _canvasUi = Instantiate(Resources.Load<CastUIView>(PrefabPath));
            _canvasUi.transform.SetParent(transform, false);

            // SubCamera View
            var texture = new RenderTexture(200, 110, 24);
            _canvasUi.SubCameraRawImage.texture = texture;
            var rect = _canvasUi.SubCameraRawImage.GetComponent<RectTransform>().rect;
            var aspect = rect.width / rect.height;
            _subCameraStrategy = new AgentFocusStrategy();
            _subCamera =
                SubCamera.CreateComponent(rootObject, _subCameraStrategy, texture, aspect);
            _canvasUi.SubCameraToggleButton.onClick.AddListener(() =>
            {
                SubCameraIsActive = !SubCameraIsActive;
            });
            SubCameraIsActive = false;

            _creatureInfoList = new CreatureInfoList(_canvasUi.ALifeList);
            _creatureInfoList.SelectedCell
                .Where(x => x != null)
                .Subscribe(x => SelectedCreature.Value = x.Creature)
                .AddTo(this);

            // Selected Creature
            SelectedCreature.Subscribe(OnSelected).AddTo(this);
            
            // Cursor
            FallbackedEventsObservable
                .Where(x => x.Item1 == FallbackedEventReceiver.FallbackedEvents.RightClick)
                .Where(x => Vector2.Distance(x.Item2.position, x.Item2.pressPosition) < 0.1f)
                .Subscribe(x => _canvasUi.Cursor.position = x.Item2.position)
                .AddTo(this);

            // ToolBar
            LeftToolBar = new ToolBar(_canvasUi.ToolBarCategoryList, _canvasUi.ToolBarList, _canvasUi.ToolBarListRoot);
            LeftToolBar.ListVisible = false;
        }
        
        private void OnSelected(GameObject creature)
        {
            _creatureInfoList.Select(creature);
            _currentTargetCreature = creature;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _canvasUi.gameObject.SetActive(!_canvasUi.gameObject.activeSelf);
            }
        }

        private void LateUpdate()
        {
            var timeRate = Time.deltaTime / Time.unscaledDeltaTime;

            _canvasUi.Time.text = TimeFormatFromSeconds(Time.time) + " (x" + timeRate.ToString("0.0") + ")";
            _canvasUi.CreatureCount.text = _creatureInfoList.CurrentCreaturesCount().ToString();
//            _canvasUi.FoodCount.text = status.CurrentFoodCount.ToString();

            _creatureInfoList.UpdateList();
        }

        private string TimeFormatFromSeconds(double seconds)
        {
            var secInt = (int) Math.Floor(seconds);
            var hour = secInt / 3600;
            var min = (secInt % 3600) / 60;
            var sec = seconds % 60.0f;
            return string.Format("{0:00}:{1:00}:{2:00.0}", hour, min, sec);
        }
    }
}