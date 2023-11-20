

namespace Core.Helpers
{
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
    }
}
