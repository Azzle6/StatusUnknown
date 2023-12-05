using System;

namespace Player
{
    using UnityEngine;

    public class EnvironementalHazard : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private HitContext hitContext;

        private void Awake()
        {
            hitContext.HitTriggerEvent += InflictDamage;
        }
        
        private void InflictDamage(IDamageable target, Vector3 hitPosition)
        {
            target.TakeDamage(damage, Vector3.zero);
        }
    }

}
