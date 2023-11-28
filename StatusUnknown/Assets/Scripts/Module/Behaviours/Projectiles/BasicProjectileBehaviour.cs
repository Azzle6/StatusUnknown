namespace Module.Behaviours.Projectiles
{
    using UnityEngine;

    public class BasicProjectileBehaviour : InstantiatedProjectileModule
    {
        protected override void OnUpdate()
        {
            
        }

        protected override void OnTick()
        {
            
        }

        protected override void OnFixedUpdate()
        {
            this.Move();
            this.CollisionBehaviour();
        }

        private void CollisionBehaviour()
        {
            Collider[] collisions = this.CheckCollisions();
            if (collisions.Length > 0)
            {
                Debug.Log($"First element collides : {collisions[0]}");
                IDamageable damageable = collisions[0].GetComponent<IDamageable>();
                if(damageable != null)
                    damageable.TakeDamage(this.Data.damages, Vector3.zero);
                Destroy(this.gameObject);
            }
        }

        private void Move()
        {
            this.transform.position += this.transform.forward * (this.Data.speed * Time.fixedDeltaTime);
        }
    }
}
