using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MotionGenerator;
using UnityEngine;

namespace BodyGenerator.Manipulatables
{
    public class Sensor : ManipulatableBase
    {
        private string _key;
        private float _range;
        private readonly State _state = new State();
        private Type _type;

        private Sensor()
        {
        }

        public static Sensor CreateComponent(GameObject obj, Type type, string key, float range)
        {
            return obj.AddComponent<Sensor>()._CreateComponent(type, key, range);
        }

        private Sensor _CreateComponent(Type type, string key, float range)
        {
            _key = key;
            _range = range;
            _state[_key] = new DenseVector(3);
            _type = type;
            return this;
        }

        public override State GetState()
        {
            var minDistance = float.MaxValue;
            _state.Set(_key, -Vector3.one); // when no candidate found
            foreach (var candidate in FindObjectsOfType(_type).Select(o => o as MonoBehaviour))
            {
                var distance = Vector3.Distance(transform.position, candidate.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    var inversedMyRotation = Quaternion.Inverse(transform.rotation);
                    var relativeEachTargetPosition =
                        inversedMyRotation * (candidate.transform.position - transform.position) / _range;
                    _state.Set(_key, relativeEachTargetPosition);
                }
            }

            return _state;
        }
    }
}