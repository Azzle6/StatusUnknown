
using System;
using System.Collections;

namespace Player
{
    using UnityEngine;

    public class TwinBlade : MeleeWeapon
    {
        public int comboIndex;
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
            }
            else
            {
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

        public override void Cast()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeCastPlayerState") || weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeBuildUpPlayerState"))
                return;
            
            if (cooldownCoroutine != default)
            {
                StopCoroutine(cooldownCoroutine);
                cooldownCoroutine = default;
            }
            
            weaponManager.playerStateInterpretor.AddState("MeleeCastPlayerState",PlayerStateType.ACTION, false);
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);

            weaponManager.playerAnimator.SetTrigger("MeleeHit");   
            weaponManager.playerAnimator.SetInteger("MeleeCombo", comboIndex);
         
                

        }

        public override void BuildUp()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeBuildUpPlayerState"))
                return;
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);

        }

        public override void Active()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeActivePlayerState"))
                return;
            
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);

        }

        public override void Recovery()
        {
            /*if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeRecoveryPlayerState"))
                return;*/
            weaponManager.playerStateInterpretor.Behave(twinBladeStat.attacks[comboIndex],PlayerStateType.ACTION);

            comboIndex++;
            Debug.Log(comboIndex + " combo index" + twinBladeStat.attacks.Length + " length");
            if (comboIndex > twinBladeStat.attacks.Length)
            {
                Debug.Log("Combo index reset");
                comboIndex = 0;
            }

            cooldownCoroutine = StartCoroutine(Cooldown());
        }

        public override IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(twinBladeStat.attacks[comboIndex].cooldownTime + twinBladeStat.attacks[comboIndex].cooldownTime);
            comboIndex = 0;
            Debug.Log("Combo index reset in coroutine");
        }
    }
}
