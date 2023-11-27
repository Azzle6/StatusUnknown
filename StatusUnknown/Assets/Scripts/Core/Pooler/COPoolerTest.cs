using System;
using Core.Pooler;
using UnityEngine;
using UnityEngine.VFX;

public class COPoolerTest : MonoBehaviour
{
    [SerializeField] private CoPoolVFX vfxToPool;
    [SerializeField] private VisualEffect vfxPrefab;
    [SerializeField] private VisualEffect vfx;
    
    [SerializeField] private CoPoolProjectile projectileToPool;
    [SerializeField] private Weapon.Projectile projectilePrefab;
    [SerializeField] private Weapon.Projectile projectile;

    private void Awake()
    {
        vfxToPool = new CoPoolVFX();
        vfxToPool.prefab = vfxPrefab;
        vfxToPool.baseCount = 10;
        ComponentPooler.Instance.CreatePool(vfxPrefab, 10);
        
        projectileToPool = new CoPoolProjectile();
        projectileToPool.prefab = projectilePrefab;
        projectileToPool.baseCount = 10;
        ComponentPooler.Instance.CreatePool(projectilePrefab, 10);
        
    }

    private void OnEnable()
    {
        vfx = ComponentPooler.Instance.GetPooledObject<VisualEffect>(vfxPrefab.name);
        projectile = ComponentPooler.Instance.GetPooledObject<Weapon.Projectile>(projectilePrefab.name);
    }
}
