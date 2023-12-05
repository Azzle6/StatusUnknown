namespace Module.Behaviours
{
    using System.Collections.Generic;
    using Core.Pooler;
    using Definitions;
    using UnityEngine;
    using UnityEngine.VFX;

    public abstract class InstantiatedProjectileModule : InstantiatedModule
    {
        protected ProjectileBehaviourData ProjectileData;
        
        //Dynamic data
        protected readonly HashSet<Collider> AlreadyHitEnemy = new HashSet<Collider>();
        protected VisualEffect ProjectileVFX;

        protected override void OnInit(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data)
        {
            this.ProjectileData = (ProjectileBehaviourData)data;

            if (info.LastHit != null)
                this.AlreadyHitEnemy.Add(info.LastHit);
                
            
            VisualEffect tempSpawnVFX = ComponentPooler.Instance.GetPooledObject<VisualEffect>("EmptyVisualEffect");
            tempSpawnVFX.visualEffectAsset = this.ProjectileData.shootVFX;
            tempSpawnVFX.transform.rotation = info.Rotation;
            tempSpawnVFX.transform.position = transform.position;
            tempSpawnVFX.Play();
            
            ProjectileVFX = ComponentPooler.Instance.GetPooledObject<VisualEffect>("EmptyVisualEffect");
            ProjectileVFX.visualEffectAsset = this.ProjectileData.projectileVFX;
            ProjectileVFX.transform.position = transform.position;
            ProjectileVFX.transform.rotation = info.Rotation;
            ProjectileVFX.transform.SetParent(transform);
            ProjectileVFX.Play();
            
            this.OnInitProjectile();
        }

        protected void SpawnHitVFX(Vector3 position, Vector3 direction)
        {
            VisualEffect hitVFX = ComponentPooler.Instance.GetPooledObject<VisualEffect>("EmptyVisualEffect");
            hitVFX.visualEffectAsset = this.ProjectileData.hitVFX;
            hitVFX.transform.position = position;
            hitVFX.transform.rotation = Quaternion.Euler(direction);
            hitVFX.Play();
        }

        protected virtual void OnInitProjectile()
        { }

        protected override void OnFixedUpdate()
        {
            this.Move();
            this.CollisionBehaviour();
        }

        protected virtual void Move()
        {
            this.transform.position += this.transform.forward * (this.ProjectileData.speed * Time.fixedDeltaTime);
        }

        protected override void CollisionBehaviour()
        {
            Collider[] collisions = this.CheckCollisions();
            if (collisions.Length > 0 && !this.AlreadyHitEnemy.Contains(collisions[0]))
            {
                Collider firstCollider = collisions[0];
                IDamageable damageable = firstCollider.GetComponent<IDamageable>();
                if(damageable != null)
                    damageable.TakeDamage(this.ProjectileData.Damages, Vector3.zero);
                
                Vector3 closestPoint = firstCollider.ClosestPoint(this.transform.position);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, transform.rotation, firstCollider));
                this.SpawnHitVFX(closestPoint, -transform.forward);
                this.DestroyModule();
            }
        }

        protected override void DestroyModule()
        {
            ComponentPooler.Instance.ReturnObjectToPool(this.ProjectileVFX);
            Destroy(this.gameObject);
        }
    }
}
