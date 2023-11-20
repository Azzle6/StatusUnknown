namespace Inventory.Containers
{
    using Core.Helpers;
    using Item;

    public interface IItemsDataContainer
    {
        public VectorIntItemDictionary GetAllItems();

        public void SaveAllItems(VectorIntItemDictionary content);

        public void ClearData();
    }
}
