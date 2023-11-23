using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatusUnknown.Content.Editors
{
    [CustomEditor(typeof(ScriptableObjectManager), true)]
    public class ScriptableObjectManagerEditor : Editor
    {
        [SerializeField] private VisualTreeAsset VisualTree;
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
