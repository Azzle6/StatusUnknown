using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemySwarmi : EnemyContext
{

    [Range(0f, 1f)]
    public float hit = 0;
    public float hitFreq = 1;
    public EnemyStats swarmiStats;
    public override EnemyStats stats => swarmiStats;

    protected override void EnemyTakeDamage(float damage, Vector3 force)
    {
        Debug.Log("Enemy took damage");
        StartCoroutine(HurtBlink());
        base.EnemyTakeDamage(damage, force);
    }
    public IEnumerator HurtBlink()
    {
        hit = 1;
        meshRenderer.material.SetFloat("_Hit", hit);
        float startTime = Time.time;
        while (Time.time - startTime < hitFreq)
        {
            //Debug.Log($"blink {startTime}, {Time.time}, {startTime - Time.time}, {speed}");
            hit = Mathf.PingPong(Time.time / hitFreq, 1);
            meshRenderer?.material?.SetFloat("_Hit", 1);// debug hit
            yield return null;
        }
        hit = 0;
        meshRenderer.material.SetFloat("_Hit", hit);
        yield return null;

    }

    protected override void InitializeEnemy()
    {
        base.InitializeEnemy();
        SwitchState(new SwarmiIdle());
    }


}
