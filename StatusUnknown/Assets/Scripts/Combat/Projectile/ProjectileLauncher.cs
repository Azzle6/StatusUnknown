using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] ProjectileProfile projectileProfile;
    [Button("Fire!")]
    public void Fire()
    {
        Projectile.InstantiateShoot(transform.position, transform.forward, projectileProfile);
    }
}
