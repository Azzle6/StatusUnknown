using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using StatusUnknown.Utils.AssetManagement;
using TMPro;
using UnityEditor;
using System.Linq;

namespace StatusUnknown.CoreGameplayContent
{
    // TODO : split this class to separate Runtime from Editor..

    [RequireComponent(typeof(AudioSource))]
    public class GameplayManager : MonoBehaviour, INotifyValueChanged<bool>
    {
        #region Data
        [Header("General")]
        //[SerializeField] AudioSource source; 
        [SerializeField] private AbilityConfigSO_Base[] buildToSimulate; 
        private int buildToSimulatePreviousLength; 

        [SerializeField] private string buildSaveName = "Build_Playstyle_Num"; 
        private const int DELAY_BETWEEN_ABILITIES = 1;
        private int currentIndex;
        private int lastIndex;
        private int damageCounter;
        public static Action OnSimulationDone;

        [Header("Build")]
        [Space, SerializeField] private CombatSimulatorSO customBuildSO;
        [SerializeField] private bool useCustomBuildSO;
        public bool value
        {
            get { return customBuildSO; }
            set
            {
                using (ChangeEvent<bool> evt = ChangeEvent<bool>.GetPooled(useCustomBuildSO, value))
                {
                    if (evt.newValue == true && customBuildSO != null)
                    {
                        RepopulateBuildArray();
                        RefreshDamageAreaStack();
                    }
                }
            }
        } // Editor

        private CombatSimulatorSO simulatorInstance;
        [SerializeField] private List<GameObject> spawnedAreasObj = new List<GameObject>();  

        GameObject currentActiveAreaObj; // Runtime
        private List<Enemy> currentEnemiesInArea; // Runtime

        [Header("Ability Types Template")]
        [Space, SerializeField] private DamageType_Burst template_burst;
        [SerializeField] private DamageType_OverTime template_overTime;
        [SerializeField] private DamageType_Delayed template_delayed;
        private AbilityConfigTemplate[] abilityConfigTemplates = new AbilityConfigTemplate[3];
        private (AbilityInfos infos, AbilityConfigSO_Base so) currentAbilityData = new(); // Runtime
        private AbilityConfigSO_Burst currentAbilityConfigSO_Burst = null;
        private AbilityConfigSO_OverTime currentAbilityConfigSO_OverTime = null;
        private AbilityConfigSO_Delayed currentAbilityConfigSO_Delayed = null;
        //[SerializeField] private bool showTemplate = false; 

        [Header("Encounter")]
        [Space, SerializeField] private EnemyEncounterConfigSO EnemyEncounterSO;
        [SerializeField] private string encounterSaveName = "Encounter_Difficulty_Num";

        [Header("UI")]
        [SerializeField] private TMP_Text totalDamage_UI;
        [SerializeField] private GameplayDataSO gameplayDataSO; 

        [Header("Save & Load")]
        [SerializeField] private GameObject[] prefab_enemy = new GameObject[3];
        private const string LOG_ERROR_OVERWRITE_ENCOUNTER =
            "Encounter could not be saved. Please provide a valid name (different from \"Encounter_Difficulty_Num\"). \n" +
            "If you want to overwrite an existing encounter, use the same name in the \"Encounter Save Name\" field.";

        private const string LOG_ERROR_OVERWRITE_BUILD =
            "Build could not be saved. Please provide a valid name (different from \"Build_Playstyle_Num\"). \n" +
            "If you want to overwrite an existing build, use the same name in the \"Build Save Name\" field.";

        private const string LOG_ERROR_ENCOUNTER_NULL = "Could not generate encounter. Make sure the \"Enemy Encounter SO\" field is not empty.";
        private const string LOG_ERROR_BUILD_NULL = "No abilities were found on your \"buildToSimulate\" array.";

        [Header("-- DEBUG --")]
        [SerializeField, Tooltip("If you deleted some areas by mistake")] private bool refreshAllAreas; 
        #endregion

        #region Unity Callbacks
        private void OnValidate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            
            value = useCustomBuildSO; // avoid raw OnValidate calls like this on

            // From Play to Editor
            if (gameplayDataSO.MustRefreshAreasObj || refreshAllAreas)
            {
                gameplayDataSO.MustRefreshAreasObj = false;
                Debug.Log(EditorApplication.isPlaying); 
                if (gameplayDataSO.ExitedPlayMode)
                {
                    //spawnedAreasObj = GameObject.FindGameObjectsWithTag("Area").ToList(); // going to need a better solution
                }

                RefreshDamageAreaStack();
                return;
            }

