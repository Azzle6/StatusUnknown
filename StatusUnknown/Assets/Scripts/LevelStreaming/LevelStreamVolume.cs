using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEditor;
#endif

public partial class LevelStreamVolume : MonoBehaviour
{
    public string SceneAssetPath;
    [SerializeField,HideInInspector]
    Vector3 boundsCenter, boundsSize;
    [SerializeField] Vector3 boundsExpand;
    GameObject loadRootObject;
    [field : SerializeField] public bool FirstLoad {  get; private set; }
    [field: SerializeField] public bool IsLoaded { get; private set; }
    public Bounds Bounds 
    {
        get 
        { 
            Bounds bounds = new Bounds(boundsCenter, boundsSize);
            bounds.Expand(boundsExpand);
            return bounds; 
        } 
    }

    
    // TODO : OPTIMISATION group all Update call from this script
    private void Update()
    {
        bool inView = LevelStreamHandler.IsBoundsInView(Bounds);
        if(inView && !IsLoaded)
            LoadStreamVolume();
        if (!inView && IsLoaded)
            UnLoadStreamVolume();
    }
    void LoadStreamVolume()
    {
        if (FirstLoad == false)
        {
            StartCoroutine(LoadingSceneOperation());
            FirstLoad = true;
        }
        if(loadRootObject != null)
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
    void UpdateBoundsFromScene(Scene scene)
    {
        GameObject[] rootObjects = null;
        rootObjects = scene.GetRootGameObjects();
        UpdateBoundsFromRootObject(rootObjects[0]);
        
    }

    void UpdateBoundsFromRootObject(GameObject rootObject)
    {
        transform.position = rootObject.transform.position;
        Bounds bounds = GetObjectBounds(rootObject);

        boundsCenter = bounds.center;
        boundsSize = bounds.size;
    }

}
#if UNITY_EDITOR
public partial class LevelStreamVolume : MonoBehaviour
{
    [Button("ShowScene")]
    void ShowSceneInEditor()
    {
        Scene scene = EditorSceneManager.OpenScene(SceneAssetPath, OpenSceneMode.Additive);
        UpdateBoundsFromScene(scene);
    }

    [Button("HideScene")]
    void HideSceneInEditor()
    {
        Scene scene = SceneManager.GetSceneByPath(SceneAssetPath);
        if (scene.IsValid())
        {
            UpdateBoundsFromScene(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.Refresh();
        }
        EditorSceneManager.CloseScene(scene, true);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Bounds.center, Bounds.size);
    }

    #region Bounds for childObjects
    static Bounds GetObjectBounds(GameObject parentObject)
    {
        Bounds resultBounds = new Bounds();
        if (parentObject == null) return resultBounds; //TODO : debugLog error


        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();
        if (renderers.Length <= 0) return resultBounds; //TODO : debugLog error

        resultBounds = renderers[0].bounds;
        for (int i = 0; i < renderers.Length; i++)
        {
            resultBounds = SummBounds(resultBounds, renderers[i].bounds);
        }
        return resultBounds;
    }
    static Bounds SummBounds(Bounds bounds1, Bounds bounds2)
    {
        Vector3 max = Vector3.Max(bounds1.max, bounds2.max);
        Vector3 min = Vector3.Min(bounds1.min, bounds2.min);

        Vector3 size = max - min;
        Vector3 center = min + (size / 2);
        return new Bounds(center, size);
    }
    #endregion

}
#endif
