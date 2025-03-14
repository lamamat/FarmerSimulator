using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Scriptable/Tool")]
public class Tool_Scriptable : BaseItem_Scriptable
{
    public override string getID()
    {
        return "2" + ID;
    }
}
