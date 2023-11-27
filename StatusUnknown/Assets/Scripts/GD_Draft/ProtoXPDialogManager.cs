using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class ProtoXPDialogManager : MonoBehaviour
{
    public static ProtoXPDialogManager instance;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogtext;

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

    void DisplayDialogBox(bool enable)
    {
        dialogBox.SetActive(enable);
    }

    void ChangeDialogText(string newText)
    {
        dialogtext.text = newText;
    }

    public void StartDialog(ProtoFXDialogSO dialogSO)
    {
        DisplayDialogBox(true);
        ChangeDialogText(dialogSO.text);
        ProtoXPTimer.instance.IncrementTimerValue(dialogSO.timerAddValue);
    }

    public void CloseDialogBox()
    {
        DisplayDialogBox(false);
    }
}
