using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXExample : MonoBehaviour
{
    [SerializeField] private SFX sfxToPlay;

    private void Start()
    {
        sfxToPlay.PlaySFX(); 
    }
}
