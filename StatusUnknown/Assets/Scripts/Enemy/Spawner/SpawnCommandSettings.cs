using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCommandSettings : ScriptableObject
{
    public int quantity = 1;
    public int iteration = 1;
    public bool executeOnce = false;
    [Header("Timing")]
    public float delay = 0;
    public enum StartMode { AUTO, DEATH}
    public StartMode startMode = StartMode.AUTO;
   
    

}
