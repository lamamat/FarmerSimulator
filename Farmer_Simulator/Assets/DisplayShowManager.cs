using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayShowManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> displayObjects; // List of display objects
    private int currentIndex = 0; // ตำแหน่งของ GameObject ที่แสดงอยู่ตอนนี้

    void Start()
    {
        ShowCurrentDisplay(); // แสดง GameObject ตัวแรกใน list เมื่อเริ่มต้น
    }

    void Update()
    {
        // อาจจะมีการเช็คการกดปุ่มที่นี่ถ้าต้องการ
    }

    public void DisplayShow()
    {
        Debug.Log("next!!!");
        Next(); // เมื่อเรียกใช้จะไปที่ display ถัดไป
    }

    public void Next()
    {
        currentIndex++; // ไปข้างหน้า 1 step
        if (currentIndex >= displayObjects.Count) // ถ้า index เกินขอบเขต list
        {
            currentIndex = 0; // กลับไปที่แรก
        }
        ShowCurrentDisplay();
    }

    public void Previous()
    {
        currentIndex--; // ย้อนกลับ 1 step
        if (currentIndex < 0) // ถ้า index น้อยกว่า 0
        {
            currentIndex = displayObjects.Count - 1; // ไปที่สุดท้าย
        }
        ShowCurrentDisplay();
    }

    private void ShowCurrentDisplay()
    {
        // ซ่อนทุก GameObject ก่อน
        foreach (var obj in displayObjects)
        {
            obj.SetActive(false);
        }

        // แสดง GameObject ที่ตรงกับ currentIndex
        if (displayObjects.Count > 0)
        {
            displayObjects[currentIndex].SetActive(true);
        }
    }
}
