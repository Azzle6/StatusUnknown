namespace Module.Behaviours.Projectiles
{
    public class WaveProjectileBehaviour : InstantiatedProjectileModule
    {
        private float projectileScale = 1;
        
        protected override void OnHit(IDamageable target)
        {
            this.currentDamages -= this.ProjectileData.Damages * 0.15f;
            this.projectileScale *= 0.85f;
            this.ProjectileData.CollisionShape.Scale = this.projectileScale;
            this.ProjectileVFX.vfx.SetFloat("Size", this.GetAverageProjectileWidth() * this.projectileScale);
        }
    }
}
