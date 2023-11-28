namespace Enemy.Sniper
{
    using UnityEngine;
    public class SniperEliteBullet : SniperBullet
    {
        [SerializeField] GameObject enemyToSpawn;
        protected override void DestroyProjectile(bool hitSuccess)
        {
            if (enemyToSpawn != null && !hitSuccess)
                Instantiate(enemyToSpawn,transform.position, transform.rotation);

            base.DestroyProjectile(hitSuccess);
        }
    }
}