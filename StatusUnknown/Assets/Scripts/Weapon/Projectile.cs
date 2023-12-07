using Combat.HitProcess;
using pooler;

namespace Weapon
{
    using UnityEngine;
    using System.Collections;
    using Core.Pooler;
    using Module;
    using Module.Behaviours;
    using UnityEngine.VFX;
    
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public float damage;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private VisualEffectAsset hitVFX;
        public HitSphere hitShape;
        [SerializeField] private LayerMask layerMask; 
        private VisualEffectHandler tempHitVFX;
        public float knockbackStrength = 10f;
        public float lifeTime = 5f;
        private bool isCheckingCollision;

        private CompiledModule moduleToCast;
        
        private void OnEnable()
        {
            StartCoroutine(DestroyAfterTime());
            isCheckingCollision = false;
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        
        public void Launch(float damage, Vector3 direction, float speed, CompiledModule moduleToCast)
        {
            this.damage = damage;
            rb.velocity = direction * speed;
            this.moduleToCast = moduleToCast;
        }

        private void FixedUpdate()
        {
            if (!isCheckingCollision)
                return;
            CollisionBehaviour();
        }
        
        public void StartCheckingCollision()
        {
            isCheckingCollision = true;
        }
        
        protected void CollisionBehaviour()
        {
            Collider[] collisions = hitShape.DetectColliders(transform.position,transform.rotation,
                layerMask);
            if (collisions.Length > 0)
            {
       
                Collider firstCollider = collisions[0];
                IDamageable damageable = firstCollider.GetComponent<IDamageable>();
                if (damageable != null)
                    damageable.TakeDamage(damage, transform.forward * knockbackStrength);

                ModuleBehaviourHandler.Instance.InstantiateModuleBehaviour(this.moduleToCast, new InstantiatedModuleInfo(transform.position, transform.rotation, collisions[0]));
                tempHitVFX = ComponentPooler.Instance.GetPooledObject<VisualEffectHandler>("EmptyVisualEffect");

                tempHitVFX.StartVFX(hitVFX,5);
                tempHitVFX.transform.position = transform.position;
                ComponentPooler.Instance.ReturnObjectToPool(this);
            }
        }
        
        
        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(lifeTime);
            ComponentPooler.Instance.ReturnObjectToPool(this);
        }
    }
}


