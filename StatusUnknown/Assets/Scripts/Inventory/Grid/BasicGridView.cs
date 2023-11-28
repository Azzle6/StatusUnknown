namespace Inventory.Grid
{
    using Containers;
    using Item;
    using UnityEngine.UIElements;

    public class BasicGridView : GridView
    {
        public BasicGridView(VisualElement root, Shape shape, IItemsDataContainer container, E_ItemType[] typesContained) : base(root, shape, container, typesContained)
        {
        }

        protected override void OnNewContainerLoad(IItemsDataContainer newContainer)
        { }
    }
}