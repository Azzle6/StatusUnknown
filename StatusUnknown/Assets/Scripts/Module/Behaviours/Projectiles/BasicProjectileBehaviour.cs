namespace Module.Behaviours.Projectiles
{
    using UnityEngine;

    public class BasicProjectileBehaviour : InstantiatedProjectileModule
    {
        protected override void OnUpdate()
        {
            Debug.Log("Basic projectile Update.");
        }

        protected override void OnTick()
        {
            Debug.Log("Basic projectile Tick.");
        }

        protected override void OnFixedUpdate()
        {
            this.CheckCollisions();
        }
    }
}
