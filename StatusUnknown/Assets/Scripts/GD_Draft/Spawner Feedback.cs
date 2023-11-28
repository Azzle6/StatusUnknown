using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerFeedback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destruction());
    }

    private IEnumerator Destruction()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

}
