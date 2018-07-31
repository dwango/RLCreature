using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras.SubCameras
{
    public class SubCamera : MonoBehaviour
    {
        private Camera _camera;
        private RenderTexture _texture;
        
        public IFocusStrategy Strategy { get; set; }

        public bool IsActive
        {
            get { return _camera.enabled; }
            set { _camera.enabled = value; }
        }
        
        public static SubCamera CreateComponent(GameObject gameObject, IFocusStrategy strategy, RenderTexture texture, float aspect)
        {
            var self = gameObject.AddComponent<SubCamera>();
            self._CreateComponent(texture, aspect);
            self.Strategy = strategy;
            return self;
        }
        private void _CreateComponent(RenderTexture texture, float aspect)
        {
            var obje = new GameObject("SubCamera");
            obje.transform.parent = gameObject.transform;
            _camera = obje.AddComponent<Camera>();
            _camera.targetTexture = texture;
            _camera.aspect = aspect;
            _texture = texture;
        }

        private void Update()
        {
            var attitude = Strategy.Update();
            _camera.transform.position = attitude.Position;
            _camera.transform.LookAt(attitude.LookAt);
        }

        private void OnDestroy()
        {
            Destroy(_texture);
            Destroy(_camera.gameObject);
        }
    }
}