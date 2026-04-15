using HarmonyLib;
using System;
using CameraMod.Camera.Networking;
using UnityEngine;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(GorillaTagger), "Start")]
    public class StartPatch {
        public static void Postfix() {
            new GameObject().AddComponent<CameraController>();
            ReplicationHandler.Initialize();
            CameraController.Instance.Init();
        }
    }
}