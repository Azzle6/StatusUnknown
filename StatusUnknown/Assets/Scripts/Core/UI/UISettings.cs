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
        [FoldoutGroup("Assets references")]
        public VisualTreeAsset slotTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset itemSquareTemplate;
        [FoldoutGroup("Assets references")]
        public VisualTreeAsset itemTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset triggerTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset weaponSelectionButtonTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset triggerSelectionButtonTemplate;
        [FoldoutGroup("Assets references")]
        public InputActionAsset inputsAction;
        [FoldoutGroup("Assets references")] 
        public Texture basicTriggerIcon;

        [FoldoutGroup("Parameters")]
        public float slotWidth;
        [FoldoutGroup("Parameters")]
        public float triggerWidth;

        [FoldoutGroup("Styles naming")]
        public string hiddenStyle;
    }
}
