using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Object = UnityEngine.Object;
using StatusUnknown.CoreGameplayContent;

// -- PROTOTYPING GOALS --
// A set of centralised Editor Windows to quickly manipulate : gameplay data, audio, visuals, animations, etc..
    // And navigate through complex design content like :
        // Weapons & Modules
        // Enemies
        // NPCs
        // The Player Character (3Cs)

// A (set of) centralised window(s) to quickly create/edit/import/export gameplay data from or to spreadsheet
// A (set of) centralised window(s) to quickly edit/import data from design documents (notion)

namespace StatusUnknown.Utils.AssetManagement
{
    public enum PatternMatchingRule { [HideInInspector] NONE = -1, ShortName, LongName }

    public enum PrefabType { [HideInInspector] NONE = -1, Character, Enemy, Weapon, Props, Area, UI }
    public enum ModulePlaystyle { [HideInInspector] NONE = -1, Offense, Defense, Support }
    public enum ModuleType { [HideInInspector] NONE = -1, Excelsior, Hera, Pulse }

    public class ScriptableObjectsContentManager : MonoBehaviour
    {
        public static ScriptableObjectsContentManager Instance;

        [Header("Labels")]
        [SerializeField] private List<PrefabType> prefabType; 
        [SerializeField] private List<ModulePlaystyle> playstyleType;
        [SerializeField] private List<ModuleType> moduleTypes;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;
        }

        // [ValidateInput("MustHaveMeshRenderer")]
        //public GameObject prefab;
        //public bool MustHaveMeshRenderer;
        [Header("Data")]
        [Space, SerializeField] private List<GameObject> loadedPrefabs = new List<GameObject>();
        [SerializeField] private List<AbilityConfigSO_Base> loadedAbilities = new List<AbilityConfigSO_Base>();

        //public bool MustHaveMeshRenderer;

        [PropertySpace, Button, GUIColor("yellow")]
        public void LoadModulesWithLabels()
        {
            (List<ModulePlaystyle> playstyle, List<ModuleType> type) filters = new(playstyleType, moduleTypes); 

            bool processing = false;
            string labelsToFetch = ""; 

            if (processing) return;
            processing = true;

            List<string> queryFilters = new List<string>();
            foreach (var item in filters.playstyle)
            {
                queryFilters.Add(item.ToString()); 
            } 
            foreach (var item in filters.type)
            {
                queryFilters.Add(item.ToString()); 
            }

            for (int i = 0; i < queryFilters.Count; i++)
            {
                string separator = i != queryFilters.Count() - 1 ? " " : ""; 
                labelsToFetch += string.Concat("l:", queryFilters[i], separator); 
            }

            StatusUnknown_AssetManager.LoadAssetsWithMatchingLabels(labelsToFetch, out List<Object> list, StatusUnknown_AssetManager.SAVE_PATH_ABILITY);

            loadedAbilities.Clear(); 
            foreach (var item in list)
            {
                Debug.Log(item.name);
                loadedAbilities.Add(item as AbilityConfigSO_Base); 
            } 
        }

        [PropertySpace, Button, GUIColor("yellow")]
        public void LoadPrefabsWithLabels()
        {
            string labelsToFetch = string.Empty;

            List<string> queryFilters = new List<string>();
            foreach (var item in prefabType)
            {
                queryFilters.Add(item.ToString());
            }

            for (int i = 0; i < queryFilters.Count; i++)
            {
                string separator = i != queryFilters.Count() - 1 ? " " : "";
                labelsToFetch += string.Concat("l:", queryFilters[i], separator);
            }

            StatusUnknown_AssetManager.LoadAssetsWithMatchingLabels(labelsToFetch, out List<Object> list, StatusUnknown_AssetManager.SAVE_PATH_PREFABS, true);

            loadedPrefabs.Clear();
            foreach (var item in list)
            {
                Debug.Log(item.name);
                loadedPrefabs.Add(item as GameObject);
            }
        } 

        //[PropertySpace, Button]
        public void LoadAssetsOfType(PatternMatchingRule patternMatching = PatternMatchingRule.NONE)
        {
            List<string> fruits =
                    new List<string> { "apple", "passionfruit", "banana", "mango",
                                    "orange", "blueberry", "grape", "strawberry" };

            IEnumerable<string> filteredList = new List<string>();

            switch (patternMatching)
            {
                case PatternMatchingRule.ShortName:
                    filteredList = fruits.Where(fruit => fruit.Length <= 5);
                    // loadedAssets = StatusUnknown_AssetManager.LoadAssetsOfType(asset => GetType(Object)); 
                    break;
                case PatternMatchingRule.LongName:
                    filteredList = fruits.Where(fruit => fruit.Length > 5);
                    break;
                case PatternMatchingRule.NONE:
                    break;
            }

            foreach (string fruit in filteredList)
            {
                Debug.Log($"All fruits matching pattern {patternMatching} : {fruit}");
            }
        }
    }
}

