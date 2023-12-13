namespace Inventory
{
    using System;
    using Core;
    using Core.SingletonsSO;
    using Core.UI;
    using Grid;
    using Item;
    using Module;
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
        private const string ITEM_INFO_PANEL_NAME = "selectedItemInfoZone";
        
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
        private VisualElement itemInfoPanel;

        private GridView inventoryGridView;
        private TriggerGridView triggerGridView;

        private bool isDisplayed;

        private WeaponData selectedWeaponData;
        private UISettings uiSettings;
        private OutputReferencesSO outputReferences;

        private void OnEnable()
        {
            this.weaponSelectionButtonsRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_SELECTION_BUTTONS_NAME);
            this.triggersButtonsRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_TRIGGERS_BUTTONS_NAME);
            this.inventoryRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_NAME);
            this.weaponInfoPanel = this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_INFO_PANEL_NAME);
            this.triggerInfoPanel = this.uiDocument.rootVisualElement.Q<VisualElement>(TRIGGER_INFO_PANEL_NAME);
            this.itemInfoPanel = this.uiDocument.rootVisualElement.Q<VisualElement>(ITEM_INFO_PANEL_NAME);

            this.uiSettings = UIHandler.Instance.uiSettings;
            this.outputReferences = UIHandler.Instance.outputReferences;

            UIHandler.Instance.onSlotFocusEvent += this.OnSlotFocus;
            
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
                    uiSettings.weaponSelectionButtonTemplate.Instantiate();
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
                this.playerInventory.InventoryData.Shape, this.playerInventory.InventoryData, new E_ItemType[] { E_ItemType.MODULE, E_ItemType.WEAPON});
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
                    uiSettings.triggerSelectionButtonTemplate.Instantiate();
                triggerSelectionButton.RegisterCallback<NavigationSubmitEvent>((e) => this.SelectTriggerIndex(index));
                triggerSelectionButton.Q<VisualElement>("triggerIcon").style.backgroundImage =
                    outputReferences.weaponOutputReferences[this.selectedWeaponData.triggerInfoData[i].weaponTriggerType].icon.texture;
                triggerSelectionButton.Q<TextElement>("triggerName").text = this.selectedWeaponData.triggerInfoData[i].weaponTriggerType.ToString().Replace("_", " ");
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

        private void OnSlotFocus(Slot slot)
        {
            this.OnHoverItem(slot.ItemView);
        }
        
        private void OnHoverItem(ItemView itemView)
        {
            VisualElement itemOutputDescriptionParent = this.itemInfoPanel.Q<VisualElement>("itemOutputInfoZone");
            itemOutputDescriptionParent.Clear();
            if (itemView == null)
            {
                SetName("");
                SetDescription("");
                return;
            }
            
            ItemData data = itemView.ItemData;
            
            switch (data.GridItemDefinition.ItemType)
            {
                case E_ItemType.MODULE:
                    ModuleData modData = (ModuleData)data;
                    SetName(modData.definition.itemName);
                    SetDescription(modData.definition.description);
                    foreach (var modDef in modData.definition.outputs)
                    {
                        ModuleOutputDefinition def =
                            this.outputReferences.moduleOutputReferences[modDef.moduleTriggerType];
                        AddOutputList(def.icon, def.description, def.output.ToString());
                    }
                    break;
                case E_ItemType.WEAPON:
                    WeaponData weaponData = (WeaponData)data;
                    SetName(weaponData.definition.itemName);
                    SetDescription(weaponData.definition.description);
                    foreach (var weaponTrigger in weaponData.definition.triggers)
                    {
                        WeaponOutputDefinition def =
                            this.outputReferences.weaponOutputReferences[weaponTrigger.weaponTrigger];
                        AddOutputList(def.icon, def.description, def.output.ToString());
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return;

            void SetName(string itemName)
            {
                this.itemInfoPanel.Q<Label>("itemInfoName").text = itemName;
            }
            
            void SetDescription(string description)
            {
                this.itemInfoPanel.Q<Label>("itemInfoDescription").text = description;
            }

            void AddOutputList(Sprite icon, string description, string outputName)
            {
                VisualElement outputDesc = UIHandler.Instance.uiSettings.outputDescriptionTemplate.Instantiate();
                outputDesc.Q<VisualElement>("outputIcon").style.backgroundImage = icon.texture;
                outputDesc.Q<Label>("outputDescription").text = description;
                outputDesc.Q<Label>("outputName").text = outputName;
                itemOutputDescriptionParent.Add(outputDesc);
            }
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

        private void OnDisable()
        {
            UIHandler.Instance.onSlotFocusEvent -= this.OnSlotFocus;
        }
    }
}
