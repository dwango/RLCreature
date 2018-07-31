using System.Collections.Generic;
using System.Linq;
using RLCreature.BodyGenerator;
using RLCreature.Sample.Common.UI.Cameras.OperatableCameras;
using UnityEngine;

namespace RLCreature.Sample.Common.UI.Cameras.SubCameras
{
    public class AgentFocusStrategy : IFocusStrategy
    {
        private const float TransitionTime = 6f;
        private const float InitialDistance = 3f;
        private const float FarAwayDistance = 5;

        private readonly int _interval;
        private readonly List<Agent> _creatures;
        private int _currentIndex;
        private float _lastChangedTime;
        private CameraAttitude _lastCameraAttitude;
        private IOperatableCamera _camera;

        public AgentFocusStrategy(int interval = 5)
        {
            _interval = interval;
            _creatures = new List<Agent>();
            _lastCameraAttitude = new CameraAttitude
            {
                LookAt = new Vector3(-1f, 0, -10f),
                Position = new Vector3(0.0f, 10.0f, -40.0f),
            };
            // 最初は対象がいないので定点を見ておく
            _camera = new MonitoringCamera(_lastCameraAttitude);
        }

        public void Add(Agent creature)
        {
            _creatures.Add(creature);
        }

        public void Remove(Agent creature)
        {
            if (!_creatures.Contains(creature))
            {
                return;
            }

            _creatures.Remove(creature);
        }

        public CameraAttitude Update()
        {
            var time = Time.unscaledTime - _lastChangedTime;
            if (time > _interval)
            {
                _lastChangedTime = Time.unscaledTime;
                if (!_creatures.Any())
                {
                    return _lastCameraAttitude;
                }

                _currentIndex++;
                if (_currentIndex >= _creatures.Count)
                {
                    _currentIndex = 0;
                }

                var creature = _creatures[_currentIndex];

                var transform = creature.transform.GetChild(0).transform;
                _camera = new FocusCamera(transform,
                    transitionTime: TransitionTime, initialDistance: InitialDistance, farAwayDistance: FarAwayDistance);
                _camera.Initialize(_lastCameraAttitude);
            }

            _lastCameraAttitude = _camera.Update(Time.unscaledDeltaTime);
            return _lastCameraAttitude;
        }
    }
}