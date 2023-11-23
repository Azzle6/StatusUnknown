using System;
using Core.Pooler;
using UnityEngine;
using UnityEngine.VFX;

public class COPoolerTest : MonoBehaviour
{
    [SerializeField] private CoPoolVFX vfxToPool;
    [SerializeField] private VisualEffect vfxPrefab;
    [SerializeField] private VisualEffect vfx;

    private void Awake()
    {
        vfxToPool = new CoPoolVFX();
        vfxToPool.Component = vfxPrefab;
        vfxToPool.baseCount = 10;
        ComponentPooler.Instance.CreatePool(vfxPrefab, 10);
    }

    private void Start()
    {
        vfx = ComponentPooler.Instance.GetPooledObject<VisualEffect>(vfxPrefab.name);
    }
}
