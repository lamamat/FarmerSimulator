using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolSystemROBOT : MonoBehaviour
{
    [SerializeField] GameObject Battery;
    public List<Transform> points;
    public float moveSpeed = 2f;
    public float rotateSpeed = 90f; // องศาต่อวินาที
    public bool isMoving = false;
    public float waitTimeAtPoints = 5f;

    private int currentIndex = 0;
    private bool isWaiting = false;

    private Quaternion targetRotation;
    private bool needRotation = false;

    void Update()
    {
        if (!isMoving || isWaiting || points.Count == 0)
            return;

        MoveToNextPoint();
    }

    void MoveToNextPoint()
    {
        Transform target = points[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // หมุนไปทางเป้าหมายก่อนเดิน
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            targetRot = Quaternion.Euler(0, targetRot.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed / 45f);
        }

        // เดินเข้าเป้าหมาย
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAndRotateSimultaneously());
        }
    }

    IEnumerator WaitAndRotateSimultaneously()
    {
        isWaiting = true;

        // กำหนดมุมหมุนแบบ Absolute ตามตำแหน่ง
        if (currentIndex >= 1 && currentIndex <= 4)
        {
            targetRotation = Quaternion.Euler(0, 90f, 0);
            needRotation = true;
        }
        else if (currentIndex >= 5 && currentIndex <= 8)
        {
            targetRotation = Quaternion.Euler(0, -90f, 0);
            needRotation = true;
        }
        else
        {
            needRotation = false;
        }

        float timer = 0f;
        while (timer < waitTimeAtPoints)
        {
            timer += Time.deltaTime;

            if (needRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
                {
                    needRotation = false;
                }
            }

            yield return null;
        }

        // ไปจุดถัดไป
        currentIndex = (currentIndex + 1) % points.Count;
        isWaiting = false;
    }

    public void SetBattery()
    {
        Battery.SetActive(true);
        isMoving = true;
    }

    public void StopRobot()
    {
        Battery.SetActive(false);
        isMoving = false;

        // เซตตำแหน่งและหมุนกลับ
        transform.position = new Vector3(1.294618f, -0.4868677f, -8.083042f);
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentIndex = 0;
    }
}
