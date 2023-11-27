namespace Weapon
{
    using UnityEngine;

    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public float damage;
        [SerializeField] private Rigidbody rb;
        public float knockbackStrength = 10f;
        
        public void Launch(float damage, Vector3 direction, float speed)
        {
            this.damage = damage;
            rb.velocity = direction * speed;
        }
    
        public void Hit(IDamageable target)
        {
            target.TakeDamage(damage, transform.forward * knockbackStrength);
        } 
    }
}


