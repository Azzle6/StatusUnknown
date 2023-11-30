namespace Weapon
{
    using System.Collections;
    using UnityEngine;
    using Player;
    
    public abstract class MeleeWeapon : Weapon
    {
        public Coroutine cooldownCoroutine;
        public int comboIndex;
        public MeleeAttack[] attacks;
        private int comboIndexWhenCDStarted;


        public virtual void Cast()
        {
            if (comboIndex > attacks.Length -1)
                comboIndex = 0;
            
            if (cooldownCoroutine != default)
            {
                StopCoroutine(cooldownCoroutine);
                cooldownCoroutine = default;
            }
            


            
            weaponManager.playerAnimator.SetTrigger("MeleeHit");   
            weaponManager.playerAnimator.SetFloat("MeleeCombo", comboIndex);
            
            weaponManager.playerStateInterpretor.Behave(this,PlayerStateType.ACTION);
        }

        public virtual void BuildUp()
        {

        }

        public virtual void Active()
        {
            if (weaponManager.playerStateInterpretor.CheckState(PlayerStateType.ACTION, "MeleeActivePlayerState"))
                return;
            
        }

        public virtual void Recovery()
        {
            if ((gameObject.activeSelf) && (cooldownCoroutine == default) && (comboIndex < attacks.Length -1))
                cooldownCoroutine = StartCoroutine(Cooldown());
            
            
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
        
        public MeleeAttack GetAttack()
        {
            return attacks[comboIndex];
        }

    }

}

