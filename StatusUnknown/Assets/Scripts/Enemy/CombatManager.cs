using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CombatManager 
{
    public static Transform playerTransform { get; private set; }
    public static void SetPlayerTarget(Transform playerT)
    {
        playerTransform = playerT;
        //Debug.Log(playerTransform.name);
    }
    public static bool PlayerInRange(Vector3 position,float range)
    {
        if (playerTransform == null) return false;
        float sqrtPlayerDistance = Vector3.SqrMagnitude(position - playerTransform.position);
        return (sqrtPlayerDistance < range * range);
    }

    public static bool PlayerInView(Vector3 position, Vector3 lookDirection, float range, float viewAngle, LayerMask obstructMask)
    {
        if (playerTransform == null) return false;
        Vector3 playerVector = playerTransform.position - position;
        bool inRange = PlayerInRange(position, range);
        bool inAngle = Vector3.Angle(lookDirection, playerVector) < viewAngle;
        bool isVisible = Physics.Raycast(position, playerVector, range, obstructMask) == false;

        return inRange && inAngle && isVisible;
    }
}
