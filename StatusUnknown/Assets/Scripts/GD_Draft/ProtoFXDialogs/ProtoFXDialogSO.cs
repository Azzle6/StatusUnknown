using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProtoXPDialog", menuName = "ScriptableObjects/ProtoXPDialogSO", order = 1)]
public class ProtoFXDialogSO : ScriptableObject
{
    public string dialogName;
    public bool displayName;

    public bool displayDialog;
    public bool justTimer;
    public string text;
    public float timerAddValue;
    
    public bool displayImage;
    public Sprite image;
}
