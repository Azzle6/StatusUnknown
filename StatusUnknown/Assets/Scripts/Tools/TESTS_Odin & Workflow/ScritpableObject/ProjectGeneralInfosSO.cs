using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "General Infos", menuName = "Status Unknown/General Infos", order = 1)]
public class ProjectGeneralInfosSO : SerializedScriptableObject
{
    public enum InfoType { Gameplay, Systems, Combat, Narrative }

    [SerializeField]
    public Dictionary<float, NoteContainer> generalInfos = new Dictionary<float, NoteContainer>();

    [Title("Add New Notes")]
    [SerializeField, OnValueChanged("LoadInfos")] private InfoType infoType;

    [GUIColor("green")]
    [ButtonGroup("Buttons")] private void AddInfos() { }

    [GUIColor("red")]
    [ButtonGroup("Buttons")] private void RemoveInfos() { } 

    private void LoadInfos() { }    

    public ReadOnlyCollection<NoteContainer> GetAllNotes(bool newestFirst = true) { return default; }  

    [SerializeField]
    public class NoteContainer { }
}