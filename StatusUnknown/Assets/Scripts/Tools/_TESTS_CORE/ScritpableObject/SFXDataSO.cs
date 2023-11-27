using Sirenix.OdinInspector;
using StatusUnknown.Content;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "New SFX Clip", fileName = CoreContentStrings.PATH_CONTENT_FEEDBACKS + "SFX")]
public class SFXDataSO : ScriptableObject
{
    [SerializeField, Required("Name field is empty", InfoMessageType.Warning)] private string modifierName = string.Empty;
    [Space, Title("Audio Clip"), Required("Missing audioclip", InfoMessageType.Warning)] public AudioClip clip;

    [Title("Settings")][Range(0f, 1f)] public float volume = 1f;
    [Range(0f, 0.2f)] public float volumeVariation = 0.05f;
    [Range(0f, 2f)] public float pitch = 1f;
    [Range(0f, 0.2f)] public float pitchVariation = 0.1f;


    // create a copy of the provided audioclip with new pitch settings to simulate other type of voice.
    // integrate this with WWISE
}
