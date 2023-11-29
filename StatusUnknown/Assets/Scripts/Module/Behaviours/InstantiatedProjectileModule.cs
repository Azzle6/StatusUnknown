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

        public void Init(ProjectileBehaviourData data, CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            this.BaseInit(compiledModule, info);
            this.Data = data;
            
            this.gameObject.AddComponent<MeshFilter>().mesh = data.mesh;
            this.gameObject.AddComponent<MeshRenderer>().material = data.material;

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

        protected Collider[] CheckCollisions()
        {
            Collider[] result = this.Data.shape.DetectColliders(this.transform.position, this.transform.rotation,
                this.Data.layerMask);

            return result;
        }

        protected virtual void Move()
        {
            this.transform.Translate(this.transform.forward * (this.Data.speed * Time.fixedDeltaTime));
        }

        protected virtual void CollisionBehaviour()
        {
            Collider[] collisions = this.CheckCollisions();
            if (collisions.Length > 0 && !this.AlreadyHitEnemy.Contains(collisions[0]))
            {
                Collider firstCollider = collisions[0];
                IDamageable damageable = firstCollider.GetComponent<IDamageable>();
                if(damageable != null)
                    damageable.TakeDamage(this.Data.damages, Vector3.zero);
                
                Vector3 closestPoint = firstCollider.ClosestPoint(this.transform.position);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, Quaternion.Euler(closestPoint - this.transform.position), firstCollider));
                Destroy(this.gameObject);
            }
        }
    }
}
