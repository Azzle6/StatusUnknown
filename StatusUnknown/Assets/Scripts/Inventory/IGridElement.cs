namespace Inventory
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public interface IGridElement
    {
        public GridView Grid { get; set; }
        public Vector2Int GridPosition { get; set; }
        public VisualElement ViewRoot { get; set; }
        public VisualElement FocusElement { get; set; }
    }
}
