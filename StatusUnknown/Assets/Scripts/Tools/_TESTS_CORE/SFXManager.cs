using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    private static SFXManager instance;
    public static SFXManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<SFXManager>();
            return instance; 
        }
    }

    public enum SFXType { Combat, Footsteps, Other }

    [HorizontalGroup("AudioSource"), SerializeField] private AudioSource defaultAudioSource;

    [TabGroup("SFX")][AssetList(Path = "Data/Audio/SFX", AutoPopulate = true)] public List<SFXDataSO> sfxData;
    [TabGroup("Ambient")][AssetList(Path = "Data/Audio/Ambient", AutoPopulate = true)] public List<SFXDataSO> ambientData;
    [Button] 
    public static void PlaySFX(SFXDataSO sfx, bool waitToFinish = true, AudioSource audioSource = null) 
    {
        if (!audioSource)
        {
            Debug.LogError("Audiosource field was empty");
            return; 
        }

        if (audioSource.isPlaying && waitToFinish) return;
        audioSource.PlayOneShot(sfx.clip); 

        
    }

    [HorizontalGroup("AudioSource"), ShowIf("@defaultAudioSource == null")]
    [GUIColor("yellow"), Button(size:ButtonSizes.Small)]
    private void AddAudioSource()
    {
        defaultAudioSource = gameObject.GetComponent<AudioSource>();    
    }
}
