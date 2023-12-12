namespace Module.Behaviours.Zone
{
    using UnityEngine;

    public class MineBehaviour : InstantiatedZoneModule
    {
        protected override void OnZoneInit()
        {
            this.gameObject.AddComponent<MeshFilter>().mesh = this.ZoneData.Mesh;
            this.gameObject.AddComponent<MeshRenderer>().material = this.ZoneData.Material;

            if (Physics.Raycast(transform.position, Vector3.down * 3, out var hit, LayerMask.NameToLayer("Walkable")))
                transform.position = hit.point;
        }
        
        protected override void OnFixedUpdate()
        {
            Collider[] colliders = this.CheckCollisions();

            if (colliders.Length > 0)
            {
                this.ApplyZoneDamage();
                this.DestroyModule();
            }
        }
    }
}
