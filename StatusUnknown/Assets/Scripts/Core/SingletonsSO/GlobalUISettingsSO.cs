namespace Core.SingletonsSO
{
    using UnityEngine;
    using UnityEngine.UIElements;

    [CreateAssetMenu(menuName = "CustomAssets/UISettingsSO", fileName = "UISettingsSO")]
    public class GlobalUISettingsSO : SingletonSO<GlobalUISettingsSO>
    {
        public VisualTreeAsset baseGridTreeAsset;
        public VisualTreeAsset slotTemplate;

        public string inventoryUIName, weaponUIName;
    }
}
