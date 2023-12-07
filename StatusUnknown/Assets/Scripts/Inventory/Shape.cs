using System.Collections.Generic;
using Core.Helpers;

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
        public bool[] content = Array.Empty<bool>();

        public Shape(Vector2Int shapeSize, Vector2Int anchor, bool[] content)
        {
            this.shapeSize = shapeSize;
            this.anchor = anchor;
            this.content = content;
        }

        #region UTILITIES
        public bool GetContentFromPosition(Vector2Int position)
        {
            int index = position.y * this.shapeSize.x + position.x;
            return index < this.content.Length && this.content[index];
        }

        public Vector2Int[] GetPositionsRelativeToAnchor()
        {
            List<Vector2Int> result = new List<Vector2Int>();
            for (int i = 0; i < this.content.Length; i++)
            {
                if (this.content[i])
                    result.Add(GridHelper.GetGridPositionFromIndex(i, shapeSize.x) - anchor);
            }
            return result.ToArray();
        }
        #endregion
    }
}
