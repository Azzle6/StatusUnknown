using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;

//[CustomEditor(typeof(EnemyEncounterConfigScriptableObject))]
public class EnemyEncounterEditor : Editor
{
    public VisualTreeAsset _UXML; // goes through all the tags and instantiate them

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        _UXML.CloneTree(root);

        var foldout = new Foldout() { viewDataKey = "EnemyEncounterFullInspectorFoldout", text = "Full Inspector" };
        InspectorElement.FillDefaultInspector(foldout, serializedObject, this);
        root.Add(foldout);
        return root;
    }
}
