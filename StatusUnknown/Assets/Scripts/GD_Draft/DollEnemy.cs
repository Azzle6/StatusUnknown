using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollEnemy : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("PLayer");
        transform.LookAt(player.transform);
    }
}
