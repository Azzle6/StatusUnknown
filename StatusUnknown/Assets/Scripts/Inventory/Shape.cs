namespace Inventory
{
    using UnityEngine;
    using System;
    using Sirenix.OdinInspector;

    [Serializable]
    public class Shape
    {
        [ReadOnly]
        public Vector2Int  shapeSize;
        [ReadOnly]
        public Vector2Int anchor;
        [ReadOnly]
        public bool[] shapeContent;

        public Shape(Vector2Int shapeSize, Vector2Int anchor, bool[] shapeContent)
        {
            this.shapeSize = shapeSize;
            this.anchor = anchor;
            this.shapeContent = shapeContent;
        }

        #region UTILITIES
        public bool GetContentFromPosition(Vector2Int position)
        {
            return this.shapeContent[position.y * this.shapeSize.x + position.x];
        }
        #endregion
    }
}