            // when going from play to edit, only way to avoid refreshing.. ?
            if (EditorApplication.isCompiling ||
                EditorApplication.isUpdating ||
                GameObject.FindGameObjectsWithTag("Area").Length == buildToSimulate.Length) 
                return;

            // In Editor
            if (buildToSimulatePreviousLength != buildToSimulate.Length && buildToSimulate.Length >= 0)
            {
                try
                {
                    spawnedAreasObj.Clear();
                    spawnedAreasObj = GameObject.FindGameObjectsWithTag("Area").ToList(); // going to need a better solution

                    if (buildToSimulate.Length - buildToSimulatePreviousLength > 0)
                    {
                        for (int i = buildToSimulatePreviousLength; i < buildToSimulate.Length; i++)
                        {
                            GameObject prefabInstance = buildToSimulate[i].GetArea();
                            //prefabInstance.name = string.Empty;
                            //prefabInstance.name = string.Concat(prefabInstance.name, " ", buildToSimulate[i].name); 
                            spawnedAreasObj.Add(Instantiate(prefabInstance));
                        }
                    }
                    else if (buildToSimulate.Length - buildToSimulatePreviousLength < 0)
                    {
                        for (int i = buildToSimulatePreviousLength - 1; i >= buildToSimulate.Length; i--)
                        {
                            DestroyImmediate(spawnedAreasObj[i]);
                        }
                    }
                }
                catch (NullReferenceException e)
                {
                    RefreshDamageAreaStack(e);
                }

                buildToSimulatePreviousLength = buildToSimulate.Length;
            }
        }

        private void OnEnable()
        {
            // From Editor to Play
            BindAreaSceneObjectsToList();
            EditorApplication.playModeStateChanged += OnEditorApplicationStateChanged;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnEditorApplicationStateChanged;
        }
        #endregion

        // In Play
        #region RUNTIME
        private void OnEditorApplicationStateChanged(PlayModeStateChange currentPlayMode)
        {
            if (currentPlayMode == PlayModeStateChange.EnteredPlayMode)
            {
                gameplayDataSO.MustRefreshAreasObj = false;
                gameplayDataSO.ExitedPlayMode = false;
            }
            else if (currentPlayMode == PlayModeStateChange.ExitingPlayMode)
            {
                gameplayDataSO.MustRefreshAreasObj = true;
                gameplayDataSO.ExitedPlayMode = true; 
            }
        }

        [Button(ButtonHeight = 100), PropertySpace, GUIColor("green")]
        public void StartSimulation() // Entry Point (done once)
        {
            Init();

            StopAllCoroutines(); 
            CancelInvoke(); 

            StartCoroutine(nameof(SetDamagePayload)); 
        }

        private void Init()
        {
            totalDamage_UI.SetText(string.Empty);

            abilityConfigTemplates = new AbilityConfigTemplate[]
            {
                template_burst,
                template_overTime,
                template_delayed,
            };

            if (useCustomBuildSO && customBuildSO != null)
            {
                simulatorInstance = customBuildSO;
            }
            else
            {
                if (buildToSimulate.Length == 0)
                {
                    Debug.LogError(LOG_ERROR_BUILD_NULL);
                    return;
                }

                CreateNewBuildInstance();
            }

            currentIndex = 0;
            lastIndex = simulatorInstance.GetAbilitiesArrayLength() - 1;
            currentAbilityData = simulatorInstance.GetRootAbilityData();

            gameplayDataSO.Init();
        }

        private void BindAreaSceneObjectsToList()
        {
            Debug.Log("binding area scene objects to list");
            //spawnedAreasObj.Clear();
            //spawnedAreasObj = GameObject.FindGameObjectsWithTag("Area").ToList();
            foreach (var item in spawnedAreasObj)
            {
                item.SetActive(false);
            }

            //spawnedAreasObj.Reverse();

            buildToSimulatePreviousLength = buildToSimulate.Length;
        }

