using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Quest Journal", menuName = "Status Unknown/Narrative/Quest Journal")]
    public class QuestJournalSO : ScriptableObject
    {
        [SerializeField] private List<QuestSO> activeQuests; 

        public void AddQuest(QuestSO questToAdd)
        {
            if (activeQuests.Contains(questToAdd)) return; // quest already added

            activeQuests.Add(questToAdd);
            questToAdd.QuestIndex = activeQuests.Count - 1;
        }

        public void RemoveQuest(QuestSO questToRemove)
        {
            if (activeQuests.Remove(questToRemove))
            {
                Debug.Log("Quest Successfully removed"); 
            }
        }
    }
}
