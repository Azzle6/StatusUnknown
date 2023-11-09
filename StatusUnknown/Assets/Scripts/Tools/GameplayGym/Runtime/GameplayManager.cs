using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UIElements;

// Editor, Window, PropertyDrawer
namespace StatusUnknown.CoreGameplayContent
{
    // [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(AudioSource))] 
    public class GameplayManager : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] AudioSource source; 
        [Space, SerializeField] private CombatSimulatorScriptableObject CombatSimulator;
        [SerializeField] private Enemy[] enemies = new Enemy[0];
        private const int DELAY = 1;

        [Header("Damage Types Template")]
        [Space, SerializeField] private DamageType_Burst damage_burst;
        [SerializeField] private DamageType_DOT damage_dot;
        [SerializeField] private DamageType_Delayed damage_delayed;
        private DamageType[] damageTypeArray = new DamageType[3];
        private EDamageType currentDamageType;

        private int currentIndex; 
        private int lastIndex; 

        // -- LATER --
        // save and load combat config locally + remote
        // save and load encounter config + remote
        // save and load damage types (for ex DOT -> poison or fire, BURST -> stun or explosion, etc..) locally + remote 

        private void OnEnable()
        {
            damageTypeArray = new DamageType[]
            {
                damage_burst, 
                damage_dot,
                damage_delayed,
            };

            if (CombatSimulator == null) return; 

            currentDamageType = CombatSimulator.GetRootValue();
        }

        #region CORE LOGIC
        [Button]
        public void Simulate()
        {
            CancelInvoke(nameof(SetNewDamageType));

            currentIndex = 0;
            lastIndex = CombatSimulator.GetAbilitiesArrayLength() - 1; 
            InvokeRepeating(nameof(SetNewDamageType), 0f, DELAY);
        }


        private void SetNewDamageType()
        {
            DoDamage(currentDamageType); 
        }

        // REFACTOR : strategy pattern for different types of damage application
        // for now, just a plain ugly switch case
        private void DoDamage(EDamageType eDmgType)
        {
            DamageType damageType = damageTypeArray[(int)eDmgType];
            int damageValue = damageType.GetDamageValue(); 
            Debug.Log($"damage of type {damageType.GetType()} being applied with value {damageValue}");

            // DAMAGE
            // foreach enemy
            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(damageValue); 
            }

            OnDamageDone(); 
        }

        private void OnDamageDone()
        {
            currentIndex++;
            if (currentIndex >  lastIndex)
            {
                CancelInvoke(nameof(SetNewDamageType));
                currentIndex = 0;
                Debug.Log("SIMULATION DONE"); 
                return; 
            }

            Debug.Log("setting to new damage type value"); 
            currentDamageType = CombatSimulator.GetValueAtIndex(currentIndex);
        }
        #endregion

        #region LOAD
        #endregion

        #region SAVE
        private void SaveFullSimulationPayload() { }
        #endregion
    }

    public class DamageType
    {
        [SerializeField, Range(1, 100)] protected int damage = 1;

        [Space, SerializeField] protected AudioClip damageSFX;
        [SerializeField] protected ParticleSystem damageVFX;

        public int GetDamageValue() => damage;
        /* public virtual void DoAudiovisualFeedback()
        {

        } */
    }

    [Serializable]
    public class DamageType_Burst : DamageType
    {

    }

    [Serializable]
    public class DamageType_DOT : DamageType
    {
        [SerializeField, Range(0.1f, 2f)] private float tickDelay = 0.5f; 
    }

    [Serializable]
    public class DamageType_Delayed : DamageType
    {
        [SerializeField, Range(0.5f, 5f)] private float damageDelay = 1f;
    }
}
