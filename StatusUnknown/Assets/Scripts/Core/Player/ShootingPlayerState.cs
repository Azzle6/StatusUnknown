using System.Collections.Generic;

namespace Core.Player
{
    using System.Collections;
    using UnityEngine;

    public class ShootingPlayerState : PlayerState
    {
        private Coroutine shooting;
        private Transform snapTo;
        private int closestTargetIndex;
        private Vector3[] midPointToTarget;
        private Vector3[] outPointToTarget;
        private Vector3 playerDirection;
        private RaycastHit[] snapHitsIn;
        private Ray inRay;
        private Ray reverseRay;
        private RaycastHit reverseHit;
        private float bestdistanceToClosestTarget;
        private float distanceToClosestTarget;
        private int weaponNo;

        private Ray playerToMouseRay;
        private bool isShooting;
        private Camera mainCamera;
        
        private Collider[] visibleColliders;
        private Plane[] frustumPlanes;
        private List<Collider> confirmedInTheFrustrum;
        
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
            DebugDrawFrustrum();
            confirmedInTheFrustrum = new List<Collider>();
            foreach (Collider collider in visibleColliders)
            {
                if (GeometryUtility.TestPlanesAABB(frustumPlanes, collider.bounds))
                {
                    confirmedInTheFrustrum.Add(collider);
                }
            }
        }

        private void DebugDrawFrustrum()
        {
        }
        
        
        
        private void SnapToTarget()
        {
            playerDirection = playerStateInterpretor.transform.forward;
            inRay = new Ray(playerStateInterpretor.transform.position, playerDirection);
            snapHitsIn = Physics.RaycastAll(inRay, 50);
            if (snapHitsIn.Length == 0)
                return;
                        
            midPointToTarget = new Vector3[snapHitsIn.Length];
            outPointToTarget = new Vector3[snapHitsIn.Length];
            bestdistanceToClosestTarget = 1000;
            for (int x = 0; x < snapHitsIn.Length; x++)
            {
                reverseRay = new Ray(snapHitsIn[x].point+ (playerDirection*30), -playerDirection);
                Debug.DrawRay(snapHitsIn[x].point+ (playerDirection*30), -playerDirection * 50, Color.yellow);

                if (snapHitsIn[x].collider.Raycast(reverseRay, out reverseHit, 50f))
                {
                    midPointToTarget[x] = (snapHitsIn[x].point + reverseHit.point) / 2;
                    outPointToTarget[x] = reverseHit.point;
         
                    distanceToClosestTarget = Vector3.Distance(midPointToTarget[x], snapHitsIn[x].collider.transform.position);
                    
                     if (Vector3.Distance(snapHitsIn[x].point, reverseHit.point) < bestdistanceToClosestTarget)
                     {
                        closestTargetIndex = x;
                        snapTo = snapHitsIn[x].collider.transform;
                     } 
                }
                

            }
            
            Debug.DrawRay(playerStateInterpretor.transform.position, playerDirection * 50, Color.green);

            if (snapTo == null)
                return;
        
            playerStateInterpretor.transform.LookAt(snapTo.transform);

                    
        } 
        
        public override void OnStateExit()
        {
            if (shooting != default)
                StopCoroutine(shooting);
            isShooting = false;
        }
        
        private void OnDrawGizmos()
        {
            if (mainCamera == null)
                return;
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(mainCamera.transform.position, 50f);
            
            if (snapHitsIn == null)
                return;
            if (snapHitsIn.Length == 0)
                return;

            for (int x = 0; x < snapHitsIn.Length; x++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(snapHitsIn[x].point, 0.5f);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(midPointToTarget[x], 0.5f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(outPointToTarget[x], 0.5f);
            }
        }
        
    }

        
}
