
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LevelStreaming.Editor
{
    public class LevelStreamEditor
    {
        [MenuItem("GameObject/GenerateLevelStreamVolume")]//TODO : disable command when no gameObjects are selected
        static void CreateLevelStreamVolume()
        {
            if (Selection.activeGameObject == null || !Selection.activeGameObject.gameObject) return;

            string folderPath = CreateFolderPath();
            GameObject sceneObject = Selection.activeGameObject.gameObject;
            Transform parentTransform = sceneObject.transform.parent;

            Scene scene = CreateScene($"{Selection.activeTransform.name}_SubScene", folderPath, sceneObject);
            CreateLevelStreamObject(sceneObject, parentTransform, scene.name, folderPath);

        }
        static string CreateFolderPath()
        {
            // create folder
            Scene activeScene = SceneManager.GetActiveScene();
            string folderName = $"{activeScene.name}_SubScenes";
            string folderPath = $"Assets";//TODO : Get Root scene path

            if (!AssetDatabase.IsValidFolder($"{folderPath}/{folderName}"))
                AssetDatabase.CreateFolder(folderPath, folderName);

            AssetDatabase.Refresh();
            return $"{folderPath}/{folderName}";
        }
        static Scene CreateScene(string name, string folder, GameObject sceneObject)
        {
            Scene mainScene = SceneManager.GetActiveScene();
            string filePath = $"{folder}/{name}.unity";
            Debug.Log(filePath);

            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

            //delete old scene
            while (AssetDatabase.LoadAssetAtPath(filePath, typeof(SceneAsset)) != null)
                AssetDatabase.DeleteAsset(filePath);

            //Move gameObject
            sceneObject.transform.parent = null;
            SceneManager.MoveGameObjectToScene(sceneObject, newScene);

            EditorSceneManager.SaveScene(newScene, filePath);
            AssetDatabase.Refresh();

            // refocus on the main scene
            SceneManager.SetActiveScene(mainScene);
            EditorSceneManager.SaveScene(mainScene);
            AssetDatabase.Refresh();

            return newScene;
        }
        static GameObject CreateLevelStreamObject(GameObject originObject, Transform parentTransform, string sceneName, string folderPath)
        {
            Bounds bounds = GetObjectBounds(originObject);
            string objectName = $"{originObject.name}_LevelStream";

            GameObject oldObject = GameObject.Find(objectName);
            if (oldObject != null)
                GameObject.DestroyImmediate(oldObject);

            GameObject levelStreamObject = new GameObject(objectName);

            levelStreamObject.transform.position = originObject.transform.position;
            levelStreamObject.transform.parent = parentTransform;

            LevelStreamVolume levelStreamZone = levelStreamObject.AddComponent<LevelStreamVolume>();
            levelStreamZone.Initialize(sceneName, folderPath, bounds);

            return levelStreamObject;

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
}


