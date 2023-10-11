

namespace LevelStreaming.Data
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    [Serializable]
    public class LevelStreamSceneData
    {
        [field:SerializeField] public string SceneName {  get; private set; }
        public List<LevelStreamVolumeData> LevelStreamVolumeDatas = new List<LevelStreamVolumeData>();

        public LevelStreamSceneData(string sceneName, List<LevelStreamVolumeData> volumeDatas)
        {
            SceneName = sceneName;
            LevelStreamVolumeDatas = volumeDatas;
        }
    }
}