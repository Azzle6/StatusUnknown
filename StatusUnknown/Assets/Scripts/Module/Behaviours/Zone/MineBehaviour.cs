namespace Module.Behaviours.Zone
{
    using UnityEngine;

    public class MineBehaviour : InstantiatedZoneModule
    {
        private float explosionRange;

        protected override void OnZoneInit()
        {
            this.gameObject.AddComponent<MeshFilter>().mesh = this.ZoneData.Mesh;
            this.gameObject.AddComponent<MeshRenderer>().material = this.ZoneData.Material;
        }
        
        protected override void OnFixedUpdate()
        {
            Collider[] colliders = this.CheckCollisions();

            if (colliders.Length > 0)
            {
                this.ApplyZoneDamage();
                Destroy(this.gameObject);
            }
        }
    }
}
