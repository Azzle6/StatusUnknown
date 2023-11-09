namespace Core.UI
{
    using System;
    using global::UI.Global.TabbedMenu;
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIInventory : MonoBehaviour
    {
        private const string INVENTORY_GRID_NAME = "inventoryGrid";
        private const string WEAPON_GRID_NAME = "weaponGrid";
        
        [SerializeField]
        private UIDocument uiDocument;
        [SerializeField]
        private string weaponsTabElementName;
        [SerializeField]
        private string inventoryInterfaceName;

        [SerializeField, BoxGroup("Debug")] 
        private GridDataSO inventoryData;
        [SerializeField, BoxGroup("Debug")] 
        private GridDataSO weaponData;

        private VisualElement inventoryRoot;
        private VisualElement weaponTabRoot;
        private TabbedMenuController weaponTabbedController;

        private GridView inventoryGridView;
        private GridView weaponGridView;

        private bool isDisplayed;

        private void OnEnable()
        {
            this.weaponTabRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(this.weaponsTabElementName);
            this.weaponTabbedController = new(this.weaponTabRoot);
            this.weaponTabbedController.RegisterTabCallbacks();

            this.inventoryRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(inventoryInterfaceName);

            this.inventoryGridView = new GridView(this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_GRID_NAME),
                    this.inventoryData);
            
            this.weaponGridView = new GridView(this.uiDocument.rootVisualElement.Q<VisualElement>(WEAPON_GRID_NAME),
                this.weaponData);
        }

        [Button]
        public void Display()
        {
            this.isDisplayed = !this.isDisplayed;
            this.inventoryRoot.style.display = this.isDisplayed ? DisplayStyle.None : DisplayStyle.Flex;
        }
    }
}
