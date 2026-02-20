using GorillaNetworking;
using HarmonyLib;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(CosmeticsV2Spawner_Dirty), nameof(CosmeticsV2Spawner_Dirty.ProcessLoadOpInfos))]
    public class CosmeticsLoaded {
        public static void Postfix() {
            CameraController.Instance.InitCosmeticsHider();
        }
    }
}
