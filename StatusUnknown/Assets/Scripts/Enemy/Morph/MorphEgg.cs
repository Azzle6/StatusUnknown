using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphEgg : MonoBehaviour,IDamageable
{
    
    [SerializeField] GameObject succesEnemyToSpawn;
    [SerializeField] GameObject[] failedEnemiesToSpawn;
    [SerializeField] MeshRenderer meshRenderer;

    float currentLifePoints;
    float currentMorphDuration;
    bool initialized = false;

    [Header("Event")]
    [SerializeField] MorphGameEvent endMorphEvent;// TODO fix editor implementation


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

        currentLifePoints = lifePoints;
        currentMorphDuration = UnityEngine.Random.Range((1 - durationRandomness) * morphDuration, morphDuration); ;
        Invoke("Hatch", currentMorphDuration);
    }
    void EndMorph()
    {
        // TODO : Define usefull parameters
        endMorphEvent?.RaiseEvent(null);
        MorphEvents.EndMorphEvent(null);
        StopAllCoroutines();
        
    }
    void Explode()
    {
        EndMorph();
        Debug.Log($"Explode{gameObject}");
        for(int i = 0; i < failedEnemiesToSpawn.Length; i++)
        {
            if(failedEnemiesToSpawn[i] != null)
            {
                //TODO : set spawn location
                Instantiate(failedEnemiesToSpawn[i], transform.position, Quaternion.identity);
            }
        }
        
    }
    void Hatch()
    {
        EndMorph();
        Debug.Log($"Hatch{gameObject}");
        if(succesEnemyToSpawn != null)
        {
            //TODO : set spawn location
            Instantiate(succesEnemyToSpawn, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);

    }

    public void TakeDamage(float damage, Vector3 force)
    {
        currentLifePoints -= damage;
        Debug.Log($"{damage} {currentLifePoints}/{lifePoints}");
        StartCoroutine(HurtBlink(0.1f));
        if(currentLifePoints < 0)
        {
            Explode();
        }
    }

    public IEnumerator HurtBlink(float blinkFreq)
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
