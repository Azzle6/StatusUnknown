using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyContext : MonoBehaviour, IDamageable
{
    [SerializeField] HitContext[] hitContexts;
    [SerializeField] protected Animator animator;
    EnemyState currentState;
    public string stateName => currentState.ToString();
    public abstract EnemyStats stats { get; }
    public Vector3 Velocity { get { return body.velocity; } }
    Quaternion rotation;

    [SerializeField] protected Rigidbody body;
    [SerializeField] protected LayerMask avoidanceMask;
    public event Action<EnemyContext> OnDeathEvent;

    [Header("Debug")]
    [SerializeField] protected MeshRenderer meshRenderer;
    float currentHealth;
    void OnEnable()
    {
        foreach (var context in hitContexts)
            context.HitTriggerEvent += PerformHitEffect;
    }
    void OnDisable() 
    { 
        foreach (var context in hitContexts)
            context.HitTriggerEvent -= PerformHitEffect;
    }
    private void Start()
    {
        InitializeEnemy();
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
    void PerformHitEffect(IDamageable target)
    {
        if(target.ToString() == "null") return;
        target.TakeDamage(stats.AttackDamage, transform.forward * 3);
    }
    public void AddForce(Vector3 force)
    {
        body?.AddForce(force);
    }
    public Vector3 GetAvoidance()
    {
        // avoidance
        Vector3 avoidForce = Vector3.zero;
        var avoidColliders = Physics.OverlapSphere(transform.position, stats.avoidDistance, avoidanceMask);
        foreach (var collider in avoidColliders)
        {
            Vector3 repulsePoint = collider.ClosestPoint(transform.position);
            Vector3 repulseVector = transform.position - repulsePoint;
            float repulseMagnitude = repulseVector.magnitude;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -repulseVector, out hit, stats.avoidDistance, avoidanceMask))
            {
                Debug.DrawLine(transform.position, repulsePoint);
                float hitAngle = Vector3.Angle(Vector3.up, hit.normal);
                avoidForce += (repulseVector * stats.avoidStrength / (repulseMagnitude * repulseMagnitude));
            }

        }
        if (avoidColliders.Length > 0)
            return avoidForce / avoidColliders.Length;

        return Vector3.zero;
    }
    public void RotateTowards( Vector3 direction, float angleSpeed)
    {
        //Debug.Log("desiredVelocity " + _desiredDirection);
        if(direction == Vector3.zero) return;
        Quaternion desiredRotation = Quaternion.LookRotation(direction.normalized);
        rotation = Quaternion.RotateTowards(rotation, desiredRotation, angleSpeed * Time.deltaTime);
        // assign rotation
        Vector3 forward = rotation * Vector3.forward;
        if(forward != Vector3.zero)
            transform.forward = forward; // required to fit with navMeshAgent constraints;
    }
    public void PlayAnimation(string name)
    {
        animator.Play(name);
    }
    public void TakeDamage(float damage, Vector3 force)
    {
        EnemyTakeDamage(damage, force);
    }

    protected virtual void EnemyTakeDamage(float damage, Vector3 force)
    {
        currentHealth -= damage;
        AddForce(force);
        //Debug.Log($"{gameObject.name} took {damage} damage {currentHealth}/{stats.health}");
        if (currentHealth <= 0)
            Death();
    }
    protected virtual void Death()
    {
        Debug.Log($"Death {gameObject.name}");
        OnDeathEvent?.Invoke(this);
        Destroy(gameObject);
    }
    protected virtual void InitializeEnemy()
    {
        currentHealth = stats.health;
        Debug.Log($" current health {currentHealth}");
        rotation = transform.rotation;
    }
    void OnDrawGizmos()
    {
        if(Application.isPlaying)
            currentState.DebugGizmos();
    }
}
