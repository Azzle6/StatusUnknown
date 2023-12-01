using Player;

namespace Input
{
    using System.Collections;
    using UnityEngine.InputSystem;
    using UnityEngine;


    public static class GamePadRumbleManager
    {
        public static IEnumerator ExecuteRumbleWithTime(GamePadRumbleWithTimer rumbleData, bool stopRumbleAfter, float proportion = 1)
        {
            float timer = 0;
            float lowFrequency;
            float highFrequency;
            while (timer < rumbleData.duration)
            {
                timer += Time.deltaTime;
                lowFrequency = rumbleData.maxLowFrequency * rumbleData.lowFrequencyCurve.Evaluate(timer) * proportion;
                highFrequency = rumbleData.maxHighFrequency * rumbleData.highFrequencyCurve.Evaluate(timer) * proportion;
                Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
                yield return null;
            }

            if (stopRumbleAfter)
                StopRumble();
        }
        
        public static void ExecuteRumbleWithoutTime(GamePadRumbleWithoutTimer rumbleData)
        {
            Gamepad.current.SetMotorSpeeds(rumbleData.lowFrequency, rumbleData.highFrequency);
        }
    
        public static void StopRumble()
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }

    }

}
