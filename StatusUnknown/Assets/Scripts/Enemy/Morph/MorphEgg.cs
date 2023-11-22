using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphEgg : MonoBehaviour,IDamageable
{
    
    [SerializeField] GameObject succesEnemyToSpawn;
    [SerializeField] GameObject[] failedEnemiesToSpawn;

    float currentLifePoints;

    [Header("Event")]
    [SerializeField] MorphGameEvent endMorphEvent;// TODO fix editor implementation

    
    [Header("TODO : Morph Scriptable")]

    [SerializeField] float morphDuration = 2f;
    [SerializeField] float lifePoints = 100;

    public void InitializeEgg()
    {
        currentLifePoints = lifePoints;
        Invoke("Hatch", morphDuration);
    }
    void EndMorph()
    {
        // TODO : Define usefull parameters
        endMorphEvent?.RaiseEvent(null);
        MorphEvents.EndMorphEvent(null);
        Destroy(gameObject);
    }
    void Explode()
    {
        Debug.Log($"Explode{gameObject}");
        /*foreach(var enemy in failedEnemiesToSpawn)
        {
            Instantiate(enemy,transform.position, Quaternion.identity);
        }*/
        EndMorph();
    }
    void Hatch()
    {
        Debug.Log($"Hatch{gameObject}");
        //Instantiate(succesEnemyToSpawn, transform.position, Quaternion.identity);
        EndMorph();
        
    }

    public void TakeDamage(float damage, Vector3 force)
    {
        currentLifePoints -= damage;
        if(currentLifePoints < 0)
        {
            Explode();
        }
    }
}
