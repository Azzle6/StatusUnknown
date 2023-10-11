using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelStreamHandler 
{
    static Plane[] viewPlanes { get; set; }
    public static void UpdateViewPlanesFromCamera(Camera camera)
    {
        viewPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
    }
    public static bool IsBoundsInView(Bounds bounds)
    {
        if(viewPlanes == null) return false;
        return GeometryUtility.TestPlanesAABB(viewPlanes, bounds);
    }
}
