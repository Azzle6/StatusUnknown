using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStat stat;
    private float health;

    private void Awake()
    {
        health = stat.maxHealth;
    }

    public void TakeDamage(float damage, Vector3 force)
    {
        health -= damage;
        Debug.Log("Player took " + damage + " damage");
    }
    
    public void Heal(float amount)
    {
        health += amount;
    }
}
