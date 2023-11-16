using UnityEditor;
using UnityEngine;

namespace StatusUnknown.Utils
{
    namespace AssetManagement
    {
        public static class StatusUnknown_AssetManager
        {
            public static string SAVE_PATH_BUILD = "Assets/Data/Gameplay/Combat/Simulators/";
            public static string SAVE_PATH_ABILITY = "Assets/Data/Gameplay/Combat/Abilities/";
            public static string SAVE_PATH_ENCOUNTER = "Assets/Data/Gameplay/Combat/Encounters/";
            public static string SAVE_PATH_ENEMY = "Assets/Prefabs/"; 

            public static void SaveSO(ScriptableObject assetToSave, string savePath, string saveName, string extension)
            {
                if (saveName == string.Empty || extension == string.Empty)
                {
                    Debug.LogError("saveName or extension was not provided. Asset could not be saved");
                    return;
                }

                AssetDatabase.CreateAsset(assetToSave, string.Concat(savePath, saveName, extension));
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            public static GameObject InstantiatePrefabAtPosition(Object obj, Vector3 pos)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(obj) as GameObject;
                instance.transform.position = pos;
                return instance;
            }
        }
    }

    namespace JSON
    {
        // move scripts to here
    }

    namespace WebRequests
    {
        // move scripts to here
    }
}
