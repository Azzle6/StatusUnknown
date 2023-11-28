
using Sirenix.OdinInspector;
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
        morphTimeCounter = Time.time;
        StartMorphProcess();
    }

    void StartMorph()
    {
        startMorphEvent?.RaiseEvent(this);
        MorphEvents.StartMorphEvent(this);
        //Debug.Log($"StartMorph {gameObject}");


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
    [Button("StopMorphProcess")]
    public void StopMorphProcess()
    {
       
        currentMorphTimer -= Time.time - morphTimeCounter;
        //Debug.Log($"stop morph {gameObject} {currentMorphTimer}/{initialTimer}");
        CancelInvoke("StartMorph");
    }
    [Button("StartMorphProcess")]
    public void StartMorphProcess()
    {
        //Debug.Log($"start morph {gameObject} {currentMorphTimer}/{initialTimer}");
        Invoke("StartMorph", currentMorphTimer);
        morphTimeCounter = Time.time;
    }

}
