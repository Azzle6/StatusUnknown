using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage, Vector3 force)
    {
        Debug.Log(damage + " " + force);
    }
}
