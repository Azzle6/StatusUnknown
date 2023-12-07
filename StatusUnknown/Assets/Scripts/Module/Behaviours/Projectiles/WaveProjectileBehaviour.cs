namespace Module.Behaviours.Projectiles
{
    using Unity.Mathematics;
    using UnityEngine;

    public class WaveProjectileBehaviour : InstantiatedProjectileModule
    {
        private float currentDamages;
        private int collisionsRemaining = 4;

        protected override void OnInit()
        {
            this.currentDamages = this.Data.Damages;
        }
        
        protected override void CollisionBehaviour()
        {
            Collider[] collisions = this.CheckCollisions();

            foreach (var col in collisions)
            {
                if(this.AlreadyHitEnemy.Contains(col))
                    continue;
                
                this.AlreadyHitEnemy.Add(col);
                
                IDamageable damageable = col.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(this.currentDamages, Vector3.zero);
                }

                Vector3 closestPoint = col.ClosestPoint(this.transform.position);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, quaternion.Euler(closestPoint - this.transform.position), col));
                
                this.collisionsRemaining--;
                this.currentDamages -= this.Data.Damages * 0.15f;
                if(this.collisionsRemaining <= 0)
                    Destroy(this.gameObject);
            }
        }
    }
}
