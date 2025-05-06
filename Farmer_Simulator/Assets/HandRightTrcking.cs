using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRightTrcking : MonoBehaviour
{
    public List<GameObject> weapons; // อาวุธทั้งหมด
    private int currentWeaponIndex = 0;

    private bool isRightStickInUse = false; // สำหรับเช็คว่า thumbstick ถูกกดหรือยัง

    void Start()
    {
        UpdateWeaponVisibility();
    }

    void Update()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // เงื่อนไข: ถ้าเลื่อนไปทางขวา (x > 0.5) และยังไม่อยู่ในสถานะ "in use"
        if (input.x > 0.5f && !isRightStickInUse)
        {
            isRightStickInUse = true;
            SwitchToNextWeapon();
        }
        // ถ้าคืนกลับมา (ไม่เลื่อนไปขวาแล้ว) ให้ reset สถานะ
        else if (input.x < 0.2f && isRightStickInUse)
        {
            isRightStickInUse = false;
        }
    }

    void SwitchToNextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
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
