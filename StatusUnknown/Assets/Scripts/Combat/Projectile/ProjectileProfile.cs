
using UnityEngine;

[System.Serializable]
public struct ProjectileProfile 
{
    [Header("Behaviour")]
    public int damage;
    public float speed;
    public float time;
    [Header("Collision")]
    public LayerMask enemyMask;
    public LayerMask collisionMask;
    [SerializeReference]
    public HitShape hitShape;
}
