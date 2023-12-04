using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using StatusUnknown;
using StatusUnknown.Content;
using StatusUnknown.Utils.AssetManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ModuleCreatorEditorWindow : OdinEditorWindow
{
    [FolderPath, SerializeField, Required]
    private string path = "Assets/Data/Gameplay";

    [SerializeField, Required]
    private string saveName = "Ability_Save_Name";

    [SerializeField]
    [OnValueChanged(nameof(FindConfig))]
    private EAbilityType AbilityType; 

    [InlineEditor(Expanded = true)]
    public AbilityConfigSO_Burst abilityConfig;

    [GUIColor(0.5f,1f,0.5f)]
    [ButtonGroup("Base")]
    private void SaveConfig()
    {
        if (string.IsNullOrEmpty(saveName) || string.Equals(saveName, "Ability_Save_Name"))
        {
            Debug.LogError("Some error");
            return;
        }

        if (!AssetDatabase.Contains(abilityConfig))
            AssetDatabase.CreateAsset(abilityConfig, path + "/" + abilityConfig.name + ".asset");

        CreateConfig();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [GUIColor(0.5f, 0.5f, 1f)]
    [ButtonGroup("Base")]
    private void CreateConfig() 
    {
        abilityConfig = CreateInstance<AbilityConfigSO_Burst>();
    }

    [GUIColor(0.5f, 1f, 0.5f)]
    [ButtonGroup("Base")]
    private AbilityConfigSO_Burst GenerateRandomConfig() => CreateInstance<AbilityConfigSO_Burst>(); // make it with random stats and name


    private void FindConfig() 
    {
        if (abilityConfig != null)
            SaveConfig();

        List<AbilityConfigSO_Burst> templates = StatusUnknown_AssetManager.GetScriptableObjects<AbilityConfigSO_Burst>(path);
        AbilityConfigSO_Burst configTemplate = null;

        if (templates != null && templates.Count > 0)
            configTemplate = templates.Where(x => x.AbilityType == AbilityType).FirstOrDefault(); 

        if (configTemplate == null)
        {
            CreateConfig();
            abilityConfig.AbilityType = AbilityType; 
        }
        else
        {
            abilityConfig = configTemplate;
        }
    } // if none exist, create new scriptableObject and store it at proper path location
}
