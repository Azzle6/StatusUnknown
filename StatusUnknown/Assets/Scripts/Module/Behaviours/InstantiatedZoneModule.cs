using pooler;

namespace Module.Behaviours
{
    using System;
    using Core.Pooler;
    using Definitions;
    using UnityEngine;
    using UnityEngine.VFX;

    [Serializable]
    public class InstantiatedZoneModule : InstantiatedModule
    {
        protected ZoneBehaviourData ZoneData;

        protected override void OnInit(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data)
        {
            this.ZoneData = (ZoneBehaviourData) data;
            this.OnZoneInit();
        }

        protected virtual void OnZoneInit()
        { }

        protected override void CollisionBehaviour()
        { }

        protected virtual void ApplyZoneDamage()
        {
            VisualEffectHandler damageZoneVFX = ComponentPooler.Instance.GetPooledObject<VisualEffectHandler>("EmptyVisualEffect");
            damageZoneVFX.transform.rotation = transform.rotation;
            damageZoneVFX.transform.position = transform.position;
            
            damageZoneVFX.StartVFX(ZoneData.zoneBurstVFX, 1f);
            damageZoneVFX.vfx.SetFloat("Size", this.ZoneData.DamageRadius.radius);
            
            Collider[] colliders = this.ZoneData.DamageRadius.DetectColliders(this.transform.position, this.transform.rotation,
                this.ZoneData.LayerMask);
            
            foreach (var col in colliders)
            {
                IDamageable damageable = col.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(this.ZoneData.Damages, Vector3.zero);
                }
                Vector3 closestPoint = col.ClosestPoint(this.transform.position);
                VisualEffectHandler hitVFX = ComponentPooler.Instance.GetPooledObject<VisualEffectHandler>("EmptyVisualEffect");
                hitVFX.transform.rotation = transform.rotation;
                hitVFX.transform.position = col.transform.position;
                hitVFX.StartVFX(ZoneData.hitVFX, 1f);
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, transform.rotation, col));
            }
        }
    }
}
