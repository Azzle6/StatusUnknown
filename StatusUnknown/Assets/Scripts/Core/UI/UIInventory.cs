namespace Core.UI
{
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UIElements;
    using Weapons;

    public class UIInventory : MonoBehaviour
    {
        private const string INVENTORY_NAME = "InventoryInterface";
        private const string INVENTORY_GRID_NAME = "inventoryGrid";
        private const string WEAPON_GRID_NAME = "weaponGrid";
        private const string WEAPON_TRIGGERS_BUTTONS_NAME = "weaponTriggerSelectionButtons";
        private const string WEAPON_SELECTION_BUTTONS_NAME = "weaponSelectionButtons";
        
        [SerializeField]
        private UIDocument uiDocument;

        [SerializeField, BoxGroup("Debug")] 
        private PlayerInventorySO playerInventory;

        //Data
        private VisualElement inventoryRoot;
        private VisualElement weaponSelectionRoot;
        private VisualElement weaponTriggersRoot;

        private GridView inventoryGridView;
        private GridView weaponGridView;

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
                Button tabButton = new Button(() => this.SelectWeapon(weapon))
                {
                    text = weapon.definition.itemName
                };
                this.weaponSelectionRoot.Add(tabButton);
            }
        }

        private void InitInventoryView()
        {
            this.inventoryGridView = new GridView(this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_GRID_NAME),
                this.playerInventory.inventory.Shape, this.playerInventory.inventory);
        }

        private void InitWeaponGridView()
        {
            this.weaponGridView = new GridView(this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_GRID_NAME),
                this.playerInventory.equippedWeaponsData[0].definition.triggers[0].shape, this.playerInventory.equippedWeaponsData[0].triggerInfoData[0]);
            
            this.SelectWeapon(this.playerInventory.equippedWeaponsData[0]);
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
                Button triggerButton = new Button(() => this.SelectTriggerIndex(index))
                {
                    text = this.selectedWeaponData.triggerInfoData[i].triggerType.name
                };
                this.weaponTriggersRoot.Add(triggerButton);
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
                this.weaponGridView.LoadContent();
            }
        }
    }
}
