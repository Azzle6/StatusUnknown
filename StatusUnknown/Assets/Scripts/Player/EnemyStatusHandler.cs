using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusHandler : MonoBehaviour
{
    
    public void ApplyDotStart(IDamageable target, float duration, float tickRate, float damage, Vector3 force)
    {
        StartCoroutine(ApplyDot(target, duration, tickRate, damage, force));
    }
    
    public IEnumerator ApplyDot(IDamageable target, float duration, float tickRate, float damage, Vector3 force)
    {
        float timer = 0;
        while (timer <= duration)
        {
            target.TakeDamage(damage, force);
            timer += tickRate;
            yield return new WaitForSeconds(tickRate);
        }
    }
}
