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
        StartCoroutine(ProcessWaveContext(waveContextQueue.Dequeue()));
    }
    IEnumerator ProcessWaveContext( WaveContext waveContext)
    {
        float timeCounter = Time.time;
        waveContext.wave.DeathEvent += () =>
        {
            waveDeathCounter++;
            if (waveDeathCounter >= waveContexts.Length)
                CallEndEvent();
        };
        yield return new WaitUntil(() => Time.time - timeCounter > waveContext.initialDelay || waveContext.wave.finished);
        waveContext.wave.ProcessWave();
        if(waveContextQueue.Count > 0)
            StartCoroutine(ProcessWaveContext(waveContextQueue.Dequeue()));
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


