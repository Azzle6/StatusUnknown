namespace Module.Behaviours.Zone
{
    public class ExplosionZoneBehaviour : InstantiatedZoneModule
    {
        protected override void OnStart()
        {
            this.CollisionBehaviour();
            Destroy(this.gameObject);
        }
    }
}
