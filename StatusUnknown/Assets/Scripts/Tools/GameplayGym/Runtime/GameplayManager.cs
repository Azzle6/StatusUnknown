using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using StatusUnknown.Utils.AssetManagement;

namespace StatusUnknown.CoreGameplayContent
{
    [RequireComponent(typeof(AudioSource))] 
    public class GameplayManager : MonoBehaviour
    {
        [Header("General")]
        //[SerializeField] AudioSource source; 
        [SerializeField] private CombatSimulatorSO CombatSimulatorSO;
        [SerializeField] private EnemyEncounterConfigSO EnemyEncounterSO;
        [SerializeField] private string encounterSaveName = "Encounter_Difficulty_Num"; 

        private const int DELAY = 1;

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

        [Header("Save & Load")]
        [SerializeField] private GameObject[] prefab_enemy = new GameObject[3];
        private const string LOG_ERROR_OVERWRITE_ENCOUNTER =
            "Encounter could not be saved. Please provide a valid name (different from \"Encounter_Difficulty_Num\"). \n" +
            "If you want to overwrite an existing encounter, use the same name in the \"Encounter Save Name\" field.";

        private const string LOG_ERROR_ENCOUNTER_NULL = "Could not generate encounter. Make sure the \"Enemy Encounter SO\" field is not empty.";

        private void OnEnable()
        {
            abilityConfigTemplates = new AbilityConfigTemplate[]
            {
                template_burst, 
                template_overTime,
                template_delayed,
            };

            if (CombatSimulatorSO == null) return; 

            currentAbilityData = CombatSimulatorSO.GetRootAbilityData();
        }

        [Button(ButtonHeight = 40), PropertySpace, GUIColor("green")]
        public void GenerateEncounter()
        {
            GameObject[] curentlySpawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < curentlySpawnedEnemies.Length; i++)
            {
                DestroyImmediate(curentlySpawnedEnemies[i]);
            }

            try
            {
                for (int i = 0; i < EnemyEncounterSO.EnemyDatas.Length; i++)
                {
                    EnemyData currentEnemyData = EnemyEncounterSO.EnemyDatas[i];
                    GameObject instance = StatusUnknown_AssetManager.InstantiatePrefabAtPosition(prefab_enemy[currentEnemyData.enemyConfig.Type_ID], currentEnemyData.position);
                }
            }
            catch (Exception e) 
            {
                Debug.LogError(string.Concat(LOG_ERROR_ENCOUNTER_NULL, $"\n Full error message infos {e.Message}"));
            }
        }

        #region CORE LOGIC
        [Button(ButtonHeight = 100), PropertySpace, GUIColor("green")]
        public void StartSimulation() // Entry Point (done once)
        {
            currentIndex = 0;
            lastIndex = CombatSimulatorSO.GetAbilitiesArrayLength() - 1;
            StopAllCoroutines(); 
            CancelInvoke(); 

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

            currentAbilityData = CombatSimulatorSO.GetAbilityDataAtIndex(currentIndex);
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
        [Button(ButtonHeight = 40), PropertySpace, GUIColor("yellow")]
        private void SaveEncounter() 
        {
            if (string.Equals(encounterSaveName, "Encounter_Difficulty_Num"))
            {
                Debug.LogError(LOG_ERROR_OVERWRITE_ENCOUNTER);
                return; 
            }

            GameObject[] currentlySpawnedEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            EnemyEncounterConfigSO enemyEncounterConfigSO = ScriptableObject.CreateInstance<EnemyEncounterConfigSO>();
            enemyEncounterConfigSO.name = encounterSaveName; 
            enemyEncounterConfigSO.EnemyDatas = new EnemyData[currentlySpawnedEnemies.Length];

            for (int i = 0; i < currentlySpawnedEnemies.Length; i++)
            {
                enemyEncounterConfigSO.EnemyDatas[i] = new EnemyData()
                {
                    enemyConfig = currentlySpawnedEnemies[i].GetComponent<Enemy>().EnemyConfigSO,
                    position = currentlySpawnedEnemies[i].transform.position
                };
            }

            StatusUnknown_AssetManager.SaveSO(enemyEncounterConfigSO, StatusUnknown_AssetManager.SAVE_PATH_ENCOUNTER, encounterSaveName, ".asset");
            encounterSaveName = "Encounter_Difficulty_Num"; // cheap solution to avoid overwriting existing asset by accident
        }

        // [Button(ButtonHeight = 40), PropertySpace]
        private void SaveFullSimulationPayload() { }
        #endregion
    }

