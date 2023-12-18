namespace Enemy.Sniper
{
    using System.Collections;
    using System.Xml.Linq;
    using UnityEngine;
    using UnityEngine.VFX;

    public class EnemySniper : EnemyContext
    {
        public SniperStats sniperStats;
        public LayerMask obstacleMask;
        public float tpCooldown;
        
        [Header("Shoot")]
        public GameObject bulletPrefab;
        [HideInInspector]
        public float initialAttackDuration;
        public float attackCooldown;

        [field: SerializeField] public Transform shootingPoint { get; private set; }
        [Header("TP")]
        [SerializeField] VisualEffect VFX_TP;
        public override EnemyStats stats => sniperStats;
        private void Start()
        {
            tpCooldown = sniperStats.TpRndCooldown;
            initialAttackDuration = stats.AttackDuration * Random.value;
            InitializeEnemy();
            SwitchState(new SniperIdle());
        }

        protected override void EnemyTakeDamage(float damage, Vector3 force)
        {
            Debug.Log("Enemy took damage");
            base.EnemyTakeDamage(damage, force);
        }


        public void PlayTpVfx(Vector3 position, Vector3 endPosition, float killTime)
        {
           if(VFX_TP == null) return;
           VisualEffect vfx = Instantiate(VFX_TP,position,Quaternion.identity);
            vfx.SetVector3("TargetPosition", endPosition);
            vfx.SetFloat("Duration", killTime);
            Destroy(vfx.gameObject,killTime);
        }

    }
}