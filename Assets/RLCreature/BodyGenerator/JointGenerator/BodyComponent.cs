using UnityEngine;
using UnityEngine.Assertions;

namespace RLCreature.BodyGenerator.JointGenerator
{
    public class BodyComponent
    {
        private MasterConnector[] _masterConnectors;
        private readonly SlaveConnector _slaveConnector;
        public GameObject _instance;

        public BodyComponent(GameObject prefab)
        {
            _instance = GameObject.Instantiate(prefab);
            _masterConnectors = _instance.GetComponentsInChildren<MasterConnector>();
            _slaveConnector = _instance.GetComponentInChildren<SlaveConnector>();
            if (_instance.GetComponent<Rigidbody>() == null)
            {
                _instance.AddComponent<Rigidbody>();
            }

            _instance.GetComponent<Rigidbody>().solverIterations = 30;
            
            foreach (var collider in _instance.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;

            }
            foreach (var collider in _instance.GetComponentsInChildren<Rigidbody>())
            {
                Object.Destroy(collider);                

            }

        }

        public void Connect(int connectorId, BodyComponent otherComponent)
        {
//            Assert.IsTrue(connectorId < _slaveConnector.Length);
//            foreach (var connectorr in _slaveConnector)
//            {
//                UnityEngine.Debug.Log(connectorr);
//                UnityEngine.Debug.Log(connectorr.transform.rotation);
//            }


            var connector = _masterConnectors[connectorId];
            UnityEngine.Debug.Log(connector.name);
            UnityEngine.Debug.Log(connector.transform.rotation);
            UnityEngine.Debug.Log(otherComponent._instance.transform.rotation);
            otherComponent._instance.transform.rotation = Quaternion.Inverse(connector.transform.rotation);
            UnityEngine.Debug.Log(otherComponent._instance.transform.rotation);
            otherComponent._instance.transform.position = connector.transform.position;
            foreach (var collider in otherComponent._instance.GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;

            }
            foreach (var collider in otherComponent._instance.GetComponentsInChildren<Rigidbody>())
            {
                Object.Destroy(collider);                

            }


//            var joint = connector.gameObject.AddComponent<ConfigurableJoint>()
//            joint.connectedBody = otherComponent._connenctorFrom.GetComponent<Rigidbody>();
        }

        public void ConnectRandom(BodyComponent otherComponent)
        {
            Connect(UnityEngine.Random.Range(0, _masterConnectors.Length), otherComponent);
        }
    }
}