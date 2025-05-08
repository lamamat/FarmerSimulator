using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RaCaster : MonoBehaviour
{
    public BatteryManager batterymeg;
    //sleep fade 
    [SerializeField] GameObject SleepFade; //use when sleep
    [SerializeField] GameObject ControllerLerLeft;


    private PlayerData playerData;
    [Header("Cost To Upgrade")]
    [SerializeField] int UpgradePlotCost = 1000;
    [SerializeField] int UpgradePotCost = 500;
    [SerializeField] TMP_Text CostToUpgrade;
    [Header("Hold Indicators")]
    public List<ParticleSystem> gunParticle; // GameObject สำหรับแต่ละปืน 1-6
    [SerializeField] private GameObject DisPlayUpgrade;
    [SerializeField] private GameObject upgradePlotIndicator;
    [SerializeField] private GameObject upgradePotIndicator;

    public float rayLength = 100f;
    public LineRenderer lineRenderer;
    public LayerMask hitMask;
    private HandRightTrcking handRight;

    public float fireCoolDown = 1.0f; // Cooldown time in seconds for firing
    private float lastFireTime = 0f; // Time of the last fire action

    [Header("Seed")]
    public List<Seed_Scriptable> seedList = new List<Seed_Scriptable>();
    [SerializeField] private int seedIndex = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text ui_Text;

    void Start()
    {
        handRight = GetComponent<HandRightTrcking>();
        getSeedList();
        playerData = FindObjectOfType<PlayerData>(); // หาตัว PlayerData
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
            HandleUpgradeIndicators(hit);
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
            else{
                HandleWeaponActionRelease(handRight.currentWeaponIndex);
            }
        }
        else
        {
            uiText("");
            DisPlayUpgrade.SetActive(false);
            HandleWeaponActionRelease(handRight.currentWeaponIndex);

        }


    }

    private void HandleWeaponActionRelease(int currentWeaponIndex)
    {
        switch (currentWeaponIndex)
        {
            case 1: // Upgrade Gun
                stopPratical(0);
                break;
            case 2: // Cure Gun
                stopPratical(1);
                break;
            case 3: // Harvest Gun
                stopPratical(2);
                break;
            case 4: // Plant Gun
                stopPratical(3);
                break;
            case 5: // Watered Gun
                stopPratical(4);
                break;
            case 6: // Grow Up Gun
                stopPratical(5);
                break;
            default:
                break;
        }
    }

    private void HandleUpgradeIndicators(RaycastHit hit)
    {
        BuyBatteryDisplay(hit);
        UpgradPlotDisplay(hit);
    }

    void BuyBatteryDisplay(RaycastHit hit)
    {
        if (hit.collider.CompareTag("SmallBattery"))
        {
            uiText("press to buy battery");
          //  batterymeg.SpawnSmallBattery();
        }
        else if (hit.collider.CompareTag("BigBattery"))
        {
            uiText("press to buy BIG battery");
           // batterymeg.SpawnBigBattery();
        }
        else
        {
        }
    }
    void UpgradPlotDisplay(RaycastHit hit)
    {
        if (hit.collider.CompareTag("UpgradePlot"))
        {
            DisPlayUpgrade.SetActive(true);
            upgradePlotIndicator.SetActive(true);
            upgradePotIndicator.SetActive(false);
            CostToUpgrade.text = $"Cost: {UpgradePlotCost}";

            if (playerData.Money < UpgradePlotCost)
            {
                uiText("Not enough money");
            }
            else
            {
                uiText(""); // ถ้าเงินพอ ไม่ต้องแสดงข้อความ
            }
        }
        else if (hit.collider.CompareTag("UpgradePot"))
        {
            DisPlayUpgrade.SetActive(true);
            upgradePlotIndicator.SetActive(false);
            upgradePotIndicator.SetActive(true);
            CostToUpgrade.text = $"Cost: {UpgradePotCost}";

            if (playerData.Money < UpgradePotCost)
            {
                uiText("Not enough money");
            }
            else
            {
                uiText("");
            }
        }
        else
        {
            DisPlayUpgrade.SetActive(false);
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
        bool isBed = hit.collider.CompareTag("bed");

        switch (weaponIndex)
        {
            case 0: // Hand
                if (isBed)
                {
                    Debug.Log("sleep");
                    StartCoroutine(SleepFadeCountDown());
                }
                else
                {
                    // เช็ค tag ของ hit object
                    if (hit.collider.CompareTag("Next"))
                    {
                        HandleDisplayChange(hit.collider, true); // true = next
                    }
                    else if (hit.collider.CompareTag("Previous"))
                    {
                        HandleDisplayChange(hit.collider, false); // false = previous
                    }
                    // เช็ค Tag "ToFarm"
                    else if (hit.collider.CompareTag("ToFarm"))
                    {
                        FastTravelManager.instance.TeleportPlayer(0); // GameObject[0]
                    }
                    // เช็ค Tag "ToBalance"
                    else if (hit.collider.CompareTag("ToBalance"))
                    {
                        FastTravelManager.instance.TeleportPlayer(1); // GameObject[1]
                    }
                    // เช็ค Tag "ToBed"
                    else if (hit.collider.CompareTag("ToBed"))
                    {
                        FastTravelManager.instance.TeleportPlayer(2); // GameObject[2]
                    }
                    else if (hit.collider.CompareTag("SmallBattery"))
                    {
                        if(playerData.Money > 200)
                        {
                            playerData.SubtractMoney(200);
                            batterymeg.SpawnSmallBattery();
                        }
                        else
                        {
                            uiText("You don't have enough money");
                        }
                    }
                    else if (hit.collider.CompareTag("BigBattery"))
                    {
                        if (playerData.Money > 500)
                        {
                            playerData.SubtractMoney(500);
                            batterymeg.SpawnBigBattery();
                        }
                        else
                        {
                            uiText("You don't have enough money");
                        }
                    }
                    else
                    {
                    }
                }
                break;
            case 1: // Upgrade Gun
                PlayPratical(0);

                UpgradeObject upgradeTarget = hit.collider.GetComponent<UpgradeObject>();
                if (upgradeTarget != null)
                {
                    if (hit.collider.CompareTag("UpgradePlot"))
                    {
                        if (playerData.Money >= UpgradePlotCost)
                        {
                            playerData.SubtractMoney(UpgradePlotCost);
                            upgradeTarget.Debughit();
                        }
                        else
                        {
                            uiText("Not enough money");
                        }
                    }
                    else if (hit.collider.CompareTag("UpgradePot"))
                    {
                        if (playerData.Money >= UpgradePotCost)
                        {
                            playerData.SubtractMoney(UpgradePotCost);
                            upgradeTarget.Debughit();
                        }
                        else
                        {
                            uiText("Not enough money");
                        }
                    }
                    else
                    {
                        upgradeTarget.Debughit(); // เผื่อมี tag อื่นในอนาคต
                    }
                }
                else
                {
                    Debug.LogWarning("UpgradeObject NOT FOUND on: " + hit.collider.gameObject.name);
                }
                break;
            case 2: // Cure Gun
                PlayPratical(1);

                plantData.HandleCureAction();
                break;
            case 3: // Harvest Gun
                PlayPratical(2);

                plantData.HandleHarvestAction();
                break;
            case 4: // Plant Gun
                PlayPratical(3);

                plantData.HandlePlantAction(seedList[seedIndex]);
                break;
            case 5: // Watered Gun
                PlayPratical(4);

                plantData.HandleWaterAction();
                break;
            case 6: // Grow Up Gun
                PlayPratical(5);
                
                plantData.HandleGrowAction();
                break;
            default:
                break;
        }
    }
    private void HandleDisplayChange(Collider collider, bool isNext)
    {
        DisplayShowManager displayManager = collider.GetComponentInParent<DisplayShowManager>();
        if (displayManager != null)
        {
            if (isNext)
            {
                displayManager.Next(); // ถ้าเป็น "next"
            }
            else
            {
                displayManager.Previous(); // ถ้าเป็น "previous"
            }
        }
        else
        {
            Debug.LogWarning("DisplayManager not found on parent of: " + collider.gameObject.name);
        }
    }
    void HandleWeaponActionHold(int weaponIndex, RaycastHit hit)
    {
        Plant_Data plantData = hit.collider.GetComponentInParent<Plant_Data>();
        bool isBed = hit.collider.CompareTag("bed");
        switch (weaponIndex)
        {
            case 0: // Hand
                if (isBed)
                {
                    uiText("Press to sleep");
                }
                else
                {
                    uiText("");
                }
            break;
            case 2: // Cure Gun
                if (plantData.isInfected)
                {
                    uiText("infected");
                }
                else
                {
                    uiText("not infected");
                }
            break;
            case 3: // Harvest Gun
                if (plantData._plantStage == Plant_Data.PlantStage.Harvest)
                {
                    uiText($"Ready to harvest \nSell Price : {plantData.SeedData.SellPrice}");
                }
                else if(plantData._plantStage == Plant_Data.PlantStage.Dead)
                {
                    uiText($"This Plant is Dead \nSell Price : 0");
                }
                else
                {
                    uiText("Not ready to harvest");
                }
            break;
            case 4: // Plant Gun
                selectSeed();
            break;
            case 5: // Watered Gun
                if (plantData._plantStage == Plant_Data.PlantStage.None || plantData._plantStage == Plant_Data.PlantStage.Dead) // Add null check for plantData
                {
                    uiText("no Plant");
                }
                else uiText(plantData.isWatered ? "Watered" : "Not watered");
            break;
            case 6: // Grow Up Gun
                if (plantData._plantStage == Plant_Data.PlantStage.None || plantData._plantStage == Plant_Data.PlantStage.Dead) // Add null check for plantData
                {
                    uiText("no Plant");
                }
                else if (plantData._plantStage == Plant_Data.PlantStage.Harvest)
                {
                    uiText("Max Grow Up");
                }
                else uiText("Can grow up \n price : 100");
            break;
            default:
                uiText("");
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

    private void PlayPratical(int index)
    {
        if (gunParticle[index] != null)
        {
            gunParticle[index].Play();
        }
    }

    private void stopPratical(int index)
    {
        if (gunParticle[index] != null)
        {
            gunParticle[index].Stop();
        }
    }

    IEnumerator SleepFadeCountDown()
    {
        ControllerLerLeft.SetActive(false);
        SleepFade.SetActive(true);
        yield return new WaitForSeconds(5);
        Time_Manager.instance.ToNextDay();
        SleepFade.SetActive(false);
        ControllerLerLeft.SetActive(true);
    }

}
