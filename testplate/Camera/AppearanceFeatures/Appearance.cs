using UnityEngine;

namespace CameraMod.Camera.AppearanceFeatures {
    public class Appearance {
        public GameObject go;
        public Transform transform => go.transform;

        private GameObject adminLabel;
        public bool isOwner {
            get {
                return adminLabel.activeSelf;
            }
            set {
                adminLabel.SetActive(value);
            }
        }
        
        public Appearance(GameObject go) {
            this.go = go;
            adminLabel = go.transform.Find("IsOwner").gameObject;
        }
    }
}