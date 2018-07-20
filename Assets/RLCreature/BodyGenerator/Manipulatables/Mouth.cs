using System;
using UnityEngine;

namespace RLCreature.BodyGenerator.Manipulatables
{
    public class Mouth : ManipulatableBase
    {
        private Type _type;

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
            
            var collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            
            return this;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent(_type) != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}