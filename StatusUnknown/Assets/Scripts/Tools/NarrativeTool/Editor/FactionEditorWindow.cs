using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using StatusUnknown.Content.Narrative;
using StatusUnknown.Utils.AssetManagement;
using System;
using UnityEditor;
using UnityEngine;

// more advanced 
// Odin Attribute Drawers, C# MemberInfos (System.Reflection), SirenixEditorFields, GUIHelper, EditorGUI, SirenixEditorGUI
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

        #region Tabs
        [HorizontalGroup("NPCTabs")][EnumToggleButtons, PropertyOrder(0)][GUIColor(CoreToolsStrings.COLOR_QOL), HideLabel, OnValueChanged(nameof(UpdateShowOptions))] public NPCEditorOptions Options;
        #endregion

        [PropertySpace(40)] public NPC FactionNPC = new NPC();
        internal NpcSO npcToCreate;

        private void UpdateShowOptions()
        {
            FactionNPC.ShowQuestOptions(Options == NPCEditorOptions.Quest); 
        }

        #region Save
        [BoxGroup("Save")] public bool showSaveFields = false;
        [BoxGroup("Save")][ShowIf(nameof(showSaveFields), true)][Required("Do not forget to fill in the \"Save Name\" field", InfoMessageType.Warning)] public string saveName = string.Empty;

        [BoxGroup("Save")]
        [ShowIf(nameof(showSaveFields), true)]
        [GUIColor(CoreToolsStrings.COLOR_QOL)]
        [Button("Give Default Name", CoreToolsStrings.BUTTON_LAYOUT_SMALL, Icon = SdfIconType.Newspaper, Stretch = false, ButtonAlignment = 0f, ButtonHeight = 50)]
        public void GiveDefaultSaveName() { saveName = FactionNPC.Faction + "_NPC_Name"; }

        [ShowIf("@saveName != string.Empty && showSaveFields == true")]
        [BoxGroup("Save")]
        [PropertySpace(5)]
        [GUIColor(CoreToolsStrings.COLOR_SAVE)]
        [Button("Save", CoreToolsStrings.BUTTON_LAYOUT_SMALL, Icon = SdfIconType.Save, Stretch = false, ButtonAlignment = 0f, ButtonHeight = 50)]
        public void SaveAsset() 
        {
            npcToCreate = CreateInstance<NpcSO>(); 

            npcToCreate.Faction = FactionNPC.Faction;   
            npcToCreate.name = saveName;
            npcToCreate.sprite = FactionNPC.sprite;
            npcToCreate.description = FactionNPC.description; 

            npcToCreate.rank1ceil = FactionNPC.rank1ceil;
            npcToCreate.rank2ceil = FactionNPC.rank2ceil;
            npcToCreate.rank3ceil = FactionNPC.rank3ceil;
            npcToCreate.rank4ceil = FactionNPC.rank4ceil;

            npcToCreate.mainQuests = FactionNPC.mainQuest;
            npcToCreate.secondaryQuests = FactionNPC.secondaryQuests;   
            npcToCreate.npcVoice = FactionNPC.npcVoice;

            npcToCreate.placeholderItems = FactionNPC.placeholderItems; 

            StatusUnknown_AssetManager.SaveSO(npcToCreate, CoreAssetManagementStrings.SAVE_PATH_NPC, saveName, ".asset"); 
        }

        [ShowIf(nameof(showSaveFields), true)]
        [FolderPath, SerializeField, Required]
        [BoxGroup("Save")]
        private string rootGameplayDataPath = "Assets/Data/Narrative/NPC";

        [PropertySpace(5)]
        [GUIColor(CoreToolsStrings.COLOR_QOL)]
        [Button("Load", CoreToolsStrings.BUTTON_LAYOUT_SMALL, Icon = SdfIconType.Save, Stretch = false, ButtonAlignment = 0f, ButtonHeight = 50)]
        public NpcSO LoadAsset(NpcSO assetToLoad) 
        {
            BuildEditorWindowFromLoadedSO(assetToLoad);
            return assetToLoad; 
        }

        private void BuildEditorWindowFromLoadedSO(NpcSO reference)
        {
            FactionNPC.name = reference.name;
            FactionNPC.sprite = reference.sprite; 
            FactionNPC.description = reference.description;

            FactionNPC.rank1ceil = reference.rank1ceil;
            FactionNPC.rank2ceil = reference.rank2ceil;
            FactionNPC.rank3ceil = reference.rank3ceil;
            FactionNPC.rank4ceil = reference.rank4ceil;

            FactionNPC.mainQuest = reference.mainQuests;
            FactionNPC.secondaryQuests = reference.secondaryQuests;
            FactionNPC.npcVoice = reference.npcVoice;

            FactionNPC.placeholderItems = reference.placeholderItems;
        }
        #endregion

        #region BACKEND
        public class NPC
        {
            private const float r = 0.8f; 
            private const float g = 0f;
            private const float b = 0f;
            private const int rankCeilMax = 2000;
            private bool showQuestOptions = true; 

            #region Data
            [HorizontalGroup("Core")]
            [VerticalGroup("Core/character")][GUIColor(CoreToolsStrings.COLOR_ENUMS), HideLabel] public Faction Faction;
            [VerticalGroup("Core/character"), PropertyOrder(0)] public string name = "Faction NPC";
            [VerticalGroup("Core/character")][PreviewField(300, ObjectFieldAlignment.Left), PropertyOrder(0)] public Sprite sprite;
            [VerticalGroup("Core/character")][TextArea(10, 20)] public string description = "This is the default NPC from the SAA Faction"; 
            [VerticalGroup("Core/character"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 1 Ceil"), PropertySpace(SpaceBefore = 20)] public float rank1ceil = 100;
            [VerticalGroup("Core/character"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 2 Ceil")] public float rank2ceil = 300;
            [VerticalGroup("Core/character"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 3 Ceil")] public float rank3ceil = 600;
            [VerticalGroup("Core/character"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 4 Ceil"), PropertySpace(SpaceBefore = 0, SpaceAfter = 20)] public float rank4ceil = 1000;

            [VerticalGroup("Core/quests", VisibleIf = nameof(showQuestOptions))][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public MainQuestsDataSO mainQuest = null;
            [VerticalGroup("Core/quests")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public SecondaryQuestDataSO secondaryQuests = null;
            [VerticalGroup("Core/quests")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM), PreviewField(Alignment = ObjectFieldAlignment.Left)] public AudioClip npcVoice = null;


            /* [Serializable] public struct FactionItems
            {
                readonly string itemName;
                readonly ReputationRank unlockRank; 
                
                public FactionItems(string name, ReputationRank rank)
                {
                    itemName  = name;   
                    unlockRank = rank;
                }
            } */
            [VerticalGroup("Core/items", VisibleIf = "@showQuestOptions == false")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public string[] placeholderItems = new string[5]; 

            public void ShowQuestOptions(bool showQuestOptions)
            {
                this.showQuestOptions = showQuestOptions;   
            }
            #endregion
        }
        #endregion
    }
}

