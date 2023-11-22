using Core.EventsSO;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MorphBehaviour : MonoBehaviour
{
    
    [SerializeField] GameObject morphEggPrefab;
    [SerializeField] MorphGameEvent startMorphEvent;// TODO fix editor implementation
    float currentMorphTimer;
    float morphTimeCounter;

    [Header("TODO : Morph Scriptable")]
    [SerializeField] float morphTimer = 20;
    [SerializeField, Range(0, 1)] float timerRandomness;
    [Header("Debug")]
    [SerializeField] bool morphing;
    

    private void OnEnable()
    {
        MorphEvents.MorphStart += StartMorphListener;
        MorphEvents.MorphEnd += EndMorphListener;
    }

    private void OnDisable()
    {
        MorphEvents.MorphStart -= StartMorphListener;
        MorphEvents.MorphEnd -= EndMorphListener;
    }


    private void Start()
    {
        currentMorphTimer = Random.Range((1-timerRandomness) * morphTimer, morphTimer);
        StartMorphProcess();
    }
    [Button("StartMorph")]
    void StartMorph()
    {
        startMorphEvent.RaiseEvent(this);
        MorphEvents.StartMorphEvent(this);
        Debug.Log("StartMorph");
        morphing = true;

        //TODO : morph shit
        SpawnEgg();
        Destroy(gameObject);
    }
    void SpawnEgg()
    {
        GameObject eggPrefab = Instantiate(morphEggPrefab,transform.position,Quaternion.identity);
        MorphEgg morphEgg = eggPrefab.GetComponent<MorphEgg>();
        morphEgg.InitializeEgg();
    }


    void EndMorphListener(MorphBehaviour morphOrigin)
    {
        if (morphOrigin != this)
            StartMorphProcess();
    }
    void StartMorphListener(MorphBehaviour morphOrigin)
    {
        if(morphOrigin != this)
            StopMorphProcess();
    }

    public void StopMorphProcess()
    {
        currentMorphTimer -= Time.time - morphTimeCounter;
        CancelInvoke("StartMorph");
    }
    public void StartMorphProcess()
    {
        Invoke("StartMorph", currentMorphTimer);
        morphTimeCounter = Time.time;
    }

}
