using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BatteryManager : MonoBehaviour
{
    public PlayerData platerdata;
    [SerializeField] TMP_Text YouSurvvied;
    [SerializeField] TMP_Text moneycollect;
    [SerializeField] GameObject PowerRanout;
    public GameObject SmallBattery;
    public GameObject BigBattery;
    public List<Transform> spawnPoints = new List<Transform>();

    public List<GameObject> spawnedObjects = new List<GameObject>();
    private int currentIndex = 0;

    private void Start()
    {
        SpawnSmallBattery();
        SpawnSmallBattery();
        SpawnSmallBattery();
        SpawnSmallBattery();
    }
    void Update()
    {
        if (spawnedObjects.Count <= 0)
        {
            PowerRanout.SetActive(true);
            YouSurvvied.text = "You survived : " + Time_Manager.instance.currentDayCount + "Days";
            moneycollect.text = "Money Collected totally : " + platerdata.Money;
        }

    }

    public void SpawnSmallBattery()
    {
        SpawnObject(SmallBattery);
    }
    public void SpawnBigBattery()
    {
        SpawnObject(BigBattery);
    }
    void SpawnObject(GameObject prefab)
    {
        if (currentIndex >= spawnPoints.Count)
        {
            Debug.Log("เต็ม");
            return;
        }

        Vector3 spawnPosition = spawnPoints[currentIndex].position;
        Quaternion spawnRotation = spawnPoints[currentIndex].rotation;

        GameObject obj = Instantiate(prefab, spawnPosition, spawnRotation);
        spawnedObjects.Add(obj);
        currentIndex++;

        Battery battery = obj.GetComponent<Battery>();
        if (battery != null)
        {
            battery.SetManager(this); // ให้รู้ว่า Manager คือใคร
        }

        UpdateBatteryActivation(); // เปิด/ปิด Battery.cs ตามลำดับ
    }

    public void RemoveLastObject()
    {
        if (currentIndex <= 0) return;

        currentIndex--;
        GameObject objToRemove = spawnedObjects[currentIndex];
        spawnedObjects.RemoveAt(currentIndex);
        Destroy(objToRemove);
        Debug.Log($"Removed object at index {currentIndex}");

        UpdateBatteryActivation(); // อัปเดตสถานะ Battery ตัวที่ควรทำงาน
    }

    public void OnBatteryDestroyed(GameObject destroyedObject)
    {
        if (spawnedObjects.Contains(destroyedObject))
        {
            int index = spawnedObjects.IndexOf(destroyedObject);
            spawnedObjects.RemoveAt(index);
            currentIndex--;

            UpdateBatteryActivation();
        }
    }

    void UpdateBatteryActivation()
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            Battery b = spawnedObjects[i].GetComponent<Battery>();
            if (b != null)
            {
                b.SetActiveState(i == spawnedObjects.Count - 1); // เฉพาะตัวสุดท้ายเปิด
            }
        }
    }
}
