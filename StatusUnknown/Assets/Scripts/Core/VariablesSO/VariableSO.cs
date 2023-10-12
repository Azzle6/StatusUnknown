namespace Core.VariablesSO
{
    using System;
    using UnityEngine;
    public class VariableSO<T> : ScriptableObject
    {
        private T value;

        public T Value
        {
            get => this.value;
            set => this.onValueChanged?.Invoke(value);
        }

        private Action<T> onValueChanged;

        public void RegisterOnValueChanged(Action<T> method)
        {
            this.onValueChanged += method;
        }

        public void UnregisterOnValueChanged(Action<T> method)
        {
            this.onValueChanged -= method;
        }
    }
}
