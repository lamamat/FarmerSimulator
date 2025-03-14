using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is only for player data don't use it for other reason
public class PlayerData : MonoBehaviour
{
    [Header("Player Data")]
    [SerializeField] private int _money; // player money
    [SerializeField] private List<InventoryData> _inventory; // player inventory

    [Header("Player Info")]
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;

    public int Money{get{return _money;} set{_money = value;}}
    public List<InventoryData> Inventory{get{return _inventory;} set{_inventory = value;}}

    private BaseItem_Scriptable itemONHand;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            CollectItem(FindAnyObjectByType<Plant_Data>().SeedData);
        }
    }

    internal void AddItemToInventory(BaseItem_Scriptable item)
    {
        CollectItem(item);
    }

    #region Inventory (Collect , remove)
    // collect item on hand
    public void CollectItemOnHand(){
        if(itemONHand != null){
            CollectItem(itemONHand);
        }
    }

    // collect item
    public void CollectItem(BaseItem_Scriptable item){
        bool itemFound = false;

        for (int i = 0; i < _inventory.Count; i++)
        {
            if (_inventory[i].ID == item.getID())
            {
                Debug.Log("Already have this item: " + item.name);
                _inventory[i].Amount++;
                Debug.Log("Collect Item: " + item.name + " Amount: " + _inventory[i].Amount);
                itemFound = true;
                break;
            }
        }

        if (!itemFound)
        {
            Debug.Log("Add new item: " + item.name);
            ConvertToInventory(item.getID());
            Debug.Log("Collect Item: " + item.name);
        }
    }

    // remove item
    public void RemoveSelectItem(BaseItem_Scriptable item){
        if(_inventory == null){
            return;
        }
        if(_inventory.Count > 0){
            for(int i = 0; i < _inventory.Count; i++){
                if(_inventory[i].ID == item.getID()){
                    _inventory[i].Amount--;
                    if(_inventory[i].Amount <= 0){
                        _inventory.RemoveAt(i);
                    }
                    break;
                }
            }
        }
    }

    // remove all item
    public void RemoveAllSelectItem(BaseItem_Scriptable item){
        if(_inventory == null){
            return;
        }
        if(_inventory.Count > 0){
            for(int i = 0; i < _inventory.Count; i++){
                if(_inventory[i].ID == item.getID()){
                    _inventory.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void ConvertToInventory(string ID){
        if(_inventory == null){
            _inventory = new List<InventoryData>();
        }

        InventoryData data = new InventoryData(ID,1);
        Debug.Log("Convert");
        
        _inventory.Add(data);
    }
    #endregion
}
