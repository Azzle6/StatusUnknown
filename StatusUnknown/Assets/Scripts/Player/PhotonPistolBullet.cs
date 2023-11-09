using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PhotonPistolBullet : MonoBehaviour
{
    [HideInInspector] public float damage;
    public float knockbackStrength = 10f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage,  (other.transform.position - transform.position) * knockbackStrength);
            Debug.Log(other.name);
        }
      
    }
}
