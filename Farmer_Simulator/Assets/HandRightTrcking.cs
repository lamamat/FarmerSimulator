using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRightTrcking : MonoBehaviour
{
    public List<GameObject> weapons; // List of weapons
    public int currentWeaponIndex = 0;

    private bool isRightStickInUse = false; // To track thumbstick usage
    public float cooldownTime = 0.5f; // Cooldown time in seconds
    private float lastSwitchTime = 0f; // Time of the last weapon switch

    void Start()
    {
        UpdateWeaponVisibility();
    }

    void Update()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Check if enough time has passed since the last switch
        if (Time.time - lastSwitchTime >= cooldownTime)
        {
            if (input.x > 0.5f && !isRightStickInUse)
            {
                isRightStickInUse = true;
                lastSwitchTime = Time.time; 
                SwitchToNextWeapon();
            }
            else if (input.x < -0.5f && !isRightStickInUse)
            {
                isRightStickInUse = true;
                lastSwitchTime = Time.time;
                backSwitchToNextWeapon();
            }
        }

        // Reset isRightStickInUse when the thumbstick is released
        if (input.x > -0.5f && input.x < 0.5f)
        {
            isRightStickInUse = false;
        }
    }

    void SwitchToNextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        UpdateWeaponVisibility();
    }

    void backSwitchToNextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Count) % weapons.Count;
        UpdateWeaponVisibility();
    }

    void UpdateWeaponVisibility()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == currentWeaponIndex);
        }
    }
}
