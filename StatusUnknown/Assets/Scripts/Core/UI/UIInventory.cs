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

        private Weapon selectedWeapon;

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
            foreach (Weapon weapon in this.playerInventory.equippedWeaponsData)
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
                this.playerInventory.inventory.Shape, this.playerInventory.inventory.content);
        }

        private void InitWeaponGridView()
        {
            this.weaponGridView = new GridView(this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_GRID_NAME),
                this.playerInventory.equippedWeaponsData[0].definition.triggers[0].shape, this.playerInventory.equippedWeaponsData[0].triggerInfoData[0].content);
            
            this.SelectWeapon(this.playerInventory.equippedWeaponsData[0]);
        }

        private void SelectWeapon(Weapon weapon)
        {
            this.selectedWeapon = weapon;
            this.RefreshWeaponTriggers();
            this.SelectTriggerIndex(0);
        }

        private void RefreshWeaponTriggers()
        {
            this.weaponTriggersRoot.Clear();
            for (var i = 0; i < this.selectedWeapon.triggerInfoData.Length; i++)
            {
                int index = i;
                Button triggerButton = new Button(() => this.SelectTriggerIndex(index))
                {
                    text = this.selectedWeapon.triggerInfoData[i].triggerType.name
                };
                this.weaponTriggersRoot.Add(triggerButton);
            }
        }

        private void SelectTriggerIndex(int index)
        {
            if (this.selectedWeapon.triggerInfoData.Length == 0 || this.selectedWeapon.triggerInfoData.Length - 1 < index)
            {
                Debug.Log($"Cannot select trigger index {index}, out of range.");
                return;
            }
            
            this.weaponGridView.LoadNewData(this.selectedWeapon.definition.triggers[index].shape, this.selectedWeapon.triggerInfoData[index].content);
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
