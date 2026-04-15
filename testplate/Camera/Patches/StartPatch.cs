using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CameraMod.Camera.Comps;
using CameraMod.Camera.Networking;
using GorillaNetworking;
using UnityEngine;
using UnityEngine.Networking;

namespace CameraMod.Camera.Patches {
    [HarmonyPatch(typeof(GorillaTagger), "Start")]
    public class StartPatch {
        
        static IEnumerator FetchIDs(string pastebin, Action<HashSet<string>> then) {
            UnityWebRequest request = UnityWebRequest.Get(pastebin);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) {
                Debug.LogError("FetchWatermarkDeleteUserids filed: " + request.error);
            } else {
                then(request.downloadHandler.text.Split(Environment.NewLine).ToHashSet()); 
            }
        }

        public static HashSet<string> owners;
        public static string UserID = "";
        private static IEnumerator Main() {
            while (PlayFabAuthenticator.instance.GetPlayFabPlayerId() == null) {
                yield return new WaitForSeconds(1);
            }

            UserID = PlayFabAuthenticator.instance.GetPlayFabPlayerId();

            bool watermarkEnabled = true;
            yield return FetchIDs("https://pastebin.com/raw/EHB6SJnz", (noWatermarkIds) => {
                watermarkEnabled = !noWatermarkIds.Contains(PlayFabAuthenticator.instance.GetPlayFabPlayerId());
            });
            yield return FetchIDs("https://pastebin.com/raw/XhXfFv3Q", (result) => {
                owners = result;
            });
            var mainGO = new GameObject();
            
            mainGO.AddComponent<InputManager>();
            
            var ui = mainGO.AddComponent<UI>();
            ui.watermarkEnabled = watermarkEnabled;
            
            mainGO.AddComponent<CameraController>();
            CameraController.Instance.Init();
            
            ReplicationHandler.Initialize();
            
        }
        public static void Postfix() {
            GorillaTagger.Instance.StartCoroutine(Main());
        }
    }
}