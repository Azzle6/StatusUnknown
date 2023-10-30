namespace Core.SingletonsSO
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/SingletonSO/UISettings", fileName = "UISettings")]
    public class UISettings : SingletonSO<UISettings>
    {
        public VisualTreeAsset slotTreeAsset;
        public VisualTreeAsset itemTemplate;
        public InputActionAsset inputsAction;
    }
}
