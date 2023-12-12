namespace Enemy
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    public static class EnemyEvents
    {
        public static List<EnemyContext> enemiesAlive = new List<EnemyContext>();
        public static IReadOnlyCollection<EnemyContext> EnemiesAlive => enemiesAlive;
        public static event Action<EnemyContext> EnemyBirth, EnemyDeath;
        public static event Action AllEnemiesDied;
        public static bool Spawning {  get; private set; }
        static bool deathState { get { return (enemiesAlive.Count <= 0 && MorphEvents.activeMorphEgg == null && !Spawning); } }
        public static void CallEnemyBirthEvent(EnemyContext context)
        {
            Debug.Log($"Enmy_Birth: {context.gameObject.name}");
            enemiesAlive.Add(context);
            EnemyBirth?.Invoke(context);
        }
        public static void CallEnemyDeathEvent(EnemyContext context)
        {
            Debug.Log($"Enmy_Death: {context.gameObject.name} / Remain: {enemiesAlive.Count} / Morphing {MorphEvents.activeMorphEgg}");
            enemiesAlive.Remove(context);
            EnemyDeath?.Invoke(context);

            if (deathState)
            {
                Debug.Log("AllEnemiesDiedEvent");
                AllEnemiesDied?.Invoke();
            }
                
        }
        public static void SetSpawning(bool spawning)
        {
            bool lastDeathState = deathState;
            Spawning = spawning;

            if (!lastDeathState && deathState)
            {
                Debug.Log("AllEnemiesDiedEvent");
                AllEnemiesDied?.Invoke();
            }
        }


    }
}