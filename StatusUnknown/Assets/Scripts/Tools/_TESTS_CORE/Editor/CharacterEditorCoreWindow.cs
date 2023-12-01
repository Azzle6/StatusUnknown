using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using StatusUnknown.Content.Narrative;
using StatusUnknown.Tools;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// more advanced 
    // Odin Attribute Drawers, C# MemberInfos (System.Reflection), SirenixEditorFields, GUIHelper, EditorGUI, SirenixEditorGUI

namespace StatusUnknown.Content
{
    public struct CoreContentStrings
    {
        public const string PATH_CONTENT_ROOT = "Status Unknown/";
        public const string PATH_CONTENT_GAMEPLAY = PATH_CONTENT_ROOT + "Gameplay/";
        public const string PATH_CONTENT_NARRATIVE = PATH_CONTENT_ROOT + "Narrative/";
        public const string PATH_CONTENT_FEEDBACKS = PATH_CONTENT_ROOT + "Feedbacks/";
        public const string PATH_DATA_ROOT = "Assets/Data/";
    }

    namespace Narrative
    {
        [Serializable]
        internal class Weapon // load or create new weapon
        {
            #region Odin Property Processor
            [HideInInspector] public const bool SHOW_METADATA = false;
            #endregion

            CoreContentStrings s = new CoreContentStrings();

            [SerializeField, LabelWidth(700)] string weaponName = "New_Weapon_Name"; 
            [SerializeField, PreviewField] protected Sprite imageUI;
            [SerializeField, PreviewField] protected GameObject weaponPrefab; // previz
            [SerializeField, PreviewField] protected AudioClip shootDefaultSound; // previz
            [SerializeField, PreviewField] protected Sprite shootDefaultFXTexture; // previz
            [SerializeField, PreviewField] protected Animation shootDefaultAnimation; // previz
            [SerializeField, TextArea(5, 10)] protected string description;
            [SerializeField, PropertyRange(0, 100)] protected int unlockCost = 30;
            [PropertySpace(10), SerializeField, PropertyRange(0.1f, 10f)] private float attackSpeed = 1.5f;
            [SerializeField, PropertyRange(0, 100)] private int damage = 5;

            public int Damage { get => damage; }
            public float AttackSpeed { get => attackSpeed; }
            public string Description { get => description; }   
        }

        internal class Bow : Weapon
        {
            [SerializeField, PropertyRange(1f, 10f)] private float fireRange = 1.5f;
        }

        internal class Sword : Weapon
        {
            [SerializeField, PropertyRange(0f, 1f)] private float stunChancePercent;
        }

        [Serializable]
        public class ModuleInfos // load or create new module
        {
            [SerializeField][GUIColor(CoreToolsStrings.COLOR_ENUMS)] protected EAbilityType abilityType = EAbilityType.Offense;
            [SerializeField][GUIColor(CoreToolsStrings.COLOR_ENUMS)] protected EPayloadType PayloadType;
            [SerializeField, Range(0, 100)] protected int payloadValue;
            [SerializeField] protected GameObject effectArea;
        }

        [Serializable]
        public class StatsInfos // load or create new stat
        {
            [GUIColor(CoreToolsStrings.COLOR_ENUMS)] public enum Stat { [HideInInspector] NONE = -1, ModuleSlots, Cost, Damage, AttackSpeed }

            [SerializeField] private Stat stat;

            [ProgressBar(0, 100)]
            [SerializeField] private int baseValue;

            [TextArea(2, 10)]
            [SerializeField] private string description;
        }

        [Serializable]
        public class CameraSettings // load or create new camera settings
        {
            [HorizontalGroup("Default", 800), LabelText("@$property.NiceName"), HideLabel] public bool editSettings; 

