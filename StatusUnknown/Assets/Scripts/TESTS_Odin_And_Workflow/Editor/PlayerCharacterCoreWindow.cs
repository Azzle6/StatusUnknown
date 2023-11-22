using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using StatusUnknown.CoreGameplayContent;
using System;
using UnityEditor;
using UnityEngine;

public class PlayerCharacterCoreWindow : OdinEditorWindow
{
    [MenuItem("Status Unknown/Odin Tests/Player Character Editor")]
    private static void OpenWindow()
    {
        PlayerCharacterCoreWindow window = GetWindow<PlayerCharacterCoreWindow>("Player Character Editor");
        window.minSize = new Vector2(1920 * 0.5f, 1080 * 0.5f);
        window.maxSize = new Vector2(1920, 1080);
        window.Show();
    } 

    [EnumToggleButtons]
    public EAbilityType Abilities;

    [ShowIf("Abilities", InfoMessageType.Info)]
    public Vector3 show_Info;

    [PreviewField(60), HideLabel]
    [HorizontalGroup("Split", 60)]
    public Sprite sprite;

    [HorizontalGroup("Split/Amount")]
    [Button("Weapon Default Name")]
    public void SetName_Default_Weapon() { saveName = "Weapon_Basic_Melee"; }

    [HorizontalGroup("Split/Amount")]
    [Button("Module Default Name")]
    public void SetName_Default_Module() { saveName = "Module_Basic_Offense"; }

    [HorizontalGroup("Split/Amount")]
    [Button("NPC Default Name")]
    public void SetName_Default_NPC() { saveName = "NPC_Name"; }

    [TabGroup("Character", "Character")]
    public string character = "Player Character";

    [TabGroup("Weapon", "Weapon")]
    public Weapon Weapon = new Weapon();

    [TabGroup("Weapon", "Stats")]
    public StatsInfos[] StartingStats = new StatsInfos[1];

    [TabGroup("Weapon", "Modules")]
    public ModuleInfos[] StartingModules = new ModuleInfos[3];

    [TabGroup("Character", "Controls")]
    public ControlsSettings ControlSettings = new ControlsSettings();

    [TabGroup("Character", "Camera")]
    public CameraSettings CameraSettings = new CameraSettings();

    [BoxGroup("Save"), Required]
    public string saveName;

    [BoxGroup("Save"), FolderPath, SerializeField, Required]
    private string rootGameplayDataPath = "Assets/Data/Gameplay";
}

[Serializable]
public class Weapon // load or create new weapon
{
    [SerializeField] private GameObject weaponPrefab; // previz
    [SerializeField] private AudioClip shootDefaultSound; // previz
    [SerializeField] private Sprite shootDefaultFXTexture; // previz
    [SerializeField] private AnimationClip shootDefaultAnimation; // previz
    [SerializeField, TextArea(2, 10)] private string description;
}

[Serializable]
public class ModuleInfos // load or create new module
{
    [SerializeField] protected EAbilityType abilityType = EAbilityType.Offense;
    [SerializeField] protected EPayloadType PayloadType;
    [SerializeField, Range(0, 100)] protected int payloadValue;
    [SerializeField] protected GameObject effectArea;
}

[Serializable]
public class StatsInfos // load or create new stat
{
    public enum Stat { [HideInInspector] NONE = -1, HP, Mana, Strength, Dexterity, Damage, MoveSpeed }

    [SerializeField] private Stat stat;

    [ProgressBar(0, 100)]
    [SerializeField] private int baseValue;

    [TextArea(2, 10)]
    [SerializeField] private string description; 
}

[Serializable]
public class CameraSettings // load or create new camera settings
{
    [SerializeField] private float zoomOutBlendTime = 0.5f;
    [SerializeField] private float followCharacterDamping = 0.2f; 
}

[Serializable]
public class ControlsSettings // load or create new controls settings
{
    [SerializeField] private string inputName = "";
}
