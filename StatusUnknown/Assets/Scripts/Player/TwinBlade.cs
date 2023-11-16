
using System;
using System.Collections;

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
        }

        public override void Cast()
        {
            base.Cast();
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);
        }

        public override void BuildUp()
        {
            base.BuildUp();
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);
        }

        public override void Active()
        {
            base.Active();
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);

        }

        public override void Recovery()
        {
            base.Recovery();
            /*if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeRecoveryPlayerState"))
                return;*/
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);

            Debug.Log(comboIndex + " combo index" + twinBladeStat.attacks.Length + " length");
            if (comboIndex > twinBladeStat.attacks.Length -1)
            {
                Debug.Log("Combo index reset");
                comboIndex = 0;
            }


        }

        public override IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(twinBladeStat.attacks[comboIndex].cooldownTime + twinBladeStat.attacks[comboIndex].cooldownTime);
            comboIndex = 0;
            Debug.Log("Combo index reset in coroutine");
        }
        
    }
}
