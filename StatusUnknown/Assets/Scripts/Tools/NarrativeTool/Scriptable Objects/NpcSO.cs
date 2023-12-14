using Sirenix.OdinInspector;
using StatusUnknown.Tools;
using StatusUnknown.Utils.AssetManagement;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [ManageableData]
    [CreateAssetMenu(fileName = "NPC_Faction_Name", menuName = CoreContentStrings.PATH_CONTENT_NARRATIVE + "NPC")]
    public class NpcSO : ScriptableObject
    {
        private const float r = 0.8f;
        private const float g = 0f;
        private const float b = 0f;
        private const int rankCeilMax = 2000;
        private const bool disableField = true; 

        [DisableIf(nameof(disableField)), PropertySpace(SpaceBefore = 0, SpaceAfter = 20)] public Faction Faction; 

        [HorizontalGroup("Core")]
        [VerticalGroup("Core/left"), PropertyOrder(0)] public new string name = "Faction NPC";
        [VerticalGroup("Core/left")][PreviewField(300, ObjectFieldAlignment.Left), PropertyOrder(0)] public Sprite sprite;
        [VerticalGroup("Core/left")][TextArea(10, 20)] public string description = string.Empty;
        [VerticalGroup("Core/left"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 1 Ceil"), PropertySpace(SpaceBefore = 20)] public float rank1ceil = 100;
        [VerticalGroup("Core/left"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 2 Ceil")] public float rank2ceil = 300;
        [VerticalGroup("Core/left"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 3 Ceil")] public float rank3ceil = 600;
        [VerticalGroup("Core/left"), ProgressBar(0, rankCeilMax, r, g, b), LabelText("Rank 4 Ceil"), PropertySpace(SpaceBefore = 0, SpaceAfter = 20)] public float rank4ceil = 1000;

        [VerticalGroup("Core/right")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public MainQuestsDataSO mainQuests = null;
        [VerticalGroup("Core/right")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public SecondaryQuestDataSO secondaryQuests = null;
        [VerticalGroup("Core/right")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM), PreviewField(Alignment = ObjectFieldAlignment.Left)] public AudioClip npcVoice = null;

        [HorizontalGroup("Items")][LabelWidth(CoreToolsStrings.LABEL_SIZE_MEDIUM)] public string[] placeholderItems = new string[5]; 
    }
}
