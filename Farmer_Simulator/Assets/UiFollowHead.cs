using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowHead : MonoBehaviour
{
    public Transform head; // ตั้งให้เป็น Main Camera
    public float distanceFromHead = 1.5f;

    void Update()
    {
        if (head == null) return;

        // วางไว้หน้ากล้อง
        transform.position = head.position + head.forward * distanceFromHead;

        // มองตามหัว
        transform.rotation = Quaternion.LookRotation(transform.position - head.position);
    }
}
