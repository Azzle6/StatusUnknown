using System.Linq;
using UnityEngine;
using XNode;

namespace StatusUnknown.Tools.Narrative
{
    [NodeWidth(140), NodeTint(100, 70, 70)]
    [CreateNodeMenu("Logic Node")]
    public class ANDNode : DialogConditionNode
    {
        [Input, HideInInspector] public bool input;
        [Output, HideInInspector] public bool output;
        public override bool Led { get { return output; } }

        protected override void OnInputChanged()
        {
            bool newInput = GetPort("input").GetInputValues<bool>().All(x => x); // And Logic

            if (input != newInput)
            {
                input = newInput;
                output = newInput; // And Logic
                SendSignal(GetPort("output"));
            }
        }

        public override object GetValue(NodePort port)
        {
            return output;
        }
    }
}
