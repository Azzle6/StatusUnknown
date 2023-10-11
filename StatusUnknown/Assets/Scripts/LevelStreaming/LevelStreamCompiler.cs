
namespace LevelStreaming
{
    using UnityEngine;
    using LevelStreaming.Data;
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine.SceneManagement;
    using Sirenix.OdinInspector;
    using UnityEditor.SceneManagement;

    [ExecuteAlways]
    public partial class LevelStreamCompiler : MonoBehaviour
    {
        [SerializeField] Camera viewCamera;
        [SerializeField] List<LevelStreamVolume> levelStreamVolumes = new List<LevelStreamVolume>();

        #region Data Register
        private void OnEnable()
        {
            LevelStreamDataSO.Instance.UpdateDataEvent += UpdateData;
        }

        private void OnDisable()
        {
            LevelStreamDataSO.Instance.UpdateDataEvent -= UpdateData;
        }
        #endregion

        private void Update()
        {
            if (Application.IsPlaying(this))
                CheckVolumeInView();
            
        }

        private void CheckVolumeInView()
        {
            Plane[] viewPlanes = GeometryUtility.CalculateFrustumPlanes(viewCamera);
            for (int i = 0; i < levelStreamVolumes.Count; i++)
            {
                bool volumeInView = GeometryUtility.TestPlanesAABB(viewPlanes, levelStreamVolumes[i].Data.Bounds);
                StreamVolume(levelStreamVolumes[i], volumeInView);
            }
 
        }

        #region Load & UnLoad Level Stream Volume
        void StreamVolume(LevelStreamVolume volume, bool load)
        {
            if (volume.IsLoaded == load) return;
            if(load)
                LoadStreamVolume(ref volume);
            if (!load)
                UnLoadStreamVolume(ref volume);
        }

        void LoadStreamVolume(ref LevelStreamVolume streamVolume)
        {
            if (streamVolume.FirstLoad == false)
            {
                StartCoroutine(LoadingSceneOperation(streamVolume));
                streamVolume.FirstLoad = true;
            }
            streamVolume.LoadRootObject.SetActive(true);
            streamVolume.IsLoaded = true;
        }
        void UnLoadStreamVolume(ref LevelStreamVolume streamVolume)
        {
            streamVolume.LoadRootObject.SetActive(false);
            streamVolume.IsLoaded = false;
        }
        IEnumerator LoadingSceneOperation(LevelStreamVolume streamVolume)
        {
            var AsyncOp = SceneManager.LoadSceneAsync(streamVolume.Data.SceneAssetPath, LoadSceneMode.Additive);
            Scene activeScene = SceneManager.GetActiveScene();
            Scene loadedScene = SceneManager.GetSceneByName(streamVolume.Data.SceneName);

            while (!AsyncOp.isDone)
            {
                yield return null;
            }

            // parent all root objects in the loaded scene to the loadRootObject
            var rootObjects = loadedScene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                SceneManager.MoveGameObjectToScene(rootObject, activeScene);
                rootObject.transform.parent = streamVolume.LoadRootObject.transform;
            }
            SceneManager.UnloadSceneAsync(streamVolume.Data.SceneAssetPath);
        }
        #endregion
        private void MarkActiveSceneDirty()
        {
#if UNITY_EDITOR
            Scene activeScene = SceneManager.GetActiveScene();
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
#endif  
        }

        private void OnDrawGizmos()
        {
            if(levelStreamVolumes != null)
            {
                foreach (var levelStreamVolume in levelStreamVolumes)
                {
                    Gizmos.color = levelStreamVolume.IsLoaded ?  Color.green : Color.yellow ;
                    Gizmos.DrawWireCube(levelStreamVolume.Data.Bounds.center, levelStreamVolume.Data.Bounds.size);
                }
            }
        }
    }
#if UNITY_EDITOR
    partial class LevelStreamCompiler : MonoBehaviour
    {
        #region Help
        [Button("UpdateData")]
        private void UpdateData()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            ClearLevelStreamVolumes();
            List<LevelStreamVolumeData> streamVolumeDatas = LevelStreamDataSO.Instance.GetLeveStreamVolumeDataFromScene(sceneName);
            if (streamVolumeDatas == null)
                return;

            foreach (var streamVolumeData in streamVolumeDatas)
            {
                GameObject loadRootObject = new GameObject($"{streamVolumeData.SceneName}_levelStreamRoot");
                loadRootObject.transform.position = streamVolumeData.Position;
                levelStreamVolumes.Add(new LevelStreamVolume(streamVolumeData, loadRootObject));
            }
            MarkActiveSceneDirty();
        }
        [Button("ClearData")]
        void ClearLevelStreamVolumes()
        {
            foreach (var levelStreamVolume in levelStreamVolumes)
            {
                if (Application.isEditor)
                    GameObject.DestroyImmediate(levelStreamVolume.LoadRootObject);
                else
                    GameObject.Destroy(levelStreamVolume.LoadRootObject);
            }

            levelStreamVolumes = new List<LevelStreamVolume>();
            MarkActiveSceneDirty();
        }
        [Button("Preview Scenes")]
        private void PreviewAllLevelStreamScenes()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            foreach (var levelStreamVolume in levelStreamVolumes)
                EditorSceneManager.OpenScene(levelStreamVolume.Data.SceneAssetPath, OpenSceneMode.Additive);
            SceneManager.SetActiveScene(activeScene);

        }
        [Button("Hide PreviewScenes")]
        private void HideAllLevelStreamScenes()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            int sceneCount = EditorSceneManager.sceneCount;

            // Parcourez toutes les scènes ouvertes
            for (int i = sceneCount - 1; i > 0; i--)
            {
                if(i != 0)
                {
                    Scene scene = EditorSceneManager.GetSceneAt(i);
                    EditorSceneManager.CloseScene(scene,true);
                }
            }
            SceneManager.SetActiveScene(activeScene);
        }

        #endregion
    }
#endif

}