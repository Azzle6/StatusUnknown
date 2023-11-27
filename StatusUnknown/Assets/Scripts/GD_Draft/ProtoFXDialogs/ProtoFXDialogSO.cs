using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProtoXPDialog", menuName = "ScriptableObjects/ProtoXPDialogSO", order = 1)]
public class ProtoFXDialogSO : ScriptableObject
{
    [SerializeField] public string dialogName;

    [SerializeField] public bool displayDialog;
    [SerializeField] public string text;
    [SerializeField] public float timerAddValue;

    [SerializeField] public bool displayImage;
    [SerializeField] public Sprite image;
}
