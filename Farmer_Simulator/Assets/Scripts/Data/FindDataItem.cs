using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public class FindDataItem : MonoBehaviour
{
    public List<Data> items = new List<Data>();

    private void Awake()
    {
        // Find all BaseItem_Scriptable assets in the project
        string[] guids = AssetDatabase.FindAssets("t:BaseItem_Scriptable");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            BaseItem_Scriptable item = AssetDatabase.LoadAssetAtPath<BaseItem_Scriptable>(path);
            if (item != null)
            {
                // Create a new Data object and populate it
                Data data = new Data
                {
                    ID = item.getID(), // Ensure getID() exists in BaseItem_Scriptable
                    item = item
                };

                items.Add(data);
                Debug.Log($"Found BaseItem_Scriptable: {item.name}, ID: {data.ID}");
            }
        }

        Debug.Log($"Collected {items.Count} BaseItem_Scriptable items.");
    }

    public void AddItemToPlayer(string id, PlayerData player)
    {
        Data foundItem = items.Find(data => data.ID == id);
        if (foundItem != null)
        {
            player.AddItemToInventory(foundItem.item);
            Debug.Log($"Added item with ID: {id} to player inventory.");
        }
        else
        {
            Debug.LogWarning($"Item with ID: {id} not found.");
        }
    }
}

[System.Serializable]
public class Data{
    public string ID;
    public BaseItem_Scriptable item;
}