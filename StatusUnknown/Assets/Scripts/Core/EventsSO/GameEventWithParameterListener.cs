namespace Core.EventsSO
{
    using UnityEngine;
    using UnityEngine.Events;
    
    public abstract class GameEventWithParameterListener<T> : ScriptableObject
    {
        [SerializeField] private GameEventWithParameter<T> gameEvent;

        public GameEventWithParameter<T> GameEvent
        {
            get => this.gameEvent;
            set => this.gameEvent = value;
        }

        [SerializeField] private UnityEvent<T> unityEventResponse;

        private void OnEnable()
        {
            if(this.gameEvent == null)
                return;
            
            this.gameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if(this.gameEvent == null)
                return;
            
            this.gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T parameter)
        {
            if(this.unityEventResponse != null)
                this.unityEventResponse.Invoke(parameter);
        }
        
    }
}