            [SerializeField, VerticalGroup("Details"), EnableIf(nameof(editSettings)), PropertyRange(0f, 5f)] private float zoomOutBlendTime = 0.5f;
            [SerializeField, VerticalGroup("Details"), EnableIf(nameof(editSettings)), PropertyRange(0f, 1f)] private float followCharacterDamping = 0.2f;
            [SerializeField, VerticalGroup("Details"), EnableIf(nameof(editSettings))] public bool showAllSettings = false;
            public string ID;
            public FilterMode TextureFilterMode; 
        }

        [Serializable]
        public class ControlsSettings // load or create new controls settings
        {
            [SerializeField] private string inputName = "";
        }
    }
}

namespace StatusUnknown.Tools
{
    public enum Faction { Player, SAA, Hera, Excelsior, Pulse }
    public enum Item { Weapon, Armor, Augments, Modules }

    public struct CoreToolsStrings
    {
        internal const string ROOT_MENU_PATH = "Status Unknown/Tools/";

        internal const int LABEL_SIZE_SMALL = 180;
        internal const int LABEL_SIZE_MEDIUM = 250;
        internal const int BUTTON_LAYOUT_SMALL = 250;
        internal const string COLOR_SAVE = "green";
        internal const string COLOR_QOL = "yellow";
        internal const string COLOR_ENUMS = "purple";
        internal const string COLOR_DANGER = "red";
    }

    namespace CharacterEditor
    {
        public class CharacterEditorCoreWindow : OdinEditorWindow
        {

            [MenuItem(CoreToolsStrings.ROOT_MENU_PATH + "Character Editor")]
            private static void OpenWindow()
            {
                CharacterEditorCoreWindow window = GetWindow<CharacterEditorCoreWindow>("Character Editor");

                Vector2 userScreenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height); 
                window.minSize = userScreenSize * 0.9f;
                window.maxSize = userScreenSize * 1f;
                window.position = new Rect(new Vector2(100,10), window.minSize);
                window.Show(); 
            }

            [EnumToggleButtons, PropertyOrder(-1)][GUIColor(CoreToolsStrings.COLOR_ENUMS)] public Faction Faction;
            [EnumToggleButtons, PropertyOrder(0)][GUIColor(CoreToolsStrings.COLOR_ENUMS)] public Item Items;

            [PropertySpace(SpaceAfter = 25), HorizontalGroup("Base", LabelWidth = 100, Title = "BASE")]

            [VerticalGroup("Base/left", GroupName = "CHARACTER"), PropertyOrder(0)] public new string name = "Player Character";
            [VerticalGroup("Base/left")][PreviewField(300, ObjectFieldAlignment.Left), PropertyOrder(0)] public Sprite image;
            [VerticalGroup("Base/left", GroupName = "CHARACTER")][TextArea(10, 20)] public string description = string.Empty;
            [VerticalGroup("Base/left", GroupName = "CHARACTER")][HideIf("Faction", Faction.Player)] public DialogueSO[] unlockedDialogues = new DialogueSO[2];

            [VerticalGroup("Base/right", GroupName = "WEAPON")]
            [SerializeField, VerticalGroup("Base/right/weapon", GroupName = "WEAPON", VisibleIf = "@Items == Item.Weapon"), LabelWidth(CoreToolsStrings.LABEL_SIZE_SMALL), LabelText("@$value.weaponName")] private Weapon Weapon = new Weapon();
            [VerticalGroup("Base/right/weapon", GroupName = "WEAPON"), SerializeField, InfoBox("@GetWeaponDescription()", Icon = SdfIconType.Info, InfoMessageType = InfoMessageType.Info), HideLabel] string weaponInfos;
            private string GetWeaponDescription() => string.IsNullOrEmpty(Weapon.Description) ? "No Weapon Description has been written yet" : Weapon.Description;

