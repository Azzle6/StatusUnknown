using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorFieldNavigator 
{
    public static float fieldDensity = 1; // TODO : Add this varaiable to inspector window in some way
    public static Vector3 PositionToBoundPosition(Vector3 position)
    {
        int posX = Mathf.RoundToInt(position.x / fieldDensity);
        int posY = Mathf.RoundToInt(position.y / fieldDensity);
        int posZ = Mathf.RoundToInt(position.z / fieldDensity);
        return new Vector3(posX, posY, posZ) * fieldDensity;

    }
}
