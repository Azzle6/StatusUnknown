namespace LevelStreaming.Editor
{
    using Core.Helper;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    public static class LevelStreamEditor
    {
        #region Genreate Stream Scene from GameObject
        [MenuItem("GameObject/GenerateStreamScene")]
        static void GenerateStreamScene()
        {
            GameObject sceneObject = Selection.activeGameObject.gameObject;
            Vector3 sceneObjectPosition = sceneObject.transform.position;
            Bounds sceneObjectBounds = BoundsHelper.GetObjectBounds(sceneObject);

            string folderPath = CreateFolderPath();
            Scene scene = CreateSceneFromObject(sceneObject, folderPath);
            CreateLevelStreamVolumeObject($"{sceneObject.name}_LevelStreamVolume", scene.path, sceneObjectPosition, sceneObjectBounds);
        }
        static string CreateFolderPath()
        {
            // create folder
            Scene activeScene = SceneManager.GetActiveScene();
            string folderName = $"{activeScene.name}_StreamScenes";
            string folderPath = $"Assets/Scenes/Levels";

            if (!AssetDatabase.IsValidFolder($"{folderPath}/{folderName}"))
                AssetDatabase.CreateFolder(folderPath, folderName);

            AssetDatabase.Refresh();
            return $"{folderPath}/{folderName}";
        }
        static Scene CreateSceneFromObject(GameObject sceneObject, string folderPath)
        {
            Scene mainScene = SceneManager.GetActiveScene();
            string assetPath = $"{folderPath}/{sceneObject.name}_StreamScene.unity";

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
                List<EditorBuildSettingsScene> newSceneList = new List<EditorBuildSettingsScene>(buildScenes);
                newSceneList.Add(new EditorBuildSettingsScene(assetPath, true));
                EditorBuildSettings.scenes = newSceneList.ToArray();
                EditorSceneManager.MarkSceneDirty(SceneManager.GetSceneByPath(assetPath));
                EditorSceneManager.SaveOpenScenes();
            }
        }
        static void CreateLevelStreamVolumeObject(string objectName, string sceneAssetPath, Vector3 position, Bounds objectBounds)
        {
            GameObject levelStreamVolumeObject = new GameObject(objectName);
            levelStreamVolumeObject.transform.position = position;
            LevelStreamVolume levelStreamVolume = levelStreamVolumeObject.AddComponent<LevelStreamVolume>();
            levelStreamVolume.Initialize(sceneAssetPath, objectBounds);
        }

        [MenuItem("GameObject/GenerateStreamScene", true)]
        static bool CheckStreamScene()
        {
            return Selection.activeGameObject != null;
        }
        #endregion

        #region Generate Level Stream Volume from SceneAsset
        [MenuItem("Assets/GenerateLevelStreamVolume")]
        static void GenerateLevelStreamVolume()
        {
            string sceneAssetPath = RenamedAssetPathInSceneStream(Selection.activeObject);
            Scene streamScene = EditorSceneManager.OpenScene(sceneAssetPath, OpenSceneMode.Additive);
            GameObject rootStreamSceneObject = streamScene.GetRootGameObjects()[0];

            string objectName = $"{rootStreamSceneObject.name}_LevelStreamVolume";
            Vector3 position = rootStreamSceneObject.transform.position;
            Bounds objectBounds = BoundsHelper.GetObjectBounds(rootStreamSceneObject);
            CreateLevelStreamVolumeObject(objectName, sceneAssetPath, position, objectBounds);

        }
        static string RenamedAssetPathInSceneStream(Object assetObject)
        {
            string sceneAssetPath = AssetDatabase.GetAssetPath(assetObject);
            if (assetObject.name.Contains("_StreamScene"))
                return sceneAssetPath;

            string sceneStreamName = $"{assetObject.name}_StreamScene";
            return AssetDatabase.RenameAsset(sceneAssetPath, sceneStreamName);
        }
        [MenuItem("Assets/GenerateLevelStreamVolume", true)]
        static bool CheckLevelStreamVolume()
        {
            return Selection.activeObject.GetType() == typeof(SceneAsset);
        }
        #endregion
    }
}