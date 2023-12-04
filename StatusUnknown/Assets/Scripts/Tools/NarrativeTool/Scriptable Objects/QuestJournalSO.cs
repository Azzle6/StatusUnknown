using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Quest Journal", menuName = "Status Unknown/Narrative/Quest Journal")]
    public class QuestJournalSO : ScriptableObject
    {
        public List<QuestSO> activeQuests = new List<QuestSO>();
        private QuestSO cachedQuest;

        public List<QuestSO> CompletedQuests = new List<QuestSO>();
        public Action<int, Faction, QuestObjectSO> OnQuestCompletion; 

        public void Init(Action<int, Faction, QuestObjectSO> OnQuestCompletionCallback)
        {
            OnQuestCompletion = OnQuestCompletionCallback;
        }

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

        public bool QuestIsDone(QuestSO quest)
        {
            if (!activeQuests.Contains(quest)) return false;
            cachedQuest = quest;

            if (quest.QuestObjectIsRetrieved)
            {
                CompletedQuests.Add(quest);
            }

            return cachedQuest.QuestObjectIsRetrieved; 
        }

        public void GiveRewardOnQuestCompletion()
        {
            foreach (QuestSO quest in CompletedQuests)
            {
                OnQuestCompletion.Invoke(quest.ReputationCompletionBonus, quest.FactionQuestGiver, quest.QuestReward);
            }
        }

        /* public bool QuestObjectWasRetrieved(QuestObjectSO questObj)
        {
            return cachedQuest.QuestObjectToRetrieve != null;
        } */
    }
}
