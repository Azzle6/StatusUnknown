
using System;
using System.Collections;
using DG.Tweening;
using Unity.Mathematics;

namespace Player
{
    using UnityEngine;

    public class TwinBlade : MeleeWeapon
    {
        [SerializeField] private TwinBladeStat twinBladeStat;
        [SerializeField] private Transform bladeLeft;
        [SerializeField] private Transform bladeRight;

        private void OnEnable()
        {
            attacks = twinBladeStat.attacks;
            comboIndex = 0;
        }

        public override void ActionPressed()
        {
            Cast();
        }

        public override void ActionReleased()
        {
            
        }

        public override void Reload(Animator playerAnimator)
        {
            return;
        }

        public override void Switched(Animator playerAnimator, bool OnOff)
        {
            if (!OnOff)
            {
                bladeLeft.gameObject.SetActive(false);
                bladeRight.gameObject.SetActive(false);
                foreach (HitContext hitContext in hitContexts)
                    hitContext.HitTriggerEvent -= Hit;
            }
            else
            {
                foreach (HitContext hitContext in hitContexts)
                    hitContext.HitTriggerEvent += Hit;
                playerAnimator.SetInteger("WeaponID", twinBladeStat.weaponID);
                bladeLeft.gameObject.SetActive(true);
                bladeRight.gameObject.SetActive(true);
                bladeLeft.parent = weaponManager.lHandTr;
                bladeLeft.position = weaponManager.lHandTr.position;
                bladeLeft.rotation = weaponManager.lHandTr.rotation;
                bladeRight.parent = weaponManager.rHandTr;
                bladeRight.position = weaponManager.rHandTr.position;
                bladeRight.rotation = weaponManager.rHandTr.rotation;
                weaponManager.rigLHand.weight = 0;
                weaponManager.rigRHand.weight = 0;
            }
        }

        public override void Hit()
        {
            
        }

        public void Hit(IDamageable target)
        {
            target.TakeDamage(twinBladeStat.attacks[comboIndex].attackDamage, Vector3.zero);
            if (comboIndex == 2)
            {
                weaponManager.enemyStatusHandler.ApplyDotStart(target, twinBladeStat.dotDuration,
                    twinBladeStat.dotTickRate, twinBladeStat.dotDamage, Vector3.zero);
            }
            
        }
        
        

        public override void Cast()
        {
            base.Cast();
        }

        public override void BuildUp()
        {
            base.BuildUp();
        }

        public override void Active()
        {
            base.Active();
        }

        public override void Recovery()
        {
            base.Recovery();
            
        }

    
        
    }
}
