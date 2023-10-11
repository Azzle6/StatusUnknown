namespace LevelStreaming.Data
{
    using System;
    using UnityEngine;
    [Serializable]
    public class LevelStreamVolumeData
    {
        [field:SerializeField,HideInInspector] public string SourceSceneName { get; private set; }
        [field: SerializeField] public string SceneName { get; private set; }
        [field: SerializeField] public string SceneAssetPath { get; private set; }

        public Vector3 Position { get; private set; }
        [SerializeField,HideInInspector] Vector3 boundsCenter;
        [SerializeField,HideInInspector] Vector3 boundsSize;
        [SerializeField] Vector3 boundsExpand;

        public LevelStreamVolumeData(string sourceSceneName, string createdSceneName, string folderPath, Vector3 position, Vector3 boundsCenter, Vector3 boundsSize)
        {
            this.SourceSceneName = sourceSceneName;
            this.SceneName = createdSceneName;
            this.SceneAssetPath = $"{folderPath}/{createdSceneName}.unity";
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

