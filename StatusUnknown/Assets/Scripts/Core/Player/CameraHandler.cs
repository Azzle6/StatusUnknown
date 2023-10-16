using System;

namespace Core.Player
{
    using UnityEngine;
    
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Camera cam;
        private float zoomTimer;
        private Vector3 targetPos;
        [SerializeField] private CamState currentCamState;

        private void LateUpdate()
        {
            switch (currentCamState)
            {
                case CamState.EXPLORING:
                    Exploring();
                    break;
                case CamState.FIGHT:
                    Fighting();
                    break;
                case CamState.SHOWING:
                    break;
                
            }
        }
        
        public void SetCamState(CamState camState)
        {
            currentCamState = camState;
        }
        
        private void Exploring()
        {
            targetPos = playerTransform.position + CameraStat.Instance.defaultOffset;
            if (transform.position != targetPos)
                cam.transform.position = Vector3.Slerp(cam.transform.position, targetPos, CameraStat.Instance.smoothSpeed * Time.deltaTime);
        }

        private void Fighting()
        {
            targetPos = playerTransform.position + CameraStat.Instance.fightModeOffset;
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, CameraStat.Instance.smoothSpeed * Time.deltaTime);
        }
        
    }
    
    public enum CamState
    {
        EXPLORING,
        FIGHT,
        SHOWING
    }

}
