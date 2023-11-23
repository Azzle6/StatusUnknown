using System;
using DG.Tweening;

namespace Player
{
    
    using Core.VariablesSO.VariableTypes;
    using UnityEngine;

    public class MedikitPickup : MonoBehaviour
    {
        [SerializeField] private IntVariableSO medikitAmount;
        [SerializeField] private int amount;

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
            }
        }
    }

}
