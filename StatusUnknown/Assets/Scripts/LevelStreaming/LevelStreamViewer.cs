namespace LevelStreaming
{
    using UnityEngine;
    public class LevelStreamViewer : MonoBehaviour
    {
        [SerializeField] Camera viewCam;
        private void Update()
        {
            LevelStreamHandler.UpdateViewPlanesFromCamera(viewCam);
        }
    }
}