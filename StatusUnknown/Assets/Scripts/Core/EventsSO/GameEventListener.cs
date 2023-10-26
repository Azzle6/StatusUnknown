namespace Core.EventsSO
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.Events;
    
    public class GameEventListener : MonoBehaviour
    {
        [Required]
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            this.Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            this.Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            this.Response.Invoke();
        }
    }
}
