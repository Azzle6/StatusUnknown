using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Projectile : HitContext
{
    [SerializeField] ProjectileProfile profile;


    private void OnEnable()
    {
        HitTriggerEvent += PerformProjectileHit;
    }

    private void OnDisable()
    {
        HitTriggerEvent += PerformProjectileHit;
    }
    void PerformProjectileHit(IDamageable target)
    {
        target.TakeDamage(profile.damage, Vector3.zero);
    }
    private void Update()
    {
        transform.position += transform.forward * profile.speed;
        var solidColliders = profile.hitShape.DetectColliders(transform.position, transform.rotation,profile.collisionMask);
        if (solidColliders.Length > 0)
            DestroyProjectile();
    }
    private void DestroyProjectile()
    {
        Destroy(this.gameObject);
    }
    public static Projectile InstantiateShoot(Vector3 position, Vector3 direction, ProjectileProfile projectileProfile)
    {
        Projectile projectile = new Projectile();
        projectile.profile = projectileProfile;
        projectile.hitShape = projectileProfile.hitShape;
        projectile.hitMask = projectileProfile.enemyMask;
        projectile.transform.position = position;
        projectile.transform.forward = direction;
        return projectile;
    }

}
