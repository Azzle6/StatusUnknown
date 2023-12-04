using StatusUnknown.Content.Narrative;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using XNode;

// original credits : Aurora Dialogue System

// [CreateNodeMenu("Get Quest Journal Node")]
// [CreateNodeMenu("Register Choice Node")]

// chaque PNJ de faction a un pool de quêtes
// jauge de réputation % -> nouvelle quête d'histoire (main quest de faction)

namespace StatusUnknown.Tools.Narrative
{
	[Serializable]
	public struct Connection {}
    public enum AccessType { Get, Set }

    [NodeWidth(400), NodeTint(20, 120, 20)]
    [CreateNodeMenu("Dialogue Node")]
    public class DialogueNode : Node
    {
        [Serializable]
        protected struct DialogueLine
        {
            [TextArea] public string answer;
            public bool isOptionalDialogue;
            public Sprite icon;
            public QuestObjectSO optionalGameplayEffect;

            public DialogueLine(string answer, bool isOptionalDialogue, Sprite icon, QuestObjectSO optionalGameplayEffect) 
            { 
                this.answer = answer;
                this.isOptionalDialogue = isOptionalDialogue;
                this.icon = icon;
                this.optionalGameplayEffect = optionalGameplayEffect;
            }
        }

        [Input, SerializeField] public Connection input;
        [TextArea] public string initiator;

        [TextArea][Output(dynamicPortList = true)] public List<string> answers; // DO NOT CHANGE FIELD SIGNATURE
        [Output(dynamicPortList = true), SerializeField] protected List<DialogueLine> DialogueLines; 

        [Input, SerializeField] public Data conditionalAnswer;
		private bool conditionIsValid = false; 

        protected override void Init()
        {
            base.Init();
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);

            if (from.node.GetType() != typeof(ValidationNode)) return;

            conditionalAnswer = GetInputValue("conditionalAnswer", new Data());
            conditionIsValid = conditionalAnswer.isValid; 

            UpdatePorts();  
            VerifyConnections();
        }

        public override void OnRemoveConnection(NodePort port)
        {
            if (port.IsOutput && port.node.GetType() != typeof(ValidationNode)) return;

            base.OnRemoveConnection(port);

            conditionalAnswer = GetInputValue("conditionalAnswer", new Data());
            conditionIsValid = conditionalAnswer.isValid;

            if (!conditionIsValid && conditionalAnswer.dialogue != null)
            {
                answers.Remove(conditionalAnswer.dialogue);
                return; 
            }

			if (answers.Contains(conditionalAnswer.dialogue)) return;

            VerifyConnections();

            answers.Add(conditionalAnswer.dialogue);
            UpdatePorts();
        }

        public override object GetValue(NodePort port)
		{
            return null;
		}

		public bool HasAnswers()
		{
			return answers.Count != 0 && (answers.Count != 1 || !answers[0].Equals(""));
		}
	}
}

