using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    public static FastTravelManager instance;

    [SerializeField] private GameObject[] travelDestinations; // Array of destinations (ToFarm, ToBalance, ToBed)

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ฟังก์ชั่นในการย้ายตำแหน่งผู้เล่น
    public void TeleportPlayer(int destinationIndex)
    {
        if (destinationIndex >= 0 && destinationIndex < travelDestinations.Length)
        {
            Vector3 targetPosition = travelDestinations[destinationIndex].transform.position;
            // กำหนดค่า Y ตายตัว = 0.1
            Vector3 newPosition = new Vector3(targetPosition.x, 0.1f, targetPosition.z);
            player.transform.position = newPosition;

            Debug.Log($"Player teleported to {travelDestinations[destinationIndex].name} at position: {newPosition}");
        }
        else
        {
            Debug.LogWarning("Invalid destination index!");
        }
    }
}
