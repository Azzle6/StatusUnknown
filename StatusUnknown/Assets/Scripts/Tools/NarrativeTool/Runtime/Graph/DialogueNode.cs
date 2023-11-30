using Sirenix.OdinInspector;
using StatusUnknown.Tools.Narrative;
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Aurore.DialogSystem
{
	[Serializable]
	public struct Connection {}

    [NodeWidth(400), NodeTint(20, 120, 20)]
    [CreateNodeMenu("Dialogue Node")]
	public class DialogueNode : Node
	{
        [Input, SerializeField] public Connection input;
        [TextArea] public string initiator;

		public string title;

		public Sprite img;
		public AudioClip audio;

		[TextArea][Output(dynamicPortList = true)] public List<string> answers; // DO NOT CHANGE FIELD SIGNATURE
		[Input, SerializeField] public bool conditionalAnswer = false;
		private bool conditionIsValid = false; 
        [TextArea, ShowIf("conditionIsValid")] public string additionalAnswer;

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
			if (from.node.GetType() != typeof(DialogueConditionalAnswerNode)) return; 

            conditionIsValid = GetInputValue("conditionalAnswer", false);
            base.OnCreateConnection(from, to);
        }

        public override void OnRemoveConnection(NodePort port)
        {
			if (port.IsOutput && port.node.GetType() != typeof(DialogueConditionalAnswerNode)) return;

            conditionIsValid = GetInputValue("conditionalAnswer", false);
            base.OnRemoveConnection(port);
        }

        public override object GetValue(NodePort port)
		{
            return null;
		}

		public DialogueType GetDialogueType()
		{
			return img == null ? DialogueType.Simple : DialogueType.Full;
		}

		public bool HasAnswers()
		{
			return answers.Count != 0 && (answers.Count != 1 || !answers[0].Equals(""));
		}
	}
}