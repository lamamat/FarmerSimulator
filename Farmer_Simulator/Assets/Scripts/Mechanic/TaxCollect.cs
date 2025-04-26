using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxCollect : MonoBehaviour
{
    [SerializeField] private int taxAmount = 100; // Amount of tax to collect
    [SerializeField] private float taxIntervalDay = 5f; // Interval in days for tax collection
    private bool isTaxCollected = false; // Flag to check if tax has been collected

    void Update()
    {
        CollectTax();
    }

    public void CollectTax()
    {
        if (Time_Manager.instance.currentDayCount % taxIntervalDay == 0 && !isTaxCollected)
        {
            isTaxCollected = true; // Set the flag to true to prevent multiple collections
            Debug.Log($"Tax of {taxAmount} collected on day {Time_Manager.instance.currentDayCount}");
            Game_Manager.instance.playerData.SubtractMoney(taxAmount);
            // Add logic to deduct taxAmount from player's resources here
        }
        else if (Time_Manager.instance.currentDayCount % taxIntervalDay != 0)
        {
            isTaxCollected = false; // Reset the flag for the next collection cycle
        }
    }
}
