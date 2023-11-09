namespace Core.UI
{
    using Inventory;
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIInventory : MonoBehaviour
    {
        private const string INVENTORY_GRID_NAME = "inventoryGrid";
        private const string FIRST_WEAPON_GRID_NAME = "weaponGrid";
        
        [SerializeField]
        private UIDocument uiDocument;
        [SerializeField]
        private string weaponsTabElementName;
        [SerializeField]
        private string inventoryInterfaceName;

        [SerializeField, BoxGroup("Debug")] 
        private GridDataSO inventoryData;
        [SerializeField, BoxGroup("Debug")] 
        private GridDataSO[] weaponsData;

        private VisualElement inventoryRoot;
        private VisualElement weaponTabRoot;

        private GridView inventoryGridView;
        private GridView weaponGridView;

        private bool isDisplayed;

        private void OnEnable()
        {
            this.weaponTabRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(this.weaponsTabElementName);
            this.inventoryRoot = this.uiDocument.rootVisualElement.Q<VisualElement>(inventoryInterfaceName);
            
            this.RefreshWeaponsTriggerTabs();

            this.InitGridViews();

            this.inventoryRoot.style.display = DisplayStyle.None;
        }

        private void InitGridViews()
        {
            this.inventoryGridView = new GridView(this.uiDocument.rootVisualElement.Q<VisualElement>(INVENTORY_GRID_NAME),
                this.inventoryData);
            this.weaponGridView = new GridView(this.uiDocument.rootVisualElement.Q<VisualElement>(FIRST_WEAPON_GRID_NAME),
                this.weaponsData[0]);
        }

        private void RefreshWeaponsTriggerTabs()
        {
            this.weaponTabRoot.Clear();
            foreach (GridDataSO weapon in this.weaponsData)
            {
                Button tabButton = new Button(() => SwitchWeaponGrid(weapon))
                {
                    text = weapon.name
                };
                this.weaponTabRoot.Add(tabButton);
            }
        }

        private void SwitchWeaponGrid(GridDataSO newGrid)
        {
            this.weaponGridView.LoadNewContent(newGrid);
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
