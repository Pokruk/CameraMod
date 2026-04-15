using System.Collections.Generic;
using UnityEngine;

namespace CameraMod.Camera.AppearanceFeatures {
    public class AppearancesLoader {
        private static Dictionary<string, GameObject> appearancePrefabs = new Dictionary<string, GameObject>();

        private static bool isLoaded = false;
        private static void Load() {
            var appearancesGO = CameraController.LoadBundle("Appearances", ".appearances");
            void LoadPrefab(string name) {
                var meshPrefab = appearancesGO.transform.Find(name).gameObject;
                appearancePrefabs[name] = meshPrefab;
            }

            LoadPrefab("Default");
            
            isLoaded = true;
        }
        private static GameObject InstantiateAppearanceGO(string name) {
            if (!isLoaded) {
                Load();
            }

            var instance = GameObject.Instantiate(appearancePrefabs[name]);
            return instance;
        }

        public static AppearanceFeatures.Appearance InstantiateAppearance(string name) {
            var instance = InstantiateAppearanceGO(name);
            instance.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            return new AppearanceFeatures.Appearance(instance);
        }
    }
}