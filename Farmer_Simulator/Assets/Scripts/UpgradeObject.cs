using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeObject : MonoBehaviour
{
    public enum UpgradeType
    {
        ActivateObject,
        ReplaceObject
    }

    [Header("Upgrade Settings")]
    public UpgradeType upgradeType;

    [Tooltip("ใช้กรณี UpgradeType = ActivateObject")]
    public GameObject objectToActivate;

    [Tooltip("ใช้กรณี UpgradeType = ReplaceObject")]
    public GameObject oldObject;
    public GameObject newObject;

    public void Debughit()
    {
        Debug.Log(gameObject.name + " I'm hit, upgrade triggered!");

        switch (upgradeType)
        {
            case UpgradeType.ActivateObject:
                if (objectToActivate != null)
                {
                    objectToActivate.SetActive(true);
                    Debug.Log("Activated: " + objectToActivate.name);
                }
                break;

            case UpgradeType.ReplaceObject:
                if (oldObject != null && newObject != null)
                {
                    oldObject.SetActive(false);
                    newObject.SetActive(true);
                    Debug.Log("Replaced " + oldObject.name + " with " + newObject.name);
                }
                break;
        }

        // ซ่อนตัวเองหลังอัปเกรดเสร็จ
        gameObject.SetActive(false);
    }
}
