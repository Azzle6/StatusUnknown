using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    [Serializable]
    public class Slot : GridElement
    {
        public ItemView item;

        #region CONSTRUCTOR
        public Slot(Vector2Int pos, VisualElement visual, GridView gridView)
        {
            this.gridPosition = pos;
            this.viewRoot = visual;
            this.focusElement = visual.Q<VisualElement>("gridSlot");
            this.grid = gridView;
        }
        #endregion //CONSTRUCTOR

        public void SetOccupied(ItemView itemRef)
        {
            bool isOccupied = itemRef != null;
            
            if(isOccupied)
                this.focusElement.AddToClassList("usedSlot");
            else
                this.focusElement.RemoveFromClassList("usedSlot");

            this.item = itemRef;
            this.focusElement.focusable = !isOccupied;
        }
    }
}
