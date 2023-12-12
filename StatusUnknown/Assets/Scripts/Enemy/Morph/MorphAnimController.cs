namespace Enemy.Morph
{
    using UnityEngine;
    
    public class MorphAnimController : MonoBehaviour 
    {
        [SerializeField] Animator eggAnimator;
        [SerializeField] MorphEgg morphEgg;
        private void OnEnable()
        {
            this.morphEgg.endMorphEvent += this.ProcessEggEndAnimation;
        }
        private void OnDisable()
        {
            this.morphEgg.endMorphEvent -= this.ProcessEggEndAnimation;
        }

    void ProcessEggEndAnimation(bool sucess)
    {
        string animationClip = (!sucess) ? "MorphEggDestroyed" : "MorphEggFinish";
        eggAnimator.Play(animationClip);
    }

    }
}
