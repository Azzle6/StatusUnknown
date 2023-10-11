namespace LevelStreaming.Data
{
    using Core.SingletonsSO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Sirenix.OdinInspector;
    using System;
    using static UnityEngine.InputManagerEntry;

    [CreateAssetMenu(fileName = "LevelStreamGlobalData", menuName = "CustomAssets/LevelStream", order = 2)]
    public class LevelStreamDataSO : SingletonSO<LevelStreamDataSO>
    {
        [SerializeField] List<LevelStreamSceneData> sceneDatas = new List<LevelStreamSceneData>();
        public IReadOnlyList<LevelStreamSceneData> SceneDatas { get { return sceneDatas.AsReadOnly(); } }
        public event Action UpdateDataEvent;
        public void AddData(LevelStreamVolumeData levelStreamVolumeData)
        {
            Debug.Log("Add data" + levelStreamVolumeData.SceneName);
            if (sceneDatas == null) sceneDatas = new List<LevelStreamSceneData>();

            foreach (var sceneData in sceneDatas)
            {
                if(sceneData.SceneName == levelStreamVolumeData.SourceSceneName)
                {
                    for(int i = 0; i < sceneData.LevelStreamVolumeDatas.Count; i++)
                    {
                        if (sceneData.LevelStreamVolumeDatas[i].SceneAssetPath == levelStreamVolumeData.SceneAssetPath)
                        {
                            sceneData.LevelStreamVolumeDatas[i] = levelStreamVolumeData;
                            UpdateData();
                            return;
                        }  
                    }
                    sceneData.LevelStreamVolumeDatas.Add(levelStreamVolumeData);
                    UpdateData();
                    return;
                }
            }
            sceneDatas.Add(new LevelStreamSceneData(levelStreamVolumeData.SourceSceneName,new List<LevelStreamVolumeData>() { levelStreamVolumeData}));
            UpdateData();
        }
        public List<LevelStreamVolumeData> GetLeveStreamVolumeDataFromScene(string sceneName)
        {
            for (int i = 0; i < sceneDatas.Count; i++)
            {
                if(sceneName == sceneDatas[i].SceneName)
                    return sceneDatas[i].LevelStreamVolumeDatas;
            }
            return null;
        }

        [Button("RefreshData")]
        void RefreshData()
        {
            // delete old data without valid asset path & potential duplicata
            for(int i = sceneDatas.Count - 1; i >= 0; i--)
            {
                for(int j = sceneDatas[i].LevelStreamVolumeDatas.Count-1; j >= 0 ; j--)
                {
                    LevelStreamVolumeData volumeData = sceneDatas[i].LevelStreamVolumeDatas[j];
                    if (AssetDatabase.LoadAssetAtPath(volumeData.SceneAssetPath, typeof(SceneAsset)) == null)
                        sceneDatas[i].LevelStreamVolumeDatas.RemoveAt(j);
                    if (sceneDatas[i].LevelStreamVolumeDatas.Count <= 0 )
                        sceneDatas.RemoveAt(i);
                }
            }
            UpdateData();
        }
        void UpdateData()
        {
            if (UpdateDataEvent != null)
                UpdateDataEvent();
        }

        
    }
}