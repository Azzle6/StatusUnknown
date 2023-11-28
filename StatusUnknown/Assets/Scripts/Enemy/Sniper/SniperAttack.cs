using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAttack : EnemyState
{
    EnemySniper currentContext => context as EnemySniper;
    SniperStats sniperStats => currentContext.sniperStats;
    float attackDuration;
    float shootDelay;
    Vector3 target;
    bool attacked = false;
    public override void DebugGizmos()
    {
        Gizmos.color = (CombatManager.PlayerInView(transform.position, transform.forward, sniperStats.AttackRange, 180, currentContext.obstacleMask)) ? Color.green : Color.red; ;
        Gizmos.DrawWireSphere(transform.position, sniperStats.AttackRange);
    }

    public override void Update()
    {
        attackDuration -= Time.deltaTime;
        shootDelay -= Time.deltaTime;
        if(shootDelay <= 0 && !attacked)
        {
            attackDuration = sniperStats.AttackDuration;
            attacked = true;
            Debug.Log("Shoot");
            Shoot();
        }
        if (attackDuration <= 0 && attacked)
            currentContext.SwitchState(new SniperIdle());

    }
    void Shoot()
    {
        GameObject bulletObj  = GameObject.Instantiate(currentContext.bulletPrefab, transform.position, Quaternion.identity);
        SniperBullet bullet = bulletObj.GetComponent<SniperBullet>();
        Vector3 dir = target - transform.position;
        bullet.LaunchProjectile(currentContext.shootingPoint.position, dir);
    }

    protected override void Initialize()
    {
        target = CombatManager.playerTransform.position;
        attackDuration = sniperStats.AttackDuration;
        shootDelay = sniperStats.shootDelay;
    }
}

