namespace CameraMod.Extensions.GUI {
    public class GUIFloatField {
        private string xStr;
        
        public void Draw(ref float value) {
            if (xStr == null) {
                xStr = value.ToString();
            }
            xStr = UnityEngine.GUILayout.TextField(xStr).Replace(",", ".");
            if (float.TryParse(xStr, out float x))
                value = x;
        }
    }
}