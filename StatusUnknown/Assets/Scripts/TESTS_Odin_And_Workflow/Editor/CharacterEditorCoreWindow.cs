using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using StatusUnknown.Content.Narrative;
using StatusUnknown.Content.Serializables;
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
    struct SU_Content
    {
        internal const string PATH_CONTENT_ROOT = "Status Unknown/"; 
        internal const string PATH_CONTENT_GAMEPLAY = PATH_CONTENT_ROOT + "Gameplay/";
        internal const string PATH_CONTENT_NARRATIVE = PATH_CONTENT_ROOT + "Narrative/";
    }

    namespace Narrative
    {
        [CreateAssetMenu(fileName = "Dialogue", menuName = SU_Content.PATH_CONTENT_NARRATIVE + "Dialogue")]
        public class DialogueSO : SerializedScriptableObject
        {
            [SerializeField]
            private Dictionary<Faction, string> dialogueLine = new Dictionary<Faction, string>()
            {
                { Faction.Hera, "We are Hera, and we will rule." },
                { Faction.SAA, "Yeah... You mad bro ?" },
                { Faction.Pulse, "LET'S DESTROY EVERYTHING !!!" },
            };
        }
    }

    namespace Serializables
    {
        [Serializable]
        public class Weapon // load or create new weapon
        {
            [HideInInspector] public const bool SHOW_METADATA = false;

            [SerializeField, PreviewField] protected Sprite imageUI;
            [SerializeField, PreviewField] protected GameObject weaponPrefab; // previz
            [SerializeField, PreviewField] protected AudioClip shootDefaultSound; // previz
            [SerializeField, PreviewField] protected Sprite shootDefaultFXTexture; // previz
            [SerializeField, PreviewField] protected Animation shootDefaultAnimation; // previz
            [SerializeField, TextArea(5, 10)] protected string description;
            [SerializeField, PropertyRange(0, 100)] protected int unlockCost = 30;
            [PropertySpace(10), SerializeField, PropertyRange(0.1f, 10f)] private float attackSpeed = 1.5f;
            [SerializeField, PropertyRange(0, 100)] private int damage = 5;

            public int Damage { get => damage; set => damage = value; }
            public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
        }

        public class Bow : Weapon
        {
            [SerializeField, PropertyRange(1f, 10f)] private float fireRange = 1.5f;
        }

        public class Sword : Weapon
        {
            [SerializeField, PropertyRange(0f, 1f)] private float stunChancePercent;
        }

        [Serializable]
        public class ModuleInfos // load or create new module
        {
            [SerializeField][GUIColor(SU_Tools.COLOR_ENUMS)] protected EAbilityType abilityType = EAbilityType.Offense;
            [SerializeField][GUIColor(SU_Tools.COLOR_ENUMS)] protected EPayloadType PayloadType;
            [SerializeField, Range(0, 100)] protected int payloadValue;
            [SerializeField] protected GameObject effectArea;
        }

        [Serializable]
        public class StatsInfos // load or create new stat
        {
            [GUIColor(SU_Tools.COLOR_ENUMS)] public enum Stat { [HideInInspector] NONE = -1, ModuleSlots, Cost, Damage, AttackSpeed }

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
    }
}

namespace StatusUnknown.Tools
{
    public enum Faction { Player, SAA, Hera, Excelsior, Pulse }
    struct SU_Tools
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

            [MenuItem(SU_Tools.ROOT_MENU_PATH + "Character Editor")]
            private static void OpenWindow()
            {
                CharacterEditorCoreWindow window = GetWindow<CharacterEditorCoreWindow>("Character Editor");
                window.minSize = new Vector2(1920 * 0.5f, 1080 * 0.5f);
                window.maxSize = new Vector2(1920, 1080);
                window.Show();
            }

            [EnumToggleButtons, PropertyOrder(-1)][GUIColor(SU_Tools.COLOR_ENUMS)] public Faction Faction;

            [PropertySpace(SpaceAfter = 25), HorizontalGroup("Base", LabelWidth = 100, Title = "BASE")]

