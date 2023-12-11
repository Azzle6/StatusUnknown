using Core.EventsSO.GameEventsTypes;

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
        //Grids
        private const string INVENTORY_GRID_NAME = "inventoryGrid";
        private const string WEAPON_GRID_NAME = "weaponTriggerGrid";
        //Buttons
        private const string WEAPON_TRIGGERS_BUTTONS_NAME = "weaponTriggerSelectionButtons";
        private const string WEAPON_SELECTION_BUTTONS_NAME = "weaponSelectionButtons";
        private const string WEAPON_INFO_BUTTON_NAME = "weaponInfoButton";
        //Context elements
        private const string WEAPON_INFO_PANEL_NAME = "weaponInfoPanel";
        private const string TRIGGER_INFO_PANEL_NAME = "triggerInfoPanel";
        
        [SerializeField]
        private UIDocument uiDocument;
        [SerializeField]
        private PlayerInventorySO playerInventory;

        //Data
        private VisualElement inventoryRoot;
        private VisualElement weaponSelectionButtonsRoot;
        private VisualElement triggersButtonsRoot;
        private VisualElement weaponInfoPanel;
        private VisualElement triggerInfoPanel;

        private GridView inventoryGridView;
        private TriggerGridView triggerGridView;

        private bool isDisplayed;

        private WeaponData selectedWeaponData;
        
        [SerializeField] private BoolGameEvent displayPlayerInfoEvent;

        private void OnEnable()
        {
            this.weaponSelectionButtonsRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_SELECTION_BUTTONS_NAME);
            this.triggersButtonsRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_TRIGGERS_BUTTONS_NAME);
            this.inventoryRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_NAME);
            this.weaponInfoPanel = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_INFO_PANEL_NAME);
            this.triggerInfoPanel = this.uiDocument.rootVisualElement.Q<VisualElement>(TRIGGER_INFO_PANEL_NAME);
            
            this.RefreshWeaponsList();
            this.InitInventoryView();
            this.InitWeaponGridView();
            
            this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_INFO_BUTTON_NAME).RegisterCallback<NavigationSubmitEvent>((e) => DisplayWeaponInfo(true));

            this.inventoryRoot.style.display = DisplayStyle.None;
        }

        private void RefreshWeaponsList()
        {
            this.weaponSelectionButtonsRoot.Clear();
            foreach (WeaponData weapon in this.playerInventory.equippedWeaponsData)
            {
                VisualElement weaponSelectionButton =
                    UIHandler.Instance.uiSettings.weaponSelectionButtonTemplate.Instantiate();
                weaponSelectionButton.RegisterCallback<NavigationSubmitEvent>((e) => this.SelectWeapon(weapon));
                weaponSelectionButton.Q<VisualElement>("weaponIcon").style.backgroundImage =
                    weapon.definition.icon.texture;
                weaponSelectionButton.Q<TextElement>("weaponName").text = weapon.definition.itemName;
                
                this.weaponSelectionButtonsRoot.Add(weaponSelectionButton);
            }
        }

        private void InitInventoryView()
        {
            this.inventoryGridView = new BasicGridView(this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_GRID_NAME),
                this.playerInventory.inventory.Shape, this.playerInventory.inventory, new E_ItemType[] { E_ItemType.MODULE, E_ItemType.WEAPON});
        }

        private void InitWeaponGridView()
        {
            this.triggerGridView = new TriggerGridView(this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_GRID_NAME),
                this.playerInventory.equippedWeaponsData[0].definition.triggers[0].shape, this.playerInventory.equippedWeaponsData[0].triggerInfoData[0], new E_ItemType[] { E_ItemType.MODULE});
        }

        private void SelectWeapon(WeaponData weaponData)
        {
            this.selectedWeaponData = weaponData;
            this.RefreshWeaponTriggers();
            this.SelectTriggerIndex(0, false);
        }

        private void DisplayWeaponInfo(bool display)
        {
            this.weaponInfoPanel.style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
            this.triggerInfoPanel.style.display = display ? DisplayStyle.None : DisplayStyle.Flex;
        }

        private void RefreshWeaponTriggers()
        {
            this.triggersButtonsRoot.Clear();
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
                
                this.triggersButtonsRoot.Add(triggerSelectionButton);
            }
        }

        private void SelectTriggerIndex(int index, bool forceDisplay = true)
        {
            if (this.selectedWeaponData.triggerInfoData.Length == 0 || this.selectedWeaponData.triggerInfoData.Length - 1 < index)
            {
                Debug.Log($"Cannot select trigger index {index}, out of range.");
                return;
            }
            
            if(forceDisplay)
                DisplayWeaponInfo(false);
            
            this.triggerGridView.LoadNewData(this.selectedWeaponData.definition.triggers[index].shape, this.selectedWeaponData.triggerInfoData[index]);
        }

        [Button, HideInEditorMode]
        public void Display(bool display)
        {
            this.isDisplayed = display;

            //Debug.Log($"{(this.isDisplayed ? "Display" : "Hide")} inventory.");
            this.inventoryRoot.style.display = this.isDisplayed ? DisplayStyle.Flex : DisplayStyle.None;
            if (this.isDisplayed)
            {
                this.inventoryGridView.FocusOnGrid();
                this.inventoryGridView.LoadContent();
                this.SelectWeapon(this.playerInventory.equippedWeaponsData[0]);
            }
        }

        public bool IsOpen()
        {
            return this.inventoryRoot.style.display == DisplayStyle.Flex;
        }
    }
}
