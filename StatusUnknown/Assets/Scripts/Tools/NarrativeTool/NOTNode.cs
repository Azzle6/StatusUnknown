using UnityEngine;
using XNode;
using System.Linq;

namespace StatusUnknown.Tools.Narrative
{
    [NodeWidth(140), NodeTint(100, 100, 70)]
    [CreateNodeMenu("Logic Node")]
    public class NOTNode : DialogConditionNode
    {
        [Input, HideInInspector] public bool input;
        [Output, HideInInspector] public bool output = true;
        public override bool Led { get { return output; } }

        protected override void OnInputChanged()
        {
            bool newInput = GetPort("input").GetInputValues<bool>().Any(x => x); // NOT logic

            if (input != newInput)
            {
                input = newInput;
                output = !newInput; // NOT logic
                SendSignal(GetPort("output"));
            }
        }

        public override object GetValue(NodePort port)
        {
            return output;
        }
    }
}