            [VerticalGroup("Base/right/weapon", GroupName = "STATS")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public StatsInfos[] StartingStats = new StatsInfos[1];
            [VerticalGroup("Base/right/modules", GroupName = "MODULES", VisibleIf = "@Items == Item.Modules"), LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public ModuleInfos[] StartingModules = new ModuleInfos[3];

            [PropertySpace][ShowIf("Faction", Faction.Player)] public bool showCameraAndControls = false;

            [TabGroup("Character", "Controls")][ShowIf(nameof(showCameraAndControls), true)] public ControlsSettings ControlSettings = new ControlsSettings();
            [TabGroup("Character", "Camera")]
            [ShowIf(nameof(showCameraAndControls), true), LabelText("@$property.NiceName")]

            [SerializeField] private bool showAllSettings; 
            [OnStateUpdate("@#(DefaultCameraSettings.ID).State.Visible = " + nameof(showAllSettings))]
            [OnStateUpdate("@#(DefaultCameraSettings.TextureFilterMode).State.Visible = " + nameof(showAllSettings))]
            public CameraSettings DefaultCameraSettings = new CameraSettings();

            [PropertySpace] public bool showSaveFields = false;

            [ShowIf(nameof(showSaveFields), true)][BoxGroup("Save"), Required("Do not forget to fill in the \"Save Name\" field", InfoMessageType.Warning)] public string saveName = string.Empty; 

            [ShowIf(nameof(showSaveFields), true)]
            [HorizontalGroup("Save/ModulesSet", CoreToolsStrings.BUTTON_LAYOUT_SMALL)]
            [GUIColor(CoreToolsStrings.COLOR_QOL)]
            [Button("Weapon Set - Default Name")]
            public void Faction_Default_ModulesSet() { saveName = $"{Faction}_DialogueSet_ID"; }

            [ShowIf(nameof(showSaveFields), true)]
            [HorizontalGroup("Save/WeaponSet", CoreToolsStrings.BUTTON_LAYOUT_SMALL)]
            [GUIColor(CoreToolsStrings.COLOR_QOL)]
            [Button("Module Set - Default Name")]
            public void Faction_Default_WeaponSet() { saveName = $"{Faction}_WeaponSet_ID"; }

            [ShowIf(nameof(showSaveFields), true)]
            [HorizontalGroup("Save/DialogueSet", CoreToolsStrings.BUTTON_LAYOUT_SMALL)]
            [GUIColor(CoreToolsStrings.COLOR_QOL)]
            [Button("NPC Set - Default Name")]
            public void Faction_Default_DialogueSet() { saveName = $"{Faction}_DialogueSet_ID"; }

            [ShowIf("@saveName != string.Empty && showSaveFields == true")]
            [HorizontalGroup("Save/Save", CoreToolsStrings.BUTTON_LAYOUT_SMALL)]
            [PropertySpace(25), GUIColor(CoreToolsStrings.COLOR_SAVE)]
            [Button("Save", Icon = SdfIconType.Save)]
            public void SaveAsset() { }

            [ShowIf(nameof(showSaveFields), true)]
            [BoxGroup("Save"), FolderPath, SerializeField, Required]
            private string rootGameplayDataPath = "Assets/Data/Gameplay";


            internal class WeaponPropertyProcessor<T> : OdinPropertyProcessor<T> where T : Weapon
            {
                public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
                {
                    for (int i = 0; i < propertyInfos.Count; i++)
                    {
                        //Debug.Log("property name : " + propertyInfos[i].PropertyName);
                    }

                    propertyInfos.AddValue("DPS",
                        (ref Weapon w) => w.AttackSpeed * w.Damage,
                        (ref Weapon w, float value) => { },
                        new BoxGroupAttribute("Balancing Data"),
                        new EnableIfAttribute("SHOW_METADATA", true));
                }
            }

            internal class ModuleInfosPropertyProcessor : OdinPropertyProcessor<ModuleInfos>
            {
                public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
                {
                    //
                }
            }

            internal class StatsInfosPropertyProcessor : OdinPropertyProcessor<StatsInfos>
            {
                public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
                {
                    //
                }
            }

            internal class DialogueSOPropertyProcessor : OdinPropertyProcessor<DialogueSO>
            {
                public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
                {
                    //
                }
            }
        }
    }
}
