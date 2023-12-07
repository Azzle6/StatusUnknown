using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCompiler : MonoBehaviour
{
    [SerializeField] SpawnCommand[] commands;
    Queue<SpawnCommand> commandsQueue = new Queue<SpawnCommand>();
    public void ProcessCommands()
    {
        if(commandsQueue.Count <= 0) return;

        var currentCommand = commandsQueue.Dequeue();
        StartCoroutine(ExecuteCommand(currentCommand, currentCommand.settings.delay));

        // if condition next command
        currentCommand.deathEvent += ProcessCommands;
        // if other condition
        ProcessCommands();
    }
    IEnumerator ExecuteCommand(SpawnCommand command, float delay)
    {
        for(int i = 0; i < command.settings.iteration; i++)
        {
            yield return new WaitForSeconds(delay);
            for (int j = 0; j < command.settings.quantity; i++)
            {
                GameObject obj = Instantiate(command.enemy, command.spawnLocation.SpawnPosition(), Quaternion.identity);
                EnemyContext enemy = obj.GetComponent<EnemyContext>();
                if(enemy != null) command.AddEnemy(enemy);
            }
        }
    }
}
[System.Serializable]
public class SpawnCommand
{
    public GameObject enemy;
    [SerializeReference]
    public SpawnLocation spawnLocation;
    public SpawnCommandSettings settings;
    // event
    public event Action deathEvent;
    // Count enemy
    HashSet<EnemyContext> enemyContexts = new HashSet<EnemyContext>();
    int enemyDeathCounter = 0;
    void RemoveEnemy(EnemyContext enemy)
    {
        enemy.OnDeathEvent -= RemoveEnemy;
        enemyContexts.Remove(enemy);

        enemyDeathCounter++;
        if(enemyContexts.Count >= settings.iteration * settings.quantity)
            deathEvent?.Invoke();
    }
    public void AddEnemy(EnemyContext enemy)
    {
        if (enemyContexts.Contains(enemy)) return;
        enemyContexts.Add(enemy);
        enemy.OnDeathEvent += RemoveEnemy;
    }
}
