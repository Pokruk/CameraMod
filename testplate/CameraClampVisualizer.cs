using UnityEngine;

namespace CameraMod {
    public class CameraClampVisualizer {
        private static Texture2D MakeCircleTexture(int size, Color color) {
            Texture2D tex = new Texture2D(size, size);
            Color transparent = new Color(0, 0, 0, 0);
            Vector2 center = new Vector2(size / 2f, size / 2f);
            float radius = size / 2f;

            for (int y = 0; y < size; y++) {
                for (int x = 0; x < size; x++) {
                    float dist = Vector2.Distance(new Vector2(x, y), center);
                    if (dist <= radius)
                        tex.SetPixel(x, y, color);
                    else
                        tex.SetPixel(x, y, transparent);
                }
            }

            tex.Apply();
            return tex;
        }

        private static bool initiated = false;

        private static Texture2D circleTex;
        private static Color circleColor = new Color(1, 1, 1, 0.2f);


        private static Texture2D pointerTex;
        private static Color pointerColor = new Color(1, 0, 0, 1f);

        private static void DrawCircle(float centerX, float centerY, float radius, Texture2D texture) {
            var diameter = radius * 2;
            GUI.DrawTexture(new Rect(centerX - radius, centerY - radius, diameter, diameter), texture);
        }
        private static Vector3 GetDifferenceAngle(Transform from, Transform to) {
            return new Vector3(
                Mathf.DeltaAngle(from.eulerAngles.x, to.eulerAngles.x),
                Mathf.DeltaAngle(from.eulerAngles.y, to.eulerAngles.y),
                Mathf.DeltaAngle(from.eulerAngles.z, to.eulerAngles.z)
            );
        }

        public static void OnGUI(UnityEngine.Camera cam, Transform objectToFollow, float maxAngle) {
            if (!initiated) {
                circleTex = MakeCircleTexture(256, circleColor);
                pointerTex = MakeCircleTexture(256, pointerColor);
                initiated = true;
            }
        
            float screenHeight = Screen.height;
        
            float radius = Mathf.Tan(maxAngle * Mathf.Deg2Rad)
                           / Mathf.Tan((cam.fieldOfView / 2f) * Mathf.Deg2Rad)
                           * (screenHeight / 2f);
        
            Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
            DrawCircle(center.x, center.y, radius, circleTex);


            Vector3 camPoint = cam.transform.position + objectToFollow.forward;
            Vector3 camScreenPos = cam.WorldToScreenPoint(camPoint);
            
            DrawCircle(camScreenPos.x, screenHeight - camScreenPos.y, 5, pointerTex);
        }

    }
}
