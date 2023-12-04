using System;
using System.Linq;
using UnityEngine;
using XNode;
using StatusUnknown.Tools.Narrative;
using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using StatusUnknown;

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

        public bool CurrentDialogueQuestsDone; // { get; private set; }


        private DialogueNode rootNode;
        public DialogueNode CurrentNode { get; set; }

        internal void Init()
        {
            playerData.Init();
            DoQuestValidation(); 
        }


        // very basic implementation. If even ONE quest is not done, = false
        // create new dialog options or anything if quests are done.
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
			//Root node has no input
			foreach (var node in nodes.Where(node => node.HasPort("execIn") && !node.GetInputPort("execIn").IsConnected)) 
			{
				if (rootNode != null && rootNode.IsRootNode())
					throw new MonoRootDialogGraphException($"Two or more roots are found in {name}");

                rootNode = node as DialogueNode;

				if (rootNode.IsRootNode())
				{
					break; 
				}
            }

			if(rootNode == null) throw new NullReferenceException($"There is no root node in the current graph : {name}");

			CurrentNode = rootNode;
            rootNode.OnEnter();

            /* Type nodeType = rootNode.GetType();
			if (nodeType.IsAssignableFrom(typeof(INodeState)))
			{
                rootNode.OnEnter();
            } */

            return rootNode;
		} // TODO : Edit Here is root conditional (for ex: character has item, has already visited, etc..)

		/// <summary>
		/// Retrieve the next node in a graph according to the current one and the answer's index chosen.
		/// </summary>
		/// <param name="current">The current node we are on.</param>
		/// <param name="outputIndex">The index of the answer chosen.</param>
		/// <returns>Return a DialogNode corresponding to the next one.</returns>
		public static DialogueNode GetNext(DialogueNode current, int outputIndex) 
		{
            current.MoveNext(); ; 

            var port = current.GetPort($"DialogueLines {outputIndex}");
			return !port.IsConnected ? null : port.Connection.node as DialogueNode;
		}	
		
		// to test
		public static DialogueNode GetPrevious(DialogueNode current)
		{
			//string additionalValue = inputIndex == -1 ? "" : inputIndex.ToString(); 

            var port = current.GetPort("input");
            return !port.IsConnected ? null : port.Connection.node as DialogueNode;
        }

        internal void GiveReward()
        {
            playerData.QuestJournal.GiveRewardOnQuestCompletion();
        }
    }
}