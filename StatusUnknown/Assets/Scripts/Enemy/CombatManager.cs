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
        bool inAngle = Mathf.Acos( Vector3.Dot(lookDirection.normalized,playerVector.normalized))*Mathf.Rad2Deg < viewAngle; // TODO : fix angle position
        //Debug.Log($"AngleBetween {Mathf.Acos(Vector3.Dot(lookDirection.normalized, playerVector.normalized))*Mathf.Rad2Deg} < {viewAngle} {inAngle}");
        bool isVisible = !Physics.Raycast(position, playerVector, Mathf.Min(range, playerVector.magnitude), obstructMask);

        Debug.DrawRay(position, playerVector.normalized * 2, (inAngle && inRange)?Color.green : Color.red);
        Debug.DrawRay(position, lookDirection * 3, isVisible?Color.yellow : Color.red);

        return inRange && isVisible;
    }
}
