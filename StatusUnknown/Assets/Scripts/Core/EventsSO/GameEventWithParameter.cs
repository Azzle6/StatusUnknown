namespace Core.EventsSO
{
    using UnityEngine;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;

    public abstract class GameEventWithParameter<T> : ScriptableObject
    {
        private readonly HashSet<GameEventWithParameterListener<T>> eventListeners =
            new HashSet<GameEventWithParameterListener<T>>();
        [Button("Raise Event"),HideInEditorMode]
        public void RaiseEvent(T parameter)
        {
            foreach (GameEventWithParameterListener<T> listener in this.eventListeners)
                listener.OnEventRaised(parameter);
        }

        public void RegisterListener(GameEventWithParameterListener<T> listener)
        {
            this.eventListeners.Add(listener);
        }
        
        public void UnregisterListener(GameEventWithParameterListener<T> listener)
        {
            if (this.eventListeners.Contains(listener))
                this.eventListeners.Remove(listener);
        }
    }
}
