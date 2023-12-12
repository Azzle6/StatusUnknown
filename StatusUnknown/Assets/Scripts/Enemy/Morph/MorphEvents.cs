using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MorphEvents 
{
    // TODO : Define usefull parameters more than MorphBehaviour
    public static event Action<MorphBehaviour> MorphStart, MorphEnd;
    public static MorphEgg activeMorphEgg {  get; private set; }
    public static void StartMorphEvent(MorphBehaviour morphOrigin)
    {
        Debug.Log("StartMorphEvent");
        MorphStart?.Invoke(morphOrigin);
    }
    public static void EndMorphEvent(MorphBehaviour morphOrigin)
    {
        Debug.Log("EndMorphEvent");
        MorphEnd?.Invoke(morphOrigin);
    }

    public static void RegisterActiveMorphEgg(MorphEgg egg)
    {
        activeMorphEgg = egg;
        egg.endMorphEvent += (bool sucess) => { activeMorphEgg = null; };
    }

}
