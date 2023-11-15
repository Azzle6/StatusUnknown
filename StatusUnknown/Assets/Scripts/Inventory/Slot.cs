using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    using Item;

    [Serializable]
    public class Slot : IGridElement
    {
        public GridView Grid { get; set; }
        public Vector2Int GridPosition { get; set; }
        public VisualElement ViewRoot { get; set; }
        public VisualElement FocusElement { get; set; }
        
        public ItemView item;

        #region CONSTRUCTOR
        public Slot(Vector2Int pos, VisualElement visual, GridView gridView)
        {
            this.GridPosition = pos;
            this.ViewRoot = visual;
            this.FocusElement = visual.Q<VisualElement>("gridSlot");
            this.Grid = gridView;
        }
        #endregion //CONSTRUCTOR

        public void SetOccupied(ItemView itemRef)
        {
            bool isOccupied = itemRef != null;
            
            if(isOccupied)
                this.FocusElement.AddToClassList("usedSlot");
            else
                this.FocusElement.RemoveFromClassList("usedSlot");

            this.item = itemRef;
            this.FocusElement.focusable = !isOccupied;
        }

        
    }
}
