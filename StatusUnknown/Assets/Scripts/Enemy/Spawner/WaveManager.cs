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
        [SerializeField]
        int waveCounter;
        [SerializeField] bool startOnAwake;

        public List<EnemyContext> enemyContexts = new List<EnemyContext>();
        private void Awake()
        {
            for (int i = 0; i < this.waveContexts.Length; i++)
            {
                this.waveContextQueue.Enqueue(this.waveContexts[i]);
            }

            if (this.startOnAwake)
                this.StartWaveProcess();
        }
        private void OnEnable()
        {
            EnemyEvents.EnemyBirth += (EnemyContext Ec) => { enemyContexts = EnemyEvents.enemiesAlive; };
            EnemyEvents.EnemyDeath += (EnemyContext Ec) => { enemyContexts = EnemyEvents.enemiesAlive; };
        }
        private void OnDisable()
        {
            EnemyEvents.AllEnemiesDied -= SkipToNextWave;
        }
        public void StartWaveProcess()
        {
            waveCounter = 0;
            EnemyEvents.AllEnemiesDied += SkipToNextWave;
            this.ProcessWave();

        }
        void ProcessWave()
        {
            if (this.waveContextQueue.Count <= 0) return;
            var waveContext = this.waveContextQueue.Dequeue();

            waveContext.wave?.ProcessWave();
            this.waveCounter++;
            // Next Wave
            StartCoroutine(ProcessNextWave(waveContext.initialDelay));
        }
        void SkipToNextWave()
        {
            this?.StopAllCoroutines();
            if (this.waveCounter >= this.waveContexts.Length)
            {
                Debug.Log($"{this.waveCounter} > {this.waveContexts.Length}");
                this.EndProcessWave();
            }
            else
                ProcessWave();
        }
        IEnumerator ProcessNextWave(float delay)
        {
            yield return new WaitForSeconds(delay);
            ProcessWave();
        }
        void EndProcessWave()
        {
            Debug.Log("End Event");
            //EnemyEvents.AllEnemiesDied -= SkipToNextWave;
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