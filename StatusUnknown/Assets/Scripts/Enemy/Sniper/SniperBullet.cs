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
    private void OnEnable()
    {
        hitContext.HitTriggerEvent += PerformHit;
    }
    private void OnDisable()
    {
        hitContext.HitTriggerEvent -= PerformHit;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == obstacleLayer)
        {
            DestroyProjectile();
        }
    }
    public void LaunchProjectile(Vector3 start, Vector3 dir)
    {
        transform.parent = null;
        transform.position = start;
        rb.velocity = dir.normalized * speed;
        transform.forward = dir;
    }
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
    void PerformHit(IDamageable damageable)
    {
        damageable.TakeDamage(damage,Vector3.zero);
        DestroyProjectile();
    }

}
