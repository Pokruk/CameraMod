using UnityEngine;

namespace CameraMod.Extensions.GUI {
    public class GUIVector3 {
        
        private GUIFloatField x;
        private GUIFloatField y;
        private GUIFloatField z;
        
        public GUIVector3() {
            x = new GUIFloatField();
            y = new GUIFloatField();
            z = new GUIFloatField();
        }

        public void Draw(ref Vector3 value) {
            GUILayout.BeginHorizontal();

            x.Draw(ref value.x);
            y.Draw(ref value.y);
            z.Draw(ref value.z);

            GUILayout.EndHorizontal();
        }
    }
}