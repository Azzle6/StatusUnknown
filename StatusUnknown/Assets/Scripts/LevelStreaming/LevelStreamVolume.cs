namespace LevelStreaming
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.SceneManagement;
    using UnityEngine;
    using Sirenix.OdinInspector;
    using LevelStreaming.Helper;
    using LevelStreaming;

#if UNITY_EDITOR
    using UnityEditor.SceneManagement;
    using UnityEditor;
#endif
    public partial class LevelStreamVolume : MonoBehaviour
    {
        [field: SerializeField]
        public string SceneAssetPath { get; private set; } // TODO : Debug Missing Reference
        [SerializeField, HideInInspector]
        Vector3 boundsCenter, boundsSize;
        [SerializeField] Vector3 boundsExpand;
        GameObject loadRootObject;
        public bool FirstLoad { get; private set; }
        public bool IsLoaded { get; private set; }
        public Bounds Bounds
        {
            get
            {
                Bounds bounds = new Bounds(boundsCenter, boundsSize);
                bounds.Expand(boundsExpand);
                return bounds;
            }
        }

        private void OnEnable()
        {
            LevelStreamHandler.UpdateViewEvent += StreamVolume;
        }

        private void OnDisable()
        {
            LevelStreamHandler.UpdateViewEvent -= StreamVolume;
        }
        void StreamVolume()
        {
            bool inView = LevelStreamHandler.IsBoundsInView(Bounds);
            if (inView && !IsLoaded)
                LoadStreamVolume();
            if (!inView && IsLoaded)
                UnLoadStreamVolume();
        }
        public void Initialize(string sceneAssetPath, Bounds objectBounds)
        {
            SceneAssetPath = sceneAssetPath;
            boundsCenter = objectBounds.center;
            boundsSize = objectBounds.size;
        }
        #region Load & Unload
        void LoadStreamVolume()
        {
            if (FirstLoad == false)
            {
                StartCoroutine(LoadingSceneOperation());
                FirstLoad = true;
            }
            if (loadRootObject != null)
                loadRootObject.SetActive(true);
            IsLoaded = true;
        }
        IEnumerator LoadingSceneOperation()
        {
            var AsyncOp = SceneManager.LoadSceneAsync(SceneAssetPath, LoadSceneMode.Additive);

            while (!AsyncOp.isDone)
            {
                yield return null;
            }
            Scene loadedScene = SceneManager.GetSceneByPath(SceneAssetPath);
            // parent all root objects in the loaded scene to the loadRootObject
            var rootObjects = loadedScene.GetRootGameObjects();
            rootObjects[0].transform.parent = transform;
            loadRootObject = rootObjects[0];
            SceneManager.UnloadSceneAsync(SceneAssetPath);
        }
        void UnLoadStreamVolume()
        {
            if (loadRootObject != null)
                loadRootObject.SetActive(false);
            IsLoaded = false;
        }
        #endregion
        #region Bound Operations
        void UpdateBoundsFromOpenScene(Scene scene)
        {
            GameObject[] rootObjects = null;
            rootObjects = scene.GetRootGameObjects();
            UpdateBoundsFromRootObject(rootObjects[0]);

        }
        void UpdateBoundsFromRootObject(GameObject rootObject)
        {
            transform.position = rootObject.transform.position;
            Bounds bounds = BoundsHelper.GetObjectBounds(rootObject);

            boundsCenter = bounds.center;
            boundsSize = bounds.size;
        }
        #endregion
    }
#if UNITY_EDITOR
    public partial class LevelStreamVolume : MonoBehaviour
    {
        [Button("ShowScene")]
        void ShowSceneInEditor()
        {
            Scene scene = EditorSceneManager.OpenScene(SceneAssetPath, OpenSceneMode.Additive);
            UpdateBoundsFromOpenScene(scene);
        }

        [Button("HideScene")]
        void HideSceneInEditor()
        {
            Scene scene = SceneManager.GetSceneByPath(SceneAssetPath);
            if (scene.IsValid())
            {
                UpdateBoundsFromOpenScene(scene);
                EditorSceneManager.SaveScene(scene);
                AssetDatabase.Refresh();
            }
            EditorSceneManager.CloseScene(scene, true);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }


    }
#endif
}

