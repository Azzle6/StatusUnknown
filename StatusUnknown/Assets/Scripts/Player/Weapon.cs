using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void TriggerPressed();
    
    public abstract void TriggerReleased();
    
    public abstract void Reload();
}
