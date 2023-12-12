using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class ProtoXPTimer : MonoBehaviour
{
    public static ProtoXPTimer instance;
    [SerializeField] private UIDocument uiDocument;
    private Label timerLabel;
    [NonSerialized] public float timer;

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
        
        timerLabel = uiDocument.rootVisualElement.Q<Label>("Timer");
    }

    private void Update()
    {
        IncrementTimerValue(Time.deltaTime);
    }

    void RefreshTimerUI()
    {
        timerLabel.text = "Timer : " + Mathf.FloorToInt(timer / 60) + " minutes " + (timer % 60).ToString("#.00");
    }

    public void IncrementTimerValue(float value)
    {
        timer += value;
        RefreshTimerUI();
    }
}
