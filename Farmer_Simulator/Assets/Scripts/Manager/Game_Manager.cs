using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : SingletonClass<Game_Manager>
{
    private FindDataItem findDataItem;
    internal PlayerData playerData;

    public int moneyCollectToday = 0;

    void OnEnable()
    {
        Time_Manager.OnNewMorning += HandleOnNewMorning;
    }

    void OnDisable()
    {
        Time_Manager.OnNewMorning -= HandleOnNewMorning;
    }
    private void HandleOnNewMorning()
    {
        // Reset the money collected today
        moneyCollectToday = 0;
    }

    

    void Start()
    {
        findDataItem = GetComponent<FindDataItem>();
        playerData = FindObjectOfType<PlayerData>();

        // Test add item to player
        // findDataItem.AddItemToPlayer("01",FindObjectOfType<PlayerData>());
        // findDataItem.AddItemToPlayer("02",FindObjectOfType<PlayerData>());
        // findDataItem.AddItemToPlayer("11",FindObjectOfType<PlayerData>());
    }
}
