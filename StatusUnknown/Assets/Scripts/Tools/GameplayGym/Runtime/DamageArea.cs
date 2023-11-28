using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace StatusUnknown.Content
{
    public class DamageArea : MonoBehaviour, ISelfValidator 
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

        public void Validate(SelfValidationResult result)
        {
            if (areaCollider == null)
            {
                // how to add multiple fixes to a single result ?
                result.AddWarning("Area Collider field is empty")
                    .WithFix("Add Sphere Collider", () => Fix_SetCollider(out Collider sphereCol, true))
                    .WithFix("Add Box Collider", () => Fix_SetCollider(out Collider boxCol, false)); 

            }
            else if (!areaCollider.isTrigger)
            {
                result.AddError("Your collider is not set to Trigger and will not detect enemy prefabs")
                    .WithFix("Set to \"Is Trigger\"", () => areaCollider.isTrigger = true); 
            }
        }

        private void Fix_SetCollider(out Collider compToAdd, bool asSphereCollider)
        {
            if (TryGetComponent(out compToAdd))
            {
                areaCollider = compToAdd; 
            }
            else
            {
                if (asSphereCollider)
                {
                    areaCollider = gameObject.AddComponent<SphereCollider>();
                }
                else
                {
                    areaCollider = gameObject.AddComponent<BoxCollider>();
                }
            }

            areaCollider.isTrigger = true; 
        }
    }
}

