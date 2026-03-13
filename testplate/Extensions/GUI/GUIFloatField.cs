namespace CameraMod.Extensions.GUI {
    public class GUIFloatField {
        public GUIFloatField(float value) {
            xStr = value.ToString("F5");
        }
 
        public float value;
        private string xStr;
        public void Draw() {
            xStr = UnityEngine.GUILayout.TextField(xStr).Replace(",", ".");
            if (float.TryParse(xStr, out float x))
                value = x;
        }
    }
}