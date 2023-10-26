namespace Core.Helper
{
    using UnityEngine;
    public static class BoundsHelper
    {
        public static Bounds GetObjectBounds(GameObject parentObject)
        {
            Bounds resultBounds = new Bounds();
            if (parentObject == null) 
                return resultBounds; //TODO : debugLog error

            Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length <= 0)
                return resultBounds; //TODO : debugLog error

            resultBounds = renderers[0].bounds;
            for (int i = 0; i < renderers.Length; i++)
                resultBounds = SummBounds(resultBounds, renderers[i].bounds);

            return resultBounds;
        }
        public static Bounds SummBounds(Bounds bounds1, Bounds bounds2)
        {
            Vector3 max = Vector3.Max(bounds1.max, bounds2.max);
            Vector3 min = Vector3.Min(bounds1.min, bounds2.min);

            Vector3 size = max - min;
            Vector3 center = min + size / 2;
            return new Bounds(center, size);
        }
    }
}