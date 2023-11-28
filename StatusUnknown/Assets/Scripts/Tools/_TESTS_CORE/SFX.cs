using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Voice Modifier
/// </summary>
[Serializable]
public class SFX
{
    [Header("Data")]
    [LabelText("SFX Type")]
    [LabelWidth(100)]
    [OnValueChanged(nameof(UpdateSFXDisplay))]
    [InlineButton("PlaySFX")]
    public SFXManager.SFXType sfxType = SFXManager.SFXType.Combat;

    [LabelText("$sfxLabel")]
    [LabelWidth(100)]
    [ValueDropdown("SFXType")]
    [OnValueChanged(nameof(UpdateSFXDisplay))]
    [InlineButton("SelectSFX")]
    public SFXDataSO sfxToPlay;
    private string sfxLabel = "SFX";

    [Header("Settings")]
    [SerializeField] private bool showSettings = false;

    [ShowIf("showSettings")]
    [EnableIf("editSettings")]
    [SerializeField] private bool editSettings = false;

    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [ShowIf("showSettings")]
    [EnableIf("editSettings")]
    [SerializeField] private SFXDataSO sfxBase;

    [ShowIf("showSettings")]
    [EnableIf("editSettings")]
    [SerializeField] private bool waitToPlay = true;

    [ShowIf("showSettings")]
    [EnableIf("editSettings")]
    [SerializeField] private bool useDefault = true;

    [DisableIf("useDefault")]
    [ShowIf("showSettings")]
    [EnableIf("editSettings")]
    [SerializeField] private AudioSource audioSource;

    public void PlaySFX() 
    {
        if (useDefault || audioSource == null)
        {
            SFXManager.PlaySFX(sfxToPlay, waitToPlay, null); 
        }
        else
        {
            SFXManager.PlaySFX(sfxToPlay, waitToPlay, audioSource); 
        }
    }

    private void UpdateSFXDisplay() 
    {
        sfxLabel = sfxType.ToString();
        sfxBase = sfxToPlay; 
    }

    private void SelectSFX()
    {
        Selection.activeObject = sfxToPlay; 
    }

    private List<SFXDataSO> SFXType()
    {
        List<SFXDataSO> sfxList = new List<SFXDataSO>();    

        switch(sfxType)
        {
            case SFXManager.SFXType.Combat:
                sfxList = SFXManager.Instance.sfxData;
                break;
            case SFXManager.SFXType.Footsteps:
                sfxList = SFXManager.Instance.ambientData;
                break; 
        }
        return sfxList; 
    }
}
