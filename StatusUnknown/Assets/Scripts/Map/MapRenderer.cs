namespace Map
{
    using UnityEngine;
    using System.IO;
    public class MapRenderer : MonoBehaviour
    {
        public RenderTexture renderTexture;
        private Camera myCamera;

        void Start()
        {
            myCamera = GetComponent<Camera>();
            myCamera.targetTexture = renderTexture;
            myCamera.Render();

            SaveRenderTextureToDisk(renderTexture, "SavedRenderTexture.png");
        }

        void SaveRenderTextureToDisk(RenderTexture renderTexture, string fileName)
        {
            // Convert RenderTexture to Texture2D
            Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            // Convert Texture2D to byte array
            byte[] bytes = texture2D.EncodeToPNG();

            // Save byte array to disk as PNG
            Debug.Log(Path.Combine(Application.dataPath, fileName));
            File.WriteAllBytes(Path.Combine(Application.dataPath, fileName), bytes);
        }
    }

}
