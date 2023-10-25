using System.Collections.Generic;
using Yarn.Unity;

/// <summary>
/// Base Class to store dialogue variables. Must be inherited from
/// </summary>

namespace StatusUnknown.Systems.Dialogue
{
    public abstract class VariableStorageBase : VariableStorageBehaviour
    {
        public override void Clear()
        {

        }

        public override bool Contains(string variableName)
        {
            throw new System.NotImplementedException();
        }

        public override (Dictionary<string, float> FloatVariables, Dictionary<string, string> StringVariables, Dictionary<string, bool> BoolVariables) GetAllVariables()
        {
            throw new System.NotImplementedException();
        }

        public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
        {

        }

        public override void SetValue(string variableName, string stringValue)
        {

        }

        public override void SetValue(string variableName, float floatValue)
        {

        }

        public override void SetValue(string variableName, bool boolValue)
        {

        }

        public override bool TryGetValue<T>(string variableName, out T result)
        {
            throw new System.NotImplementedException();
        }
    }
}
