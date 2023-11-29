using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnCommand : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField, SerializeReference] SpawnLocation spawnLocation = new SimpleSpawnLocation();
    [SerializeField] int spawnQty =1;
    [SerializeField] float delay = 0;
    [SerializeField] int repeat = 1;
    [Header("Debug")]
    [SerializeField] bool spwanOnEwake;

    private void Awake()
    {
        if(spwanOnEwake)
            SpawnProcess();
    }

    public void SpawnProcess()
    {
        for(int i = 0;i < repeat;i++)
            Invoke("SpawnBatch", (i + 1) * delay);

    }
    void SpawnBatch()
    {
        for(int i = 0; i < spawnQty;i++)
            Instantiate(objectToSpawn, spawnLocation.SpawnPosition(), Quaternion.identity);

    }

}
