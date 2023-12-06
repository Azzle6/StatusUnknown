namespace Inventory
{
    using Core.SingletonsSO;
    using Grid;
    using Item;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Weapons;

    public class UIInventoryDisplayer : MonoBehaviour
    {
        private const string INVENTORY_NAME = "InventoryInterface";
        private const string INVENTORY_GRID_NAME = "inventoryGrid";
        private const string WEAPON_GRID_NAME = "weaponTriggerGrid";
        private const string WEAPON_TRIGGERS_BUTTONS_NAME = "weaponTriggerSelectionButtons";
        private const string WEAPON_SELECTION_BUTTONS_NAME = "weaponSelectionButtons";
        
        [SerializeField]
        private UIDocument uiDocument;
        [SerializeField]
        private PlayerInventorySO playerInventory;

        //Data
        private VisualElement inventoryRoot;
        private VisualElement weaponSelectionRoot;
        private VisualElement weaponTriggersRoot;

        private GridView inventoryGridView;
        private WeaponGridView weaponGridView;

        private bool isDisplayed;

        private WeaponData selectedWeaponData;

        private void OnEnable()
        {
            this.weaponSelectionRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_SELECTION_BUTTONS_NAME);
            this.weaponTriggersRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_TRIGGERS_BUTTONS_NAME);
            this.inventoryRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_NAME);
            
            this.weaponSelectionRoot.Clear();
            
            this.RefreshWeapons();
            this.InitInventoryView();
            this.InitWeaponGridView();

            this.inventoryRoot.style.display = DisplayStyle.None;
        }

        private void RefreshWeapons()
        {
            this.weaponSelectionRoot.Clear();
            foreach (WeaponData weapon in this.playerInventory.equippedWeaponsData)
            {
                VisualElement weaponSelectionButton =
                    UIHandler.Instance.uiSettings.weaponSelectionButtonTemplate.Instantiate();
                weaponSelectionButton.RegisterCallback<NavigationSubmitEvent>((e) => this.SelectWeapon(weapon));
                weaponSelectionButton.Q<VisualElement>("weaponIcon").style.backgroundImage =
                    weapon.definition.view.texture;
                weaponSelectionButton.Q<TextElement>("weaponName").text = weapon.definition.itemName;
                
                this.weaponSelectionRoot.Add(weaponSelectionButton);
            }
        }

        private void InitInventoryView()
        {
            this.inventoryGridView = new BasicGridView(this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_GRID_NAME),
                this.playerInventory.inventory.Shape, this.playerInventory.inventory, new E_ItemType[] { E_ItemType.MODULE, E_ItemType.WEAPON});
        }

        private void InitWeaponGridView()
        {
            this.weaponGridView = new WeaponGridView(this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_GRID_NAME),
                this.playerInventory.equippedWeaponsData[0].definition.triggers[0].shape, this.playerInventory.equippedWeaponsData[0].triggerInfoData[0], new E_ItemType[] { E_ItemType.MODULE});
        }

        private void SelectWeapon(WeaponData weaponData)
        {
            this.selectedWeaponData = weaponData;
            this.RefreshWeaponTriggers();
            this.SelectTriggerIndex(0);
        }

        private void RefreshWeaponTriggers()
        {
            this.weaponTriggersRoot.Clear();
            for (var i = 0; i < this.selectedWeaponData.triggerInfoData.Length; i++)
            {
                int index = i;
                VisualElement triggerSelectionButton =
                    UIHandler.Instance.uiSettings.triggerSelectionButtonTemplate.Instantiate();
                triggerSelectionButton.RegisterCallback<NavigationSubmitEvent>((e) => this.SelectTriggerIndex(index));
                triggerSelectionButton.Q<VisualElement>("triggerIcon").style.backgroundImage =
                    UIHandler.Instance.iconsReferences.weaponOutputReferences[this.selectedWeaponData.triggerInfoData[i].weaponTriggerType].texture;
                triggerSelectionButton.Q<TextElement>("triggerName").text = this.selectedWeaponData.triggerInfoData[i].weaponTriggerType.ToString();
                triggerSelectionButton.Q<TextElement>("triggerIndex").text = $"T{i + 1}";
                
                this.weaponTriggersRoot.Add(triggerSelectionButton);
            }
        }

        private void SelectTriggerIndex(int index)
        {
            if (this.selectedWeaponData.triggerInfoData.Length == 0 || this.selectedWeaponData.triggerInfoData.Length - 1 < index)
            {
                Debug.Log($"Cannot select trigger index {index}, out of range.");
                return;
            }
            
            this.weaponGridView.LoadNewData(this.selectedWeaponData.definition.triggers[index].shape, this.selectedWeaponData.triggerInfoData[index]);
        }

        [Button, HideInEditorMode]
        public void Display()
        {
            this.isDisplayed = !this.isDisplayed;
            
            Debug.Log($"{(this.isDisplayed ? "Display" : "Hide")} inventory.");
            
            this.inventoryRoot.style.display = this.isDisplayed ? DisplayStyle.Flex : DisplayStyle.None;
            if (this.isDisplayed)
            {
                this.inventoryGridView.FocusOnGrid();
                this.inventoryGridView.LoadContent();
                this.SelectWeapon(this.playerInventory.equippedWeaponsData[0]);
            }
        }
    }
}
