
using System.Collections;
using Core.VariablesSO.VariableTypes;

namespace Player
{
    using UnityEngine;
    using Weapon;

    public class MeleeBuildUpPlayerState : PlayerState
    {
        [HideInInspector] public MeleeAttack currentAttack;
        [SerializeField] private WeaponManager weaponManager;
        private MeleeWeapon currentMeleeWeapon;
        private float superArmorDamageTaken;
        private Coroutine buildUpCoroutine;
        private float buildUpTimer;
        [SerializeField] private FloatVariableSO playerHealth;

        private void Awake()
        {
            playerHealth.RegisterOnValueChanged(SuperArmorTakeDamage);
        }
    
        public override void OnStateEnter()
        {
     
            //weaponManager.GetCurrentMeleeWeapon().BuildUp();
        }
        
        public override void Behave<T>(T x) 
        {
            if (x is MeleeWeapon meleeWeapon)
            {
                currentMeleeWeapon = meleeWeapon;
                if (buildUpCoroutine == null)
                {
                    buildUpCoroutine = StartCoroutine(BuildUp());
                }
            }
        }
        
        private IEnumerator BuildUp()
        {
            buildUpTimer = 0;
            superArmorDamageTaken = 0;
            currentMeleeWeapon.BuildUp();
            currentAttack = currentMeleeWeapon.GetAttack();
            while (buildUpTimer <= currentAttack.buildUpTime)
            {
                if (superArmorDamageTaken > currentAttack.superArmor)
                {
                    Debug.Log("Super Armor Broken");
                    playerStateInterpretor.RemoveStateCheck("MeleeBuildUpPlayerState");
                    buildUpCoroutine = default;
                    yield break;
                }
                buildUpTimer += Time.deltaTime;
                yield return null;
            }
            if (playerStateInterpretor.CheckState(PlayerStateType.ACTION,"MeleeBuildUpPlayerState"))
            {
                playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
                superArmorDamageTaken = 0;
                playerStateInterpretor.AddState("MeleeActivePlayerState", PlayerStateType.ACTION, true);
                playerStateInterpretor.Behave(currentMeleeWeapon,PlayerStateType.ACTION);
               
            }
            currentAttack = default;
            buildUpCoroutine = default;
            buildUpTimer = 0;
            currentMeleeWeapon = default;
        }
        
        public void SuperArmorTakeDamage(float damage)
        {
            superArmorDamageTaken += damage;
        }

        public override void OnStateExit()
        {
  
        }
    }
}
