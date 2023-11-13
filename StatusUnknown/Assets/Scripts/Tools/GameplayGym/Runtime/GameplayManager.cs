using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// Editor, Window, PropertyDrawer
namespace StatusUnknown.CoreGameplayContent
{
    // TODO : have to make it work in editor (problems -> callbacks, coroutines)
    // [RequireComponent(typeof(UIDocument))]
    [RequireComponent(typeof(AudioSource))] 
    public class GameplayManager : MonoBehaviour
    {
        [Header("General")]
        //[SerializeField] AudioSource source; 
        [SerializeField] private CombatSimulatorScriptableObject CombatSimulator;
        private const int DELAY = 1;

        // TODO - FEATURE : add option to overwrite values of a specific SO with template OR generate new so from template
        [Header("Ability Types Template")]
        [SerializeField] private bool showTemplate = false; 
        [Space, SerializeField] private DamageType_Burst template_burst;
        [SerializeField] private DamageType_OverTime template_overTime;
        [SerializeField] private DamageType_Delayed template_delayed;
        private AbilityConfigTemplate[] abilityConfigTemplates = new AbilityConfigTemplate[3];
        private (AbilityInfos infos, AbilityConfigSO_Base so) currentAbilityData = new();

        private AbilityConfigSO_Burst currentAbilityConfigSO_Burst = null;
        private AbilityConfigSO_OverTime currentAbilityConfigSO_OverTime = null;
        private AbilityConfigSO_Delayed currentAbilityConfigSO_Delayed = null; 


        private int currentIndex; 
        private int lastIndex;

        // -- TODO FEATURE --
        // save and load combat config locally + remote
        // save and load encounter config locally + remote
        // save and load damage types (for ex DOT -> poison or fire, BURST -> stun or explosion, etc..) locally + remote 

        private void OnEnable()
        {
            abilityConfigTemplates = new AbilityConfigTemplate[]
            {
                template_burst, 
                template_overTime,
                template_delayed,
            };

            if (CombatSimulator == null) return; 

            currentAbilityData = CombatSimulator.GetRootAbilityData();
        }

        #region CORE LOGIC
        [Button(ButtonHeight = 100), PropertySpace]
        public void StartSimulation() // Entry Point (done once)
        {
            currentIndex = 0;
            lastIndex = CombatSimulator.GetAbilitiesArrayLength() - 1;

            StartCoroutine(nameof(SetDamagePayload)); 
        }

        // REFACTOR : strategy pattern for different types of damage application
        // REFACTOR : as callback when damage payload is done
        // for now, just a plain ugly switch case
        // AbilityConfigTemplate damageType; 
        //int damageValue; 
        GameObject currentAreaObj;
        private List<Enemy> currentEnemiesInArea; 
        private IEnumerator SetDamagePayload()
        {
            Debug.Log("setting payload type to : " + currentAbilityData.infos.PayloadType);

            //damageType = abilityConfigTemplates[(int)abilityInfos.PayloadType];
            //damageValue = damageType.Damage;

            if (currentAbilityData.infos.Area != null)
            {
                currentAreaObj = Instantiate(currentAbilityData.infos.Area); 
            }

            yield return new WaitForFixedUpdate(); 
            // TODO : see if I can have only one type of SO and cast correctly (factory pattern ?)
            switch (currentAbilityData.infos.PayloadType)
            {
                case EPayloadType.Burst :
                    currentAbilityConfigSO_Burst = (AbilityConfigSO_Burst)currentAbilityData.so;  
                    DoDamage_Burst(); 
                    break;
                case EPayloadType.OverTime:
                    currentAbilityConfigSO_OverTime =  (AbilityConfigSO_OverTime)currentAbilityData.so;
                    StartCoroutine(nameof(DoDamage_DOT)); // TODO : change with custom struct for cooldown/tick delay
                    break; 
                case EPayloadType.Delayed:
                    currentAbilityConfigSO_Delayed = (AbilityConfigSO_Delayed)currentAbilityData.so;
                    Invoke(nameof(DoDamage_Delayed), currentAbilityConfigSO_Delayed.DamageDelay);
                break; 
            } 
        }

        private void DoDamage_Burst()
        {
            Debug.Log("applying burst damage"); 
            ApplyDamage(currentAbilityData.infos.PayloadValue);
            OnDamageDone(); 
        }

        private IEnumerator DoDamage_DOT()
        {
            for (int i = 0; i < currentAbilityConfigSO_OverTime.TickAmount; i++)
            {
                Debug.Log("applying DOT damage");
                ApplyDamage(currentAbilityData.infos.PayloadValue);
                yield return new WaitForSeconds(currentAbilityConfigSO_OverTime.TickDelay);
            }

            StopCoroutine(nameof(DoDamage_DOT));
            OnDamageDone();
        }

        // ERROR : "Trying to Invoke method: GameplayManager.ApplyDamage couldn't be called."
        private void DoDamage_Delayed()
        {
            Debug.Log($"applying damage with delay of {currentAbilityConfigSO_Delayed.DamageDelay} seconds");
            ApplyDamage(currentAbilityData.infos.PayloadValue);
            OnDamageDone(); 
        }

        private void ApplyDamage(int damageValue)
        {
            currentEnemiesInArea = currentAreaObj.GetComponent<DamageArea>().GetEnemiesInArea();
            foreach (var enemy in currentEnemiesInArea) 
            {
                if (enemy != null && enemy.TryGetComponent(out IDamageable damageable))
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

            currentAbilityData = CombatSimulator.GetAbilityDataAtIndex(currentIndex);
            Invoke(nameof(Callback), DELAY); 
        }

        private void Callback()
        {
            StartCoroutine(nameof(SetDamagePayload));
        }
        #endregion

        #region LOAD
        #endregion

        #region SAVE
        // [Button(ButtonHeight = 40), PropertySpace]
        private void SaveFullSimulationPayload() { }
        #endregion
    }

    // TODO FEATURE: show dps and total damage (over how much sec) for better readability
    // TODO FEATURE : set data based on curve (locally, from spreadsheet) 
    [Serializable]
    public class AbilityConfigTemplate
    {
        [SerializeField] private EAbilityType abilityType = EAbilityType.Offense;
        [SerializeField, Range(1, 100)] protected int damage = 1;
        [SerializeField] protected GameObject damageArea;

        public int Damage => damage; 
        public GameObject DamageArea => damageArea;

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
    public class DamageType_Burst : AbilityConfigTemplate
    {


    }

    [Serializable]
    public class DamageType_OverTime : AbilityConfigTemplate
    {
        [SerializeField, Range(2, 20)] private int tickAmount = 3;
        [SerializeField, Range(0.1f, 2f)] private float tickDelay = 0.5f;
        public int TickAmount => tickAmount;
        public float TickDelay => tickDelay;
    }

    [Serializable]
    public class DamageType_Delayed : AbilityConfigTemplate
    {
        [SerializeField, Range(0.5f, 5f)] private float damageDelay = 1f;
        public float DamageDelay => damageDelay;    
    }
}
