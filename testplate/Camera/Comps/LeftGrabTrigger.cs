using UnityEngine;

#pragma warning disable CS0618
namespace CameraMod.Camera.Comps {
    internal class LeftGrabTrigger : MonoBehaviour {
        private Transform leftHandT => GorillaTagger.Instance.leftHandTransform;
        private CameraController controller => CameraController.Instance;
        private Transform tabletT => controller.cameraTabletT;
        
        private void Start() {
            gameObject.layer = 18;
        }

        private void OnTriggerStay(Collider col) {
            if (col.name.Contains("Left"))
                if (InputManager.instance.LeftGrip & controller.cameraMode != CameraMode.FirstPersonView) {
                    tabletT.parent = leftHandT;
                    if (controller.cameraMode == CameraMode.FollowPlayer) controller.cameraMode = CameraMode.None;
                }

            if (!InputManager.instance.LeftGrip & (tabletT.parent == leftHandT))
                tabletT.parent = null;
        }
    }
}