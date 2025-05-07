using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaCaster : MonoBehaviour
{
    public float rayLength = 100f;
    public LineRenderer lineRenderer;
    public LayerMask hitMask;
    private HandRightTrcking handRight;

    public float fireCoolDown = 1.0f; // Cooldown time in seconds for firing
    private float lastFireTime = 0f; // Time of the last fire action

    [Header("Seed")]
    public List<Seed_Scriptable> seedList = new List<Seed_Scriptable>();
    [SerializeField]private int seedIndex = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text ui_Text;

    void Start()
    {
        handRight = GetComponent<HandRightTrcking>();
        getSeedList();
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // Cast a Ray from the current position
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        // Determine the end position of the ray
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

        if (Physics.Raycast(ray, out hit, rayLength, hitMask))
        {
            HandleWeaponActionHold(handRight.currentWeaponIndex, hit);
            // Check if the fire button on the Meta Quest controller is pressed
            if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            {
                if (Time.time - lastFireTime >= fireCoolDown)
                {
                    Debug.Log("Ray Triggered!");
                    lastFireTime = Time.time; // Update the last fire time
                    Debug.Log("Hit object: " + hit.collider.gameObject.name);
                    HandleWeaponActionPress(handRight.currentWeaponIndex, hit);
                }
            }
        }
        else
        {
            uiText("");
        }

        
    }

    private void getSeedList()
    {
        List<Data> datas = FindDataItem.instance.items;
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].item is Seed_Scriptable)
            {
                seedList.Add((Seed_Scriptable)datas[i].item);
            }
        }
    }

    void HandleWeaponActionPress(int weaponIndex, RaycastHit hit)
    {
        Plant_Data plantData = hit.collider.GetComponentInParent<Plant_Data>();

        switch (weaponIndex)
        {
            case 0: // Hand
                break;
            case 1: // Upgrade Gun
                break;
            case 2: // Cure Gun
                plantData.HandleCureAction();
                break;
            case 3: // Harvest Gun
                plantData.HandleHarvestAction();
                break;
            case 4: // Plant Gun
                plantData.HandlePlantAction(seedList[seedIndex]);
                break;
            case 5: // Watered Gun
                plantData.HandleWaterAction();
                break;
            case 6: // Grow Up Gun
                plantData.HandleGrowAction();
                break;
            default:
                break;
        }
    }

    void HandleWeaponActionHold(int weaponIndex, RaycastHit hit)
    {
        Plant_Data plantData = hit.collider.GetComponentInParent<Plant_Data>();

        switch (weaponIndex)
        {
            case 0: // Hand
                uiText("");
                break;
            case 1: // Upgrade Gun
                uiText("");
                break;
            case 2: // Cure Gun
                if(plantData.isInfected)
                {
                    uiText($"infected");
                }
                else
                {
                    uiText($"not infected");
                }
                break;
            case 3: // Harvest Gun
                if (plantData._plantStage == Plant_Data.PlantStage.Harvest)
                {
                    uiText($"Ready to harvest \nSell Price : {plantData.SeedData.SellPrice}");
                }
                else
                {
                    uiText($"Not ready to harvest");
                }
                break;
            case 4: // Plant Gun
                selectSeed();
                break;
            case 5: // Watered Gun
                if (plantData.isWatered)
                {
                    uiText($"Watered");
                }
                else
                {
                    uiText($"Not watered");
                }
                break;
            case 6: // Grow Up Gun
                uiText("");
                break;
            default:
                break;
        }
    }

    private void selectSeed()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)) // A button on Meta Quest controller
        {
            Debug.Log("A button pressed!");
            seedIndex--;
            if (seedIndex < 0)
            {
            seedIndex = seedList.Count - 1; // Wrap around to the last seed
            }
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two)) // B button on Meta Quest controller
        {
            seedIndex++;
            if (seedIndex >= seedList.Count)
            {
                seedIndex = 0; // Wrap around to the first seed
            }
        }

        ui_Text.text = $"Select Seed: {seedList[seedIndex].name} \n Price : {seedList[seedIndex].BuyPrice} \n Plant Time: {seedList[seedIndex].GrowDay}"; ;
    }

    private void uiText(string text)
    {
        ui_Text.text = text;
    }
}
