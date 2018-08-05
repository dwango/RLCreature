using System;
using MathNet.Numerics.LinearAlgebra.Double;
using MotionGenerator;
using MotionGenerator.Entity.Soul;
using UnityEngine;

namespace RLCreature.BodyGenerator.Manipulatables
{
    public class Mouth : ManipulatableBase
    {
        private Type _type;
        private readonly State _state = new State();
        public int EatenCount { get; private set; }
        
        private Mouth()
        {
        }

        public static Mouth CreateComponent(GameObject obj, Type type)
        {
            return obj.AddComponent<Mouth>()._CreateComponent(type);
        }

        private Mouth _CreateComponent(Type type)
        {
            _type = type;
            _state[GluttonySoul.Key] = new DenseVector(1);
            
            var collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.transform.localScale = transform.localScale * 1.5f;
            
            return this;
        }

        public override State GetState()
        {
            _state.Set(GluttonySoul.Key, EatenCount);
            return _state;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent(_type) != null)
            {
                Destroy(other.gameObject);
                EatenCount += 1;
            }
        }

    }
}