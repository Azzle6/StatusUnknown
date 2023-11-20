namespace Module
{
    using System;
    using Core.Helpers;
    using UnityEngine;

    [Serializable]
    public struct Output
    {
        public TriggerSO triggerType;
        public Vector2Int localPosition;
        public E_Direction direction;
    }
}
