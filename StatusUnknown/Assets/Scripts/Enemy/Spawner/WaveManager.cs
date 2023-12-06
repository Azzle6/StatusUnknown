namespace Enemy.Spawner
{
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
            for (int i = 0; i < this.waveContexts.Length; i++)
            {
                this.waveContextQueue.Enqueue(this.waveContexts[i]);
            }

            if (this.startOnAwake)
                this.StartWaveProcess();
        }
        public void StartWaveProcess()
        {
            this.ProcessWave();

        }
        void ProcessWave()
        {
            if (this.waveContextQueue.Count <= 0) return;
            var waveContext = this.waveContextQueue.Dequeue();
            waveContext.wave.DeathEvent += () =>
            {
                this.waveDeathCounter++;
                if (this.waveDeathCounter >= this.waveContexts.Length)
                    this.CallEndEvent();
            };
            waveContext.wave.ProcessWave();
            this.StartCoroutine(this.ProcessNextWave(waveContext));
        }
        IEnumerator ProcessNextWave(WaveContext previousWave)
        {
            float timeCounter = Time.time;
            yield return new WaitUntil(()=>Time.time-timeCounter >= previousWave.initialDelay || previousWave.wave.finished);
            this.ProcessWave();
        }


    
    

        void CallEndEvent()
        {
            Debug.Log("End Event");
            this.EndEvent?.Invoke();
            this.EndUEvent?.Invoke();
        }
    }
    [System.Serializable]
    public class WaveContext
    {
        public float initialDelay;
        public Wave wave;
    }
}