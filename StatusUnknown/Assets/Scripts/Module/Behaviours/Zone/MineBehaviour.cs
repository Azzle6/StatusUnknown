namespace Module.Behaviours.Zone
{
    using UnityEngine;

    public class MineBehaviour : InstantiatedZoneModule
    {
        private float explosionRange;
        protected override void OnFixedUpdate()
        {
            this.CollisionBehaviour();
        }
        
        protected override void CollisionBehaviour()
        {
            Collider[] colliders = this.CheckCollisions();

            if (colliders.Length > 0)
            {
                Collider[] hitElements = this.Data.damageZone.DetectColliders(this.transform.position, this.transform.rotation,
                    this.Data.LayerMask);
                foreach (var col in hitElements)
                {
                    IDamageable damageable = col.GetComponent<IDamageable>();
                    if(damageable != null)
                        damageable.TakeDamage(this.Data.Damages, Vector3.zero);
                
                    Vector3 closestPoint = col.ClosestPoint(this.transform.position);
                    this.OnHitEvent?.Invoke(new InstantiatedModuleInfo(closestPoint, transform.rotation, col));
                }
                Destroy(this.gameObject);
            }
        }
    }
}
