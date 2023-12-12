using System;
using System.Linq;
using UnityEngine;
using XNode;
using StatusUnknown.Tools.Narrative;
using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using System.Collections.Generic;
using StatusUnknown;
using StatusUnknown.Content; 

namespace Aurore.DialogSystem
{
	[CreateAssetMenu(fileName = "Dialogue Graph", menuName = "Status Unknown/Narrative/Dialogue Graph", order = 11)]
	public class DialogGraph : NodeGraph
	{
        [Header("Player Data")]
        [SerializeField] private PlayerDataSO playerData;

        [Header("Quest Data")]
        [Space, SerializeField] private bool dialogueHasQuests;
        [SerializeField, ShowIf(nameof(dialogueHasQuests))] QuestSO[] completableQuests;
        [SerializeField] protected Faction factionDialogueNPC;

        public bool CurrentDialogueQuestsDone { get; private set; }
        // public static Dictionary<(Vector2, int), DialogueLine> allStoredDialogueAnswers = new Dictionary<(Vector2, int), DialogueLine>();

        [SerializeField, HideInInspector] private List<DialogueLine> storedDialogueAnswers;
        [SerializeField, HideInInspector] private List<(Vector2, int)> storedKeys; 

        private DialogueNode rootNode;
        public DialogueNode CurrentNode { get; set; }

        internal void Init()
        {
            playerData.Init();
            DoQuestValidation();
        }


        /// <summary>
        /// Basic implementation of a quest validation mechanic. If event ONE quest is not done, the validation is false
        /// </summary>
        private void DoQuestValidation()
        {
            CurrentDialogueQuestsDone = true;
            for (int i = 0; i < completableQuests.Length; i++)
            {
                if (!playerData.QuestJournal.QuestIsDone(completableQuests[i]))
                {
                    CurrentDialogueQuestsDone = false;
                }
            }

            Debug.Log("quest done : " + CurrentDialogueQuestsDone);
        }

        /// <summary>
        /// Access the root of the current graph.
        /// </summary>
        /// <returns>The root node of the graph.</returns>
        /// <exception cref="NullReferenceException">Thrown if no root are found.</exception>
        /// <exception cref="MonoRootDialogGraphException">Thrown if there isn't a unique root in the graph.</exception>>
        public DialogueNode GetRoot()
		{
			rootNode = null;

			//Root node has no input and must be the only node with no input on execIn port
			foreach (var node in nodes.Where(node => node.HasPort("execIn") && !node.GetInputPort("execIn").IsConnected)) 
			{
				if (rootNode != null)
					throw new MonoRootDialogGraphException($"Two or more roots are found in {name}");

                rootNode = node as DialogueNode;
            }

			if (rootNode == null) throw new NullReferenceException($"There is no root node in the current graph : {name}");

			CurrentNode = rootNode;
            rootNode.OnEnter();

            return rootNode;
		} // TODO : Edit Here if root is conditional (for ex: character has item, has already visited, etc..)

		/// <summary>
		/// Retrieve the next node in a graph according to the current one and the answer's index chosen.
		/// </summary>
		/// <param name="current">The current node we are on.</param>
		/// <param name="outputIndex">The index of the answer chosen.</param>
		/// <returns>Return a DialogNode corresponding to the next one.</returns>
		public static DialogueNode GetNext(DialogueNode current, int outputIndex) 
		{
            current.MoveNext(outputIndex);
            var exitPort = current.GetOutputPort($"DialogueLines {outputIndex}");

            return !exitPort.IsConnected ? null : exitPort.Connection.node as DialogueNode;
		}	
		
		/// <summary>
        /// Called from any back button to go back to the previous node in the graph
        /// </summary>
        /// <param name="current">the current node from where to go back</param>
        /// <returns></returns>
		public static DialogueNode GetPrevious(DialogueNode current)
		{
			//string additionalValue = inputIndex == -1 ? "" : inputIndex.ToString(); 

            var port = current.GetPort("input");
            return !port.IsConnected ? null : port.Connection.node as DialogueNode;
        }

        /// <summary>
        /// Call to the player quest journal to add a QuestObjectSO as reward
        /// </summary>
        internal void GiveReward()
        {
            playerData.QuestJournal.GiveRewardOnQuestCompletion();
        }
    }
}