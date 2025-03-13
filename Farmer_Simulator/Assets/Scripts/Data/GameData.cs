using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// using for saving data and more
[System.Serializable]
public class GameData
{
    // player
    private int _money; // player money
    private int _gender; // use 0 and 1 for gender
    private float[] _position;

    private List<InventoryData> _inventory; 
    // Game
    private string _sceneName;


    public int money{get{return _money;}}
    public int gender{get{return _gender;}}
    public List<InventoryData> inventory{get{return _inventory;}}

    public string sceneName{get{return _sceneName;}}
    public float[] position{get{return _position;}}

    // 
    public GameData(PlayerData player){
        this._money = player.Money;

        this._position[0] = player.transform.position.x;
        this._position[1] = player.transform.position.y;
        this._position[2] = player.transform.position.z;

        this._inventory = player.Inventory;
    }

}

[System.Serializable]
public class InventoryData{
    public string ID; //collect item id
    public int Amount; // collect item amount

    public InventoryData(string ID,int amount){
        this.ID = ID;
        this.Amount = amount;
    }
}