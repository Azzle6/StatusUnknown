using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatusUnknown.CoreGameplayContent.Editors
{
    [CustomEditor(typeof(CombatSimulatorScriptableObject), true)]
    public class CombatSimulatorScriptableObjectEditor : Editor
    {
        [SerializeField] private VisualTreeAsset VisualTree;
        [SerializeField] AbilityConfigScriptableObjectPropertyDrawer drawer;
        private readonly EnumValueTracker tracker = new EnumValueTracker();

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();
            VisualElement root = VisualTree.Instantiate(); 
            inspector.Add(root);

            var field =  root.Q<EnumField>("basicEnumField");
            field.RegisterValueChangedCallback((e) =>
            {
                tracker.value = (EScriptableType)e.newValue;  
            });

            return inspector;
        }
    }

    public class EnumValueTracker : INotifyValueChanged<EScriptableType>
    {
        public EScriptableType value { get => default; set => SetValueWithoutNotify(value); }

        public static Action<EScriptableType> OnValueChanged_EScriptableType;

        public void SetValueWithoutNotify(EScriptableType newValue)
        {
            if (OnValueChanged_EScriptableType == null) { return; }

            OnValueChanged_EScriptableType(newValue);
        }
    }
}
