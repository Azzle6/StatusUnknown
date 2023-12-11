namespace Module.Behaviours.Projectiles
{
    using System.Collections.Generic;
    using Unity.Mathematics;
    using UnityEngine;

    public class WaveProjectileBehaviour : InstantiatedProjectileModule
    {
        private float currentDamages;
        private int collisionsRemaining = 4;
        protected readonly HashSet<Collider> AlreadyHitElements = new HashSet<Collider>();

        protected override void OnInitProjectile()
        {
            this.currentDamages = this.ProjectileData.Damages;
        }
        
        protected override void CollisionBehaviour()
        {
            Collider[] collisions = this.CheckCollisions();

            foreach (var col in collisions)
            {
                if(this.AlreadyHitElements.Contains(col))
                    continue;
                
                this.AlreadyHitElements.Add(col);
                
                IDamageable damageable = col.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(this.currentDamages, Vector3.zero);
                }

                Vector3 closestPoint = col.ClosestPoint(this.transform.position);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, quaternion.Euler(closestPoint - this.transform.position), col));
                
                this.collisionsRemaining--;
                this.currentDamages -= this.ProjectileData.Damages * 0.15f;
                this.SpawnHitVFX(closestPoint, -transform.forward);
                if(this.collisionsRemaining <= 0)
                    this.DestroyModule();
            }
        }
    }
}
