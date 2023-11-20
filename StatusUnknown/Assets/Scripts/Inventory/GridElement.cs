namespace Inventory
{
    using System;
    using UnityEngine;
    using UnityEngine.UIElements;

    [Serializable]
    public class GridElement
    {
        public GridView grid;
        public Vector2Int gridPosition;
        public VisualElement viewRoot;
        public VisualElement focusElement;
    }
}
