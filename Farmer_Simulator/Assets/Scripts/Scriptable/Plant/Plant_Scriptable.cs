using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "Scriptable/Plant")]
public class Plant_Scriptable : BaseItem_Scriptable
{
    public int GrowDay;
    public int SellPrice;
    public int BuyPrice;
}
