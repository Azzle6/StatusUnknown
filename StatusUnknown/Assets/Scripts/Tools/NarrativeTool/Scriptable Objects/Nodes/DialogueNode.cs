using Aurore.DialogSystem;
using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

    public enum AccessType { Get, Set }

    [NodeWidth(400), NodeTint(20, 120, 20)]
    [CreateNodeMenu("Dialogue Node")]
    public class DialogueNode : Node 
    {
        // INodeState

        [SerializeField] private bool isRootNode; // PLACEHOLDER

        [Serializable] public class State { }

        [Input, SerializeField] protected State execIn = null;
        [Output, SerializeField] protected State execOut = null; 

        [TextArea] public string dialogueOpening;

        [Output(dynamicPortList = true), SerializeField] public List<DialogueLine> DialogueLines; 
        public List<string> Answers { get; private set; } // move this to DialogGraph if getting errors in built mode

        [Input, SerializeField] public DialogueLine conditionalAnswer;
		private bool conditionIsValid = false;

        // protected INodeState NodeStateInterface { get; private set; }

        protected override void Init()
        {
            if (EditorApplication.isUpdating || EditorApplication.isCompiling) return; 

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

            //NodeStateInterface = this; 
        }

        public bool IsRootNode() => isRootNode; 

        // public virtual INodeState TryGetInterface() => NodeStateInterface;

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

        public virtual void MoveNext()
        {
            DialogGraph dialogGraph = graph as DialogGraph;

            if (dialogGraph.CurrentNode != this)
            {
                Debug.LogWarning("Node isn't active");
                return;
            }

            NodePort exitPort = GetOutputPort("execOut");

            if (!exitPort.IsConnected)
            {
                Debug.LogWarning("Node isn't connected");
                return;
            }

            DialogueNode node = exitPort.Connection.node as DialogueNode;
            node.OnEnter();
        }

        public virtual void OnEnter()
        {
            DialogGraph fmGraph = graph as DialogGraph;
            fmGraph.CurrentNode = this;
        } 

        public virtual void OnExit()
        {
            // 
        }
    }
}

