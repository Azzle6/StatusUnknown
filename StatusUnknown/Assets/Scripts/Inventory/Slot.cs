using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory
{
    [Serializable]
    public class Slot : GridElement
    {
        public bool isUsed;

        #region CONSTRUCTOR
        public Slot(Vector2Int pos, VisualElement visual, GridView gridView)
        {
            this.gridPosition = pos;
            this.view = visual;
            this.grid = gridView;
        }
        #endregion //CONSTRUCTOR

        public void SetOccupied(bool isOccupied)
        {
            if(isOccupied)
                this.view.AddToClassList("usedSlot");
            else
                this.view.RemoveFromClassList("usedSlot");

            this.isUsed = isOccupied;
            this.view.focusable = !isOccupied;
        }
        
    }
}
