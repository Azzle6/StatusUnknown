

namespace LevelStreaming
{

    using UnityEngine;
    using LevelStreaming.Data;
    [System.Serializable]
    public class LevelStreamVolume
    {
        [field:SerializeField] public LevelStreamVolumeData Data { get; private set; }
        public bool IsLoaded = false;
        public bool FirstLoad = false;
        public GameObject LoadRootObject;

        public LevelStreamVolume(LevelStreamVolumeData data,GameObject loadRootObject)
        {
            Data = data;
            LoadRootObject = loadRootObject;
        }
    }
}