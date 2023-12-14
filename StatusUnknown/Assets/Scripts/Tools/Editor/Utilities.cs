using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StatusUnknown.Utils
{
    namespace AssetManagement
    {
        public enum AssetType
        {
            NPC
        }

        public struct CoreAssetManagementStrings
        {
            public const string PATH_ROOT_DATA = "Assets/Data/";
            public const string PATH_ROOT_PREFABS = "Assets/Prefabs/";

            public const string SAVE_PATH_BUILD = PATH_ROOT_DATA + "Gameplay/Combat/Simulators/";
            public const string SAVE_PATH_ABILITY = PATH_ROOT_DATA + "Gameplay/Combat/Abilities/";
            public const string SAVE_PATH_ENCOUNTER = PATH_ROOT_DATA + "Gameplay/Combat/Encounters/";
            public const string SAVE_PATH_NPC = PATH_ROOT_DATA + "Narrative/NPC/"; 
        }

        public static class StatusUnknown_AssetManager
        {
            public static void SaveSO(ScriptableObject assetToSave, string savePath = "Assets/Data/", string saveName = "Default_Name", string extension = ".asset")
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

            // CANT DO THIS : because of my crappy architecture for scriptable objects (DON'T puth them within your tools folder)
            /* public static T LoadSO<T>(AssetType assetType,  string path) where T : ScriptableObject 
            {
                Type t; 
                switch(assetType)
                {
                    case AssetType.NPC:
                        t = NpcSO; 
                        return (T)(AssetDatabase.LoadAssetAtPath(path, t) as NpcSO)
                    break;
                }
            } */

            public static GameObject InstantiatePrefabAtPosition(Object obj, Vector3 pos)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(obj) as GameObject;
                instance.transform.position = pos;
                return instance;
            }

            /// <summary>
            /// Searches in the database for object with one ore more matching labels
            /// </summary>
            /// <param name="labels">Must follow the format l:myLabel</param>
            public static List<Object> LoadAssetsWithMatchingLabels(string labels, out List<Object> outObjs, string queryPath, bool prefabsOnly = false)
            {
                outObjs = new List<Object>();

                if (prefabsOnly)
                {
                    labels = string.Concat(labels, " ", "t:prefab"); 
                }

                string[] foundAssetsGUIDs = AssetDatabase.FindAssets(labels, new string[] { queryPath });

                foreach (string guid in foundAssetsGUIDs)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid); 

                    if (!string.IsNullOrEmpty(path))
                    {
                        outObjs.Add(AssetDatabase.LoadAssetAtPath(path, typeof(Object)));
                    }
                    else
                    {
                        Debug.LogError("some error to fill up");
                    }
                }

                return outObjs; 
            }

            public static T[] GetScriptableObjects<T>(string path)
            {
                throw new NotImplementedException();
            }

            // NOT TESTED
            static public IEnumerable<T> LoadAssetsOfType<T>(Func<T, bool> filter) where T : Object
            {
                /* 
                 *                         -- FILTER (EXAMPLE) --                 
                List<string> fruits =
                    new List<string> { "apple", "passionfruit", "banana", "mango",
                                        "orange", "blueberry", "grape", "strawberry" };

                IEnumerable<string> query = fruits.Where(fruit => fruit.Length < 6);

                IEnumerable<TSource> is directly infered from the function signature. No need to respecify it in the .Where()
                */

                var assets = AssetDatabase
                    .FindAssets($"t:{typeof(T).Name}")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<T>)
                    .Where(filter); 

                NativeArray<int> IDs = new NativeArray<int>(assets.Count() * sizeof(int), Allocator.Temp, NativeArrayOptions.UninitializedMemory);
                NativeArray<GUID> outGUIDs = new NativeArray<GUID>(IDs.Count(), Allocator.Temp, NativeArrayOptions.UninitializedMemory);

                foreach (var asset in assets)
                {
                    IDs.Append(asset.GetInstanceID());  
                }

                foreach (var asset in assets)
                {
                    Debug.Log($"{asset.name} can be found by clicking here : ", context: asset);

                    AssetDatabase.InstanceIDsToGUIDs(IDs, outGUIDs); 
                }

                for (int i = 0; i < outGUIDs.Count(); i++)
                {
                    yield return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(outGUIDs[i].ToString()), typeof(GameObject)) as T; 
                }
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

    namespace DataValidation
    {

    }
}
