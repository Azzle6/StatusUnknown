using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    private List<Enemy> enemiesInArea = new List<Enemy>(); 

    private void OnTriggerEnter(Collider other)
    {
        enemiesInArea.Add(other.GetComponent<Enemy>());         
    }

    private void OnTriggerExit(Collider other)
    {
        enemiesInArea.Remove(other.GetComponent<Enemy>());
    }

    public List<Enemy> GetEnemiesInArea() => enemiesInArea;
}
