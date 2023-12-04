namespace Module.Behaviours.Zone
{
    using System;

    [Serializable]
    public class PulsarZoneBehaviour : InstantiatedZoneModule
    {
        public int remainingPulses = 4;
        
        protected override void OnTick()
        {
            this.ApplyZoneDamage();
            this.remainingPulses--;
            if(this.remainingPulses <= 0)
                Destroy(this.gameObject);
        }
    }
}
