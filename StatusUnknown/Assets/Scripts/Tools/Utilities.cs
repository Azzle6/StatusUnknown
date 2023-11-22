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
        public static class StatusUnknown_AssetManager
        {
            public static string SAVE_PATH_BUILD = "Assets/Data/Gameplay/Combat/Simulators/";
            public static string SAVE_PATH_ABILITY = "Assets/Data/Gameplay/Combat/Abilities/";
            public static string SAVE_PATH_ENCOUNTER = "Assets/Data/Gameplay/Combat/Encounters/";
            public static string SAVE_PATH_PREFABS = "Assets/Prefabs/"; 

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

        /// <summary>
        /// Must only contain Unity callbacks and pure functions to avoid inconsistent results in the pipeline (https://en.wikipedia.org/wiki/Pure_function)
        /// </summary>
        sealed class StatusUnknown_AssetProcessor : AssetPostprocessor
        {
            /*        
            Procedural generation/modification/optimization of all kinds of assets (eg. Pugrad https://github.com/keijiro/Pugrad)
            Automatic addressable asset management (eg. Unity Addressable Importer https://github.com/favoyang/unity-addressable-importer)
             */

            const string LabelName = "SU_Animation";


            private void OnPreprocessAsset()
            {
                
            }

            private void OnPostprocessPrefab(GameObject gameObject)
            {
                
            }

            private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                
            }

            void OnPreprocessModel() // update your import settings here
            {
                /*
                 * 
                Simplify hierarchy, remove redundant objects
                Simplify geometry, delete back-facing/small/duplicate/hidden triangles
                Generate LODs (eg. Unity’s AutoLOD apparently can do this in a post-processor)
                Unwrap UVs using a custom algorithm (eg. xatlas)
                Generate additional vertex channels for use in shaders (eg. thickness, occlusion)
                 */

                var labels = AssetDatabase.GetLabels(assetImporter);

                if (labels.Contains(LabelName))
                {
                    var modelImporter = assetImporter as ModelImporter;

                    if (modelImporter)
                    {
                        // update model importer settings to exclude unnecessary objects
                        modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;
                        modelImporter.importCameras = false;
                        modelImporter.importLights = false;
                        modelImporter.importConstraints = false;
                        modelImporter.importVisibility = false;
                        modelImporter.importBlendShapes = false;
                        modelImporter.SaveAndReimport();
                    }
                }
            }

            void OnPostprocessModel(GameObject model) // modify the imported asset here
            {
                var labels = AssetDatabase.GetLabels(assetImporter);

                if (labels.Contains(LabelName))
                {
                    // clean up skinned meshes
                    foreach (var skinnedMeshRenderer in model.GetComponentsInChildren<SkinnedMeshRenderer>())
                    {
                        if (skinnedMeshRenderer.sharedMesh is var sharedMesh)
                            if (sharedMesh)
                                Object.DestroyImmediate(sharedMesh);

                        Object.DestroyImmediate(skinnedMeshRenderer);
                    }

                    // clean up meshes from mesh filters
                    foreach (var meshFilter in model.GetComponentsInChildren<MeshFilter>())
                    {
                        if (meshFilter.sharedMesh is var sharedMesh)
                            if (sharedMesh)
                                Object.DestroyImmediate(sharedMesh);

                        Object.DestroyImmediate(meshFilter);
                    }

                    // clean up all child objects in the imported model
                    foreach (var transform in model.GetComponentsInChildren<Transform>().Where(x => x.parent == model.transform))
                        Object.DestroyImmediate(transform.gameObject);
                }
            }

            /*
             * 
            Procedurally generate animation events (eg. for footstep sfx/vfx)
            The post-processing runs after the import pipeline, which means you can read the events you created manually and create new ones based on that. 
            The new events are read-only and you won’t see them in the model importer. To verify that they’re there, you can double click the animation clip to open the animation window, and check the event list at the top.
            Fix/transform/optimize the animation procedurally
            Remove meshes and unnecessary objects from animation-only FBX files (see examples above)
             */
            private void OnPreprocessAnimation()
            {

            }

            private void OnPostprocessAnimation(GameObject root, AnimationClip clip)
            {

            }

            private void OnPreprocessAudio()
            {

            }

            private void OnPostprocessAudio(AudioClip clip)
            {

            }


            /*
             * 
            Apply image effects (eg. brightness/contrast/saturation/etc… but also dithering)
            Compression (eg. ChromaPack https://github.com/keijiro/ChromaPack)
            Automatically setup import settings (eg. based on texture type)
             */
            private void OnPreprocessTexture()
            {
                
            }

            private void OnPostprocessTexture(Texture2D texture)
            {
                
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
