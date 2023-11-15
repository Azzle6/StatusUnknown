using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContext : MonoBehaviour, IDamageable
{
    [SerializeField] protected Animator animator;
    EnemyState currentState;
    public EnemyStats stats;
    public Vector3 Velocity { get { return body.velocity; } }
    Quaternion rotation;

    [SerializeField] protected Rigidbody body;
    [SerializeField] protected LayerMask avoidanceMask;

    [Header("Debug")]
    [SerializeField] MeshRenderer meshRenderer;
    float currentHealth;
    private void Start()
    {
        currentHealth = stats.health;
        rotation = transform.rotation;
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
        body.AddForce(force);
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
        currentHealth -= damage;
        AddForce(force);
        Debug.Log("Enemy took damage");
    }
}
