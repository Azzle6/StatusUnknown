namespace LevelStreaming.Editor
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using LevelStreaming.Data;
    using System.Collections.Generic;

    public static class LevelStreamEditor
    {
        [MenuItem("GameObject/GenerateLevelStreamVolume")]//TODO : disable command when no gameObjects are selected
        static void CreateLevelStreamVolume()
        {
            if (Selection.activeGameObject == null || !Selection.activeGameObject.gameObject) return;

            string folderPath = CreateFolderPath();
            GameObject sceneObject = Selection.activeGameObject.gameObject;
            Transform parentTransform = sceneObject.transform.parent;

            Scene scene = CreateScene($"{Selection.activeTransform.name}_SubScene", folderPath, sceneObject);
            GenerateLevelStreamVolumeData(sceneObject, scene.name, folderPath);
            //CreateLevelStreamObject(sceneObject, parentTransform, scene.name, folderPath);

        }
        static string CreateFolderPath()
        {
            // create folder
            Scene activeScene = SceneManager.GetActiveScene();
            string folderName = $"{activeScene.name}_SubScenes";
            string folderPath = $"Assets/Scenes/Levels";//TODO : Get Root scene path

            if (!AssetDatabase.IsValidFolder($"{folderPath}/{folderName}"))
                AssetDatabase.CreateFolder(folderPath, folderName);

            AssetDatabase.Refresh();
            return $"{folderPath}/{folderName}";
        }
        static Scene CreateScene(string name, string folder, GameObject sceneObject)
        {
            Scene mainScene = SceneManager.GetActiveScene();
            string assetPath = $"{folder}/{name}.unity";
            Debug.Log(assetPath);

            Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);

            //delete old scene
            while (AssetDatabase.LoadAssetAtPath(assetPath, typeof(SceneAsset)) != null)
                AssetDatabase.DeleteAsset(assetPath);

            //Move gameObject
            sceneObject.transform.parent = null;
            SceneManager.MoveGameObjectToScene(sceneObject, newScene);

            EditorSceneManager.SaveScene(newScene, assetPath);
            AssetDatabase.Refresh();

            // refocus on the main scene
            SceneManager.SetActiveScene(mainScene);
            EditorSceneManager.SaveScene(mainScene);
            AssetDatabase.Refresh();
            AddSceneToBuild(assetPath);
            return newScene;
        }

        static void AddSceneToBuild(string assetPath)
        {
            // Vérifiez si la scène est déjà dans les Build Settings
            bool sceneAlreadyAdded = false;
            EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene buildScene in buildScenes)
            {
                if (buildScene.path == assetPath)
                {
                    sceneAlreadyAdded = true;
                    break;
                }
            }

            if (!sceneAlreadyAdded)
            {
                // Créez une nouvelle liste de scènes qui inclut la scène existante et la nouvelle scène
                List<EditorBuildSettingsScene> newSceneList = new List<EditorBuildSettingsScene>(buildScenes);
                newSceneList.Add(new EditorBuildSettingsScene(assetPath, true));

                // Attribuez la nouvelle liste de scènes aux Build Settings
                EditorBuildSettings.scenes = newSceneList.ToArray();

                // Marquez la scène comme modifiée
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetSceneByPath(assetPath));

                // Sauvegardez les modifications des scènes ouvertes
                EditorSceneManager.SaveOpenScenes();
            }
        }
        static void GenerateLevelStreamVolumeData(GameObject originObject,string createdSceneName,string folderPath)
        {
            Bounds bounds = GetObjectBounds(originObject);
            string sourceSceneName = SceneManager.GetActiveScene().name;
            LevelStreamVolumeData data = new LevelStreamVolumeData(sourceSceneName, createdSceneName, folderPath, originObject.transform.position, bounds.center, bounds.size);
            LevelStreamDataSO.Instance.AddData(data);
            Debug.Log("Add data from editor");
        }
        public static void MarkCurrentSceneDirty()
        {
            // Obtenez la scène active
            Scene activeScene = SceneManager.GetActiveScene();

            // Marquez la scène comme modifiée
            EditorSceneManager.MarkSceneDirty(activeScene);
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


