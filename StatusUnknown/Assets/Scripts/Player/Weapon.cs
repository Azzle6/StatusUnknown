using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Transform adsRotTr;
    public float adsAngle;
    public float adsRestAngle;
    public abstract void TriggerPressed();
    
    public abstract void TriggerReleased();
    
    public abstract void Reload();
}
