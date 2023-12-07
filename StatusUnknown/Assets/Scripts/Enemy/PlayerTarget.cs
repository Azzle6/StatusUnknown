using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Start");
        CombatManager.SetPlayerTarget(transform);
    }
    private void OnEnable()
    {
        CombatManager.SetPlayerTarget(transform);
    }
    private void OnDisable()
    {
        CombatManager.SetPlayerTarget(null);
    }
}
