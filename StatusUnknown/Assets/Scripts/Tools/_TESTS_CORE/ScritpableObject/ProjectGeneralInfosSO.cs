using Sirenix.OdinInspector;
using StatusUnknown.Tools;
using StatusUnknown.Content; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "General Infos", menuName = CoreContentStrings.PATH_ROOT_CONTENT + "General Infos", order = -20)]
public class ProjectGeneralInfosSO : SerializedScriptableObject
{
    public enum InfoType { Gameplay, Systems, Combat, Narrative }

    [SerializeField]
    public Dictionary<float, NoteContainer> generalInfos = new Dictionary<float, NoteContainer>();

    [Title("Add New Notes")]
    [SerializeField, OnValueChanged("LoadInfos")] private InfoType infoType;

    [GUIColor(CoreToolsStrings.COLOR_QOL)]
    [ButtonGroup("Buttons")] private void AddInfos() { }

    [GUIColor(CoreToolsStrings.COLOR_DANGER)]
    [ButtonGroup("Buttons")] private void RemoveInfos() { } 

    private void LoadInfos() { }    

    public ReadOnlyCollection<NoteContainer> GetAllNotes(bool newestFirst = true) { return default; }  

    [SerializeField]
    public class NoteContainer { }
}