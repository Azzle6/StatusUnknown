using pooler;

namespace Module.Behaviours
{
    using System.Collections.Generic;
    using Combat.HitProcess;
    using Core.Pooler;
    using Definitions;
    using UnityEngine;

    public abstract class InstantiatedProjectileModule : InstantiatedModule
    {
        protected ProjectileBehaviourData ProjectileData;
        
        //Dynamic data
        protected VisualEffectHandler ProjectileVFX;
        protected int hitsRemaining;
        protected float currentDamages;
        protected HashSet<Collider> alreadyHitColliders;

        protected override void OnInit(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data)
        {
            this.ProjectileData = (ProjectileBehaviourData)data;
            
            this.alreadyHitColliders = new HashSet<Collider>() { info.LastHit };
            
            VisualEffectHandler tempSpawnVFX = ComponentPooler.Instance.GetPooledObject<VisualEffectHandler>("EmptyVisualEffect");
            tempSpawnVFX.transform.rotation = info.Rotation;
            tempSpawnVFX.transform.position = transform.position;
            tempSpawnVFX.StartVFX(this.ProjectileData.shootVFX, 0.5f);
            
            ProjectileVFX = ComponentPooler.Instance.GetPooledObject<VisualEffectHandler>("EmptyVisualEffect");
            ProjectileVFX.transform.position = transform.position;
            ProjectileVFX.transform.rotation = info.Rotation;
            ProjectileVFX.transform.SetParent(transform);
            ProjectileVFX.StartVFX(ProjectileData.projectileVFX, data.LifeTime);
            this.ProjectileVFX.vfx.SetFloat("Size", this.GetAverageProjectileWidth());
            this.ProjectileVFX.vfx.SetFloat("Lifetime", this.ProjectileData.LifeTime);

            this.hitsRemaining = this.ProjectileData.maxDamagedEnemies;
            this.currentDamages = this.ProjectileData.Damages;
            
            this.OnInitProjectile();
        }

        protected void SpawnHitVFX(Vector3 position, Vector3 direction)
        {
            VisualEffectHandler hitVFX = ComponentPooler.Instance.GetPooledObject<VisualEffectHandler>("EmptyVisualEffect");
            hitVFX.transform.position = position;
            hitVFX.transform.rotation = Quaternion.Euler(direction);
            hitVFX.StartVFX(ProjectileData.hitVFX, 1f);
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

            foreach (var col in collisions)
            {
                if(this.alreadyHitColliders.Contains(col))
                    return;

                this.alreadyHitColliders.Add(col);
                
                IDamageable damageable = col.GetComponent<IDamageable>();
                this.SpawnHitVFX(this.transform.position, -transform.forward);
                
                if (damageable != null)
                {
                    damageable.TakeDamage(this.currentDamages, Vector3.zero);
                    this.OnHit(damageable);
                    this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(this.transform.position, transform.rotation, col));
                    this.hitsRemaining--;
                    if (this.hitsRemaining == 0)
                    {
                        this.DestroyModule();
                        return;
                    }
                }
                else
                {
                    this.DestroyModule();
                    return;
                }
            }
        }

        protected override void OnBeforeDestroy()
        {
            ComponentPooler.Instance.ReturnObjectToPool(this.ProjectileVFX);
        }
        
        #region UTILITY
        protected float GetAverageProjectileWidth()
        {
            switch (this.ProjectileData.CollisionShape)
            {
                case HitSphere sphere:
                    return sphere.radius;
                case HitBox box :
                    return box.size.x;
                default:
                    return 1;
            }
        }
        #endregion
    }
}
