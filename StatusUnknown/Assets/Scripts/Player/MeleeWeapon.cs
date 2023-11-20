using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class MeleeWeapon : Weapon
    {
        public Coroutine cooldownCoroutine;
        public HitContext[] hitContexts;
        public int comboIndex;
        public MeleeAttack[] attacks;
        private int comboIndexWhenCDStarted;


        public virtual void Cast()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeCastPlayerState") || weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeBuildUpPlayerState"))
                return;

            if (comboIndex > attacks.Length -1)
                comboIndex = 0;
            
            if (cooldownCoroutine != default)
            {
                StopCoroutine(cooldownCoroutine);
                cooldownCoroutine = default;
            }
            

            foreach (HitContext hitContext in hitContexts)
                hitContext.enabled = true;
            
            weaponManager.playerAnimator.SetTrigger("MeleeHit");   
            weaponManager.playerAnimator.SetInteger("MeleeCombo", comboIndex);
            
            weaponManager.playerStateInterpretor.AddState("MeleeCastPlayerState",PlayerStateType.ACTION, false);
            weaponManager.playerStateInterpretor.Behave(attacks[comboIndex],PlayerStateType.ACTION);


        }

        public virtual void BuildUp()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeBuildUpPlayerState"))
                return;   
            
            weaponManager.playerStateInterpretor.Behave(attacks[comboIndex],PlayerStateType.ACTION);

        }

        public virtual void Active()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeActivePlayerState"))
                return;
            
            weaponManager.playerStateInterpretor.Behave(attacks[comboIndex],PlayerStateType.ACTION);

        }

        public virtual void Recovery()
        {
            foreach (HitContext hitContext in hitContexts)
                hitContext.enabled = false;
            cooldownCoroutine = StartCoroutine(Cooldown());
            
            weaponManager.playerStateInterpretor.Behave(attacks[comboIndex],PlayerStateType.ACTION);
            comboIndex++;

            if (comboIndex > attacks.Length -1)
            {
                comboIndex = 0;
            }
        }

        public virtual IEnumerator Cooldown()
        {
            comboIndexWhenCDStarted = comboIndex;
            yield return new WaitForSeconds(attacks[comboIndexWhenCDStarted].cooldownTime + attacks[comboIndexWhenCDStarted].cooldownTime);
            comboIndex = 0;
        }

    }

}

