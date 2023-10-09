

namespace LevelStreaming
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    public class LevelStreamVolume : MonoBehaviour
    {
        [SerializeField] string sceneName;
        [SerializeField] string scenePathAsset;
        public Bounds Bounds
        {
            get
            {
                Bounds bounds = new Bounds(boundsCenter, boundsSize);
                bounds.Expand(extends);
                return bounds;
            }
        }
        [SerializeField, HideInInspector] Vector3 boundsSize;
        [SerializeField, HideInInspector] Vector3 boundsCenter;
        [SerializeField] Vector3 extends;

        public bool IsLoaded { get; private set; }
        bool firstLoad = false;
        [SerializeField] GameObject loadRootObject;

        [ContextMenu("Load")]
        public void LoadScene()
        {
            if (firstLoad == false)
            {
                StartCoroutine(LoadingSceneOperation());
                firstLoad = true;
            }
            loadRootObject.SetActive(true);
            IsLoaded = true;
        }
        public void UnLoadScene()
        {
            loadRootObject.SetActive(false);
            IsLoaded = false;
        }

        IEnumerator LoadingSceneOperation()
        {
            var AsyncOp = SceneManager.LoadSceneAsync(scenePathAsset, LoadSceneMode.Additive);
            Scene activeScene = SceneManager.GetActiveScene();
            Scene loadedScene = SceneManager.GetSceneByName(sceneName);

            while (!AsyncOp.isDone)
            {
                yield return null;
            }

            // parent all root objects in the loaded scene to the loadRootObject
            var rootObjects = loadedScene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                SceneManager.MoveGameObjectToScene(rootObject, activeScene);
                rootObject.transform.parent = loadRootObject.transform;
            }
            SceneManager.UnloadSceneAsync(scenePathAsset);
        }
        public void Initialize(string sceneName, string sceneFolderPath, Bounds originBounds, bool isLoaded = true)
        {
            scenePathAsset = $"{sceneFolderPath}/{sceneName}.unity";
            this.sceneName = sceneName;
            boundsCenter = originBounds.center; boundsSize = originBounds.size;
            // load trigger
            IsLoaded = isLoaded;
            firstLoad = false;
            // create loadRootObject
            loadRootObject = new GameObject($"LevelStreamRoot");
            loadRootObject.transform.parent = transform;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = IsLoaded ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(boundsCenter, boundsSize);
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }

    }
}