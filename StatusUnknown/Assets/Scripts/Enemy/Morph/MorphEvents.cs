using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MorphEvents 
{
    // TODO : Define usefull parameters more tahn MorphBehaviour
    public static event Action<MorphBehaviour> MorphStart, MorphEnd;
    public static void StartMorphEvent(MorphBehaviour morphOrigin)
    {
        MorphStart?.Invoke(morphOrigin);
    }
    public static void EndMorphEvent(MorphBehaviour morphOrigin)
    {
        MorphEnd?.Invoke(morphOrigin);
    }
}