    #region Templates
    // TODO FEATURE: show dps and total damage (over how much sec) for better readability
    // TODO FEATURE : set data based on curve (locally, from spreadsheet) 
    [Serializable]
    public abstract class AbilityConfigTemplate
    {
        [SerializeField] protected EAbilityType abilityType = EAbilityType.Offense;

        [SerializeField] protected string abilitySaveName = "Ability_Type_Name";
        [SerializeField] protected GameObject damageArea;
        [SerializeField] protected EPayloadType PayloadType; 

        [SerializeField, Range(1, 100)] protected int payloadValue = 1;
        protected const string LOG_ERROR_OVERWRITE_ABILITY =
            "Ability could not be saved. Please provide a valid name (different from \"Ability_Type_Name\"). \n" +
             "If you want to overwrite an existing ability, use the same name in the \"Ability Save Name\" field."; 

        //[Space, SerializeField] protected AudioClip damageSFX;
        //[SerializeField] protected ParticleSystem damageVFX;

        /* public virtual void DoAudiovisualFeedback()
        {

        } */

        [Button, PropertySpace, GUIColor("yellow")]
        protected abstract void SaveAbility(); 
    }

    [Serializable]
    public class DamageType_Burst : AbilityConfigTemplate 
    {
        protected override void SaveAbility()
        {
            if (string.Equals(abilitySaveName, "Ability_Type_Name"))
            {
                Debug.LogError(LOG_ERROR_OVERWRITE_ABILITY);
                return;
            }

            AbilityConfigSO_Burst AbilityConfigSO = ScriptableObject.CreateInstance<AbilityConfigSO_Burst>();
            AbilityConfigSO.name = abilitySaveName;
            AbilityConfigSO.SetAbilityInfos(abilitySaveName, PayloadType, damageArea, payloadValue);

            StatusUnknown_AssetManager.SaveSO(AbilityConfigSO, StatusUnknown_AssetManager.SAVE_PATH_ABILITY, abilitySaveName, ".asset");
            abilitySaveName = "Ability_Type_Name"; 
        }
    }

    [Serializable]
    public class DamageType_OverTime : AbilityConfigTemplate
    {
        [SerializeField, Range(2, 20)] private int tickAmount = 3;
        [SerializeField, Range(0.1f, 2f)] private float tickDelay = 0.5f;

        protected override void SaveAbility()
        {
            if (string.Equals(abilitySaveName, "Ability_Type_Name"))
            {
                Debug.LogError(LOG_ERROR_OVERWRITE_ABILITY);
                return;
            }

            AbilityConfigSO_OverTime AbilityConfigSO = ScriptableObject.CreateInstance<AbilityConfigSO_OverTime>();
            AbilityConfigSO.name = abilitySaveName;
            AbilityConfigSO.SetAbilityInfos(abilitySaveName, PayloadType, damageArea, payloadValue);
            AbilityConfigSO.TickAmount = tickAmount;
            AbilityConfigSO.TickDelay = tickDelay;

            StatusUnknown_AssetManager.SaveSO(AbilityConfigSO, StatusUnknown_AssetManager.SAVE_PATH_ABILITY, abilitySaveName, ".asset");
            abilitySaveName = "Ability_Type_Name";
        }
    }

    [Serializable]
    public class DamageType_Delayed : AbilityConfigTemplate
    {
        [SerializeField, Range(0.5f, 5f)] private float damageDelay = 1f;

        protected override void SaveAbility()
        {
            if (string.Equals(abilitySaveName, "Ability_Type_Name"))
            {
                Debug.LogError(LOG_ERROR_OVERWRITE_ABILITY);
                return;
            }

            AbilityConfigSO_Delayed AbilityConfigSO = ScriptableObject.CreateInstance<AbilityConfigSO_Delayed>();
            AbilityConfigSO.name = abilitySaveName;
            AbilityConfigSO.SetAbilityInfos(abilitySaveName, PayloadType, damageArea, payloadValue);
            AbilityConfigSO.DamageDelay = damageDelay;

            StatusUnknown_AssetManager.SaveSO(AbilityConfigSO, StatusUnknown_AssetManager.SAVE_PATH_ABILITY, abilitySaveName, ".asset");
            abilitySaveName = "Ability_Type_Name";
        }
    }
    #endregion 
}
