using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace CameraMod.Camera.Networking {
    struct TabletReplication {
        public Transform target;
        public Transform mesh;
    }
    public class ReplicationHandler : IOnEventCallback, IInRoomCallbacks, IConnectionCallbacks {
        private static ReplicationHandler instance;
        public static void Initialize() {
            instance = new ReplicationHandler();
            instance.Init();
        }
        
        private void Init() {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private static TabletReplication NewTablet() {
            var mesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.Destroy(mesh.GetComponent<Collider>());
            GameObject.Destroy(mesh.GetComponent<Rigidbody>());
            var renderer = mesh.GetComponent<MeshRenderer>();
            var shader = Shader.Find("GorillaTag/UberShader");
            
            renderer.material.shader = shader;
            
            mesh.transform.localScale = new Vector3(0.5f, 0.2f, 0.5f);
            mesh.SetActive(false);

            var target = new GameObject();

            return new TabletReplication() {
                mesh = mesh.transform,
                target = target.transform
            };
        }
        
        public void OnEvent(EventData photonEvent) {
            if (photonEvent.Code != ReplicationPatch.ReplicationCode) {
                return;
            }
            if (photonEvent.Sender == 0) return;
            var sender = PhotonNetwork.CurrentRoom?.GetPlayer(photonEvent.Sender);
            if (sender == null) return;

            object[] content = photonEvent.CustomData as object[];
            if (content == null) return;
            if (content.Length < 4)  return;
            
            if (!(content[0] is Vector3)) return;
            if (!(content[1] is Quaternion)) return;
            if (!(content[2] is int)) return;
            if (!(content[3] is bool)) return;
            Vector3 position = (Vector3)content[0];
            Quaternion rotation = (Quaternion)content[1];
            ReplicationParent replicationParent = (ReplicationParent)content[2];
            bool toLerp = (bool)content[3];
            
            OnDataReceived(sender, position, rotation, replicationParent, toLerp);
        }
        
        private static Dictionary<Player, TabletReplication> Tablets = new Dictionary<Player, TabletReplication>();

        public static Transform GetParentT(VRRig rig, ReplicationParent replicationParent) {
            switch (replicationParent) {
                case ReplicationParent.None:
                    return null;
                case ReplicationParent.Head:
                    return rig.headMesh.transform;
                case ReplicationParent.LeftHand:
                    return rig.leftHand.rigTarget;
                case ReplicationParent.RightHand:
                    return rig.rightHand.rigTarget;
                case ReplicationParent.Body:
                    return rig.bodyTransform;
            }
            return null;
        }
        private void OnDataReceived(Player sender, Vector3 position, Quaternion rotation, ReplicationParent replicationParent, bool toLerp) {
            var rig = GorillaGameManager.instance.FindPlayerVRRig(sender);
            if (rig == null) {
                Debug.Log("Rig not found");
                return;
            }
            Transform parentT = GetParentT(rig, replicationParent);

            var isNewOne = !Tablets.TryGetValue(sender, out TabletReplication tablet);
            if (isNewOne) {
                tablet = NewTablet();
                Tablets[sender] = tablet;
            }
            
            tablet.target.parent = parentT;
            tablet.mesh.parent = parentT;
            
            var target = tablet.target;
            target.transform.localPosition = position;
            target.transform.localRotation = rotation;
            
            if (isNewOne || !toLerp) {
                tablet.mesh.position = tablet.target.position;
                tablet.mesh.rotation = tablet.target.rotation;
            }
            
            tablet.mesh.gameObject.SetActive(true);
        }
        
        public static void Lerp() {
            foreach (var tabletReplication in Tablets) {
                var tablet = tabletReplication.Value;
                tablet.mesh.localPosition = Vector3.Lerp(tablet.mesh.localPosition, tablet.target.localPosition, 0.1f);
                tablet.mesh.localRotation = Quaternion.Lerp(tablet.mesh.localRotation, tablet.target.localRotation, 0.1f);
            }
        }
        public void OnPlayerLeftRoom(Player player) {
            if (Tablets.TryGetValue(player, out TabletReplication tablet)) {
                GameObject.Destroy(tablet.target.gameObject);
                GameObject.Destroy(tablet.mesh.gameObject);
                Tablets.Remove(player);
            }
        }
        public void OnDisconnected(DisconnectCause cause) {
            foreach (var tabletReplication in Tablets) {
                GameObject.Destroy(tabletReplication.Value.target.gameObject);
                GameObject.Destroy(tabletReplication.Value.mesh.gameObject);
            }
            Tablets.Clear();
        }

        public void OnPlayerEnteredRoom(Player newPlayer) {}

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}
        public void OnMasterClientSwitched(Player newMasterClient) {}
        
        public void OnConnected() {}
        public void OnConnectedToMaster() { }

        public void OnRegionListReceived(RegionHandler regionHandler) { }
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }
        public void OnCustomAuthenticationFailed(string debugMessage) { }
    }
}