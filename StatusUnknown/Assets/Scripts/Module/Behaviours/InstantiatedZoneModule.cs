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
            VisualEffect damageZoneVFX = ComponentPooler.Instance.GetPooledObject<VisualEffect>("EmptyVisualEffect");
            damageZoneVFX.visualEffectAsset = this.ZoneData.zoneBurstVFX;
            damageZoneVFX.transform.rotation = transform.rotation;
            damageZoneVFX.transform.position = transform.position;
            
            damageZoneVFX.Play();
            
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
                VisualEffect hitVFX = ComponentPooler.Instance.GetPooledObject<VisualEffect>("EmptyVisualEffect");
                hitVFX.visualEffectAsset = this.ZoneData.hitVFX;
                hitVFX.transform.rotation = transform.rotation;
                hitVFX.transform.position = col.transform.position;
                
                this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, transform.rotation, col));
            }
        }
    }
}
