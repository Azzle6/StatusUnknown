namespace Module.Behaviours.Projectiles
{
    using UnityEngine;

    public class WaveProjectileBehaviour : InstantiatedProjectileModule
    {
        private float projectileScale = 1;
        
        protected override void OnHit(IDamageable target)
        {
            this.currentDamages -= this.ProjectileData.Damages * 0.15f;
            this.projectileScale *= 0.3f;
            Debug.Log($"New projectile scale = {this.projectileScale}");
            this.ProjectileVFX.vfx.SetFloat("Size", this.GetAverageProjectileWidth() * this.projectileScale);
        }
    }
}
