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


        public virtual void Cast()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeCastPlayerState") || weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeBuildUpPlayerState"))
                return;
            
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

        }

        public virtual void BuildUp()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeBuildUpPlayerState"))
                return;   
        }

        public virtual void Active()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeActivePlayerState"))
                return;
        }

        public virtual void Recovery()
        {
            comboIndex++;
            foreach (HitContext hitContext in hitContexts)
                hitContext.enabled = false;
            cooldownCoroutine = StartCoroutine(Cooldown());
        }

        public abstract IEnumerator Cooldown();

    }

}

