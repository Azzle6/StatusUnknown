using Sirenix.OdinInspector;
using System;
using UnityEngine;
using StatusUnknown.Tools;

namespace StatusUnknown
{
    public enum Faction 
    { 
        Player = -1, 
        SAA, 
        Hera, 
        Excelsior, 
        Pulse 
    }

    public enum Item 
    { 
        Weapon, 
        Armor, 
        Augments, 
        Modules 
    }

    public enum EScriptableType
    {
        NONE = -1,
        Ability,
        Enemy,
        Encounter
    }

    public enum EAbilityType
    {
        Offense,
        Defense,
        Support,
        Control
    }

    public enum EPayloadType
    {
        Burst,
        OverTime,
        Delayed
    }

    namespace Tools
    {
        public struct CoreToolsStrings
        {
            public const string ROOT_MENU_PATH = "Status Unknown/Tools/";

            public const int LABEL_SIZE_SMALL = 180;
            public const int LABEL_SIZE_MEDIUM = 250;
            public const int BUTTON_LAYOUT_SMALL = 250;
            public const string COLOR_SAVE = "green";
            public const string COLOR_QOL = "yellow";
            public const string COLOR_ENUMS = "purple";
            public const string COLOR_DANGER = "red";
        }
    }

    namespace Content
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
}
