using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

// original credits : Aurora Dialogue System

// [CreateNodeMenu("Get Quest Journal Node")]
// [CreateNodeMenu("Register Choice Node")]

// chaque PNJ de faction a un pool de quêtes
// jauge de réputation % -> nouvelle quête d'histoire (main quest de faction)

namespace StatusUnknown.Tools.Narrative
{
    [Serializable]
    public struct DialogueLine
    {
        [TextArea] public string answer;
        [LabelWidth(Node.LABEL_WIDTH_MEDIUM)] public bool isOptionalDialogue;
        [LabelWidth(Node.LABEL_WIDTH_MEDIUM)] public Sprite icon;
        [LabelWidth(Node.LABEL_WIDTH_MEDIUM)] public QuestObjectSO optionalGameplayEffect;
        internal bool isValid;

        public DialogueLine(string answer, bool isOptionalDialogue, Sprite icon, QuestObjectSO optionalGameplayEffect, bool isValid)
        {
            this.answer = answer;
            this.isOptionalDialogue = isOptionalDialogue;
            this.icon = icon;
            this.optionalGameplayEffect = optionalGameplayEffect;
            this.isValid = isValid;
        }       
    }

    [Serializable]
	public struct Input {}
    public enum AccessType { Get, Set }

    [NodeWidth(400), NodeTint(20, 120, 20)]
    [CreateNodeMenu("Dialogue Node")]
    public class DialogueNode : Node
    {

        [Input, SerializeField] public Input input;
        [TextArea] public string initiator;

        [Output(dynamicPortList = true), SerializeField] public List<DialogueLine> DialogueLines; 
        public List<string> Answers { get; private set; }

        [Input, SerializeField] public DialogueLine conditionalAnswer;
		private bool conditionIsValid = false; 

        protected override void Init()
        {
            base.Init();

            if (EditorApplication.isPlayingOrWillChangePlaymode || Application.isPlaying)
            {
                Debug.Log("setting answers on entering play mode"); 
                Answers.Clear();
                foreach (var item in DialogueLines)
                {
                    Answers.Add(item.answer); 
                }
            }
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);

            if (from.node.GetType() != typeof(ValidationNode)) return;

            conditionalAnswer = GetInputValue("conditionalAnswer", new DialogueLine());
            conditionIsValid = conditionalAnswer.isValid; 

            UpdatePorts();  
            VerifyConnections();
        }

        public override void OnRemoveConnection(NodePort port)
        {
            if (port.IsOutput && port.node.GetType() != typeof(ValidationNode)) return;

            base.OnRemoveConnection(port);

            conditionalAnswer = GetInputValue("conditionalAnswer", new DialogueLine());
            conditionIsValid = conditionalAnswer.isValid;

            if (!conditionIsValid && conditionalAnswer.answer != null)
            {
                DialogueLines.Remove(conditionalAnswer);
                return; 
            }

			if (DialogueLines.Contains(conditionalAnswer)) return;

            VerifyConnections();

            DialogueLines.Add(conditionalAnswer);
            UpdatePorts();
        }

        public override object GetValue(NodePort port)
		{
            return null;
		}

		public bool HasAnswers()
		{
			return DialogueLines.Count != 0 && (DialogueLines.Count != 1 || !DialogueLines[0].Equals(""));
		}
	}
}

