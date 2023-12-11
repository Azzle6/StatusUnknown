using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MorphEgg : MonoBehaviour,IDamageable
{
    
    [SerializeField] GameObject succesEnemyToSpawn;
    [SerializeField] GameObject[] failedEnemiesToSpawn;
    [SerializeField] MeshRenderer[] meshRenderers;

    float currentLifePoints;
    [HideInInspector]
    public float currentMorphDuration;
    bool initialized = false;
    bool broken;

    [Header("Event")]
    [SerializeField] MorphGameEvent endMorphGameEvent;
    public event Action<bool> endMorphEvent;

    [Header("TODO : Morph Scriptable")]

    [SerializeField] float morphDuration = 2f;
    [SerializeField,Range(0,1)] float durationRandomness = 0.5f;
    [SerializeField] float lifePoints = 100;
    void Start()
    {
        InitializeEgg();
    }
    public void InitializeEgg()
    {
        if (initialized) return;
        initialized = true;

        broken = false;
        MorphEvents.RegisterActiveMorphEgg(this);

        currentLifePoints = lifePoints;
        currentMorphDuration = UnityEngine.Random.Range((1 - durationRandomness) * morphDuration, morphDuration); ;
        Invoke("Hatch", currentMorphDuration);
    }

    void callEndMorphEvent(bool sucess)
    {
        endMorphGameEvent?.RaiseEvent(null);
        endMorphEvent?.Invoke(sucess);
    }
    void Explode()
    {
        broken = true;
        CancelInvoke();

        Debug.Log($"Explode{gameObject}");
        ProcessExplodeDelay();
    }
    async void ProcessExplodeDelay()
    {
        await Task.Delay(2000);
        for (int i = 0; i < failedEnemiesToSpawn.Length; i++)
        {
            if (failedEnemiesToSpawn[i] != null)
            {
                //TODO : set spawn location
                Instantiate(failedEnemiesToSpawn[i], transform.position, Quaternion.identity);
            }
        }
        callEndMorphEvent(false);
        Destroy(gameObject);
    }
    void Hatch()
    {
        broken = true;
        CancelInvoke();

        Debug.Log($"Hatch{gameObject}");
        ProcessHatchDelay();
    }
    async void ProcessHatchDelay()
    {
        await Task.Delay(2000);
        if (succesEnemyToSpawn != null)
        {
            //TODO : set spawn location
            Instantiate(succesEnemyToSpawn, transform.position, Quaternion.identity);
        }
        callEndMorphEvent(true);
        Destroy(gameObject, 1);
    }

    public void TakeDamage(float damage, Vector3 force)
    {
        currentLifePoints -= damage;
        //Debug.Log($"{damage} {currentLifePoints}/{lifePoints}");
        HurtBlinkAll(meshRenderers);
        if(currentLifePoints < 0 && ! broken)
        {
            Explode();
        }
    }
    void HurtBlinkAll(MeshRenderer[] renderers)
    {
        for(int i = 0;i < renderers.Length;i++)
            StartCoroutine(HurtBlink(0.1f, renderers[i]));

    }
    public IEnumerator HurtBlink(float blinkFreq, MeshRenderer meshRenderer)
    {
        float hit = 1;
        meshRenderer.material.SetFloat("_Hit", hit);
        float startTime = Time.time;
        while (Time.time - startTime < blinkFreq)
        {
            //Debug.Log($"blink {startTime}, {Time.time}, {startTime - Time.time}, {speed}");
            hit = Mathf.PingPong(Time.time / blinkFreq, 1);
            meshRenderer?.material?.SetFloat("_Hit", 1);// debug hit
            yield return null;
        }
        hit = 0;
        meshRenderer.material.SetFloat("_Hit", hit);
        yield return null;

    }
}
