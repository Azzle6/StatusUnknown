namespace Module.Behaviours
{
    using System.Collections.Generic;
    using Definitions;
    using UnityEngine;

    public abstract class InstantiatedProjectileModule : InstantiatedModule
    {
        protected ProjectileBehaviourData Data;
        
        //Dynamic data
        protected readonly HashSet<Collider> AlreadyHitEnemy = new HashSet<Collider>();

        protected override void OnInit(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data)
        {
            this.Data = (ProjectileBehaviourData)data;

            if (info.LastHit != null)
                this.AlreadyHitEnemy.Add(info.LastHit);
            
            this.OnInit();
        }

        protected virtual void OnInit()
        { }

        protected override void OnFixedUpdate()
        {
            this.Move();
            this.CollisionBehaviour();
        }

        protected virtual void Move()
        {
            this.transform.position += this.transform.forward * (this.Data.speed * Time.fixedDeltaTime);
        }

        protected override void CollisionBehaviour()
        {
            Collider[] collisions = this.CheckCollisions();
            if (collisions.Length > 0 && !this.AlreadyHitEnemy.Contains(collisions[0]))
            {
                Collider firstCollider = collisions[0];
                IDamageable damageable = firstCollider.GetComponent<IDamageable>();
                if(damageable != null)
                    damageable.TakeDamage(this.Data.damages, Vector3.zero);
                
                Vector3 closestPoint = firstCollider.ClosestPoint(this.transform.position);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, transform.rotation, firstCollider));
                Destroy(this.gameObject);
            }
        }
    }
}
