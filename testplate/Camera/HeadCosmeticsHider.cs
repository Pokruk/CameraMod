using System.Linq;
using UnityEngine;
using static GorillaNetworking.CosmeticsController;

namespace CameraMod.Camera {
    public class HeadCosmeticsHider : FromCameraHider {
        public static void setHeadCosmeticsEnabled(bool enable) {
            var headItems = instance.currentWornSet.items.Where(
                item =>
                    item.itemCategory == CosmeticCategory.Face ||
                    item.itemCategory == CosmeticCategory.Hat
            );

            foreach (var item in headItems) {
                var registry = GorillaTagger.Instance.offlineVRRig.cosmeticsObjectRegistry;
                var cosmeticInstance = registry.Cosmetic(item.displayName);

                CosmeticSlots slotType;
                if (item.itemCategory == CosmeticCategory.Face) {
                    slotType = CosmeticSlots.Face;
                } else if (item.itemCategory == CosmeticCategory.Hat) {
                    slotType = CosmeticSlots.Hat;
                } else {
                    Debug.Log("Lol why is this here");
                    continue;
                }

                if (enable)
                    cosmeticInstance.EnableItem(slotType, VRRig.LocalRig);
                else
                    cosmeticInstance.DisableItem(slotType);
            }
        }

        public override void Hide() {
            if (CameraController.Instance.cameraMode != CameraMode.FirstPersonView) return;

            setHeadCosmeticsEnabled(false);
        }

        public override void Show() {
            setHeadCosmeticsEnabled(true);
        }
    }
}