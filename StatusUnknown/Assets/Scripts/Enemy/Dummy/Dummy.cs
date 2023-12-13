namespace Enemy.Dummy
{
    using UnityEngine;
    public class Dummy : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage, Vector3 force)
        {
            Debug.Log($"Dummy hit. Took {damage} damage.");
        }
    }
}
