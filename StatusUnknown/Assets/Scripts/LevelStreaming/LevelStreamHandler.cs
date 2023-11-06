namespace LevelStreaming
{
    using UnityEngine;
    using System;
    public static class LevelStreamHandler
    {
        private static Plane[] viewPlanes;
        public static Action UpdateViewEvent;
        public static void UpdateViewPlanesFromCamera(Camera camera)
        {
            viewPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            if (UpdateViewEvent != null)
                UpdateViewEvent();
        }
        public static bool IsBoundsInView(Bounds bounds)
        {
            if (viewPlanes == null) 
                return false;

            return GeometryUtility.TestPlanesAABB(viewPlanes, bounds);
        }

    }
}