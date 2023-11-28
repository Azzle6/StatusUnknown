using Sirenix.OdinInspector.Editor;
using System;
using UnityEngine;

public struct SU_GroupDrawers
{
    public const string POSITIONS = "positions";
    public const string VALUES = "values"; 
}

public class EditorCallbackTracker : OdinEditor
{
    // override void  
}

[Serializable]
public class SomeData : MonoBehaviour
{
    [ColorFoldoutGroup(SU_GroupDrawers.POSITIONS, 0f, 1f, 0f, 1f)] public string top;
    [ColorFoldoutGroup(SU_GroupDrawers.POSITIONS)] public string middle;
    [ColorFoldoutGroup(SU_GroupDrawers.POSITIONS)] public string bottom;

    [ColorFoldoutGroup(SU_GroupDrawers.VALUES, 1f, 0f, 0f, 1f)] public int first = 1;
    [ColorFoldoutGroup(SU_GroupDrawers.VALUES)] public int second = 2;
    [ColorFoldoutGroup(SU_GroupDrawers.VALUES)] public int third = 3;
}
