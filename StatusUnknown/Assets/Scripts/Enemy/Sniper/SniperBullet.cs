using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class SniperBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] HitContext hitContext;
    const int obstacleLayer = 8;
    [SerializeField] float speed = 5;
    [SerializeField] float damage = 3;
    [SerializeField] float radius = 0.5f;
    [SerializeField] LayerMask obstacleLayerMask;
    [Header("Projectile")]
    [SerializeField] GameObject hitVFX;
    private void OnEnable()
    {
        hitContext.HitTriggerEvent += PerformHit;
    }
    private void OnDisable()
    {
        hitContext.HitTriggerEvent -= PerformHit;
    }
    private void Update()
    {
        if (Physics.CheckSphere(transform.position, radius, obstacleLayerMask))
        {
            DestroyProjectile(false);
        }
    }
    public void LaunchProjectile(Vector3 start, Vector3 dir)
    {
        transform.parent = null;
        transform.position = start;
        rb.velocity = dir.normalized * speed;
        transform.forward = dir;
    }
    protected virtual void DestroyProjectile( bool hitSuccess)
    {
        Destroy(gameObject);
    }
    void PerformHit(IDamageable damageable,Vector3 hitPosition)
    {
        damageable.TakeDamage(damage,Vector3.zero);
        if(hitVFX)
            PlayVFX(hitVFX, hitPosition,hitPosition - transform.position,3);

        DestroyProjectile(true);
    }
    public virtual void PlayVFX(GameObject vfxPrefab, Vector3 position, Vector3 forward, float duration)
    {
        // TODO : implement Pooler
        GameObject vfxObj = Instantiate(vfxPrefab, position, Quaternion.identity);
        vfxObj.transform.forward = forward;
        Destroy(vfxObj, duration);
    }


}
