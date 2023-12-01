using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoSpawner : MonoBehaviour
{
    public GameObject feedback;
    public List<Transform> spawnPoints;
    public List<GameObject> wave;
    public bool isInfinite;
    public float timer = 10;
    public bool onCD;
    public bool isSpawning;
    
    void Start()
    {
        isSpawning = false;
        onCD = false;
    }

    void Update()
    {
        if (isSpawning && onCD == false)
        {
            SpawnWave();
            onCD = true;
            if (isInfinite)
            {
                StartCoroutine("Cooldown");
            }
        }
    }
    
    private void SpawnWave()
    {
        for (int i = 0; i < wave.Count; i++)
        {
            StartCoroutine(SpawnEnnemy(i));
        }
    }

    IEnumerator SpawnEnnemy(int i)
    {
        Instantiate(feedback, spawnPoints[i]);
        yield return new WaitForSeconds(2);
        Instantiate(wave[i], spawnPoints[i]);
    }
    
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(timer);
        onCD = false;
    }
}
