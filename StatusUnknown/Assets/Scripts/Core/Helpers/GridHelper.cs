namespace Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using Inventory;
    using UnityEngine;
    public static class GridHelper
    {
        public static Vector2Int GetGridPositionFromIndex(int index, int width)
        {
            return new Vector2Int(index % width, index / width);
        }

        public static int GetIndexFromGridPosition(Vector2Int position, int width)
        {
            return width * position.y + position.x;
        }

        public static bool IsInGrid(Vector2Int position, Vector2Int size)
        {
            return (position.x < size.x && position.y < size.y && position.x >= 0 && position.y >= 0);
        }
        
        public static Vector2Int[] GetPositionsFromShape(Shape shape, Vector2Int pos)
        {
            List<Vector2Int> result = new List<Vector2Int>();
            Vector2Int[] itemShapeCoord = shape.GetPositionsRelativeToAnchor();
            
            foreach (var coord in itemShapeCoord)
                result.Add(coord + pos);

            return result.ToArray();
        }

        public static Vector2Int DirectionToVectorInt(E_Direction direction, bool invertY = false)
        {
            switch (direction)
            {
                case E_Direction.UP:
                    return invertY ? Vector2Int.down : Vector2Int.up;
                case E_Direction.DOWN:
                    return invertY ? Vector2Int.up : Vector2Int.down;
                case E_Direction.LEFT:
                    return Vector2Int.left;
                case E_Direction.RIGHT:
                    return Vector2Int.right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }

    public enum E_Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
}
