namespace Module.Behaviours.Zone
{
    using UnityEngine;

    public class PulsarZoneBehaviour : InstantiatedZoneModule
    {
        private int remainingPulses = 4;
        
        protected override void OnTick()
        {
            this.CollisionBehaviour();
            Debug.Log("Pulse");
            this.remainingPulses--;
            if(this.remainingPulses <= 0)
                Destroy(this.gameObject);
        }
    }
}
