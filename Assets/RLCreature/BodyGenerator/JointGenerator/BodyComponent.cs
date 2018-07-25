using System;
using System.Linq;
using MessagePack.Decoders;
using UnityEngine;
using UnityEngine.Assertions;
using Joint = RLCreature.BodyGenerator.Manipulatables.Joint;
using Random = UnityEngine.Random;

namespace RLCreature.BodyGenerator.JointGenerator
{
    public class BodyComponent
    {
        private readonly MasterConnector[] _masterConnectors;
        private readonly SlaveConnector _slaveConnector;
        public readonly GameObject CentralBody;

        public BodyComponent(GameObject prefab, Vector3 pos, int connectorId = -1, BodyComponent parent = null)
        {
            CentralBody = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            CentralBody.AddComponent<Rigidbody>();

            _masterConnectors = CentralBody.GetComponentsInChildren<MasterConnector>();
            foreach (var connector in _masterConnectors)
            {
                connector.gameObject.SetActive(false);
            }

            _slaveConnector = CentralBody.GetComponentInChildren<SlaveConnector>();
            _slaveConnector.gameObject.SetActive(false);


            if (parent != null)
            {
                if (connectorId >= 0)
                {
                    parent.Connect(connectorId, this);
                }
                else
                {
                    parent.ConnectRandom(this);
                }
            }
            else
            {
                ToRigid();
            }
        }

        public BodyComponent(GameObject prefab, int connectorId = -1, BodyComponent parent = null): this(
            prefab, Vector3.up*10, connectorId, parent){}


        private void ToRigid()
        {
            if (CentralBody.GetComponent<Rigidbody>() == null)
            {
                CentralBody.AddComponent<Rigidbody>();
            }

            foreach (var rigid in CentralBody.GetComponentsInChildren<Rigidbody>())
            {
                rigid.solverIterations = 30;
            }
        }


        private MasterConnector EnableConnector(int connectorId)
        {
            var connector = _masterConnectors[connectorId];
            GameObject.Destroy(connector.GetComponent<Collider>());
            connector.gameObject.SetActive(true);
            return connector;
        }

        private void EnableSlaveConnector()
        {
            _slaveConnector.gameObject.SetActive(true);
            GameObject.Destroy(_slaveConnector.GetComponent<Collider>());
        }

        private void Connect(int connectorId, BodyComponent otherComponent)
        {
            Assert.IsTrue(connectorId < _masterConnectors.Length);
            var connector = EnableConnector(connectorId);
            otherComponent.EnableSlaveConnector();
            var rot = connector.transform.rotation * Quaternion.Inverse(otherComponent
                          ._slaveConnector.transform.rotation);
            otherComponent.CentralBody.transform.rotation = rot * otherComponent.CentralBody.transform.rotation;
            var diff = (connector.transform.position - otherComponent._slaveConnector.transform.position);
            otherComponent.CentralBody.transform.position += diff;

            otherComponent.ToRigid();

            var joint = CentralBody.gameObject.AddComponent<ConfigurableJoint>();
            joint.lowAngularXLimit = new SoftJointLimit {limit = -90};
            joint.highAngularXLimit = new SoftJointLimit {limit = 90};
            joint.angularYLimit = new SoftJointLimit {limit = 90};
            joint.angularZLimit = new SoftJointLimit {limit = 90};
            joint.projectionMode = JointProjectionMode.PositionAndRotation; // 爆発防止
            joint.connectedBody = otherComponent.CentralBody.GetComponent<Rigidbody>();
            joint.anchor = connector.transform.localPosition;
            Joint.CreateComponent(joint, targetForce: 1);
            Physics.IgnoreCollision(CentralBody.GetComponent<Collider>(),
                otherComponent.CentralBody.GetComponent<Collider>());
            connector.available = false;
        }

        public bool HasAvailableConnector()
        {
            return _masterConnectors.Any(x => x.available);
        }

        public void ConnectRandom(BodyComponent otherComponent)
        {
            var avairableConnectorIDs = _masterConnectors
                .Select((i, x) => new Tuple<int, MasterConnector>(x, i))
                .Where(x => x.Item2.available)
                .Select(x => x.Item1)
                .ToList();

            Connect(avairableConnectorIDs[Random.Range(0, avairableConnectorIDs.Count)], otherComponent);
        }
    }
}