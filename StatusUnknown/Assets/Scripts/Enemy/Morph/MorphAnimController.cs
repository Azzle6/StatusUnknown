using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MorphAnimController : MonoBehaviour 
{
    [SerializeField] Animator eggAnimator;
    [SerializeField] MorphEgg morphEgg;
    private void OnEnable()
    {
        morphEgg.endMorphEvent += ProcessEggEndAnimation;
    }
    private void OnDisable()
    {
        morphEgg.endMorphEvent -= ProcessEggEndAnimation;
    }

    void ProcessEggEndAnimation(bool sucess)
    {
        string animationClip = (!sucess) ? "MorphEggDestroyed" : "MorphEggFinish";
        eggAnimator.Play(animationClip);
    }

}
