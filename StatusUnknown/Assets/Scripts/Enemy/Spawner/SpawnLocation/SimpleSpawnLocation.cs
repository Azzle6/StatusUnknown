using System;
using UnityEngine;

public class SimpleSpawnLocation : SpawnLocation
{
    [SerializeField] Transform spawnTransform;
    public override Vector3 SpawnPosition()
    {
        return spawnTransform.position;
    }

}




