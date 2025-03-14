using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : SingletonClass<Game_Manager>
{
    private Time_Manager time;
    private FindDataItem findDataItem;

    void Start()
    {
        time = GetComponent<Time_Manager>();
        findDataItem = GetComponent<FindDataItem>();

        findDataItem.AddItemToPlayer("01",FindObjectOfType<PlayerData>());
        findDataItem.AddItemToPlayer("02",FindObjectOfType<PlayerData>());
        findDataItem.AddItemToPlayer("11",FindObjectOfType<PlayerData>());
    }



    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)){
            time.SwitchTimeStage();
        }
    }
}
