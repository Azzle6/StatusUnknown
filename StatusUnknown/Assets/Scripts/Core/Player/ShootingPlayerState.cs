namespace Core.Player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShootingPlayerState : PlayerState
    {
        private Coroutine shooting;
        private Transform snapTo;
        private int weaponNo;
        
        private bool isShooting;
        private Camera mainCamera;
        
        //public variable are for editor script

        private Collider[] visibleColliders;
        private Plane[] frustumPlanes;
        [HideInInspector] public List<Collider> confirmedInTheFrustrum;
        [HideInInspector] public List<float> confirmedInTheAngle;
        public Collider closestTarget;
        private float bestAngleToClosestTarget;
        private float currentPlayerAngle;
        private float angleRequired;
        
        private void Awake()
        {
            mainCamera = Camera.main;
        }


        public override void OnStateEnter()
        {
            shooting = StartCoroutine(Shoot());
            isShooting = true;
        }
        
        public override void Behave<T>(T x)
        {
            if (x is int weapon)
                weaponNo = weapon;

            if (shooting == default)
                shooting = StartCoroutine(Shoot());
        }
        
        private IEnumerator Shoot()
        {
            while (isShooting)
            {
                //use old method to shoot
                //SnapToTarget();
                //new method : 
                FrustrumCulling();
                DetermineClosestTarget();
                //need to check if gun is automatic or not
                //need to check for the gun fire rate 
                //need to check for the gun ammo
                yield return null;
            }
        }
        
        private void FrustrumCulling()
        {
            //need to check if the target is in the camera frustrum
            //then direct gun to it
            //then shoot
            frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            
            //recover colliders in the frustrum 
            visibleColliders = Physics.OverlapSphere(mainCamera.transform.position, 50f);
           
            confirmedInTheFrustrum = new List<Collider>();
            confirmedInTheAngle = new List<float>();
            closestTarget = default;
            foreach (Collider collider in visibleColliders)
            {
                if (GeometryUtility.TestPlanesAABB(frustumPlanes, collider.bounds) &&
                    (collider.gameObject.TryGetComponent(out Target target)))
                {
                    confirmedInTheFrustrum.Add(collider);
                    confirmedInTheAngle.Add(TurningADirectionInAngle((collider.transform.position - playerStateInterpretor.transform.position).normalized));
                }
            }
          
        }

        private void DetermineClosestTarget()
        {
            bestAngleToClosestTarget = 1000;

            for (int x = 0; x < confirmedInTheFrustrum.Count; x++)
            {
                //angle required min and max must be determined by the distance to the player
                angleRequired = 10;
                //need to determine if the player is looking in the min max of the target if not discard this target 
                if (confirmedInTheAngle[x] < angleRequired)
                {
                    if (confirmedInTheAngle[x] < bestAngleToClosestTarget)
                    {
                        closestTarget = confirmedInTheFrustrum[x];
                        bestAngleToClosestTarget = confirmedInTheAngle[x];
                    }
                }
            }
        }
        
        private float TurningADirectionInAngle(Vector3 direction)
        {
            float angle = Vector3.Angle(playerStateInterpretor.transform.forward.normalized, direction);
            return angle;
        }
        
        
        public override void OnStateExit()
        {
            if (shooting != default)
                StopCoroutine(shooting);
            isShooting = false;
        }
        
    }
}
