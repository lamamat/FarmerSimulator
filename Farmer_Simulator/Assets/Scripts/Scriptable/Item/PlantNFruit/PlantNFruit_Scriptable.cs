using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "Scriptable/Plant")]
public class PlantNFruit_Scriptable : BaseItem_Scriptable
{
    public int SellPrice;
    public int BuyPrice;
    public int spoilDay;
    public bool canSpoil;

    [Header("Plant Prefab")]
    public GameObject plantPrefab;

    public override string getID()
    {
        return "1" + ID;
    }
}
