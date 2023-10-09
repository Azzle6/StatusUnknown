namespace Inventory
{
    using UnityEngine;
    using System;
    
    [Serializable]
    public class Shape
    {
        public Vector2Int  shapeSize;
        public Vector2Int anchor;
        public bool[] shapeContent;

        public Shape(Vector2Int shapeSize, Vector2Int anchor, bool[] shapeContent)
        {
            this.shapeSize = shapeSize;
            this.anchor = anchor;
            this.shapeContent = shapeContent;
        }
    }
}
