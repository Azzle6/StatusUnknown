namespace Core.EventsSO
{
    using UnityEngine;
    using System.Collections.Generic;
    
    public abstract class GameEventWithParameter<T> : ScriptableObject
    {
        private readonly HashSet<GameEventWithParameterListener<T>> eventListeners =
            new HashSet<GameEventWithParameterListener<T>>();

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
