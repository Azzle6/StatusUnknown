using System;

namespace Input
{
    using UnityEngine;
    
    [Serializable]
    public class GamePadRumbleWithTimer 
    {
        [Range(0,1)] public float maxLowFrequency;
        [Range(0,1)] public float maxHighFrequency;
        public AnimationCurve lowFrequencyCurve;
        public AnimationCurve highFrequencyCurve;
        public float duration;
    }
    
    [Serializable]
    public class GamePadRumbleWithoutTimer 
    {
        [Range(0,1)] public float lowFrequency;
        [Range(0,1)] public float highFrequency;
    }
   

    
}

