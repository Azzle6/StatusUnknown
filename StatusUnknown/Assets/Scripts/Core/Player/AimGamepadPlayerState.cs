namespace Core.Player
{
    using UnityEngine;
    using System.Collections;
    public class AimGamepadPlayerState : PlayerState
    {
        private Vector2 aimDirection;
        private Coroutine aiming;
        private Transform snapTo;
        [SerializeField] private PlayerStat playerStat;
        public override void OnStateEnter()
        {
            
            aiming = StartCoroutine(Aim());
        }
        public override void Behave<T>(T x)
        {
            if (x is Vector2 aim)
                aimDirection = aim;
            if (aiming == default)
                aiming = StartCoroutine(Aim());
        }

        private IEnumerator Aim()
        {
            while (aimDirection.magnitude > 0.01f)
            {
                TargetDetection();
                if (snapTo == null)
                {
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), new Vector3(aimDirection.x,0,aimDirection.y), playerStat.turnSpeed);
                }
                else
                {
                    playerStateInterpretor.transform.LookAt(snapTo.transform.position);
                }

                yield return null;
            }
        }

        private void TargetDetection()
        {
            Ray ray = new Ray(playerStateInterpretor.transform.position,new Vector3(aimDirection.x,0,aimDirection.y));
            RaycastHit hit;
            Debug.DrawRay(playerStateInterpretor.transform.position, new Vector3(aimDirection.x,0,aimDirection.y) * 50, Color.blue);
            if (Physics.Raycast(ray, out hit, 50))
            {
                if (hit.collider.transform.TryGetComponent(out Target target))
                {
                    snapTo = target.transform;                 
                }
                else
                {
                    snapTo = default;
                }
            }
        }

        public override void OnStateExit()
        {
            snapTo = default;
            if (aiming != default)
            {
                StopCoroutine(aiming);
                aiming = default;
            
            } 
        }
    }
}


