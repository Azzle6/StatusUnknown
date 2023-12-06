namespace Core.Pooler
{
    
    using UnityEngine;
    using UnityEngine.VFX;
    
    [System.Serializable]
    public class COPool <T> where T : Component
    {
        public T prefab;
        public int baseCount;
    }
    
    [System.Serializable]
    public class CoPoolProjectile : COPool<Weapon.Projectile> {}
    [System.Serializable]
    public class CoPoolVFX : COPool<VisualEffect> {}
    

    

    
}