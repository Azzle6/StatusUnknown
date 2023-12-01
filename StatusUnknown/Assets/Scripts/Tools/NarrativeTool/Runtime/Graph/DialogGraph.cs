using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using System;
using System.Linq;
using UnityEngine;
using XNode;
using StatusUnknown.Tools.Narrative; 

namespace Aurore.DialogSystem
{

	[CreateAssetMenu(fileName = "Dialogue Graph", menuName = "Status Unknown/Narrative/Dialogue Graph", order = 11)]
	public class DialogGraph : NodeGraph
	{
		[Header("Quest Data")]
		[Space, SerializeField] private bool hasQuests; 
		[SerializeField, ShowIf("hasQuests")] QuestSO[] quests;
		[SerializeField, HideInInspector] private QuestJournalSO playerQuestJournal; 
		private DialogueNode _root;

		public bool CurrentDialogueQuestsDone {  get; private set; }	  

		public void Init()
        {
            DoQuestValidation();
        }

		// very basic implementation. If even ONE quest is not done, = false
        private void DoQuestValidation()
        {
			CurrentDialogueQuestsDone = true; 
            for (int i = 0; i < quests.Length; i++)
            {
                if (!playerQuestJournal.QuestIsDone(quests[i]))
                {
                    CurrentDialogueQuestsDone = false;
                }
            }

			Debug.Log("quest done : " + CurrentDialogueQuestsDone); 
        }

		public QuestObjectSO GetQuestReward(int questIndex) => quests[questIndex].QuestReward;

        /// <summary>
        /// Access the root of the current graph.
        /// </summary>
        /// <returns>The root node of the graph.</returns>
        /// <exception cref="NullReferenceException">Thrown if no root are found.</exception>
        /// <exception cref="MonoRootDialogGraphException">Thrown if there isn't a unique root in the graph.</exception>>
        public DialogueNode GetRoot()
		{
			_root = null;
			//Root node has no input
			foreach (var node in nodes.Where(node => node.HasPort("input") && !node.GetInputPort("input").IsConnected))
			{
				if (_root is not null)
					throw new MonoRootDialogGraphException($"Two or more roots are found in  {name}");
				_root = node as DialogueNode;
				
			}

			if(_root is null) throw new NullReferenceException($"There is no root node in the current graph : {name}");
			return _root;
		} // TODO : Edit Here is root conditional (for ex: character has item, has already visited, etc..)

		/// <summary>
		/// Retrieve the next node in a graph according to the current one and the answer's index chosen.
		/// </summary>
		/// <param name="current">The current node we are on.</param>
		/// <param name="outputIndex">The index of the answer chosen.</param>
		/// <returns>Return a DialogNode corresponding to the next one.</returns>
		public static DialogueNode GetNext(DialogueNode current, int outputIndex) 
		{
			var port = current.GetPort($"answers {outputIndex}");
			return !port.IsConnected ? null : port.Connection.node as DialogueNode ;
		}	
		
		// to test
		public static DialogueNode GetPrevious(DialogueNode current)
		{
			//string additionalValue = inputIndex == -1 ? "" : inputIndex.ToString(); 

            var port = current.GetPort("input");
            return !port.IsConnected ? null : port.Connection.node as DialogueNode;
        }
    }
}