using System;
using XNode;

// -- LOGIC --
// AND
// OR
// NOT
// BRANCH
// FOR
// SWITCH
// DIALOGUE BRANCH

// bool I/O from derived classes must be serialized and Hidden, because their value is not edited manually but via other node types
namespace StatusUnknown.Tools.Narrative
{
    public abstract class DialogLogicNode : Node
    {
        public Action onStateChange;
        public abstract bool Led { get; }

        public void SendSignal(NodePort output)
        {
            // Loop through port connections
            int connectionCount = output.ConnectionCount;
            for (int i = 0; i < connectionCount; i++)
            {
                NodePort connectedPort = output.GetConnection(i);

                // Get connected ports logic node
                DialogLogicNode connectedNode = connectedPort.node as DialogLogicNode;

                // Trigger it
                if (connectedNode != null) connectedNode.OnInputChanged();
            }
            onStateChange?.Invoke();
        }

        protected abstract void OnInputChanged();

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            OnInputChanged();
        }
    }
}

