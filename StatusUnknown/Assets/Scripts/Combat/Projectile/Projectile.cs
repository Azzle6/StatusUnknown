namespace Combat.Projectile
{
    using UnityEngine;
    public class Projectile : HitContext
    {
        [SerializeField] ProjectileProfile profile;
        
        private void OnEnable()
        {
            this.HitTriggerEvent += this.PerformProjectileHit;
        }

        private void OnDisable()
        {
            this.HitTriggerEvent += this.PerformProjectileHit;
        }
        
        protected virtual void PerformProjectileHit(IDamageable target)
        {
            target.TakeDamage(this.profile.damage, Vector3.zero);
        }

        protected virtual void OnUpdate()
        {
            this.transform.position += this.transform.forward * this.profile.speed;
            var solidColliders = this.profile.hitShape.DetectColliders(this.transform.position, this.transform.rotation,this.profile.collisionMask);
            if (solidColliders.Length > 0)
                this.DestroyProjectile();
        }
        
        private void Update()
        {
            this.OnUpdate();
        }
        
        private void DestroyProjectile()
        {
            Destroy(this.gameObject);
        }
        
        public static Projectile InstantiateShoot(Vector3 position, Vector3 direction, ProjectileProfile projectileProfile)
        {
            Projectile projectile = new Projectile();
            projectile.profile = projectileProfile;
            projectile.hitShape = projectileProfile.hitShape;
            projectile.hitMask = projectileProfile.enemyMask;
            projectile.transform.position = position;
            projectile.transform.forward = direction;
            return projectile;
        }

    }
}