            [VerticalGroup("Base/left", GroupName = "CHARACTER"), PropertyOrder(0)] public new string name = "Player Character";
            [VerticalGroup("Base/left")][PreviewField(300, ObjectFieldAlignment.Left), PropertyOrder(0)] public Sprite image;
            [VerticalGroup("Base/left", GroupName = "CHARACTER")][TextArea(10, 20)] public string description;
            [VerticalGroup("Base/left", GroupName = "CHARACTER")][HideIf("Faction", Faction.Player)] public DialogueSO[] unlockedDialogues = new DialogueSO[2];

            [VerticalGroup("Base/right", GroupName = "WEAPON")]
            [VerticalGroup("Base/right/weapon", GroupName = "WEAPON"), LabelWidth(SU_Tools.LABEL_SIZE_SMALL)] public Weapon Weapon = new Weapon();
            [VerticalGroup("Base/right/details", GroupName = "DETAILS")]
            [TabGroup("Base/right/details/infos", "Stats"), LabelWidth(SU_Tools.LABEL_SIZE_MEDIUM)] public StatsInfos[] StartingStats = new StatsInfos[1];
            [TabGroup("Base/right/details/infos", "Modules"), LabelWidth(SU_Tools.LABEL_SIZE_MEDIUM)] public ModuleInfos[] StartingModules = new ModuleInfos[3];

            [PropertySpace][ShowIf("Faction", Faction.Player)] public bool showCameraAndControls = false;

            [TabGroup("Character", "Controls")][ShowIf("showCameraAndControls", true)] public ControlsSettings ControlSettings = new ControlsSettings();
            [TabGroup("Character", "Camera")][ShowIf("showCameraAndControls", true)] public CameraSettings CameraSettings = new CameraSettings();

            [PropertySpace] public bool showSaveFields = false;

            [ShowIf("showSaveFields", true)][BoxGroup("Save"), Required("Do not forget to fill in the \"Save Name\" field", InfoMessageType.Warning)] public string saveName;

            [ShowIf("showSaveFields", true)]
            [HorizontalGroup("Save/ModulesSet", SU_Tools.BUTTON_LAYOUT_SMALL)]
            [GUIColor(SU_Tools.COLOR_QOL)]
            [Button("Weapon Set - Default Name")]
            public void Faction_Default_ModulesSet() { saveName = $"{Faction}_DialogueSet_ID"; }

            [ShowIf("showSaveFields", true)]
            [HorizontalGroup("Save/WeaponSet", SU_Tools.BUTTON_LAYOUT_SMALL)]
            [GUIColor(SU_Tools.COLOR_QOL)]
            [Button("Module Set - Default Name")]
            public void Faction_Default_WeaponSet() { saveName = $"{Faction}_WeaponSet_ID"; }

            [ShowIf("showSaveFields", true)]
            [HorizontalGroup("Save/DialogueSet", SU_Tools.BUTTON_LAYOUT_SMALL)]
            [GUIColor(SU_Tools.COLOR_QOL)]
            [Button("NPC Set - Default Name")]
            public void Faction_Default_DialogueSet() { saveName = $"{Faction}_DialogueSet_ID"; }

            [ShowIf("saveName", null)]
            [HorizontalGroup("Save/Save", SU_Tools.BUTTON_LAYOUT_SMALL)]
            [PropertySpace(25), GUIColor(SU_Tools.COLOR_SAVE)]
            [Button("Save")]
            public void SaveAsset() { }

            [ShowIf("showSaveFields", true)]
            [BoxGroup("Save"), FolderPath, SerializeField, Required]
            private string rootGameplayDataPath = "Assets/Data/Gameplay";


            public class WeaponPropertyProcessor<T> : OdinPropertyProcessor<T> where T : Weapon
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

            public class ModuleInfosPropertyProcessor : OdinPropertyProcessor<ModuleInfos>
            {
                public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
                {
                    //
                }
            }

            public class StatsInfosPropertyProcessor : OdinPropertyProcessor<StatsInfos>
            {
                public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
                {
                    //
                }
            }

            public class DialogueSOPropertyProcessor : OdinPropertyProcessor<DialogueSO>
            {
                public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
                {
                    //
                }
            }
        }
    }
}
