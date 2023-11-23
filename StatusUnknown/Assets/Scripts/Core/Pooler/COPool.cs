using System;
using Sirenix.Serialization;
using UnityEngine.VFX;

namespace Core.Pooler
{
    
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;

    
    [System.Serializable]
    public class COPool <T> where T : Component
    {
        public T Component;
        public int baseCount;
    }
    
    public class CoPoolProjectile : COPool<Projectile> {}
    
    public class CoPoolVFX : COPool<VisualEffect> {}
    
    public enum PoolType
    {
        PROJECTILE,
        VFX
    }
    

    
}