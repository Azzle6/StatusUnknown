using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
  
public class EnemyCharacterEditor : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();

        root.Add(new PropertyField(property.FindPropertyRelative("_PlayerColor")));
        root.Add(new PropertyField(property.FindPropertyRelative("_StartingHealth")));
        var spawnPoint = new PropertyField(property.FindPropertyRelative("_SpawnPoint")); 
        root.Add(spawnPoint);

        var spawnInspector = new Box(); // just a black background for the transform mini editor
        root.Add(spawnInspector);

        spawnPoint.RegisterCallback<ChangeEvent<Object>, VisualElement>(SpawnChanged, spawnInspector); 

        return root;    
    }

    private void SpawnChanged(ChangeEvent<Object> evt, VisualElement spawnInspector)
    {
        spawnInspector.Clear();

        var t = evt.newValue;
        if (t == null) return; 

        spawnInspector.Add(new InspectorElement(t));    
    }
}
