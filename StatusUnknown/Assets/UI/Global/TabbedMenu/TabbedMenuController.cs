namespace UI.Global.TabbedMenu
{
    using UnityEngine.UIElements;

    public class TabbedMenuController
    {
        private const string TAB_CLASS_NAME = "tab";
        private const string CURRENTLY_SELECTED_TAB_CLASS_NAME = "currentlySelectedTab";
        private const string UNSELECTED_CONTENT_CLASS_NAME = "unselectedContent";
        private const string TAB_NAME_SUFFIX = "Tab";
        private const string CONTENT_NAME_SUFFIX = "Content";

        private readonly VisualElement root;

        public TabbedMenuController(VisualElement root)
        {
            this.root = root;
        }

        public void RegisterTabCallbacks()
        {
            UQueryBuilder<Label> tabs = GetAllTabs();
            tabs.ForEach((Label tab) => { tab.RegisterCallback<NavigationSubmitEvent>(this.TabOnClick); });
        }

        private void TabOnClick(NavigationSubmitEvent e)
        {
            Label clickedTab = e.target as Label;
            if (!TabIsCurrentlySelected(clickedTab))
            {
                this.GetAllTabs().Where(
                    (tab) => tab != clickedTab && TabIsCurrentlySelected(tab)).ForEach(this.UnselectTab);
                this.SelectTab(clickedTab);
            }
        }

        private UQueryBuilder<Label> GetAllTabs()
        {
            return this.root.Query<Label>(className: TAB_CLASS_NAME);
        }
        
        private static bool TabIsCurrentlySelected(Label tab)
        {
            return tab.ClassListContains(CURRENTLY_SELECTED_TAB_CLASS_NAME);
        }
        
        private void SelectTab(Label tab)
        {
            tab.AddToClassList(CURRENTLY_SELECTED_TAB_CLASS_NAME);
            VisualElement content = this.FindContent(tab);
            content.RemoveFromClassList(UNSELECTED_CONTENT_CLASS_NAME);
        }
        
        private void UnselectTab(Label tab)
        {
            tab.RemoveFromClassList(CURRENTLY_SELECTED_TAB_CLASS_NAME);
            VisualElement content = FindContent(tab);
            content.AddToClassList(UNSELECTED_CONTENT_CLASS_NAME);
        }
        
        private VisualElement FindContent(Label tab)
        {
            return this.root.Q(GenerateContentName(tab));
        }
        
        private static string GenerateContentName(Label tab) =>
            tab.name.Replace(TAB_NAME_SUFFIX, CONTENT_NAME_SUFFIX);
    }
}
