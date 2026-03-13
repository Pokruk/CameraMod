using UnityEngine;

namespace CameraMod.Extensions.GUI {
    public class GUIVector3 {
        public Vector3 value;
        
        private GUIFloatField x;
        private GUIFloatField y;
        private GUIFloatField z;
        
        public GUIVector3(Vector3 value) {
            this.value = value;
            x = new GUIFloatField(value.x);
            y = new GUIFloatField(value.y);
            z = new GUIFloatField(value.z);
        }

        public void Draw() {
            GUILayout.BeginHorizontal();

            x.Draw();
            y.Draw();
            z.Draw();
            value = new Vector3(x.value, y.value, z.value);

            GUILayout.EndHorizontal();
        }
    }
}