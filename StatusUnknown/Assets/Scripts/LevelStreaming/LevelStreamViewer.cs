namespace LevelStreaming
{
    using System.Collections;
    using System.Collections.Generic;
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