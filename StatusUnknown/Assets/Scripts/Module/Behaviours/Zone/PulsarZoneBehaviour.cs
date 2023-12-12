namespace Module.Behaviours.Zone
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PulsarZoneBehaviour : InstantiatedZoneModule
    {
        public int remainingPulses = 4;

        protected override void OnZoneInit()
        {
            if (Physics.Raycast(transform.position, Vector3.down * 3, out var hit, LayerMask.NameToLayer("Walkable")))
                transform.position = hit.point;
        }
        
        protected override void OnTick()
        {
            this.ApplyZoneDamage();
            this.remainingPulses--;
            if(this.remainingPulses <= 0)
                this.DestroyModule();
        }
    }
}
