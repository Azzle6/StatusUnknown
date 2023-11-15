namespace Player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShootingPlayerState : PlayerState
    {
        private Coroutine shooting;
        private int weaponNo;
        
        private Camera mainCamera;
        
        //public variable are for editor script

        private Collider[] visibleColliders;
        private Plane[] frustumPlanes;
        [HideInInspector] public List<Collider> confirmedInTheFrustrum;
        [HideInInspector] public List<float> confirmedInTheAngle;
        [HideInInspector] public List<float> angleRequired;
        [HideInInspector] public Collider closestTarget;
        public PlayerStat playerStat;
        [SerializeField] private float distanceVisibleCollider;
        [SerializeField] private float distanceVisibleRadius;
        private float bestAngleToClosestTarget;
        private float currentPlayerAngle;
        private Vector3 playerPos;
        private Vector3 targetPos;
        
        [SerializeField] private WeaponManager weaponManager; 
        
        private void Awake()
        {
            mainCamera = Camera.main;
            confirmedInTheFrustrum = new List<Collider>();
            confirmedInTheAngle = new List<float>();
            angleRequired = new List<float>();
            closestTarget = default;
            playerStat.isShooting = false;
        }


        public override void OnStateEnter()
        {
            if (playerStat.weaponMelee[weaponNo])
            {
                Fire();
            }
            else
            {
                playerStat.isShooting = true;
                shooting = StartCoroutine(Shoot());
            }
        }
        
        public override void Behave<T>(T x)
        {
            if (x is int weapon)
                weaponNo = weapon;

            if (playerStat.weaponMelee[weaponNo])
            {
                Fire();
            }
            else
            {
                playerStat.isShooting = true;
                if (shooting == default)
                    shooting = StartCoroutine(Shoot());
            }
        }
        
        private IEnumerator Shoot()
        {
            while (playerStat.isShooting && playerStat.weaponMelee[weaponNo] == false)
            {
                FrustrumCulling();
                DetermineClosestTarget();
                Fire();
                yield return null;
            }
        }
        
        private void FrustrumCulling()
        {
            frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            
            //recover colliders in the frustrum 
            visibleColliders = Physics.OverlapSphere(playerStateInterpretor.transform.position + playerStateInterpretor.transform.forward * distanceVisibleCollider, distanceVisibleRadius, playerStat.aimLayerMask);
           
            confirmedInTheFrustrum.Clear(); 
            confirmedInTheAngle.Clear();
            angleRequired.Clear();
            closestTarget = default;
            foreach (Collider collider in visibleColliders)
            {
                if (GeometryUtility.TestPlanesAABB(frustumPlanes, collider.bounds))
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
  
                if (confirmedInTheAngle[x] < angleRequired[x])
                {
                    if (confirmedInTheAngle[x] < bestAngleToClosestTarget)
                    {
                        closestTarget = confirmedInTheFrustrum[x];
                        bestAngleToClosestTarget = confirmedInTheAngle[x];
                        playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), closestTarget.transform.position - playerStateInterpretor.transform.position, playerStat.turnSpeed);
                    }
                }
            }
        }

        private float TurningADirectionInAngle(Vector3 direction)
        {
            float angle = Vector3.Angle(playerStateInterpretor.transform.forward.normalized, direction);
            return angle;
        }
        
        private void Fire()
        {
            weaponManager.PressTriggerWeapon(weaponNo); 
        }
        
        public override void OnStateExit()
        {
            weaponManager.ReleaseTriggerWeapon();
            if (shooting != default)
                StopCoroutine(shooting);
            playerStat.isShooting = false;
        }
        
        private void ShowDetectionZone()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerStateInterpretor.transform.position + playerStateInterpretor.transform.forward * distanceVisibleCollider, distanceVisibleRadius);
        }
    }
}
