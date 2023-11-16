using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private Color areaShowColor = Color.white;
        [SerializeField] private BoxCollider collider;
        private readonly List<Enemy> enemiesInArea = new List<Enemy>();

        private void OnDrawGizmos()
        {
            Gizmos.color = areaShowColor;
            Gizmos.DrawWireCube(transform.position, collider.size);
        }

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

