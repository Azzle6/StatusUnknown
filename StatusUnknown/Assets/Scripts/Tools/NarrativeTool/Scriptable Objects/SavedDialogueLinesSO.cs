using Sirenix.OdinInspector;
using StatusUnknown.Tools.Narrative;
using StatusUnknown.Utils.AssetManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [ManageableData]
    [CreateAssetMenu(fileName = "Saved Dialogues", menuName = CoreContentStrings.PATH_CONTENT_NARRATIVE + "Save Data", order = 30)]
    public class SavedDialogueLinesSO : SerializedScriptableObject
    {
        [Serializable]
        public struct DialogueDataHolder
        {
            [LabelText("@$value.answer")] public DialogueLine dialogueLine;
            [HideInInspector] public Vector2 pos;
            [HideInInspector] public int ID;

            public DialogueDataHolder(DialogueLine dialogueLine, Vector2 position, int id)
            {
                this.dialogueLine = dialogueLine;
                pos = position;
                ID = id;
            }
        }

        public List<DialogueDataHolder> savedDialoguesData = new List<DialogueDataHolder>();

        // ToDictionary doesn't check if key already exists and can therefore throw errors...
        public Dictionary<(int, Vector2), DialogueDataHolder> GetSavedDialogueData() => savedDialoguesData.ToDictionary(x => (x.ID, x.pos));
    }
}
