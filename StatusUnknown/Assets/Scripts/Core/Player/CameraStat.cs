namespace Core.Player
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "CameraStat", menuName = "CustomAssets/Camera Stat", order = 1)]
    public class CameraStat : ScriptableObject
    {
        public float smoothSpeed = 2f;
        public Vector3 defaultOffset;
        public Vector3 fightModeOffset;
        public float zoomTime;
        public float unZoomTime;
    }
}


