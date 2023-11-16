using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PhotonPistolBullet : MonoBehaviour
{
    [HideInInspector] public float damage;
    public float knockbackStrength = 10f;
    
     public void Hit(IDamageable target)
     {
         target.TakeDamage(damage, transform.forward * knockbackStrength);
     }
     
}
