using System;
using System.Collections.Generic;
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
	public struct Connection {}
    public enum AccessType { Get, Set }

    [NodeWidth(400), NodeTint(20, 120, 20)]
    [CreateNodeMenu("Dialogue Node")]
	public class DialogueNode : Node
	{
        [Input, SerializeField] public Connection input;
        [TextArea] public string initiator;

		[TextArea][Output(dynamicPortList = true)] public List<string> answers; // DO NOT CHANGE FIELD SIGNATURE

        [Input, SerializeField] public Data conditionalAnswer;
		private bool conditionIsValid = false; 

        protected override void Init()
        {
            base.Init();
        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);

            if (from.node.GetType() != typeof(DialogueConditionalAnswerNode)) return;

            conditionalAnswer = GetInputValue("conditionalAnswer", new Data());
            conditionIsValid = conditionalAnswer.isValid; 

            UpdatePorts();  
            VerifyConnections();
        }

        public override void OnRemoveConnection(NodePort port)
        {
            if (port.IsOutput && port.node.GetType() != typeof(DialogueConditionalAnswerNode)) return;

            base.OnRemoveConnection(port);

            conditionalAnswer = GetInputValue("conditionalAnswer", new Data());
            conditionIsValid = conditionalAnswer.isValid;

            if (!conditionIsValid)
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

    // serialization does not work in the node graph
    /*
    public abstract class UnitySerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> keyData = new List<TKey>();

        [SerializeField, HideInInspector]
        private List<TValue> valueData = new List<TValue>();

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.Clear();
            for (int i = 0; i < this.keyData.Count && i < this.valueData.Count; i++)
            {
                this[this.keyData[i]] = this.valueData[i];
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.keyData.Clear();
            this.valueData.Clear();

            foreach (var item in this)
            {
                this.keyData.Add(item.Key);
                this.valueData.Add(item.Value);
            }
        }
    }

    [Serializable] public class DialogueLine : UnitySerializedDictionary<string, int> { }

    [Serializable]
    public class DialogueDataHolder
    {
        public DialogueLine dialogueLine;
        public bool dialogueTriggersEffect; 

    }
    */
}

