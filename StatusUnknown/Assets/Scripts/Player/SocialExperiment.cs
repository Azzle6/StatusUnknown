using System.Collections;
using System.Collections.Generic;
using Core.Pooler;
using Core.VariablesSO.VariableTypes;
using UnityEngine;
using Weapon;

public class SocialExperiment : MonoBehaviour
{
    /*[SerializeField] private float spawnFrequency = 0.5f;
    [SerializeField] private float deSpawnFrequency = 1f;
    private PhotonPistolBullet tr;
    private void Start()
    {
        StartCoroutine(Spawn());
    }
    
    private IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnFrequency);
            tr = Pooler.Instance.GetPooledObject<PhotonPistolBullet>("PlayerPhotonProjectile");
            tr.transform.position = transform.position;
            StartCoroutine(Despawn(tr.gameObject));
        }
    }


    private IEnumerator Despawn(GameObject obj)
    {
        yield return new WaitForSeconds(deSpawnFrequency);
        Pooler.Instance.ReturnObjectToPool(obj);
    }*/
    
}
