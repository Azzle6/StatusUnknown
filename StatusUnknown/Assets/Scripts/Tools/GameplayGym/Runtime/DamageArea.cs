using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private Color areaShowColor = Color.white;
        [SerializeField] private Collider areaCollider;
        private readonly List<Enemy> enemiesInArea = new List<Enemy>();
        private BoxCollider boxCollider;
        private SphereCollider sphereCollider;
        public int EnemiesInArea { get; set; }

        private void OnEnable()
        {
            EnemiesInArea = enemiesInArea.Count; 


        }

        private void OnDrawGizmos()
        {
            Gizmos.color = areaShowColor; 
            if (areaCollider.GetType() == typeof(BoxCollider))
            {
                boxCollider = areaCollider as BoxCollider; 
                Gizmos.DrawWireCube(transform.position, boxCollider.size);
            }
            else if (areaCollider.GetType() == typeof(SphereCollider))
            {
                sphereCollider = areaCollider as SphereCollider;
                Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
            }
            else
            {
                Debug.LogError("areaCollider must be of type box or sphere to properly show gizmo");
                return; 
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("trigger enter"); 
            enemiesInArea.Add(other.GetComponent<Enemy>());
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("trigger exit");
            enemiesInArea.Remove(other.GetComponent<Enemy>());
        }

        public List<Enemy> GetEnemiesInArea() => enemiesInArea;
    }
}

