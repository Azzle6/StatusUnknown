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
        public VisualTreeAsset itemIconTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset triggerTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset weaponSelectionButtonTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset triggerSelectionButtonTemplate;
        [FoldoutGroup("Assets references")] 
        public VisualTreeAsset itemEdgesHideTemplate;
        
        [FoldoutGroup("Parameters")]
        public float slotSize;
        [FoldoutGroup("Parameters")]
        public float triggerSize;

        [FoldoutGroup("Colours")]
        public Color linkedTriggerBackgroundColor;
        [FoldoutGroup("Colours")]
        public Color linkedTriggerIconColor;
        [FoldoutGroup("Colours")]
        public Color unlinkedTriggerBackgroundColor;
        [FoldoutGroup("Colours")]
        public Color unlinkedTriggerIconColor;

        [FoldoutGroup("Styles naming")]
        public string hiddenStyle;
    }
}
