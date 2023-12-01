using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [SerializeField] WaveContext[] waveContexts;
    Queue<WaveContext> waveContextQueue = new Queue<WaveContext>();
    public event Action EndEvent;
    public UnityEvent EndUEvent;
    int waveDeathCounter;
    [SerializeField] bool startOnAwake;
    private void Awake()
    {
        for (int i = 0; i < waveContexts.Length; i++)
        {
            waveContextQueue.Enqueue(waveContexts[i]);
        }

        if (startOnAwake)
            StartWaveProcess();
    }
    public void StartWaveProcess()
    {
        ProcessWave();

    }
    void ProcessWave()
    {
        if (waveContextQueue.Count <= 0) return;
        var waveContext = waveContextQueue.Dequeue();
        waveContext.wave.DeathEvent += () =>
        {
            waveDeathCounter++;
            if (waveDeathCounter >= waveContexts.Length)
                CallEndEvent();
        };
        waveContext.wave.ProcessWave();
        StartCoroutine(ProcessNextWave(waveContext));
    }
    IEnumerator ProcessNextWave(WaveContext previousWave)
    {
        float timeCounter = Time.time;
        yield return new WaitUntil(()=>Time.time-timeCounter >= previousWave.initialDelay || previousWave.wave.finished);
        ProcessWave();
    }


    
    

    void CallEndEvent()
    {
        Debug.Log("End Event");
        EndEvent?.Invoke();
        EndUEvent?.Invoke();
    }
}
[System.Serializable]
public class WaveContext
{
    public float initialDelay;
    public Wave wave;
}


