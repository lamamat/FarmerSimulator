using System.Collections.Generic;
using UnityEngine;

public class Store_System : MonoBehaviour
{
    [Header("Store Settings")]
    [SerializeField] private int pickItemCountPerDay;    

    public List<BaseItem_Scriptable> storePerDay = new List<BaseItem_Scriptable>();

    void OnEnable()
    {
        Time_Manager.OnNewMorning += HandleOnNewMorning;
    }

    void OnDisable()
    {
        Time_Manager.OnNewMorning -= HandleOnNewMorning;
    }

    void Start()
    {
        if(FindDataItem.instance.items != null)renewStore();
    }

    private void HandleOnNewMorning()
    {
        renewStore();
    }

    private void renewStore(){
        Debug.Log("Renewing store items");
        storePerDay.Clear();
        while (storePerDay.Count < pickItemCountPerDay)
        {
            BaseItem_Scriptable item = FindDataItem.instance.items[Random.Range(0, FindDataItem.instance.items.Count)].item;
            if (storePerDay.Contains(item)) continue;
            storePerDay.Add(item);
        }
    }
}
