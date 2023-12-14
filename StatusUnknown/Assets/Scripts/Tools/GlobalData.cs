using Sirenix.OdinInspector;
using System;
using UnityEngine;
using StatusUnknown.Tools;

namespace StatusUnknown
{
    public enum Faction 
    { 
        [HideInInspector] Player = -1, 
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

    public enum ReputationRank 
    { 
        Zero, 
        One, 
        Two, 
        Three 
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

    #region EditorOnly
    public enum NPCEditorOptions
    {
        Quest,
        Items
    }
    #endregion

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
    }
}
