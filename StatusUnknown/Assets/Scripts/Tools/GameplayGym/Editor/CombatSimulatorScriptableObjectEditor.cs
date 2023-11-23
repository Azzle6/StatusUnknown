using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


namespace StatusUnknown.Content.Editors
{
    // [CustomEditor(typeof(CombatSimulatorScriptableObject), true)]
    public class CombatSimulatorScriptableObjectEditor : Editor
    {
        [SerializeField] private VisualTreeAsset VisualTree;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();
            var root = VisualTree.Instantiate(); 
            inspector.Add(root);

            return inspector;
        }
    }
}

