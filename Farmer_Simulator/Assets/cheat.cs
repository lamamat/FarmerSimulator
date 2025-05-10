using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cheat : MonoBehaviour
{
    [SerializeField] PlayerData playerdata;
    [SerializeField] BatteryManager batteryManager;
    [SerializeField] GameObject PlotCheat;
    [SerializeField] GameObject PlotNormal;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) 
        {
            playerdata.AddMoney(999999);
            PlotNormal.SetActive(false);
            PlotCheat.SetActive(true);  
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            batteryManager.RemoveLastObject();
        }
    }
}
