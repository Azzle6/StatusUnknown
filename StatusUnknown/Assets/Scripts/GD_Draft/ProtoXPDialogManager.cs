using UnityEngine;

public class ProtoXPDialogManager : MonoBehaviour
{
    public static ProtoXPDialogManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    
    public void StartDialog(ProtoFXDialogSO dialogSO)
    {
        ProtoXPTimer.instance.IncrementTimerValue(dialogSO.timerAddValue);
    }
    
}
