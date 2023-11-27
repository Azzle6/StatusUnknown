using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProtoXPDialogManager : MonoBehaviour
{
    public static ProtoXPDialogManager instance;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogtext;
    public Image displayedImage;

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

    void DisplayImage(bool enable)
    {
        displayedImage.gameObject.SetActive(enable);
    }

    void ChangeImage(Sprite image)
    {
        displayedImage.sprite = image;
    }

    public void StartDialog(ProtoFXDialogSO dialogSO)
    {
        if(dialogSO.displayDialog)
        {
            DisplayDialogBox(true);
            ChangeDialogText(dialogSO.text);
        }

        if(dialogSO.displayImage)
        {
            DisplayImage(true);
            ChangeImage(dialogSO.image);
        }

        ProtoXPTimer.instance.IncrementTimerValue(dialogSO.timerAddValue);
    }

    public void CloseDialogBox()
    {
        DisplayDialogBox(false);
        DisplayImage(false);
    }
}
