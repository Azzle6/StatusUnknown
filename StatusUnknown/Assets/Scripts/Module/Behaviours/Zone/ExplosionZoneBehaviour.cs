namespace Module.Behaviours.Zone
{
    public class ExplosionZoneBehaviour : InstantiatedZoneModule
    {
        protected override void OnStart()
        {
            this.ApplyZoneDamage();
            this.DestroyModule();
        }
    }
}
