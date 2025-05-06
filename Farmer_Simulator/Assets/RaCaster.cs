using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaCaster : MonoBehaviour
{
    public float rayLength = 100f;           // ความยาวของ ray
    public LineRenderer lineRenderer;        // ใช้แสดงเส้น ray
    public LayerMask hitMask;               // กรอง layer ที่จะโดน ray

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // ยิง Ray จากปลายปืน (หรือจากกล้อง) ไปด้านหน้า
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        // วาดเส้น ray (แสดงตลอดเวลา)
        Vector3 endPosition = origin + direction * rayLength;
        if (Physics.Raycast(ray, out hit, rayLength, hitMask))
        {
            endPosition = hit.point;
        }

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, endPosition);
        }

        // คลิกเมาส์ซ้าย หรือ Trigger
        if (Input.GetMouseButtonDown(0)) // สำหรับทดสอบบน PC
        {
            if (Physics.Raycast(ray, out hit, rayLength, hitMask))
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }
        }
    }
}
