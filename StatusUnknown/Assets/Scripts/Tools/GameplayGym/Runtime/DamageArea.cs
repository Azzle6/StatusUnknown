using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    public class DamageArea : MonoBehaviour
    {
        private readonly List<Enemy> enemiesInArea = new List<Enemy>();

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
}

