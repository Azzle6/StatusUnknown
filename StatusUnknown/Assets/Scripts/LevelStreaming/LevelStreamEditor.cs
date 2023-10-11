using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class LevelStreamEditor 
{
    [MenuItem("GameObject/GenerateStreamScene")]
    static void GenerateStreamScene()
    {
        Debug.Log("GenerateStreamScene");
    }
    [MenuItem("GameObject/GenerateStreamScene", true)]
    static bool CheckStreamScene()
    {
        return Selection.activeGameObject != null;
    }


    [MenuItem("Assets/GenerateLevelStreamVolume")]
    static void GenerateLevelStreamVolume()
    {
        Debug.Log("Generate LevelStreamVolume");
    }

    [MenuItem("Assets/GenerateLevelStreamVolume",true)]
    static bool CheckLevelStreamVolume()
    {
       return Selection.activeObject.GetType() == typeof(SceneAsset);
    }
}
