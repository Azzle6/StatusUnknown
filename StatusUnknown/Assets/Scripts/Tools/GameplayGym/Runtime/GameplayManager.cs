using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Editor, Window, PropertyDrawer
namespace StatusUnknown.CoreGameplayContent
{
    public enum DamageZoneType { SingleTarget, Area, All } 
    // TODO : have to make it work in editor (problems -> callbacks, coroutines)
    // [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(AudioSource))] 
    public class GameplayManager : MonoBehaviour
    {
        [Header("General")]
        //[SerializeField] AudioSource source; 
        [SerializeField] private CombatSimulatorScriptableObject CombatSimulator;
        private const int DELAY = 1;

        [Header("Damage Types Template")] // TODO : by default, read values from scriptables in the simulator and not from the template
        [Space, SerializeField] private DamageType_Burst template_burst;
        [SerializeField] private DamageType_DOT template_dot;
        [SerializeField] private DamageType_Delayed template_delayed;
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
                template_burst, 
                template_dot,
                template_delayed,
            };

            if (CombatSimulator == null) return; 

            currentDamageType = CombatSimulator.GetRootValue();
        }

        #region CORE LOGIC
        [Button(ButtonHeight = 100), PropertySpace]
        public void StartSimulation() // Entry Point (done once)
        {
            currentIndex = 0;
            lastIndex = CombatSimulator.GetAbilitiesArrayLength() - 1;

            SetDamagePayload(currentDamageType); 
        }

        // REFACTOR : strategy pattern for different types of damage application
        // REFACTOR : as callback when damage payload is done
        // for now, just a plain ugly switch case
        DamageType damageType; 
        int damageValue; 
        private void SetDamagePayload(EDamageType eDmgType)
        {
            Debug.Log("setting damage type to : " + currentDamageType);

            damageType = damageTypeArray[(int)eDmgType];
            damageValue = damageType.Damage; 

            switch(eDmgType)
            {
                case EDamageType.Burst :
                    DoDamage_Burst(damageValue); 
                    break;
                case EDamageType.DOT:
                    maxTick = template_dot.TickAmount;
                    StartCoroutine(nameof(DoDamage_DOT)); // TODO : change with custom struct for cooldown/tick delay
                    break; 
                case EDamageType.Delayed:
                    DoDamage_Delayed(damageValue);
                break; 
            } 
        }

        private void DoDamage_Burst(int damageValue)
        {
            Debug.Log("applying burst damage"); 
            ApplyDamage(damageValue);
            OnDamageDone(); 
        }

        private int maxTick;
        private IEnumerator DoDamage_DOT()
        {
            for (int i = 0; i < maxTick; i++)
            {
                Debug.Log("applying DOT damage");
                ApplyDamage(damageValue);
                yield return new WaitForSeconds(template_dot.TickDelay);
            }

            StopCoroutine(nameof(DoDamage_DOT));
            OnDamageDone();
        }

        // ERROR : "Trying to Invoke method: GameplayManager.ApplyDamage couldn't be called."
        private void DoDamage_Delayed(int damageValue)
        {
            Debug.Log($"applying damage with delay of {template_delayed.DamageDelay} seconds");
            Invoke(nameof(ApplyDamage), template_delayed.DamageDelay);
            Invoke(nameof(OnDamageDone), template_delayed.DamageDelay * 1.5f); // :D
        }

        private void ApplyDamage(int damageValue)
        {
            foreach (var enemy in damageType.DamageArea.GetEnemiesInArea())
            {
                if (enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damageValue);
                }
            }
        }

        private void OnDamageDone()
        {
            currentIndex++;
            if (currentIndex > lastIndex)
            {
                currentIndex = 0;
                Debug.Log("SIMULATION DONE"); 
                return; 
            }

            currentDamageType = CombatSimulator.GetValueAtIndex(currentIndex);
            Invoke(nameof(Callback), DELAY); 
        }

        private void Callback()
        {
            SetDamagePayload(currentDamageType);
        }
        #endregion

        #region LOAD
        #endregion

        #region SAVE
        // [Button(ButtonHeight = 40), PropertySpace]
        private void SaveFullSimulationPayload() { }
        #endregion
    }

    // TODO : show dps and total damage (over how much sec) for better readability
    [Serializable]
    public class DamageType
    {
        [SerializeField, Range(1, 100)] protected int damage = 1;
        [SerializeField] protected DamageArea damageArea; 
        public int Damage => damage; 
        public DamageArea DamageArea => damageArea;

        //[Space, SerializeField] protected AudioClip damageSFX;
        //[SerializeField] protected ParticleSystem damageVFX;

        /* public virtual void DoAudiovisualFeedback()
        {

        } */

        // [Button(size:ButtonSizes.Small)]
        protected void SaveTemplateToScriptableObject()
        {

        }
    }

    [Serializable]
    public class DamageType_Burst : DamageType
    {


    }

    [Serializable]
    public class DamageType_DOT : DamageType
    {
        [SerializeField, Range(2, 20)] private int tickAmount = 3;
        [SerializeField, Range(0.1f, 2f)] private float tickDelay = 0.5f;
        public int TickAmount => tickAmount;
        public float TickDelay => tickDelay;
    }

    [Serializable]
    public class DamageType_Delayed : DamageType
    {
        [SerializeField, Range(0.5f, 5f)] private float damageDelay = 1f;
        public float DamageDelay => damageDelay;    
    }
}
