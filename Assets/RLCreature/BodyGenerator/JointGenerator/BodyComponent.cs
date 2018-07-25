using MessagePack.Decoders;
using UnityEngine;
using UnityEngine.Assertions;
using Joint = RLCreature.BodyGenerator.Manipulatables.Joint;

namespace RLCreature.BodyGenerator.JointGenerator
{
    public class BodyComponent
    {
        private MasterConnector[] _masterConnectors;
        private readonly SlaveConnector _slaveConnector;
        public GameObject _centralBody;

        public BodyComponent(GameObject prefab, int connectorId = 0, BodyComponent parent = null)
        {
            _centralBody = GameObject.Instantiate(prefab);
            _centralBody.AddComponent<Rigidbody>();

            _masterConnectors = _centralBody.GetComponentsInChildren<MasterConnector>();
            foreach (var connector in _masterConnectors)
            {
                connector.gameObject.SetActive(false);
            }

            _slaveConnector = _centralBody.GetComponentInChildren<SlaveConnector>();
            _slaveConnector.gameObject.SetActive(false);


            if (parent != null)
            {
                parent.Connect(connectorId, this);
            }
            else
            {
                ToRigid();
            }
        }

        private void ToRigid()
        {
            if (_centralBody.GetComponent<Rigidbody>() == null)
            {
                _centralBody.AddComponent<Rigidbody>();
            }

            foreach (var rigid in _centralBody.GetComponentsInChildren<Rigidbody>())
            {
                rigid.solverIterations = 30;
            }
        }


        private MasterConnector EnableConnector(int connectorId)
        {
            var connector = _masterConnectors[connectorId];
            connector.gameObject.SetActive(true);
            _centralBody.AddComponent<FixedJoint>().connectedBody = connector.gameObject.AddComponent<Rigidbody>();
            return connector;
        }

        private void EnableSlaveConnector()
        {
            _slaveConnector.gameObject.SetActive(true);
            _centralBody.AddComponent<FixedJoint>().connectedBody =
                _slaveConnector.gameObject.AddComponent<Rigidbody>();
        }

        private void Connect(int connectorId, BodyComponent otherComponent)
        {
            Assert.IsTrue(connectorId < _masterConnectors.Length);
            var connector = EnableConnector(connectorId);
            otherComponent.EnableSlaveConnector();
            var rot = connector.transform.rotation * Quaternion.Inverse(otherComponent
                          ._slaveConnector.transform.rotation);
            otherComponent._centralBody.transform.rotation = rot * otherComponent._centralBody.transform.rotation;
            var diff = (connector.transform.position - otherComponent._slaveConnector.transform.position);
            otherComponent._centralBody.transform.position += diff;

            otherComponent.ToRigid();

            var joint = connector.gameObject.AddComponent<ConfigurableJoint>();
            joint.lowAngularXLimit = new SoftJointLimit {limit = -90};
            joint.highAngularXLimit = new SoftJointLimit {limit = 90};
            joint.angularYLimit = new SoftJointLimit {limit = 90};
            joint.angularZLimit = new SoftJointLimit {limit = 90};
            joint.connectedBody = otherComponent._slaveConnector.GetComponent<Rigidbody>();
            Joint.CreateComponent(joint, targetForce: 1);
            Physics.IgnoreCollision(_centralBody.GetComponent<Collider>(), otherComponent._centralBody.GetComponent<Collider>());
        }

        public void ConnectRandom(BodyComponent otherComponent)
        {
            Connect(Random.Range(0, _masterConnectors.Length), otherComponent);
        }
    }
}