using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnLocation : SpawnLocation
{
    [SerializeField] Transform[] spawnTransforms;
    public override Vector3 SpawnPosition()
    {
       return spawnTransforms[Random.Range(0,spawnTransforms.Length)].position;
    }
}
