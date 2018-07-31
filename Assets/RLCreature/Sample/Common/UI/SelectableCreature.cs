using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RLCreature.Sample.Common.UI
{
    public class SelectableCreature : MonoBehaviour, IPointerClickHandler
    {
        public static readonly string Name = "UICollider";

        private GameObject _clickableCreature;
        private GameObject _referencedCreature;

        private ReactiveProperty<GameObject> _target;

        [SerializeField]
        private Transform _camera;

        [SerializeField]
        private SphereCollider _collider;

        public static SelectableCreature CreateComponent(
            GameObject clickableCreatureRoot,
            Transform camera,
            ReactiveProperty<GameObject> observer,
            GameObject referencedCreatureRoot = null)
        {
            var obj = new GameObject(Name);
            var central = clickableCreatureRoot.transform.GetChild(0);
            obj.transform.SetParent(central, false);
            var self = obj.AddComponent<SelectableCreature>();
            self._clickableCreature = clickableCreatureRoot;
            self._camera = camera;
            self._collider = obj.AddComponent<SphereCollider>();
            self._collider.isTrigger = true;
            self._target = observer;
            self._referencedCreature = referencedCreatureRoot ?? self._clickableCreature;
            return self;
        }

        void Update()
        {
            var distance = Vector3.Distance(_camera.position, transform.position);
            var coef = Mathf.Max(1.0f, Mathf.Sqrt(distance) - 5.0f);
            _collider.radius = coef;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (_target == null) return;
            _target.Value = _referencedCreature;
        }
    }
}