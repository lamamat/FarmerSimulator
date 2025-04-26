using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "Scriptable/Seed")]
public class Seed_Scriptable : BaseItem_Scriptable
{
    public int GrowDay;
    public int SellPrice;
    public int BuyPrice;
    public bool WaterNeeded;

    [Header("Seed Prefab")]
    public GameObject SeedPrefab;

    public PlantNFruit_Scriptable product;

    public List<GrowState> growStates = new List<GrowState>();

    public override string getID()
    {
        return "0" + ID;
    }
}

[Serializable]
public class GrowState{
    public string stateName;
    public GameObject plantPrefab;
}
