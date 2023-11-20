namespace Core.UI
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.Serialization;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/Data/UISettings", fileName = "UISettings")]
    public class UISettings : ScriptableObject
    {
        [FormerlySerializedAs("slotTreeAsset")] [FoldoutGroup("Assets references")]
        public VisualTreeAsset slotTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset itemSquareTemplate;
        [FoldoutGroup("Assets references")]
        public VisualTreeAsset itemTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset triggerTemplate;
        [FoldoutGroup("Assets references")]
        public InputActionAsset inputsAction;

        [FoldoutGroup("Parameters")]
        public float slotWidth;

        [FoldoutGroup("Styles naming")]
        public string hiddenStyle;
    }
}
