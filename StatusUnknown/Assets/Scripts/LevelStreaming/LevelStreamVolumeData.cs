using System;
using UnityEngine;

namespace LevelStreaming
{
    [Serializable]
    public class LevelStreamVolumeData
    {
        [field:SerializeField] public string SourceSceneName { get; private set; }
        [field: SerializeField] public string SceneName { get; private set; }
        [field: SerializeField] public string SceneAssetPath { get; private set; }

        [field: SerializeField] public Vector3 Position { get; private set; }
        Vector3 boundsCenter;
        Vector3 boundsSize;
        [SerializeField] Vector3 boundsExpand;

        public LevelStreamVolumeData(string sourceSceneName, string sceneName, string folderPath, Vector3 position, Vector3 boundsCenter, Vector3 boundsSize)
        {
            this.SourceSceneName = sourceSceneName;
            this.SceneName = sceneName;
            this.SceneAssetPath = $"{folderPath}/{sceneName}.unity";
            this.Position = position;
            this.boundsCenter = boundsCenter;
            this.boundsSize = boundsSize;
            this.boundsExpand = Vector3.zero;
        }

        public Bounds Bounds 
        { get
            {
                Bounds bounds = new Bounds(boundsCenter,boundsSize);
                bounds.Expand(boundsExpand);
                return bounds;
            }
        }
    }

}

