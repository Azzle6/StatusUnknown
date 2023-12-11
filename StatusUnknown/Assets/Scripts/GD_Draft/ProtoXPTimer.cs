using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProtoXPTimer : MonoBehaviour
{
    public static ProtoXPTimer instance;
    [NonSerialized] public float timer;
    public TMP_Text text;

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

    private void Update()
    {
        IncrementTimerValue(Time.deltaTime);
    }

    void RefreshTimerUI()
    {
        text.text = "Timer : " + Mathf.FloorToInt(timer / 60).ToString() + " minutes " + (timer % 60).ToString("#.00");
    }

    public void IncrementTimerValue(float value)
    {
        timer += value;
        RefreshTimerUI();
    }
}
