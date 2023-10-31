namespace Core.SingletonsSO
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/Data/UISettings", fileName = "UISettings")]
    public class UISettings : ScriptableObject
    {
        public VisualTreeAsset slotTreeAsset;
        public VisualTreeAsset itemTemplate;
        public InputActionAsset inputsAction;
    }
}