        // REFACTOR : strategy pattern for different types of damage application
        // REFACTOR : as callback when damage payload is done
        // for now, just a plain ugly switch case
        // AbilityConfigTemplate damageType; 
        //int damageValue; 
        private IEnumerator SetDamagePayload()
        {
            //Debug.Log("setting payload type to : " + currentAbilityData.infos.PayloadType);

            //damageType = abilityConfigTemplates[(int)abilityInfos.PayloadType];
            //damageValue = damageType.Damage;

            if (currentAbilityData.infos.Area != null)
            {
                currentActiveAreaObj = spawnedAreasObj[currentIndex]; 
                currentActiveAreaObj.SetActive(true);
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
            //Debug.Log("applying burst damage"); 
            ApplyDamage(currentAbilityData.infos.PayloadValue);
            OnDamageDone(); 
        }

        private IEnumerator DoDamage_DOT()
        {
            for (int i = 0; i < currentAbilityConfigSO_OverTime.TickAmount; i++)
            {
                //Debug.Log("applying DOT damage");
                ApplyDamage(currentAbilityData.infos.PayloadValue);
                yield return new WaitForSeconds(currentAbilityConfigSO_OverTime.TickDelay);
            }

            StopCoroutine(nameof(DoDamage_DOT));
            OnDamageDone();
        }

        // ERROR : "Trying to Invoke method: GameplayManager.ApplyDamage couldn't be called."
        private void DoDamage_Delayed()
        {
            //Debug.Log($"applying damage with delay of {currentAbilityConfigSO_Delayed.DamageDelay} seconds");
            ApplyDamage(currentAbilityData.infos.PayloadValue);
            OnDamageDone(); 
        }

        private void ApplyDamage(int damageValue)
        {
            damageCounter += damageValue;

            currentEnemiesInArea = currentActiveAreaObj.GetComponent<DamageArea>().GetEnemiesInArea();
            foreach (var enemy in currentEnemiesInArea) 
            {
                if (enemy != null && enemy.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damageValue);
                }
            }

            Debug.Log("damage"); 
        }

        private void OnDamageDone()
        {
            currentIndex++;
            if (currentIndex > lastIndex)
            {
                currentIndex = 0;
                Debug.Log("SIMULATION DONE");

                OnSimulationDone();
                Invoke(nameof(PrintTotalDamageDone), 0.5f); 
                return; 
            }

            currentAbilityData = simulatorInstance.GetAbilityDataAtIndex(currentIndex);
            Invoke(nameof(Callback_DamageApplied), DELAY_BETWEEN_ABILITIES); 
        }

        private void PrintTotalDamageDone()
        {
            totalDamage_UI.SetText(string.Concat("Total Damage : ", gameplayDataSO.TotalDamageDone.ToString()));
        }

        private void Callback_DamageApplied()
        {
            StartCoroutine(nameof(SetDamagePayload));
        }
        #endregion

        // In Editor
        #region LOAD
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

        private void RepopulateBuildArray()
        {
            buildToSimulate = new AbilityConfigSO_Base[customBuildSO.abilitiesConfig.Length];
            for (int i = 0; i < buildToSimulate.Length; i++)
            {
                buildToSimulate[i] = customBuildSO.abilitiesConfig[i];
            }
        }
        #endregion

        #region SAVE
        private void CreateNewBuildInstance()
        {
            simulatorInstance = ScriptableObject.CreateInstance<CombatSimulatorSO>();
            simulatorInstance.abilitiesConfig = new AbilityConfigSO_Base[buildToSimulate.Length];
            Array.Copy(buildToSimulate, simulatorInstance.abilitiesConfig, buildToSimulate.Length);
        }

        [Button(ButtonHeight = 40), PropertySpace, GUIColor("yellow")]
        public void SaveBuild()
        {
            if (string.Equals(buildSaveName, "Build_Playstyle_Num"))
            {
                Debug.LogError(LOG_ERROR_OVERWRITE_BUILD);
                return;
            }

            CreateNewBuildInstance();

            simulatorInstance.name = buildSaveName;
            StatusUnknown_AssetManager.SaveSO(simulatorInstance, StatusUnknown_AssetManager.SAVE_PATH_BUILD, buildSaveName, ".asset");
            buildSaveName = "Build_Playstyle_Num"; // cheap solution to avoid overwriting existing asset by accident
        }

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

        #region Error Handling
        private void RefreshDamageAreaStack(NullReferenceException nre = null)
        {
            if (nre != null)
            {
                Debug.LogError("Error : " + nre?.Message);
            }

            Debug.Log("refreshing damage area stack");
            gameplayDataSO.ExitedPlayMode = false;
            refreshAllAreas = false;

            int initialCount = spawnedAreasObj.Count;
            for (int i = 0; i < initialCount; i++)
            {
                Destroy(spawnedAreasObj[i]);
            } 

            spawnedAreasObj.Clear();   

            for (int i = 0; i < buildToSimulate.Length; i++)
            {
                GameObject prefabInstance = buildToSimulate[i].GetArea();
                //prefabInstance.name = string.Empty; 
                //prefabInstance.name = string.Concat(prefabInstance.name, " ", buildToSimulate[i].name);
                spawnedAreasObj.Add(Instantiate(prefabInstance));
            } 

            buildToSimulatePreviousLength = buildToSimulate.Length;
        }
        #endregion

        #region Implementations

        public void SetValueWithoutNotify(bool v)
        {
            useCustomBuildSO = v;
            Debug.Log("SetValueWithoutNotify: " + v);
        }
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
