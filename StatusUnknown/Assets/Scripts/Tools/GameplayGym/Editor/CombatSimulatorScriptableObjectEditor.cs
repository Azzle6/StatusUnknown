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
        private EnumValueTracker tracker = new EnumValueTracker();

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();
            VisualElement root = VisualTree.Instantiate(); 
            inspector.Add(root);

            var field =  root.Q<EnumField>("basicEnumField");
            field.RegisterValueChangedCallback((e) =>
            {
                tracker.value = (EAbilityType)e.newValue;  
            });

            return inspector;
        }
    }

    public class EnumValueTracker : INotifyValueChanged<EAbilityType>
    {
        public EAbilityType value { get => default; set => SetValueWithoutNotify(value); }

        public static Action<EAbilityType> OnValueChanged;

        public void SetValueWithoutNotify(EAbilityType newValue)
        {
            OnValueChanged(newValue);
        }
    }
}
