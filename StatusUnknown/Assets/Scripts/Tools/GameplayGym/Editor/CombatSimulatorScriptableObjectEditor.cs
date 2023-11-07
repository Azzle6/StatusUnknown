using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatusUnknown.CoreGameplayContent.Editors
{
    [CustomEditor(typeof(CombatSimulatorScriptableObject), true)]
    public class CombatSimulatorScriptableObjectEditor : Editor
    {
        [SerializeField] private VisualTreeAsset VisualTree;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new();
            inspector.Add(VisualTree.Instantiate());

            return inspector;
        }
    }
}
