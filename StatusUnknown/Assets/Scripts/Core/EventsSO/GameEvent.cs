namespace Core.EventsSO
{
    using System.Collections.Generic;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "CustomAssets/BaseGameEvent", fileName = "GameEvent", order = 0)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = this.listeners.Count - 1; i >= 0; i++)
                this.listeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {
            this.listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            this.listeners.Remove(listener);
        }
    }
}
