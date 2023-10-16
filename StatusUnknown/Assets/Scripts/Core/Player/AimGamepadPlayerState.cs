namespace Core.Player
{
    using UnityEngine;
    using System.Collections;
    public class AimGamepadPlayerState : PlayerState
    {
        private Vector2 aimDirection;
        private Coroutine aiming;
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
                playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), new Vector3(aimDirection.x,0,aimDirection.y), PlayerStat.Instance.turnSpeed);
                yield return null;
            }
        }

        public override void OnStateExit()
        {
            StopCoroutine(aiming);
        }
    }
}


