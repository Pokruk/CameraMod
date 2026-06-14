using UnityEngine;

namespace CameraMod.Camera.AppearanceFeatures {
    public class Appearance {
        public GameObject go;
        public Transform transform => go.transform;

        private GameObject adminLabel;
        public readonly string name;

        public bool isOwner {
            get {
                return adminLabel.activeSelf;
            }
            set {
                adminLabel.SetActive(value);
            }
        }
        
        public Appearance(GameObject go, string name) {
            this.go = go;
            this.name = name;
            adminLabel = go.transform.Find("Offset").Find("IsOwner").gameObject;
        }
    }
}