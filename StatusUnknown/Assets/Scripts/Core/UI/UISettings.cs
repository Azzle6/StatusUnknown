namespace Core.UI
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/Data/UISettings", fileName = "UISettings")]
    public class UISettings : ScriptableObject
    {
        [FoldoutGroup("Assets references")]
        public VisualTreeAsset slotTreeAsset;
        [FoldoutGroup("Assets references")]
        public VisualTreeAsset itemTemplate;
        [FoldoutGroup("Assets references")]
        public InputActionAsset inputsAction;

        [FoldoutGroup("Parameters")]
        public float slotWidth;
    }
}
