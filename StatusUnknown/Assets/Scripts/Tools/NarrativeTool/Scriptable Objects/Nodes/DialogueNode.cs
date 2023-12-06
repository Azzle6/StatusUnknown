using Aurore.DialogSystem;
using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Windows;
using XNode;

// original credits : Aurora Dialogue System

//                  -- COLOR CODE --
// green : dialogue
// yellow : quest
// purple : logic
// red : player

// chaque PNJ de faction a un pool de quêtes
// jauge de réputation % -> nouvelle quête d'histoire (main quest de faction)

namespace StatusUnknown.Tools.Narrative
{
    public enum AccessType { Get, Set }
    public enum ComparisonType { SmallerThan, SmallerThanOrEqual, GreaterThan, GreaterThanOrEqual, Equal, NotEqual, QuestIsDone }

    [Serializable]
    public struct DialogueLine
    {
        [TextArea] public string answer;
        [LabelWidth(200)] public bool isOptionalDialogue;

        [LabelWidth(200)] public bool storeIfSelected;
        [LabelWidth(200)] public Sprite icon;
        [LabelWidth(200)] public QuestObjectSO optionalGameplayEffect;
        [HideInInspector] public bool conditionIsValid;

        [Space, VerticalGroup("optional", VisibleIf = nameof(isOptionalDialogue))] public ComparisonType comparisonType;

        [ShowIf("@" + nameof(comparisonType) + " == ComparisonType.QuestIsDone")]
        [VerticalGroup("optional", VisibleIf = nameof(isOptionalDialogue))] public QuestSO questToValidate;

        [HideIf("@" + nameof(comparisonType) + " == ComparisonType.QuestIsDone"), LabelWidth(200)]
        [VerticalGroup("optional", VisibleIf = nameof(isOptionalDialogue)), Range(0, 1000)] public int source;

        [HideIf("@" + nameof(comparisonType) + " == ComparisonType.QuestIsDone"), LabelWidth(200)]
        [VerticalGroup("optional", VisibleIf = nameof(isOptionalDialogue)), Range(0, 1000)] public int target;

        public DialogueLine(string answer, bool isOptionalDialogue, bool storeIfSelected, Sprite icon, QuestObjectSO optionalGameplayEffect, bool isValid, int source, int target)
        {
            this.answer = answer;
            this.isOptionalDialogue = isOptionalDialogue;
            this.storeIfSelected = storeIfSelected;
            this.icon = icon;
            this.optionalGameplayEffect = optionalGameplayEffect;
            this.conditionIsValid = isValid;
            this.source = source;
            this.target = target;
            comparisonType = ComparisonType.SmallerThan;
            questToValidate = null;
        }

        public void SetIsValid(bool isValid = false)
        {
            conditionIsValid = isValid;
        }

        public bool GetIsValid()
        {
            if (!isOptionalDialogue) return true;

            return comparisonType switch
            {
                ComparisonType.SmallerThanOrEqual => source <= target,
                ComparisonType.GreaterThan => source > target,
                ComparisonType.GreaterThanOrEqual => source >= target,
                ComparisonType.Equal => source == target,
                ComparisonType.NotEqual => source != target,
                ComparisonType.QuestIsDone => questToValidate.QuestObjectIsRetrieved,
                _ => false,
            };
        }
    }

    [NodeWidth(400), NodeTint(20, 120, 20)]
    [CreateNodeMenu("Dialogue Node")]
    public class DialogueNode : Node
    {
        // INodeState

        [SerializeField] private bool isRootNode; // PLACEHOLDER

        [Serializable] public class State { }

        [Input, SerializeField] protected State execIn = null;

        [TextArea] public string dialogueOpening;

        [Output(dynamicPortList = true), SerializeField] public List<DialogueLine> DialogueLines; 

        [Input, SerializeField] public DialogueLine conditionalAnswer;
        public bool conditionIsValid; 

        // protected INodeState NodeStateInterface { get; private set; }

        protected override void Init()
        {
            base.Init();
        }

        public bool IsRootNode() => isRootNode;

        // public virtual INodeState TryGetInterface() => NodeStateInterface;

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            base.OnCreateConnection(from, to);

            if (from.node.GetType() != typeof(ValidationNode)) return;

            UpdatePorts();  
            VerifyConnections();
        }

        public override void OnRemoveConnection(NodePort port)
        {
            if (port.IsOutput && port.node.GetType() != typeof(ValidationNode)) return;

            base.OnRemoveConnection(port);

            conditionalAnswer = GetInputValue("conditionalAnswer", new DialogueLine());
            conditionIsValid = conditionalAnswer.conditionIsValid;

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

        public virtual void MoveNext(int nextIndex)
        {
            DialogueNode exitNode = !GetOutputPort($"DialogueLines {nextIndex}").IsConnected ? null : GetOutputPort($"DialogueLines {nextIndex}").Connection.node as DialogueNode;
            DialogGraph dialogGraph = graph as DialogGraph;

            if (dialogGraph.CurrentNode != this)
            {
                Debug.LogError("Node isn't active");
                return;
            }

            if (!exitNode) return; 

            exitNode?.OnEnter();
        }

        public virtual void OnEnter()
        {
            Debug.Log("initialising dialogue node");

            DialogGraph fmGraph = graph as DialogGraph;
            fmGraph.CurrentNode = this;
        }

        public virtual void OnExit()
        {
            // 
        }
    }
}

