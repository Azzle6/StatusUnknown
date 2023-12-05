using UnityEngine;

public class HurtBox : MonoBehaviour, IDamageable
{
    public void TakeDamage(float damage, Vector3 force)
    {
        //Debug.Log($"{gameObject.name} took {damage} damage with this force : {force}");
    }
}
