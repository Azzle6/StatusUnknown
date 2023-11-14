using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContext : MonoBehaviour, IDamageable
{
    [SerializeField] protected Animator animator;
    EnemyState currentState;
    public EnemyStats stats;
    [SerializeField] protected MassBody massBody;
    [Header("Debug")]
    [SerializeField] MeshRenderer meshRenderer;
    float currentHealth;
    private void Start()
    {
        currentHealth = stats.health;
    }
    private void Update()
    {
        currentState.Update();
    }
    public void SwitchState(EnemyState state)
    {
        currentState = state;
        currentState.SetContext(this);
    }

    public void AddForce(Vector3 force)
    {
        massBody.AddForce(force);
    }
    public void changeColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    public void TakeDamage(float damage, Vector3 force)
    {
        currentHealth -= damage;
        AddForce(force);
        Debug.Log("Enemy took damage");
    }
}
