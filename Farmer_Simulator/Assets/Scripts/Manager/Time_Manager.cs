using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Add OnNewMorning event to trigger a new morning in other scripts
public class Time_Manager : SingletonClass<Time_Manager>
{
    public enum timeStage { Morning,Night};

    [Header("Time Settings")]
    public int currentDayCount = 1;
    public timeStage currentTimeStage;

    // [Header("Warning")]
    // [SerializeField] private bool isPlayerLateMidnight = false;

    // Event to trigger timeStage in other scripts
    public static event Action OnNewMorning;
    public static event Action OnNight;

    public int _CurrentDayCount{
        get{
            return currentDayCount;
        }
    }

    void Start()
    {
        // Initialize the first stage and its duration
        currentTimeStage = timeStage.Morning;
    }

    void Update()
    {

    }

    public void SwitchTimeStage()
    {
        // Switch to the next time stage
        if (currentTimeStage == timeStage.Morning)
        {
            currentTimeStage = timeStage.Night;
        }
        else if (currentTimeStage == timeStage.Night)
        {
            currentTimeStage = timeStage.Morning;
            currentDayCount++;

            // Trigger the new morning event
            TriggerNewMorning();
        }
    }

    public void ToNextDay()
    {
        // Switch to the next day
        currentDayCount++;
        currentTimeStage = timeStage.Morning;

        // Trigger the new morning event
        TriggerNewMorning();
    }


    private void TriggerNewMorning()
    {
        Debug.Log("A new morning has started!");
        OnNewMorning?.Invoke(); // Invoke the event if there are subscribers
    }

    public void CheatSetTimeStage(timeStage targetStage)
    {
        
    }
}