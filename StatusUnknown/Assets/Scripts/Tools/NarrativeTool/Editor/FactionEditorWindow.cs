using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using StatusUnknown.Content.Narrative;
using StatusUnknown.Tools;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

// more advanced 
// Odin Attribute Drawers, C# MemberInfos (System.Reflection), SirenixEditorFields, GUIHelper, EditorGUI, SirenixEditorGUI
namespace StatusUnknown.Content.Narrative
{
    #region TEMP
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
    #endregion
}

namespace StatusUnknown.Tools.FactionEditor
{
    public class FactionEditorWindow : OdinEditorWindow
    {

        [MenuItem(CoreToolsStrings.ROOT_MENU_PATH + "Faction Editor")]
        private static void OpenWindow()
        {
            FactionEditorWindow window = GetWindow<FactionEditorWindow>("Faction Editor");

            Vector2 userScreenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
            window.minSize = userScreenSize * 0.9f;
            window.maxSize = userScreenSize * 1f;
            window.position = new Rect(new Vector2(100, 10), window.minSize);
            window.Show();
        }

        #region FRONTEND
        #region Top
        [HorizontalGroup("FactionTabs")][EnumToggleButtons, PropertyOrder(-1)][GUIColor(CoreToolsStrings.COLOR_ENUMS), HideLabel] public Faction Faction;
        [HorizontalGroup("NPCTabs")][EnumToggleButtons, PropertyOrder(0)][GUIColor(CoreToolsStrings.COLOR_ENUMS), HideLabel] public NPCEditorOptions Options;
        #endregion

        #region Core
        [HorizontalGroup("Core")]
        [VerticalGroup("Core/left"), PropertyOrder(0)] public new string name = "Faction NPC";
        [VerticalGroup("Core/left")][PreviewField(300, ObjectFieldAlignment.Left), PropertyOrder(0)] public Sprite sprite;
        [VerticalGroup("Core/left")][TextArea(10, 20)] public string description = string.Empty;
        [VerticalGroup("Core/left"), ProgressBar(0, 2000, 1f, 10f, 0f)] public float rank1ceil = 100;
        [VerticalGroup("Core/left"), ProgressBar(0, 2000, 1f, 1f, 0f)] public float rank2ceil = 300;
        [VerticalGroup("Core/left"), ProgressBar(0, 2000, 1f, 1f, 0f)] public float rank3ceil = 600;
        [VerticalGroup("Core/left"), ProgressBar(0, 2000, 1f, 1f, 0f)] public float rank4ceil = 1000;

        [VerticalGroup("Core/right")]
        [HorizontalGroup("Core/right/quests")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public ModuleInfos mainQuest = new ModuleInfos();
        [HorizontalGroup("Core/right/quests")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public StatsInfos secondaryQuests = new StatsInfos();
        #endregion

        #region Save
        [PropertySpace][HorizontalGroup("Save")] public bool showSaveFields = false;
        [HorizontalGroup("Save")][ShowIf(nameof(showSaveFields), true)][Required("Do not forget to fill in the \"Save Name\" field", InfoMessageType.Warning)] public string saveName = string.Empty;

        [HorizontalGroup("Save")]
        [PropertySpace][ShowIf(nameof(showSaveFields), true)]
        [GUIColor(CoreToolsStrings.COLOR_QOL)]
        [Button("Weapon Set - Default Name", CoreToolsStrings.BUTTON_LAYOUT_SMALL)]
        public void Faction_Save_Default() { saveName = $"{Faction}_NPC_Name"; }

        [ShowIf("@saveName != string.Empty && showSaveFields == true")]
        [HorizontalGroup("Save", CoreToolsStrings.BUTTON_LAYOUT_SMALL)]
        [PropertySpace(25), GUIColor(CoreToolsStrings.COLOR_SAVE)]
        [Button("Save", Icon = SdfIconType.Save)]
        public void SaveAsset() { }

        [ShowIf(nameof(showSaveFields), true)]
        [HorizontalGroup("Save"), FolderPath, SerializeField, Required]
        private string rootGameplayDataPath = "Assets/Data/Gameplay";
        #endregion
        #endregion

        #region BACKEND
        #endregion
    }
}

