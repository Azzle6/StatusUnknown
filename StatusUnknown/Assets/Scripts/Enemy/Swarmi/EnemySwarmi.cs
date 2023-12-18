
namespace Enemy.Swarmi
{
    using System.Collections;
    using UnityEngine;
    public class EnemySwarmi : EnemyContext
    {

        public EnemyStats swarmiStats;
        public override EnemyStats stats => swarmiStats;
        //[HideInInspector]
        public float attackCoolDown;
        protected override void EnemyTakeDamage(float damage, Vector3 force)
        {
            //Debug.Log("Enemy took damage");
            base.EnemyTakeDamage(damage, force);
        }



        protected override void InitializeEnemy()
        {
            base.InitializeEnemy();
            SwitchState(new SwarmiIdle());
        }


    }
}