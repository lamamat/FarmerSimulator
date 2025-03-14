using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Add OnNewMorning event to trigger a new morning in other scripts
public class Time_Manager : MonoBehaviour
{
    public enum timeStage { Morning, Afternoon, Night, Midnight };

    [Header("Time Settings")]
    public timeStage currentTimeStage;

    [Tooltip("Duration of daytime in minutes")]
    public float dayDurationMinutes = 10f;

    [Tooltip("Duration of nighttime in minutes")]
    public float nightDurationMinutes = 5f;

    [Header("Warning")]
    [SerializeField] private bool isPlayerLateMidnight = false;

    // Event to trigger timeStage in other scripts
    public static event Action OnNewMorning;
    public static event Action OnNoon;
    public static event Action OnNight;
    public static event Action OnMidnight;

    private float timeElapsed = 0f;
    private float currentStageDuration;

    private DaynNight daynNight;

    public float GetCurrentTimeElapsed()
    {
        return timeElapsed;
    }

    void Start()
    {
        // Initialize the first stage and its duration
        currentTimeStage = timeStage.Morning;
        currentStageDuration = dayDurationMinutes * 60f; // Convert minutes to seconds

        // Get the DaynNight instance
        daynNight = FindAnyObjectByType<DaynNight>();

        if (daynNight == null)
        {
            Debug.LogError("DaynNight instance not found!");
        }
        // else
        // {
        //     UpdateDaynNightTime(); // Set initial time in DaynNight
        // }
    }

    void Update()
    {
        // Increment the elapsed time
        timeElapsed += Time.deltaTime;

        if (currentTimeStage == timeStage.Midnight)
        {
            isPlayerLateMidnight = true;
        }
        else
        {
            isPlayerLateMidnight = false;
        }

        // Check if the current stage duration has elapsed
        if (timeElapsed >= currentStageDuration)
        {
            SwitchTimeStage();
        }
    }

    public void SwitchTimeStage()
    {
        // Reset the elapsed time
        timeElapsed = 0f;

        // Switch to the next time stage
        if (currentTimeStage == timeStage.Morning)
        {
            currentTimeStage = timeStage.Afternoon;
            currentStageDuration = dayDurationMinutes * 60f; // Daytime duration
        }
        else if (currentTimeStage == timeStage.Afternoon)
        {
            currentTimeStage = timeStage.Night;
            currentStageDuration = nightDurationMinutes * 60f; // Nighttime duration
        }
        else if (currentTimeStage == timeStage.Night)
        {
            currentTimeStage = timeStage.Midnight;
            currentStageDuration = dayDurationMinutes * 60f; // Midnight duration
        }
        else if (currentTimeStage == timeStage.Midnight)
        {
            currentTimeStage = timeStage.Morning;
            currentStageDuration = dayDurationMinutes * 60f; // Daytime duration

            // Trigger the new morning event
            TriggerNewMorning();
        }

        // Update the time in DaynNight
        UpdateDaynNightTime();

        Debug.Log("Time stage switched to: " + currentTimeStage);
    }

    // Update the time in DaynNight based on the current time stage
    private void UpdateDaynNightTime()
    {
        if (daynNight != null)
        {
            // Map timeStage to TimeOfDay in DaynNight
            switch (currentTimeStage)
            {
                case timeStage.Morning:
                    daynNight.SetTime(3.5f); 
                    break;
                case timeStage.Afternoon:
                    daynNight.SetTime(6f);
                    break;
                case timeStage.Night:
                    daynNight.SetTime(9.1f);
                    break;
                case timeStage.Midnight:
                    daynNight.SetTime(0f);
                    break;
            }
        }
    }


    private void TriggerNewMorning()
    {
        Debug.Log("A new morning has started!");
        OnNewMorning?.Invoke(); // Invoke the event if there are subscribers
    }

    public void CheatSetTimeStage(timeStage targetStage)
    {
        // Set the current time stage to the specified stage
        currentTimeStage = targetStage;

        // Update the duration based on the new stage
        if (currentTimeStage == timeStage.Morning || currentTimeStage == timeStage.Midnight)
        {
            currentStageDuration = dayDurationMinutes * 60f; // Daytime or midnight duration
        }
        else if (currentTimeStage == timeStage.Afternoon || currentTimeStage == timeStage.Night)
        {
            currentStageDuration = nightDurationMinutes * 60f; // Nighttime duration
        }

        // Reset the elapsed time
        timeElapsed = 0f;

        // Update the time in DaynNight
        UpdateDaynNightTime();

        Debug.Log("Cheat activated: Time stage set to " + currentTimeStage);
    }
}