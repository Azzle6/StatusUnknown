namespace Player
{
    using UnityEngine;
    public abstract class Weapon : MonoBehaviour
    {
        [Header("Ads angle")]
        public Transform adsRotTr;
        public float adsAimAngle;
        public float adsRestAngle;
        
        public abstract void TriggerPressed();
    
        public abstract void TriggerReleased();
    
        public abstract void Reload();

        public abstract void Hit();
    }

}

