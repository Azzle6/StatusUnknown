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
        [HideInInspector] public List<float> angleRequired;
        [HideInInspector] public Collider closestTarget;
        [SerializeField] private PlayerStat playerStat;
        private float bestAngleToClosestTarget;
        private float currentPlayerAngle;
        private Vector3 playerPos;
        private Vector3 targetPos;
        
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
            frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            
            //recover colliders in the frustrum 
            visibleColliders = Physics.OverlapSphere(mainCamera.transform.position, 50f);
           
            confirmedInTheFrustrum = new List<Collider>();
            confirmedInTheAngle = new List<float>();
            angleRequired = new List<float>();
            closestTarget = default;
            foreach (Collider collider in visibleColliders)
            {
                if (GeometryUtility.TestPlanesAABB(frustumPlanes, collider.bounds) && (collider.gameObject.TryGetComponent(out Target target)))
                {
                    confirmedInTheFrustrum.Add(collider);
                    playerPos = playerStateInterpretor.transform.position;
                    playerPos.y = 0;
                    targetPos = collider.transform.position;
                    targetPos.y = 0;
                    confirmedInTheAngle.Add(TurningADirectionInAngle((targetPos - playerPos).normalized));
                }
            }
          
        }

        private void DetermineClosestTarget()
        {
            bestAngleToClosestTarget = 1000;

            for (int x = 0; x < confirmedInTheFrustrum.Count; x++)
            {
                //angle required must be determined by the distance to the player
                targetPos = confirmedInTheFrustrum[x].transform.position;
                targetPos.y = 0;
                angleRequired.Add(1 + playerStat.angleRequiredMultiplierByDistance.Evaluate(Vector3.Distance(playerPos, targetPos)));
                //need to determine if the player is looking in the min max of the target if not discard this target 
                if (confirmedInTheAngle[x] < angleRequired[x])
                {
                    if (confirmedInTheAngle[x] < bestAngleToClosestTarget)
                    {
                        closestTarget = confirmedInTheFrustrum[x];
                        bestAngleToClosestTarget = confirmedInTheAngle[x];
                    }
                }
            }
        }
        
        public float TurningADirectionInAngle(Vector3 direction)
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
