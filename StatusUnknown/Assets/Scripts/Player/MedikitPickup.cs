using System;
using Core.EventsSO.GameEventsTypes;
using DG.Tweening;
using UI;

namespace Player
{
    
    using Core.VariablesSO.VariableTypes;
    using UnityEngine;

    public class MedikitPickup : MonoBehaviour
    {
        [SerializeField] private IntVariableSO medikitAmount;
        [SerializeField] private PopUpDataGameEvent popUpEvent;
        [SerializeField] private PopUpData popUpData;
        [SerializeField] private int amount;
        [SerializeField] private Sprite medikitSprite;

        private void Awake()
        {
            transform.DORotate(new Vector3(0, 360, 0), 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                medikitAmount.Value += amount;
                gameObject.SetActive(false);
                popUpEvent.RaiseEvent(popUpData);
            }
        }
    }

}
