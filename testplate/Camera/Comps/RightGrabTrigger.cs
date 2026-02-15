using UnityEngine;

#pragma warning disable CS0618
namespace CameraMod.Camera.Comps {
    internal class RightGrabTrigger : MonoBehaviour {
        private Transform rightHandT => GorillaTagger.Instance.rightHandTransform;
        private CameraController controller => CameraController.Instance;
        private Transform tabletT => controller.cameraTabletT;
        
        private void Start() {
            gameObject.layer = 18;
        }

        private void OnTriggerStay(Collider col) {
            if (col.name.Contains("Right"))
                if (InputManager.instance.RightGrip & controller.cameraMode != CameraMode.FirstPersonView) {
                    tabletT.parent = rightHandT;
                    if (controller.cameraMode == CameraMode.FollowPlayer) controller.cameraMode = CameraMode.None;
                }

            if (!InputManager.instance.RightGrip & (tabletT.parent == rightHandT))
                tabletT.parent = null;
        }
    }
}