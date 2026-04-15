using CameraMod.Camera.Comps;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace CameraMod.Camera.Networking {
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("IWrappedSerializable.OnSerializeRead", MethodType.Normal)]
    public class ReplicationPatch {
        public static ReplicationParent currentReplicationParent {
            get {
                var controller = CameraController.Instance;
                var mode = controller.cameraMode;

                if (mode == CameraMode.None) {
                    var parent = controller.cameraTabletT?.parent;

                    if (parent == LeftGrabTrigger.instance.leftHandT)
                        return ReplicationParent.LeftHand;

                    if (parent == RightGrabTrigger.instance.rightHandT)
                        return ReplicationParent.RightHand;

                    return ReplicationParent.None;
                }

                return mode switch {
                    CameraMode.FirstPersonView => ReplicationParent.Head,
                    CameraMode.FollowPlayer    => ReplicationParent.None,
                    CameraMode.ThirdPerson     => controller.followheadrot 
                        ? ReplicationParent.Head 
                        : ReplicationParent.Body,
                    _ => ReplicationParent.None
                };
            }
        }

        public static byte ReplicationCode = 27;
        public static ReplicationParent LastReplicationParent = ReplicationParent.None;
        public static void Postfix() {
            var replicationParent = currentReplicationParent;
            
            var controller = CameraController.Instance;
            var tablet = controller.tabletCameraT;

            var relativeTo = ReplicationHandler.GetParentT(VRRig.LocalRig, replicationParent);
            Vector3 position;
            Quaternion rotation; 
            if (relativeTo == null) {
                position = tablet.position;
                rotation = tablet.rotation;
            } else {
                position = relativeTo.InverseTransformPoint(tablet.position);
                rotation = Quaternion.Inverse(relativeTo.rotation) * tablet.rotation;
            }

            var toLerp = replicationParent == LastReplicationParent;
            var content = new object[] { position, rotation, currentReplicationParent, toLerp};
            var options = new RaiseEventOptions() {
                Receivers = ReceiverGroup.All,
            };
            PhotonNetwork.RaiseEvent(ReplicationCode, content, options, SendOptions.SendUnreliable);

            LastReplicationParent = replicationParent;
        } 
    }
}