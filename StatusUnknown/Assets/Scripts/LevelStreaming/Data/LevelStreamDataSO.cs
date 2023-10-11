namespace LevelStreaming.Data
{
    using Core.SingletonsSO;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Sirenix.OdinInspector;
    using System;

    [CreateAssetMenu(fileName = "LevelStreamGlobalData", menuName = "CustomAssets/LevelStream", order = 2)]
    public class LevelStreamDataSO : SingletonSO<LevelStreamDataSO>
    {
        [SerializeField] public List<LevelStreamSceneData> SceneDatas = new List<LevelStreamSceneData>();
        //public IReadOnlyList<LevelStreamSceneData> SceneDatas { get { return sceneDatas.AsReadOnly(); } }
        public event Action UpdateDataEvent;
        public void AddData(LevelStreamVolumeData levelStreamVolumeData)
        {
           
            Debug.Log("Add data" + levelStreamVolumeData.SceneName);
            if (SceneDatas == null) SceneDatas = new List<LevelStreamSceneData>();

            foreach (var sceneData in SceneDatas)
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
            SceneDatas.Add(new LevelStreamSceneData(levelStreamVolumeData.SourceSceneName,new List<LevelStreamVolumeData>() { levelStreamVolumeData}));
            UpdateData();
        }
        public List<LevelStreamVolumeData> GetLeveStreamVolumeDataFromScene(string sceneName)
        {
            for (int i = 0; i < SceneDatas.Count; i++)
            {
                if(sceneName == SceneDatas[i].SceneName)
                    return SceneDatas[i].LevelStreamVolumeDatas;
            }
            return null;
        }

        [Button("RefreshData")]
        void RefreshData()
        {
            // delete old data without valid asset path & potential duplicata
            for(int i = SceneDatas.Count - 1; i >= 0; i--)
            {
                for(int j = SceneDatas[i].LevelStreamVolumeDatas.Count-1; j >= 0 ; j--)
                {
                    LevelStreamVolumeData volumeData = SceneDatas[i].LevelStreamVolumeDatas[j];
                    if (AssetDatabase.LoadAssetAtPath(volumeData.SceneAssetPath, typeof(SceneAsset)) == null)
                        SceneDatas[i].LevelStreamVolumeDatas.RemoveAt(j);
                    if (SceneDatas[i].LevelStreamVolumeDatas.Count <= 0 )
                        SceneDatas.RemoveAt(i);
                }
            }
            UpdateData();
        }
        void UpdateData()
        {
            EditorUtility.SetDirty(this);
            if (UpdateDataEvent != null)
                UpdateDataEvent();
        }

        
    }
}