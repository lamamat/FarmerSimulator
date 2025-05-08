using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField] private int batteryAge = 3;
    private int batteryAgeNow = 0;
    private bool isActive = false;
    private BatteryManager manager;

    void OnEnable()
    {
        Time_Manager.OnNewMorning += OnNewDay;
    }

    void OnDisable()
    {
        Time_Manager.OnNewMorning -= OnNewDay;
    }

    public void SetManager(BatteryManager _manager)
    {
        manager = _manager;
    }

    public void SetActiveState(bool active)
    {
        isActive = active;
        this.enabled = active; // à»Ô´/»Ô´Ê¤ÃÔ»µìàÍ§
        Debug.Log($"{gameObject.name} Battery is now " + (active ? "ACTIVE" : "INACTIVE"));
    }

    void OnNewDay()
    {
        if (!isActive) return;

        batteryAgeNow++;
        Debug.Log($"{gameObject.name} age now: {batteryAgeNow}");

        if (batteryAgeNow >= batteryAge)
        {
            manager.OnBatteryDestroyed(gameObject);
            Destroy(gameObject);
        }
    }
}
